using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
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
            client = new HttpClient();
        }

        public async Task<List<ItemsItem>> SearchAsync(List<MeasuringDevice> measuringDevices)
        {
            int countNode = measuringDevices.Count;
            List<ItemsItem> itemsItems = new List<ItemsItem>();

            progress.Report($"({DateTime.Now}) Начало поиска." + Environment.NewLine);

            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            for (int i = 0; i < countNode; i++)
            {
                // Обращение к API
                Thread.Sleep(2000);

                var item = await GetDataAsync(measuringDevices[i]);
                itemsItems.AddRange(item);

                var info = $"({DateTime.Now}) Получаны данные ({i + 1}/{countNode})." + Environment.NewLine;

                info += $"Регестрационный номер: {measuringDevices[i].RegistrationNumber}" + Environment.NewLine
                    + $"ГЭТ: {measuringDevices[i].StatePrimaryDenchmark}" + Environment.NewLine
                    + $"Разряд: {measuringDevices[i].Discharge}" + Environment.NewLine;

                progress.Report($"{info}");
            }

            progress.Report($"({DateTime.Now}) Конец поиска." + Environment.NewLine);

            return itemsItems;
        }

        private async Task<List<ItemsItem>> GetDataAsync(MeasuringDevice measuringDevice)
        {
            try
            {
                var relativeUri = $"fundmetrology/eapi/vri?mit_number=*{measuringDevice.RegistrationNumber}*";

                using (HttpResponseMessage response = await client.GetAsync(relativeUri))
                {
                    response.EnsureSuccessStatusCode();
                    var resp = await response.Content.ReadAsStringAsync();
                    Root root = JsonConvert.DeserializeObject<Root>(resp);

                    return root.Result.Items;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
