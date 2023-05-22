using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForm.Model;
using WinForm.Model.VerificationResultModel;
using ExcelApp = Microsoft.Office.Interop.Excel;

namespace WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            measuringDevices = new();
            resultDataModels = new();
            itemsItems = new();

            client = new HttpClient();
        }

        const string baseUri = "https://fgis.gost.ru";

        private List<MeasuringDevice> measuringDevices;
        private List<ResultDataModel> resultDataModels;
        private List<ItemsItem> itemsItems;

        static HttpClient? client;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(button2.Enabled)
                    button2.Enabled = false;

                textBox1.Clear();

                //itemsItems.Clear();
                resultDataModels.Clear();
                measuringDevices.Clear();

                ImportExcelFile();

                progressBar1.Show();

                var progress = new Progress<string>();
                progress.ProgressChanged += (s, message) =>
                {
                    if (!textBox1.IsDisposed)
                        textBox1.AppendText(message + Environment.NewLine);
                };

                var sv = new SearchForVerifications(progress);
                //itemsItems = await Task.Run(() => sv.SearchAsync(measuringDevices));
                resultDataModels = await Task.Run(() => sv.SearchAsync(measuringDevices));

                progressBar1.Hide();

                //if (itemsItems.Count > 0)
                //    button2.Enabled = true;

                if (resultDataModels.Count > 0)
                    button2.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ExportExcelFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (button2.Enabled)
                    button2.Enabled = false;

                textBox1.Clear();

                //itemsItems.Clear();
                resultDataModels.Clear();
                measuringDevices.Clear();

                measuringDevices.Add(new MeasuringDevice
                {
                    RegistrationNumber = registrationNumberBox.Text,
                    StatePrimaryDenchmark = statePrimaryDenchmarkBox.Text,
                    Discharge = dischargeBox.Text
                });

                progressBar1.Show();

                var progress = new Progress<string>();
                progress.ProgressChanged += (s, message) =>
                {
                    if (!textBox1.IsDisposed)
                        textBox1.AppendText(message + Environment.NewLine);
                };

                var sv = new SearchForVerifications(progress);
                //itemsItems = await Task.Run(() => sv.SearchAsync(measuringDevices));
                resultDataModels = await Task.Run(() => sv.SearchAsync(measuringDevices));

                progressBar1.Hide();

                //if (itemsItems.Count > 0)
                //    button2.Enabled = true;

                if (resultDataModels.Count > 0)
                    button2.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExportExcelFile()
        {
            saveFileDialog1.Filter = "Excel File|*.xlsx;*.xls";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.CreatePrompt = true;
            saveFileDialog1.Title = "Export Excel File";
            saveFileDialog1.FileName = $"Список поверок";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.AppendText($"({DateTime.Now}) Экспорт файла начат." + Environment.NewLine);

                object Nothing = System.Reflection.Missing.Value;
                var app = new ExcelApp.Application();
                app.Visible = false;
                ExcelApp.Workbook workBook = app.Workbooks.Add(Nothing);
                ExcelApp.Worksheet worksheet = (ExcelApp.Worksheet)workBook.Sheets[1];
                worksheet.Name = "WorkSheet";

                // Write data
                //int rows = itemsItems.Count;
                int rows = resultDataModels.Count;

                worksheet.Cells[1, 1] = "Регистрационный номер типа";
                worksheet.Cells[1, 2] = "Наименование типа";
                worksheet.Cells[1, 3] = "Обозначение типа";
                worksheet.Cells[1, 4] = "Модицикация";
                worksheet.Cells[1, 5] = "Заводской серийный номер";
                worksheet.Cells[1, 6] = "Шифр клейма";
                worksheet.Cells[1, 7] = "Наименование организации-поверителя";
                worksheet.Cells[1, 8] = "ИНН";

                for (int a = 0, i = 2; a < rows; i++, a++)
                {
                    worksheet.Cells[i, 1] = resultDataModels[a].Mit_number;
                    worksheet.Cells[i, 2] = resultDataModels[a].Mit_title;
                    worksheet.Cells[i, 3] = resultDataModels[a].Mit_notation;
                    worksheet.Cells[i, 4] = resultDataModels[a].Mi_modification;
                    worksheet.Cells[i, 5] = resultDataModels[a].Mi_number;
                    worksheet.Cells[i, 6] = resultDataModels[a].signCipher;
                    worksheet.Cells[i, 7] = resultDataModels[a].Org_title;
                    worksheet.Cells[i, 8] = resultDataModels[a].Inn;
                }

                worksheet.SaveAs(saveFileDialog1.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, ExcelApp.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing);
                workBook.Close(false, Type.Missing, Type.Missing);
                app.Quit();

                textBox1.AppendText($"({DateTime.Now}) Экспорт файла завершён." + Environment.NewLine);
            }
        }

        private void ImportExcelFile()
        {
            openFileDialog1.Title = "Import File";
            openFileDialog1.Filter = "Import Excel File|*.xlsx;*.xls";  

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.AppendText($"({DateTime.Now}) Импорт файла начат." + Environment.NewLine);

                //Create COM Objects.
                ExcelApp.Application excelApp = new ExcelApp.Application();
                ExcelApp.Workbook excelBook = excelApp.Workbooks.Open(openFileDialog1.FileName);
                ExcelApp._Worksheet excelSheet = excelBook.Sheets[1];
                ExcelApp.Range excelRange = excelSheet.UsedRange;

                int rows = excelRange.Rows.Count;
                int cols = excelRange.Columns.Count;

                for (int i = 2; i <= rows; i++)
                {
                    measuringDevices.Add(new MeasuringDevice()
                    {
                        RegistrationNumber = excelRange.Cells[i, 1].Value2.ToString(),
                        StatePrimaryDenchmark = excelRange.Cells[i, 2].Value2.ToString(),
                        Discharge = excelRange.Cells[i, 3].Value2.ToString()
                    });
                }

                //after reading, relaase the excel project
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                textBox1.AppendText($"({DateTime.Now}) Импорт файла завершён." + Environment.NewLine);
            }
        }

        private async Task<List<ItemsItem>> GetData()
        {
            try
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var relativeUri = "fundmetrology/eapi/vri";
                List<ItemsItem> itemsItems = new List<ItemsItem>();

                using (HttpResponseMessage response = await client.GetAsync(relativeUri))
                {
                    response.EnsureSuccessStatusCode();
                    var resp = await response.Content.ReadAsStringAsync();
                    Root root = JsonConvert.DeserializeObject<Root>(resp);
                    itemsItems = root.Result.Items;
                }

                return itemsItems;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task GetVerificationResults()
        {
            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                foreach (var device in measuringDevices)
                {
                    var relativeUri = $"fundmetrology/eapi/vri?mit_number=*{device.RegistrationNumber}*";

                    using (HttpResponseMessage response = await client.GetAsync(relativeUri))
                    {
                        response.EnsureSuccessStatusCode();
                        var resp = await response.Content.ReadAsStringAsync();
                        Root root = JsonConvert.DeserializeObject<Root>(resp);
                        itemsItems.AddRange(root.Result.Items);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        void CheckInputBoxes()
        {
            if (registrationNumberBox.Text != "")
                searchButton.Enabled = true;
            else
                searchButton.Enabled = false;
        }

        private void registrationNumberBox_TextChanged(object sender, EventArgs e)
        {
            CheckInputBoxes();
        }

        private void statePrimaryDenchmarkBox_TextChanged(object sender, EventArgs e)
        {
            CheckInputBoxes();
        }

        private void dischargeBox_TextChanged(object sender, EventArgs e)
        {
            CheckInputBoxes();
        }
    }
}
