using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WinForm.Model;

namespace WinForm
{
    public class SearchForVerifications
    {
        private readonly IProgress<string> progress;

        public SearchForVerifications(IProgress<string> progress)
        {
            this.progress = progress;
        }

        public async Task SearchAsync(List<MeasuringDevice> measuringDevices)
        {
            int countNode = measuringDevices.Count;
            progress.Report($"({DateTime.Now}) Начало поиска." + Environment.NewLine);

            for (int i = 0; i < countNode; i++)
            {
                // Обращение к API
                Thread.Sleep(2000);

                var info = $"({DateTime.Now}) Получаны данные ({i + 1}/{countNode})." + Environment.NewLine;

                info += $"Регестрационный номер: {measuringDevices[i].RegistrationNumber}" + Environment.NewLine
                    + $"ГЭТ: {measuringDevices[i].StatePrimaryDenchmark}" + Environment.NewLine
                    + $"Разряд: {measuringDevices[i].Discharge}" + Environment.NewLine;

                progress.Report($"{info}");
            }

            progress.Report($"({DateTime.Now}) Конец поиска." + Environment.NewLine);
        }

    }
}
