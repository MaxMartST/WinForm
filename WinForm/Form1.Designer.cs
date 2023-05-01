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
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(12, 15);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(123, 40);
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
            button2.Enabled = false;
            button2.Location = new System.Drawing.Point(12, 61);
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
            textBox1.Location = new System.Drawing.Point(141, 15);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox1.Size = new System.Drawing.Size(647, 326);
            textBox1.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackgroundImage = Properties.Resources._1581_n1810423_big;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(splitter1);
            Controls.Add(button1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Поиск поверов ФГИС АРШИН";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.TextBox textBox1;
    }
}
