﻿using System;
using System.Collections.Generic;
using System.Reflection.Emit;
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
            rankCode = string.Empty;
        }

        private List<MeasuringDevice> measuringDevices;
        private List<ResultDataModel> resultDataModels;
        private List<ItemsItem> itemsItems;
        private string rankCode;

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
                progressBar1.Show();

                var searchParameters = new SearchParameters(
                    registrationNumberTextBox.Text,
                    YearVerificationTextBox.Text == "" ? null : Int32.Parse(YearVerificationTextBox.Text),
                    rankCode);

                var progress = new Progress<string>();
                progress.ProgressChanged += (s, message) =>
                {
                    if (!informationTextBox.IsDisposed)
                        informationTextBox.AppendText(message + Environment.NewLine);
                };

                var sv = new SearchForVerifications(progress);
                await Task.Run(() => sv.SearchByFormAsync(searchParameters));
                resultDataModels = sv.resultDataModels;

                progressBar1.Hide();

                button2.Enabled = resultDataModels.Count > 0;
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
                worksheet.Cells[1, 2] = "ГЭТ";
                worksheet.Cells[1, 3] = "Наименование типа";
                worksheet.Cells[1, 4] = "Обозначение типа";
                worksheet.Cells[1, 5] = "Модицикация";
                worksheet.Cells[1, 6] = "Заводской серийный номер";
                worksheet.Cells[1, 7] = "Регистрационный номер СИ в Перечне";
                worksheet.Cells[1, 8] = "Код разряда эталона в ГПС, которому соответствует СИ";
                worksheet.Cells[1, 9] = "Наименование поверочной схемы или методики поверки";
                worksheet.Cells[1, 10] = "Дата поверки";
                worksheet.Cells[1, 11] = "Шифр клейма";
                worksheet.Cells[1, 12] = "ИНН";
                worksheet.Cells[1, 13] = "Короткое наименование организации-поверителя";
                worksheet.Cells[1, 14] = "Полное наименование организации-поверителя";
                worksheet.Cells[1, 15] = "КПП юридического лица";
                worksheet.Cells[1, 16] = "ОГРН юридического лица";
                worksheet.Cells[1, 17] = "Адрес места нахождения юридического лица";
                worksheet.Cells[1, 18] = "ФИО руководителя юридического лица";
                worksheet.Cells[1, 19] = "Номер телефона юридического лица";
                worksheet.Cells[1, 20] = "Номер факса юридического лица";
                worksheet.Cells[1, 21] = "Адрес электронной почты юридического лица";

                for (int a = 0, i = 2; a < rows; i++, a++)
                {
                    worksheet.Cells[i, 1] = resultDataModels[a].Mit_number;
                    worksheet.Cells[i, 2] = resultDataModels[a].Npenumber;
                    worksheet.Cells[i, 3] = resultDataModels[a].Mit_title;
                    worksheet.Cells[i, 4] = resultDataModels[a].Mit_notation;
                    worksheet.Cells[i, 5] = resultDataModels[a].Modification;
                    worksheet.Cells[i, 6] = resultDataModels[a].Mi_number;
                    worksheet.Cells[i, 7] = resultDataModels[a].regNumber;
                    worksheet.Cells[i, 8] = resultDataModels[a].rankCode;
                    worksheet.Cells[i, 9] = resultDataModels[a].schemaTitle;
                    worksheet.Cells[i, 10] = resultDataModels[a].Verification_date;
                    worksheet.Cells[i, 11] = resultDataModels[a].signCipher;
                    worksheet.Cells[i, 12] = resultDataModels[a].CompanyData.Inn;
                    worksheet.Cells[i, 13] = resultDataModels[a].CompanyData.ShortNameCompany;
                    worksheet.Cells[i, 14] = resultDataModels[a].CompanyData.FullNameCompany;
                    worksheet.Cells[i, 15] = resultDataModels[a].CompanyData.Kpp;
                    worksheet.Cells[i, 16] = resultDataModels[a].CompanyData.Ogrn;
                    worksheet.Cells[i, 17] = resultDataModels[a].CompanyData.FullAddres;
                    worksheet.Cells[i, 18] = resultDataModels[a].CompanyData.GetNamePersone();
                    worksheet.Cells[i, 19] = resultDataModels[a].CompanyData.Phone;
                    worksheet.Cells[i, 20] = resultDataModels[a].CompanyData.Fax;
                    worksheet.Cells[i, 21] = resultDataModels[a].CompanyData.Mail;
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
                        //StatePrimaryDenchmark = excelRange.Cells[i, 2].Value2.ToString(),
                        //Discharge = excelRange.Cells[i, 3].Value2.ToString()
                    });
                }

                //after reading, relaase the excel project
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                informationTextBox.AppendText($"({DateTime.Now}) Импорт файла завершён." + Environment.NewLine);
            }
        }

        private void CheckInputBoxes()
        {
            searchButtonByForm.Enabled = !string.IsNullOrEmpty(registrationNumberTextBox.Text); //registrationNumberTextBox.Text != "";

            if (!string.IsNullOrEmpty(registrationNumberTextBox.Text) && comboBox1.Enabled)
                searchButtonByForm.Enabled = !string.IsNullOrEmpty(rankCode);
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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                label3.Enabled = false;
                comboBox1.Enabled = false;
                rankCode = string.Empty;

                informationTextBox.AppendText($"({DateTime.Now}) Был выбран: \"{radioButton.Text}\"" + Environment.NewLine);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                label3.Enabled = true;
                comboBox1.Enabled = true;
                rankCode = comboBox1.SelectedItem == null ? string.Empty : comboBox1.SelectedItem.ToString();

                informationTextBox.AppendText($"({DateTime.Now}) Был выбран: \"{radioButton.Text}\"" + Environment.NewLine);
            }

            CheckInputBoxes();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            rankCode = comboBox1.SelectedItem.ToString();
            CheckInputBoxes();
        }
    }
}
