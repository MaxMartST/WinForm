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
            button1 = new System.Windows.Forms.Button();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            splitter1 = new System.Windows.Forms.Splitter();
            button2 = new System.Windows.Forms.Button();
            saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            textBox1 = new System.Windows.Forms.TextBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            label1 = new System.Windows.Forms.Label();
            tabPage2 = new System.Windows.Forms.TabPage();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            label2 = new System.Windows.Forms.Label();
            registrationNumberBox = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            statePrimaryDenchmarkBox = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            dischargeBox = new System.Windows.Forms.TextBox();
            searchButton = new System.Windows.Forms.Button();
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
            // button1
            // 
            button1.Cursor = System.Windows.Forms.Cursors.Hand;
            button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            button1.Location = new System.Drawing.Point(3, 196);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(195, 40);
            button1.TabIndex = 0;
            button1.Text = "Загрузить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
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
            button2.Enabled = false;
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
            // textBox1
            // 
            textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox1.Location = new System.Drawing.Point(3, 19);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox1.Size = new System.Drawing.Size(549, 267);
            textBox1.TabIndex = 4;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox1);
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
            tabPage1.Controls.Add(button1);
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
            tabPage2.Controls.Add(searchButton);
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
            flowLayoutPanel1.Controls.Add(registrationNumberBox);
            flowLayoutPanel1.Controls.Add(label3);
            flowLayoutPanel1.Controls.Add(statePrimaryDenchmarkBox);
            flowLayoutPanel1.Controls.Add(label4);
            flowLayoutPanel1.Controls.Add(dischargeBox);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(195, 193);
            flowLayoutPanel1.TabIndex = 2;
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
            // registrationNumberBox
            // 
            registrationNumberBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            registrationNumberBox.Location = new System.Drawing.Point(3, 18);
            registrationNumberBox.Name = "registrationNumberBox";
            registrationNumberBox.Size = new System.Drawing.Size(145, 23);
            registrationNumberBox.TabIndex = 1;
            registrationNumberBox.TextChanged += registrationNumberBox_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(3, 44);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(26, 15);
            label3.TabIndex = 2;
            label3.Text = "ГЭТ";
            // 
            // statePrimaryDenchmarkBox
            // 
            statePrimaryDenchmarkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            statePrimaryDenchmarkBox.Location = new System.Drawing.Point(3, 62);
            statePrimaryDenchmarkBox.Name = "statePrimaryDenchmarkBox";
            statePrimaryDenchmarkBox.Size = new System.Drawing.Size(145, 23);
            statePrimaryDenchmarkBox.TabIndex = 3;
            statePrimaryDenchmarkBox.TextChanged += statePrimaryDenchmarkBox_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(3, 88);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(44, 15);
            label4.TabIndex = 4;
            label4.Text = "Разряд";
            // 
            // dischargeBox
            // 
            dischargeBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dischargeBox.Location = new System.Drawing.Point(3, 106);
            dischargeBox.Name = "dischargeBox";
            dischargeBox.Size = new System.Drawing.Size(145, 23);
            dischargeBox.TabIndex = 5;
            dischargeBox.TextChanged += dischargeBox_TextChanged;
            // 
            // searchButton
            // 
            searchButton.Cursor = System.Windows.Forms.Cursors.Hand;
            searchButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            searchButton.Enabled = false;
            searchButton.Location = new System.Drawing.Point(3, 196);
            searchButton.Name = "searchButton";
            searchButton.Size = new System.Drawing.Size(195, 40);
            searchButton.TabIndex = 1;
            searchButton.Text = "Поиск";
            searchButton.UseVisualStyleBackColor = true;
            searchButton.Click += searchButton_Click;
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

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox registrationNumberBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox statePrimaryDenchmarkBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox dischargeBox;
    }
}
