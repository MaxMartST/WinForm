using System;
using System.Collections.Generic;
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
        }

        private List<MeasuringDevice> measuringDevices;
        private List<ResultDataModel> resultDataModels;
        private List<ItemsItem> itemsItems;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void searchByParametersFromFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (button2.Enabled)
                    button2.Enabled = false;

                informationTextBox.Clear();
                resultDataModels.Clear();
                measuringDevices.Clear();

                ImportExcelFile();

                progressBar1.Show();

                var progress = new Progress<string>();
                progress.ProgressChanged += (s, message) =>
                {
                    if (!informationTextBox.IsDisposed)
                        informationTextBox.AppendText(message + Environment.NewLine);
                };

                var sv = new SearchForVerifications(progress);
                //resultDataModels = await Task.Run(() => sv.SearchByParametersFromFileAsync(measuringDevices));
                await Task.Run(() => sv.SearchByParametersFromFileAsync(measuringDevices));
                resultDataModels = sv.resultDataModels;

                progressBar1.Hide();

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

        private async void searchButtonByForm_Click(object sender, EventArgs e)
        {
            try
            {
                if (button2.Enabled)
                    button2.Enabled = false;

                informationTextBox.Clear();
                resultDataModels.Clear();

                var searchParameters = new SearchParameters
                {
                    RegistrationNumber = registrationNumberTextBox.Text,
                    YearVerification = YearVerificationTextBox.Text == "" ? null : Int32.Parse(YearVerificationTextBox.Text)
                };

                progressBar1.Show();

                var progress = new Progress<string>();
                progress.ProgressChanged += (s, message) =>
                {
                    if (!informationTextBox.IsDisposed)
                        informationTextBox.AppendText(message + Environment.NewLine);
                };

                var sv = new SearchForVerifications(progress);
                //resultDataModels = await Task.Run(() => sv.SearchByParametersFromFileAsync(measuringDevices));
                await Task.Run(() => sv.SearchByFormAsync(searchParameters));
                resultDataModels = sv.resultDataModels;

                progressBar1.Hide();

                if (resultDataModels.Count > 0)
                    button2.Enabled = true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Некорректно введен параметр \"Год поверки\"");
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
                informationTextBox.AppendText($"({DateTime.Now}) Экспорт файла начат." + Environment.NewLine);

                object Nothing = System.Reflection.Missing.Value;
                var app = new ExcelApp.Application();
                app.Visible = false;
                ExcelApp.Workbook workBook = app.Workbooks.Add(Nothing);
                ExcelApp.Worksheet worksheet = (ExcelApp.Worksheet)workBook.Sheets[1];
                worksheet.Name = "WorkSheet";

                int rows = resultDataModels.Count;

                worksheet.Cells[1, 1] = "Регистрационный номер типа";
                worksheet.Cells[1, 2] = "Наименование типа";
                worksheet.Cells[1, 3] = "Обозначение типа";
                worksheet.Cells[1, 4] = "Модицикация";
                worksheet.Cells[1, 5] = "Заводской серийный номер";
                worksheet.Cells[1, 6] = "Шифр клейма";
                worksheet.Cells[1, 7] = "Регистрационный номер СИ в Перечне";
                worksheet.Cells[1, 8] = "Код разряда эталона в ГПС, которому соответствует СИ";
                worksheet.Cells[1, 9] = "Наименование поверочной схемы или методики поверки";
                worksheet.Cells[1, 10] = "Дата поверки";
                worksheet.Cells[1, 11] = "Наименование организации-поверителя";
                worksheet.Cells[1, 12] = "ИНН";

                for (int a = 0, i = 2; a < rows; i++, a++)
                {
                    worksheet.Cells[i, 1] = resultDataModels[a].Mit_number;
                    worksheet.Cells[i, 2] = resultDataModels[a].Mit_title;
                    worksheet.Cells[i, 3] = resultDataModels[a].Mit_notation;
                    worksheet.Cells[i, 4] = resultDataModels[a].Mi_modification;
                    worksheet.Cells[i, 5] = resultDataModels[a].Mi_number;
                    worksheet.Cells[i, 6] = resultDataModels[a].signCipher;
                    worksheet.Cells[i, 7] = resultDataModels[a].regNumber;
                    worksheet.Cells[i, 8] = resultDataModels[a].rankCode;
                    worksheet.Cells[i, 9] = resultDataModels[a].schemaTitle;
                    worksheet.Cells[i, 10] = resultDataModels[a].Verification_date;
                    worksheet.Cells[i, 11] = resultDataModels[a].Org_title;
                    worksheet.Cells[i, 12] = resultDataModels[a].Inn;
                }

                worksheet.SaveAs(saveFileDialog1.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, ExcelApp.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing);
                workBook.Close(false, Type.Missing, Type.Missing);
                app.Quit();

                informationTextBox.AppendText($"({DateTime.Now}) Экспорт файла завершён." + Environment.NewLine);
            }
        }

        private void ImportExcelFile()
        {
            openFileDialog1.Title = "Import File";
            openFileDialog1.Filter = "Import Excel File|*.xlsx;*.xls";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                informationTextBox.AppendText($"({DateTime.Now}) Импорт файла начат." + Environment.NewLine);

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

                informationTextBox.AppendText($"({DateTime.Now}) Импорт файла завершён." + Environment.NewLine);
            }
        }

        void CheckInputBoxes()
        {
            if (registrationNumberTextBox.Text != "")
                searchButtonByForm.Enabled = true;
            else
                searchButtonByForm.Enabled = false;
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
