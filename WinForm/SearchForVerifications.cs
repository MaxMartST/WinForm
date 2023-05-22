using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForm.ExceptionServise;
using WinForm.Model;

namespace WinForm
{
    public class SearchForVerifications
    {
        const string baseUri = "https://fgis.gost.ru";
        private readonly IProgress<string> progress;
        static HttpClient? client;

        public SearchForVerifications(IProgress<string> progress)
        {
            this.progress = progress;

            var httpClientHandler = new HttpClientHandler
            {
                Proxy = new WebProxy
                {
                    Address = new Uri($"http://proxy.scrapingbee.com:8886"),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,

                    // Proxy credentials
                    Credentials = new NetworkCredential(
                    userName: "NKACMU3TRSZI9R9QMAMYX01TU0KD5M4DV4QPIB66DNI6PNCU5TWUP1RFJPV0NPFM3EKWHSVJYLYF4NYT",
                    password: "render_js=False&premium_proxy=True")
                }
            };

            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            client = new HttpClient(handler: httpClientHandler, disposeHandler: true);
            //client = new HttpClient();
        }

        public async Task<List<ResultDataModel>> SearchAsync(List<MeasuringDevice> measuringDevices)
        {
            int countNode = measuringDevices.Count;

            if (countNode == 0)
            {
                progress.Report($"({DateTime.Now}) Список пуст." + Environment.NewLine);
                return new List<ResultDataModel>();
            }
                
            List<ResultDataModel> resultDataModels = new List<ResultDataModel>();

            progress.Report($"({DateTime.Now}) Начало поиска.");

            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            for (int i = 0; i < countNode; i++)
            {
                // Обращение к API
                try
                {
                    var results = await GetDataAsync(measuringDevices[i]);
                    resultDataModels.AddRange(results);
                }
                catch (Exception ex)
                {
                    progress.Report($"({DateTime.Now}) Ошибка: {ex.Message}" + Environment.NewLine);
                    throw;
                }

                var info = $"({DateTime.Now}) Получаны данные ({i + 1}/{countNode})." + Environment.NewLine;

                info += $"\tРегистрационный номер: {measuringDevices[i].RegistrationNumber}" + Environment.NewLine
                    + $"\tГЭТ: {measuringDevices[i].StatePrimaryDenchmark}" + Environment.NewLine
                    + $"\tРазряд: {measuringDevices[i].Discharge}";

                progress.Report($"{info}");
            }

            progress.Report($"({DateTime.Now}) Конец поиска.");

            return resultDataModels;
        }

