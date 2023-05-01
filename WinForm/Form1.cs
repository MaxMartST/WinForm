using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForm.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ExcelApp = Microsoft.Office.Interop.Excel;

namespace WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            measuringDevices = new();
            client = new HttpClient();
        }

        private List<MeasuringDevice> measuringDevices;
        static HttpClient? client;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Clear();
                measuringDevices.Clear();

                ImportExcelFile();
                var data = await GetData();
                WriteListDataBox(data);

                if (measuringDevices.Count > 0)
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
                object Nothing = System.Reflection.Missing.Value;
                var app = new ExcelApp.Application();
                app.Visible = false;
                ExcelApp.Workbook workBook = app.Workbooks.Add(Nothing);
                ExcelApp.Worksheet worksheet = (ExcelApp.Worksheet)workBook.Sheets[1];
                worksheet.Name = "WorkSheet";

                // Write data
                int count = measuringDevices.Count;

                worksheet.Cells[1, 1] = "Рег. Номер СИ";
                worksheet.Cells[1, 2] = "Разряд СИ";
                worksheet.Cells[1, 3] = "Модицикация СИ";

                for (int a = 0, i = 2; a < count; i++, a++)
                {
                    worksheet.Cells[i, 1] = measuringDevices[a].RegistrationNumber;
                    worksheet.Cells[i, 2] = measuringDevices[a].Discharge;
                    worksheet.Cells[i, 3] = measuringDevices[a].Modification;
                }

                worksheet.SaveAs(saveFileDialog1.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, ExcelApp.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing);
                workBook.Close(false, Type.Missing, Type.Missing);
                app.Quit();
            }
        }

        private void ImportExcelFile()
        {
            openFileDialog1.Title = "Import File";
            openFileDialog1.Filter = "Import Excel File|*.xlsx;*.xls";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
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
                        Discharge = excelRange.Cells[i, 2].Value2.ToString(),
                        Modification = excelRange.Cells[i, 3].Value2.ToString()
                    });
                }

                //after reading, relaase the excel project
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        private void WriteListDataBox(List<ItemsItem> data)
        {
            foreach (ItemsItem item in data)
            {
                textBox1.Text += $"({DateTime.Now})" + Environment.NewLine;
                textBox1.Text += $"Регистрационный номер типа СИ: {item.Mit_number}" + Environment.NewLine;
                textBox1.Text += $"Наименование типа СИ: {item.Mit_title}" + Environment.NewLine;
                textBox1.Text += $"Обозначение типа СИ: {item.Mit_notation}" + Environment.NewLine;
                textBox1.Text += $"Модификация СИ: {item.Mi_modification}" + Environment.NewLine;
                textBox1.Text += $"Наименование организации-поверителя: {item.Org_title}" + Environment.NewLine;
                textBox1.Text += Environment.NewLine;

                //listBox1.Items.Insert(i, item: conrext);
            }
        }

        private async Task<List<ItemsItem>> GetData()
        {
            try
            {
                const string baseUri = "https://fgis.gost.ru";

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
            catch (Exception ex)
            {
                throw;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
