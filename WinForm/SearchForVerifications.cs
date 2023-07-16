using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinForm.ExceptionServise;
using WinForm.Model;
using WinForm.Model.RegistryElementModel;
using WinForm.Model.Base;

namespace WinForm
{
    public class SearchForVerifications
    {
        const string fgisGostUri = "https://fgis.gost.ru";
        const string pubFsaGovUri = "https://pub.fsa.gov.ru";
        public List<ResultDataModel> resultDataModels { get; set; }
        private readonly IProgress<string> progress;
        static HttpClient? client;
        static HttpClient? pubFsaGovClient;

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
            client.BaseAddress = new Uri(fgisGostUri);
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            pubFsaGovClient = new HttpClient();
            pubFsaGovClient.BaseAddress = new Uri(pubFsaGovUri);
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
                    await GetDataAsync(attributeLine, "");
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
            var attributeLine = $"&mit_number=*{searchParameters.RegistrationNumber}*&sort=org_title+asc";

            if (searchParameters.YearVerification != null)
                attributeLine += $"&year={searchParameters.YearVerification}";
  
            progress.Report($"({DateTime.Now}) Начало поиска.");
            progress.Report($"({DateTime.Now}) Поиск поверок по регистрационному номеру \"{searchParameters.RegistrationNumber}\"");

            try
            {
                await GetDataAsync(attributeLine, searchParameters.RankCode);
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

        private async Task GetDataAsync(string attributeLine, string? rankCode)
        {
            var signCipherInn = new Dictionary<string, CompanyData>();
            var resultDataModelList = new List<ResultDataModel>();

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
                Root root = JsonConvert.DeserializeObject<Root>(resp);

                progress.Report($"({DateTime.Now}) Найдено {root.Result.Count} позиций");

                if (root.Result.Count == 0)
                    throw new Exception($"Нет результатов");

                if (start == 0)
                    numberRequests = Math.Ceiling((decimal)root.Result.Count / root.Result.Rows);

                foreach (var item in root.Result.Items)
                {
                    // Проверка на дублирование
                    var elem = resultDataModels
                        .FirstOrDefault(x =>
                            x.Org_title == item.Org_title
                            && (x.Mi_modification == item.Mi_modification
                                || x.Mi_modification == "Нет модификации"
                                || x.Mi_modification == "-"
                                || string.IsNullOrEmpty(x.Mi_modification)));

                    if (elem != null)
                    {
                        //сравниваем по дате, берем самое свежее и заменяем на него
                        var elemDateTime = DateTime.Parse(elem.Verification_date);
                        var itemDateTime = DateTime.Parse(item.Verification_date);

                        if (elemDateTime <= itemDateTime)
                        {
                            resultDataModels.Remove(elem);
                            //resultDataModels.RemoveAll(x => 
                            //    x.Mi_modification == elem.Mi_modification 
                            //    && x.Org_title == elem.Org_title 
                            //    && x.Verification_date == elem.Verification_date);
                        }
                        else 
                        {
                            numberPosition++;
                            continue;
                        }
                    }

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
                            resultDataModel.CompanyData = signCipherInn[resultDataModel.signCipher];
                            progress.Report($"({DateTime.Now}) Шифр кейма \"{resultDataModel.signCipher}\" уже имеет ИНН, равный: \"{resultDataModel.CompanyData.Inn}\"");
                        }
                        else
                        {
                            var inn = await GetTaxpayerIdentificationNumber(resultDataModel.signCipher);

                            resultDataModel.CompanyData = await GetCompanyInfoAsync(inn);
                            signCipherInn.Add(resultDataModel.signCipher, resultDataModel.CompanyData);
                        }
                    }
                    catch (Exception ex)
                    {
                        progress.Report($"({DateTime.Now}) Ошибка: {ex.Message}");
                        continue;
                    }

                    if (verificationData.MietaList != null && !string.IsNullOrEmpty(rankCode))
                    {
                        foreach (var mieta in verificationData.MietaList)
                        {
                            if (mieta.rankCode != rankCode)
                                continue;

                            ResultDataModel addResultDataModel = (ResultDataModel)resultDataModel.Clone();

                            addResultDataModel.rankCode = mieta.rankCode;
                            addResultDataModel.regNumber = mieta.regNumber;
                            addResultDataModel.schemaTitle = mieta.schemaTitle;
                            addResultDataModel.Npenumber = await GetNpenumber(mieta.mietaURL, mieta.regNumber);

                            resultDataModels.Add(addResultDataModel);
                        }
                    }

                    if (verificationData.MiInfo.etaMI != null && !string.IsNullOrEmpty(rankCode))
                    {
                        resultDataModel.rankCode = verificationData.MiInfo.etaMI.rankCode;
                        resultDataModel.regNumber = verificationData.MiInfo.etaMI.regNumber;
                        resultDataModel.schemaTitle = verificationData.MiInfo.etaMI.schemaTitle;

                        if (verificationData.MiInfo.etaMI.rankCode == rankCode)
                            resultDataModels.Add(resultDataModel);
                    }

                    if (string.IsNullOrEmpty(rankCode))
                        resultDataModels.Add(resultDataModel);

                    numberPosition++;
                }

