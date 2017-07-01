namespace SlideClasterizationTest1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.buttonNextProcessedImage = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonAddImage = new System.Windows.Forms.Button();
            this.buttonPreviousProcessedImage = new System.Windows.Forms.Button();
            this.buttonPreviousImage = new System.Windows.Forms.Button();
            this.buttonNextImage = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(375, 282);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(394, 13);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(365, 282);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // buttonNextProcessedImage
            // 
            this.buttonNextProcessedImage.Location = new System.Drawing.Point(662, 317);
            this.buttonNextProcessedImage.Name = "buttonNextProcessedImage";
            this.buttonNextProcessedImage.Size = new System.Drawing.Size(97, 26);
            this.buttonNextProcessedImage.TabIndex = 2;
            this.buttonNextProcessedImage.Text = "->";
            this.buttonNextProcessedImage.UseVisualStyleBackColor = true;
            this.buttonNextProcessedImage.Click += new System.EventHandler(this.buttonNextProcessedImage_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // buttonAddImage
            // 
            this.buttonAddImage.Location = new System.Drawing.Point(13, 317);
            this.buttonAddImage.Name = "buttonAddImage";
            this.buttonAddImage.Size = new System.Drawing.Size(156, 44);
            this.buttonAddImage.TabIndex = 3;
            this.buttonAddImage.Text = "Add Image";
            this.buttonAddImage.UseVisualStyleBackColor = true;
            this.buttonAddImage.Click += new System.EventHandler(this.buttonAddImage_Click);
            // 
            // buttonPreviousProcessedImage
            // 
            this.buttonPreviousProcessedImage.Location = new System.Drawing.Point(559, 317);
            this.buttonPreviousProcessedImage.Name = "buttonPreviousProcessedImage";
            this.buttonPreviousProcessedImage.Size = new System.Drawing.Size(97, 26);
            this.buttonPreviousProcessedImage.TabIndex = 4;
            this.buttonPreviousProcessedImage.Text = "<-";
            this.buttonPreviousProcessedImage.UseVisualStyleBackColor = true;
            this.buttonPreviousProcessedImage.Click += new System.EventHandler(this.buttonPreviousProcessedImage_Click);
            // 
            // buttonPreviousImage
            // 
            this.buttonPreviousImage.Location = new System.Drawing.Point(188, 317);
            this.buttonPreviousImage.Name = "buttonPreviousImage";
            this.buttonPreviousImage.Size = new System.Drawing.Size(97, 26);
            this.buttonPreviousImage.TabIndex = 6;
            this.buttonPreviousImage.Text = "<-";
            this.buttonPreviousImage.UseVisualStyleBackColor = true;
            this.buttonPreviousImage.Click += new System.EventHandler(this.buttonPreviousImage_Click);
            // 
            // buttonNextImage
            // 
            this.buttonNextImage.Location = new System.Drawing.Point(291, 317);
            this.buttonNextImage.Name = "buttonNextImage";
            this.buttonNextImage.Size = new System.Drawing.Size(97, 26);
            this.buttonNextImage.TabIndex = 5;
            this.buttonNextImage.Text = "->";
            this.buttonNextImage.UseVisualStyleBackColor = true;
            this.buttonNextImage.Click += new System.EventHandler(this.buttonNextImage_Click);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(781, 13);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(438, 282);
            this.chart1.TabIndex = 7;
            this.chart1.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1242, 373);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.buttonPreviousImage);
            this.Controls.Add(this.buttonNextImage);
            this.Controls.Add(this.buttonPreviousProcessedImage);
            this.Controls.Add(this.buttonAddImage);
            this.Controls.Add(this.buttonNextProcessedImage);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button buttonNextProcessedImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonAddImage;
        private System.Windows.Forms.Button buttonPreviousProcessedImage;
        private System.Windows.Forms.Button buttonPreviousImage;
        private System.Windows.Forms.Button buttonNextImage;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}

