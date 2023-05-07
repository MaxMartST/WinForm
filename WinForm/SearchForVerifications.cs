using System;
using System.Collections.Generic;
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
            progress.Report($"({DateTime.Now}) Начало поиска." + Environment.NewLine);
    
            foreach (var device in measuringDevices)
            {
                // Обращение к API
                Thread.Sleep(2000);

                var info = $"({DateTime.Now}) Получаны данные." + Environment.NewLine;

                info += $"Регестрационный номер: {device.RegistrationNumber}" + Environment.NewLine
                    + $"ГЭТ: {device.StatePrimaryDenchmark}" + Environment.NewLine
                    + $"Разряд: {device.Discharge}" + Environment.NewLine;

                progress.Report($"{info}");
            }

            progress.Report($"({DateTime.Now}) Конец поиска." + Environment.NewLine);
        }

    }
}