        private async Task<List<ResultDataModel>> GetDataAsync(MeasuringDevice measuringDevice)
        {
            List<ResultDataModel> resultDataModels = new List<ResultDataModel>();
            var signCipherInn = new Dictionary<string, string>();

            const int rows = 100;
            int index = 0, start = 0, numberPosition = 1;
            decimal numberRequests = 0;

            do
            {
                //Thread.Sleep(2000);
                var relativeUri = $"fundmetrology/eapi/vri?start={start}&rows={rows}&mit_number=*{measuringDevice.RegistrationNumber}*"; 
                progress.Report($"({DateTime.Now}) Поиск поверок по регистрационному номеру \"{measuringDevice.RegistrationNumber}\"");

                using HttpResponseMessage response = await client.GetAsync(relativeUri);

                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                Model.VerificationResultModel.Root root = JsonConvert.DeserializeObject<Model.VerificationResultModel.Root>(resp);

                progress.Report($"({DateTime.Now}) Найдено {root.Result.Count} позиций для \"{measuringDevice.RegistrationNumber}\"");

                if (root.Result.Count == 0)
                    throw new Exception($"Нет результатов для: {measuringDevice.RegistrationNumber}");

                if (start == 0)
                    numberRequests = Math.Ceiling((decimal)root.Result.Count / root.Result.Rows);

                foreach (var item in root.Result.Items)
                {
                    var resultDataModel = new ResultDataModel()
                    {
                        Mit_notation = item.Mit_notation,
                        Mit_number = item.Mit_number,
                        Mit_title = item.Mit_title,
                        Mi_modification = item.Mi_modification,
                        Mi_number = item.Mi_number,
                        Org_title = item.Org_title
                    };

                    try
                    {
                        progress.Report($"({DateTime.Now}) Поиск шифра клейма и ИНН для изделия с Регистрационным и Заводским номером: ({resultDataModel.Mit_number}\\{resultDataModel.Mi_number})");
                        resultDataModel.signCipher = await GetRegistryElementModelAsync(item.Vri_id, numberPosition);

                        // Если signCipher уже есть в словаре, то пропускаем поиск ИНН для signCipher
                        // Иначе обращаемся к АПИ с целью найти ИНН
                        if (signCipherInn.ContainsKey(resultDataModel.signCipher))
                        {
                            resultDataModel.Inn = signCipherInn[resultDataModel.signCipher];
                            progress.Report($"({DateTime.Now}) Шифр кейма \"{resultDataModel.signCipher}\" уже имеет ИНН, равный: \"{resultDataModel.Inn}\"");
                        }
                        else
                        {
                            resultDataModel.Inn = await GetTaxpayerIdentificationNumber(resultDataModel.signCipher);
                            signCipherInn.Add(resultDataModel.signCipher, resultDataModel.Inn);
                        }
                    }
                    catch (Exception ex)
                    {
                        progress.Report($"({DateTime.Now}) Ошибка: {ex.Message}");
                        continue;
                    }

                    numberPosition++;
                    resultDataModels.Add(resultDataModel);
                }

                start += rows;
                index++;

            } while (index < numberRequests);

            return resultDataModels;
        }

        private async Task<string> GetRegistryElementModelAsync(string vri_id, int numberPosition)
        {
            Thread.Sleep(2000);
            var relativeUri = $"fundmetrology/eapi/vri/{vri_id}";
            progress.Report($"({DateTime.Now}) Поиск шифра клейма позиции № {numberPosition} (vri_id = {vri_id})");

            using HttpResponseMessage httpResponseMessage = await client.GetAsync(relativeUri);
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Model.RegistryElementModel.Root root = JsonConvert.DeserializeObject<Model.RegistryElementModel.Root>(content);

                var signCipher = root.Result.vriInfo.signCipher;
                progress.Report($"({DateTime.Now}) Шифр клейма равен \"{signCipher}\"");

                return signCipher;
            }

            if (httpResponseMessage.Content != null)
                httpResponseMessage.Content.Dispose();

            throw new SimpleHttpResponseException(httpResponseMessage.StatusCode, content);
        }

        private async Task<string> GetTaxpayerIdentificationNumber(string signCipher)
        {
            Thread.Sleep(2000);
            var relativeUri = $"fundmetrology/cm/xcdb/vcs/select?fq=sign:*{signCipher}*&q=*&fl=vcs_id,sign,act_no,act_date,title,inn,kpp,ogrn&sort=act_date+desc,sign+asc&rows=20&start=0";
            progress.Report($"({DateTime.Now}) Поиск ИНН по шифру клейма \"{signCipher}\"");

            using HttpResponseMessage httpResponseMessage = await client.GetAsync(relativeUri);
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var json = JObject.Parse(content);

                var response = json["response"];
                var docs = response["docs"];

                if (docs.Count() == 0)
                    throw new Exception($"Неудалось найти данные о ИНН по шифру клейма: \"{signCipher}\"");

                var inn = docs[0]["inn"].ToString();
                progress.Report($"({DateTime.Now}) Шифру клейма \"{signCipher}\" соответствует Инн: \"{inn}\"");

                return inn;
            }

            if (httpResponseMessage.Content != null)
                httpResponseMessage.Content.Dispose();

            throw new SimpleHttpResponseException(httpResponseMessage.StatusCode, content);
        }
    }
}
