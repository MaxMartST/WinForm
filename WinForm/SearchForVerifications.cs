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
using WinForm.ExceptionServise;
using WinForm.Model;
using WinForm.Model.RegistryElementModel;

namespace WinForm
{
    public class SearchForVerifications
    {
        const string baseUri = "https://fgis.gost.ru";
        public List<ResultDataModel> resultDataModels { get; set; }
        private readonly IProgress<string> progress;
        static HttpClient? client;

        public SearchForVerifications(IProgress<string> progress)
        {
            this.progress = progress;
            this.resultDataModels = new List<ResultDataModel>();
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

            client = new HttpClient();
            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task SearchByParametersFromFileAsync(List<MeasuringDevice> measuringDevices)
        {
            int countNode = measuringDevices.Count;   

            progress.Report($"({DateTime.Now}) Начало поиска.");

            for (int i = 0; i < countNode; i++)
            {
                progress.Report($"({DateTime.Now}) Поиск поверок по регистрационному номеру \"{measuringDevices[i].RegistrationNumber}\"");
                var attributeLine = $"&mit_number=*{measuringDevices[i].RegistrationNumber}*";
                // Обращение к API
                try
                {
                    await GetDataAsync(attributeLine);
                }
                catch (Exception ex)
                {
                    progress.Report($"({DateTime.Now}) Ошибка: {ex.Message}" + Environment.NewLine);
                    throw;
                }

                var info = $"({DateTime.Now}) Получаны данные ({i + 1}/{countNode})." + Environment.NewLine 
                    + $"\tРегистрационный номер: {measuringDevices[i].RegistrationNumber}" + Environment.NewLine;

                progress.Report($"{info}");
            }

            if (countNode == 0)
                progress.Report($"({DateTime.Now}) Входной список пуст.");

            progress.Report($"({DateTime.Now}) Конец поиска.");
        }

        public async Task SearchByFormAsync(SearchParameters searchParameters)
        {
            var attributeLine = $"&mit_number=*{searchParameters.RegistrationNumber}*";

            if (searchParameters.YearVerification != null)
                attributeLine += $"&year={searchParameters.YearVerification}";
  
            progress.Report($"({DateTime.Now}) Начало поиска.");
            progress.Report($"({DateTime.Now}) Поиск поверок по регистрационному номеру \"{searchParameters.RegistrationNumber}\"");

            try
            {
                await GetDataAsync(attributeLine);
            }
            catch (Exception ex)
            {
                progress.Report($"({DateTime.Now}) Ошибка: {ex.Message}" + Environment.NewLine);
                throw;
            }

            var info = $"({DateTime.Now}) Получаны данные" + Environment.NewLine;
            info += $"\tРегистрационный номер: {searchParameters.RegistrationNumber}" + Environment.NewLine
                + $"\tГод поверки: {(searchParameters.YearVerification == null ? null : searchParameters.YearVerification)}" + Environment.NewLine;

            progress.Report($"{info}");
            progress.Report($"({DateTime.Now}) Конец поиска.");
        }

        private async Task GetDataAsync(string attributeLine)
        {
            var signCipherInn = new Dictionary<string, string>();

            const int rows = 100;
            int index = 0, start = 0, numberPosition = 1;
            decimal numberRequests = 0;

            do
            {
                Thread.Sleep(2000);
                var relativeUri = $"fundmetrology/eapi/vri?start={start}&rows={rows}" + attributeLine; 

                using HttpResponseMessage response = await client.GetAsync(relativeUri);

                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                Model.VerificationResultModel.Root root = JsonConvert.DeserializeObject<Model.VerificationResultModel.Root>(resp);

                progress.Report($"({DateTime.Now}) Найдено {root.Result.Count} позиций");

                if (root.Result.Count == 0)
                    throw new Exception($"Нет результатов");

                if (start == 0)
                    numberRequests = Math.Ceiling((decimal)root.Result.Count / root.Result.Rows);

                foreach (var item in root.Result.Items)
                {
                    (MiInfo MiInfo, VriInfo VriInfo, List<MietaItem> MietaList) verificationData;

                    var resultDataModel = new ResultDataModel()
                    {
                        Mit_notation = item.Mit_notation,
                        Mit_number = item.Mit_number,
                        Mit_title = item.Mit_title,
                        Mi_modification = item.Mi_modification,
                        Mi_number = item.Mi_number,
                        Verification_date = item.Verification_date,
                        Org_title = item.Org_title
                    };

                    try
                    {
                        progress.Report($"({DateTime.Now}) Поиск шифра клейма и ИНН для изделия с Регистрационным и Заводским номером: ({resultDataModel.Mit_number}\\{resultDataModel.Mi_number})");

                        verificationData = await GetVerificationDataByVriId(item.Vri_id, numberPosition);
                        // шифра клейма
                        resultDataModel.signCipher = verificationData.VriInfo.signCipher;

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

                    if (verificationData.MietaList != null)
                    {
                        foreach (var mieta in verificationData.MietaList)
                        {
                            ResultDataModel addResultDataModel = (ResultDataModel)resultDataModel.Clone();

                            addResultDataModel.rankCode = mieta.rankCode;
                            addResultDataModel.regNumber = mieta.regNumber;
                            addResultDataModel.schemaTitle = mieta.schemaTitle;

                            resultDataModels.Add(addResultDataModel);
                        }
                    }

                    if (verificationData.MiInfo.etaMI != null)
                    {
                        resultDataModel.rankCode = verificationData.MiInfo.etaMI.rankCode;
                        resultDataModel.regNumber = verificationData.MiInfo.etaMI.regNumber;
                        resultDataModel.schemaTitle = verificationData.MiInfo.etaMI.schemaTitle;

                        resultDataModels.Add(resultDataModel);
                    }

                    if (verificationData.MietaList == null && verificationData.MiInfo.etaMI == null)
                    {
                        resultDataModels.Add(resultDataModel);
                    }
      
                    numberPosition++;
                }

                start += rows;
                ++index;

            } while (index < numberRequests);
        }

        private async Task<(MiInfo MiInfo, VriInfo VriInfo, List<MietaItem> MietaList)> GetVerificationDataByVriId(string vri_id, int numberPosition)
        {
            Thread.Sleep(2000);
            var relativeUri = $"fundmetrology/eapi/vri/{vri_id}";
            progress.Report($"({DateTime.Now}) Поиск шифра клейма позиции № {numberPosition}");

            using HttpResponseMessage httpResponseMessage = await client.GetAsync(relativeUri);
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Model.RegistryElementModel.Root root = JsonConvert.DeserializeObject<Model.RegistryElementModel.Root>(content);

                var signCipher = root.Result.vriInfo.signCipher;
                progress.Report($"({DateTime.Now}) Шифр клейма равен \"{signCipher}\"");

                return (root.Result.miInfo, root.Result.vriInfo, root.Result.means.mieta);
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
                progress.Report($"({DateTime.Now}) Шифру клейма \"{signCipher}\" соответствует ИНН: \"{inn}\"");

                return inn;
            }

            if (httpResponseMessage.Content != null)
                httpResponseMessage.Content.Dispose();

            throw new SimpleHttpResponseException(httpResponseMessage.StatusCode, content);
        }
    }
}