                start += rows;
                ++index;

            } while (index < numberRequests);
        }

        private async Task<string> GetNpenumber(string mietaURL, string regNumber)
        {
            Thread.Sleep(2000);
            progress.Report($"({DateTime.Now}) Поиск данных эталона по номеру: \"{regNumber}\"");

            var rmieta_id = mietaURL.Substring(mietaURL.LastIndexOf("/") + 1);
            var etalonUri = $"fundmetrology/cm/icdb/mieta/select?q=rmieta_id:{rmieta_id}&fl=number,mitype_num,mitype,minotation,modification,factory_num,year,schematype,schematitle,npenumber,npeUrl,rankcode,rankclass,applicability";

            using HttpResponseMessage httpResponseMessage = await client.GetAsync(etalonUri);
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Root root = JsonConvert.DeserializeObject<Root>(content);

                var npenumber = root.response.docs.First().npenumber;
                progress.Report($"({DateTime.Now}) Найден эталон: \"{npenumber}\"");

                return npenumber;
            }

            if (httpResponseMessage.Content != null)
                httpResponseMessage.Content.Dispose();

            throw new SimpleHttpResponseException(httpResponseMessage.StatusCode, content);
        }

        private async Task<(MiInfo MiInfo, VriInfo VriInfo, List<MietaItem> MietaList)> GetVerificationDataByVriId(string vri_id, int numberPosition)
        {
            Thread.Sleep(2000);
            progress.Report($"({DateTime.Now}) Поиск шифра клейма позиции № {numberPosition}");

            var relativeUri = $"fundmetrology/eapi/vri/{vri_id}";
            
            using HttpResponseMessage httpResponseMessage = await client.GetAsync(relativeUri);
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Root root = JsonConvert.DeserializeObject<Root>(content);

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
            progress.Report($"({DateTime.Now}) Поиск ИНН по шифру клейма \"{signCipher}\"");

            var relativeUri = $"fundmetrology/cm/xcdb/vcs/select?fq=sign:*{signCipher}*&q=*&fl=vcs_id,sign,act_no,act_date,title,inn,kpp,ogrn&sort=act_date+desc,sign+asc&rows=20&start=0";

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

        private async Task<CompanyData> GetCompanyInfoAsync(string inn)
        {
            // получить токен
            var jwt = await GetJwt();

            // проверить актуальность lwt
            if (!await CheckToken(jwt))
            {
                // получить токен повторно
                jwt = await GetJwt();
            }

            // Запрос POST с jw
            // Получить id
            var companiesId = await GetCompaniesId(jwt, inn);

            // Запрос GET
            // Получить CompanyData
            return await GetCompanyDataByCompaniesId(jwt, companiesId);
        }

        private async Task<string> GetJwt()
        {
            Thread.Sleep(2000);
            var request = new HttpRequestMessage(HttpMethod.Post, "/login");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var user = new User
            {
                username = "anonymous",
                password = "hrgesf7HDR67Bd"
            };

            var json = System.Text.Json.JsonSerializer.Serialize(user);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            request.Headers.Add("Authorization", "null");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 YaBrowser/23.5.1.714 Yowser/2.5 Safari/537.36");
            request.Headers.Add("Origin", "https://pub.fsa.gov.ru");
            request.Headers.Add("sec-ch-ua", "\"Chromium\";v=\"112\", \"YaBrowser\";v=\"23\", \"Not:A-Brand\";v=\"99\"");
            request.Headers.Add("sec-ch-ua-mobile", "?0");
            request.Headers.Add("Sec-Fetch-Dest", "empty");
            request.Headers.Add("Sec-Fetch-Mode", "cors");
            request.Headers.Add("Sec-Fetch-Site", "same-origin");

            var response = await pubFsaGovClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string authorization = response.Headers.GetValues("Authorization").FirstOrDefault();
            int index = authorization.IndexOf("Bearer") + "Bearer".Length;

            return authorization.Substring(index + 1);
        }

        private async Task<bool> CheckToken(string jwt)
        {
            Thread.Sleep(2000);
            var request = new HttpRequestMessage(HttpMethod.Get, $"token/is/actual/{jwt}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("Authorization", $"Bearer {jwt}");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 YaBrowser/23.5.1.714 Yowser/2.5 Safari/537.36");
            request.Headers.Add("Origin", "https://pub.fsa.gov.ru");
            request.Headers.Add("sec-ch-ua", "\"Chromium\";v=\"112\", \"YaBrowser\";v=\"23\", \"Not:A-Brand\";v=\"99\"");
            request.Headers.Add("sec-ch-ua-mobile", "?0");
            request.Headers.Add("Sec-Fetch-Dest", "empty");
            request.Headers.Add("Sec-Fetch-Mode", "cors");
            request.Headers.Add("Sec-Fetch-Site", "same-origin");

            var response = await pubFsaGovClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return content == "true" ? true : false;
        }

        private async Task<string> GetCompaniesId(string jwt, string inn)
        {
            Thread.Sleep(2000);
            progress.Report($"({DateTime.Now}) Поиск id компании");
            
            var requestModel = new Root
            {
                numberOfAllRecords = false,
                offset = 0,
                limit = 10,
                sort = new List<string> { "-id" },
                columns = new List<Model.RequestModel.ColumnsItem>()
            };
            requestModel.columns.Add(new Model.RequestModel.ColumnsItem { name = "applicantInn", search = $"{inn}", type = 0 });

            var content = System.Text.Json.JsonSerializer.Serialize(requestModel);
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/ral/common/showcases/get");

            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("Authorization", $"Bearer {jwt}");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 YaBrowser/23.5.1.714 Yowser/2.5 Safari/537.36");
            request.Headers.Add("Origin", "https://pub.fsa.gov.ru");
            request.Headers.Add("sec-ch-ua", "\"Chromium\";v=\"112\", \"YaBrowser\";v=\"23\", \"Not:A-Brand\";v=\"99\"");
            request.Headers.Add("sec-ch-ua-mobile", "?0");
            request.Headers.Add("Sec-Fetch-Dest", "empty");
            request.Headers.Add("Sec-Fetch-Mode", "cors");
            request.Headers.Add("Sec-Fetch-Site", "same-origin");

            var response = await pubFsaGovClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseContent);

            var item = json["items"][0];

            return item["id"].ToString();
        }

        private async Task<CompanyData> GetCompanyDataByCompaniesId(string jwt, string companiesId) 
        {
            Thread.Sleep(2000);
            progress.Report($"({DateTime.Now}) Поиск данных о компании");

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/ral/common/companies/{companiesId}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("Authorization", $"Bearer {jwt}");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 YaBrowser/23.5.1.714 Yowser/2.5 Safari/537.36");
            request.Headers.Add("Origin", "https://pub.fsa.gov.ru");
            request.Headers.Add("sec-ch-ua", "\"Chromium\";v=\"112\", \"YaBrowser\";v=\"23\", \"Not:A-Brand\";v=\"99\"");
            request.Headers.Add("sec-ch-ua-mobile", "?0");
            request.Headers.Add("Sec-Fetch-Dest", "empty");
            request.Headers.Add("Sec-Fetch-Mode", "cors");
            request.Headers.Add("Sec-Fetch-Site", "same-origin");

            var response = await pubFsaGovClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseContent);

            string phone = "" , mail = "", fax = "";
            var count = json["applicant"]["contacts"].Count();

            switch (count)
            { 
                case 1 :
                    var item = json["applicant"]["contacts"][0];
                    if (item["idType"].ToString().Equals("1")) phone = json["applicant"]["contacts"][0]["value"].ToString();
                    if (item["idType"].ToString().Equals("4")) mail = json["applicant"]["contacts"][0]["value"].ToString();
                    break;
                case 2 :
                    for (int i = 0; i < count; i++)
                    {
                        if (json["applicant"]["contacts"][i]["idType"].ToString().Equals("1")) phone = json["applicant"]["contacts"][i]["value"].ToString();
                        if (json["applicant"]["contacts"][i]["idType"].ToString().Equals("4")) mail = json["applicant"]["contacts"][i]["value"].ToString();
                    }
                    break;
                case 3 :
                    for (int i = 0; i < count; i++)
                    {
                        if (json["applicant"]["contacts"][i]["idType"].ToString().Equals("1")) phone = json["applicant"]["contacts"][i]["value"].ToString();
                        if (json["applicant"]["contacts"][i]["idType"].ToString().Equals("4")) mail = json["applicant"]["contacts"][i]["value"].ToString();
                        if (json["applicant"]["contacts"][i]["idType"].ToString().Equals("3")) fax = json["applicant"]["contacts"][i]["value"].ToString();
                    }
                    break;
            }

            return new CompanyData()
            { 
                Inn = json["applicant"]["inn"].ToString(),
                ShortNameCompany = json["applicant"]["shortName"].ToString(),
                FullNameCompany = json["applicant"]["fullName"].ToString(),
                FullAddres = json["applicant"]["addresses"][0]["fullAddress"].ToString(),
                Kpp = json["applicant"]["kpp"].ToString(),
                Ogrn = json["applicant"]["ogrn"].ToString(),
                Surname = json["applicant"]["person"]["surname"].ToString(),
                Name = json["applicant"]["person"]["name"].ToString(),
                Patronymic = json["applicant"]["person"]["patronymic"].ToString(),
                Phone = phone,
                Fax = fax,
                Mail = mail
            };
        }
    }
}
