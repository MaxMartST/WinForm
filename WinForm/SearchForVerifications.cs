﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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

            if (countNode == 0)
            {
                progress.Report($"({DateTime.Now}) Список пуст." + Environment.NewLine);

                return new List<ItemsItem>();
            }
                

            List<ItemsItem> itemsItems = new List<ItemsItem>();

            progress.Report($"({DateTime.Now}) Начало поиска.");

            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            for (int i = 0; i < countNode; i++)
            {
                // Обращение к API
                //Thread.Sleep(2000);

                try
                {
                    var item = await GetDataAsync(measuringDevices[i]);
                    itemsItems.AddRange(item);
                }
                catch (Exception ex)
                {
                    progress.Report($"({DateTime.Now}) Ошибка: {ex.Message}" + Environment.NewLine);
                    throw;
                }

                var info = $"({DateTime.Now}) Получаны данные ({i + 1}/{countNode})." + Environment.NewLine;

                info += $"\tРегестрационный номер: {measuringDevices[i].RegistrationNumber}" + Environment.NewLine
                    + $"\tГЭТ: {measuringDevices[i].StatePrimaryDenchmark}" + Environment.NewLine
                    + $"\tРазряд: {measuringDevices[i].Discharge}";

                progress.Report($"{info}");
            }

            progress.Report($"({DateTime.Now}) Конец поиска.");

            return itemsItems;
        }

        private async Task<List<ItemsItem>> GetDataAsync(MeasuringDevice measuringDevice)
        {
            List<ItemsItem> items = new List<ItemsItem>();
            const int rows = 100;
            int index = 0, start = 0;
            decimal numberRequests = 0;

            do
            {
                Thread.Sleep(2000);
                var relativeUri = $"fundmetrology/eapi/vri?start={start}&rows={rows}&mit_number=*{measuringDevice.RegistrationNumber}*";
                
                using HttpResponseMessage response = await client.GetAsync(relativeUri);

                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                Root root = JsonConvert.DeserializeObject<Root>(resp);

                if (root.Result.Count == 0)
                    throw new Exception($"Нет результатов для: {measuringDevice.RegistrationNumber}");

                if (start == 0)
                    numberRequests = Math.Ceiling((decimal)root.Result.Count / root.Result.Rows);

                items.AddRange(root.Result.Items);

                start += rows;
                index++;

            } while (index < numberRequests);

            return items;
        }
    }
}
