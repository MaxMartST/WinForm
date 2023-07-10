namespace WinForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            searchByParametersFromFileButton = new System.Windows.Forms.Button();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            splitter1 = new System.Windows.Forms.Splitter();
            button2 = new System.Windows.Forms.Button();
            saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            informationTextBox = new System.Windows.Forms.TextBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            label1 = new System.Windows.Forms.Label();
            tabPage2 = new System.Windows.Forms.TabPage();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            label2 = new System.Windows.Forms.Label();
            registrationNumberTextBox = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            YearVerificationTextBox = new System.Windows.Forms.TextBox();
            radioButton1 = new System.Windows.Forms.RadioButton();
            radioButton2 = new System.Windows.Forms.RadioButton();
            label3 = new System.Windows.Forms.Label();
            comboBox1 = new System.Windows.Forms.ComboBox();
            searchButtonByForm = new System.Windows.Forms.Button();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // searchByParametersFromFileButton
            // 
            searchByParametersFromFileButton.Cursor = System.Windows.Forms.Cursors.Hand;
            searchByParametersFromFileButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            searchByParametersFromFileButton.Location = new System.Drawing.Point(3, 196);
            searchByParametersFromFileButton.Name = "searchByParametersFromFileButton";
            searchByParametersFromFileButton.Size = new System.Drawing.Size(195, 40);
            searchByParametersFromFileButton.TabIndex = 0;
            searchByParametersFromFileButton.Text = "Загрузить";
            searchByParametersFromFileButton.UseVisualStyleBackColor = true;
            searchByParametersFromFileButton.Click += searchByParametersFromFileButton_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // splitter1
            // 
            splitter1.Location = new System.Drawing.Point(0, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(3, 450);
            splitter1.TabIndex = 2;
            splitter1.TabStop = false;
            // 
            // button2
            // 
            button2.Cursor = System.Windows.Forms.Cursors.Hand;
            button2.Location = new System.Drawing.Point(665, 398);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(123, 40);
            button2.TabIndex = 3;
            button2.Text = "Сохранить";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // fileSystemWatcher1
            // 
            fileSystemWatcher1.EnableRaisingEvents = true;
            fileSystemWatcher1.SynchronizingObject = this;
            // 
            // informationTextBox
            // 
            informationTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            informationTextBox.Location = new System.Drawing.Point(3, 19);
            informationTextBox.Multiline = true;
            informationTextBox.Name = "informationTextBox";
            informationTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            informationTextBox.Size = new System.Drawing.Size(549, 267);
            informationTextBox.TabIndex = 4;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(informationTextBox);
            groupBox1.Location = new System.Drawing.Point(233, 15);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(555, 289);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Состояние";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tabControl1);
            groupBox2.Location = new System.Drawing.Point(12, 15);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(215, 289);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = "Метод поиска";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(3, 19);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(209, 267);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(searchByParametersFromFileButton);
            tabPage1.Location = new System.Drawing.Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(201, 239);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Файл";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Location = new System.Drawing.Point(3, 3);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(195, 42);
            label1.TabIndex = 1;
            label1.Text = "Выберите и загрузите файл типа excel \r\n";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(flowLayoutPanel1);
            tabPage2.Controls.Add(searchButtonByForm);
            tabPage2.Location = new System.Drawing.Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(201, 239);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Параметры";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Controls.Add(registrationNumberTextBox);
            flowLayoutPanel1.Controls.Add(label5);
            flowLayoutPanel1.Controls.Add(YearVerificationTextBox);
            flowLayoutPanel1.Controls.Add(radioButton1);
            flowLayoutPanel1.Controls.Add(radioButton2);
            flowLayoutPanel1.Controls.Add(label3);
            flowLayoutPanel1.Controls.Add(comboBox1);
            flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(195, 193);
            flowLayoutPanel1.TabIndex = 2;
            flowLayoutPanel1.Paint += flowLayoutPanel1_Paint;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(145, 15);
            label2.TabIndex = 0;
            label2.Text = "Регестрационный номер";
            // 
            // registrationNumberTextBox
            // 
            registrationNumberTextBox.Location = new System.Drawing.Point(3, 18);
            registrationNumberTextBox.Name = "registrationNumberTextBox";
            registrationNumberTextBox.Size = new System.Drawing.Size(190, 23);
            registrationNumberTextBox.TabIndex = 1;
            registrationNumberTextBox.TextChanged += registrationNumberBox_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(3, 44);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(75, 15);
            label5.TabIndex = 7;
            label5.Text = "Год поверки";
            // 
            // YearVerificationTextBox
            // 
            YearVerificationTextBox.Location = new System.Drawing.Point(3, 62);
            YearVerificationTextBox.Name = "YearVerificationTextBox";
            YearVerificationTextBox.Size = new System.Drawing.Size(190, 23);
            YearVerificationTextBox.TabIndex = 51;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Checked = true;
            radioButton1.Location = new System.Drawing.Point(3, 91);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new System.Drawing.Size(42, 19);
            radioButton1.TabIndex = 51;
            radioButton1.TabStop = true;
            radioButton1.Text = "СИ";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new System.Drawing.Point(3, 116);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new System.Drawing.Size(64, 19);
            radioButton2.TabIndex = 53;
            radioButton2.Text = "Эталон";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Enabled = false;
            label3.Location = new System.Drawing.Point(3, 138);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(91, 15);
            label3.TabIndex = 52;
            label3.Text = "Разряд эталона";
            label3.Click += label3_Click;
            // 
            // comboBox1
            // 
            comboBox1.Enabled = false;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "0Р", "1Р", "2Р", "3Р", "4Р", "5Р", "РЭ", "СИ", "ВЭ" });
            comboBox1.Location = new System.Drawing.Point(3, 156);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(190, 23);
            comboBox1.TabIndex = 3;
            comboBox1.TabStop = false;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // searchButtonByForm
            // 
            searchButtonByForm.Cursor = System.Windows.Forms.Cursors.Hand;
            searchButtonByForm.Dock = System.Windows.Forms.DockStyle.Bottom;
            searchButtonByForm.Enabled = false;
            searchButtonByForm.Location = new System.Drawing.Point(3, 196);
            searchButtonByForm.Name = "searchButtonByForm";
            searchButtonByForm.Size = new System.Drawing.Size(195, 40);
            searchButtonByForm.TabIndex = 1;
            searchButtonByForm.Text = "Поиск";
            searchButtonByForm.UseVisualStyleBackColor = true;
            searchButtonByForm.Click += searchButtonByForm_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new System.Drawing.Point(12, 409);
            progressBar1.MarqueeAnimationSpeed = 30;
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(647, 19);
            progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            progressBar1.TabIndex = 50;
            progressBar1.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackgroundImage = Properties.Resources._1581_n1810423_big;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(progressBar1);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(button2);
            Controls.Add(splitter1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Поиск поверов ФГИС АРШИН";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button searchByParametersFromFileButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.TextBox informationTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button searchButtonByForm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox registrationNumberTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox YearVerificationTextBox;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
    }
}
