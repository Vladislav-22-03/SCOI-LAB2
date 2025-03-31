namespace СЦОИ_лаба_1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            button1 = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            histogramPictureBox = new PictureBox();
            curvePictureBox = new PictureBox();
            buttonResetCurve = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)histogramPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)curvePictureBox).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(10, 12);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(724, 663);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new Point(784, 12);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(236, 75);
            button1.TabIndex = 1;
            button1.Text = "📂 Открыть";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(784, 100);
            flowLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(236, 500);
            flowLayoutPanel1.TabIndex = 2;
            flowLayoutPanel1.WrapContents = false;
            // 
            // histogramPictureBox
            // 
            histogramPictureBox.BorderStyle = BorderStyle.FixedSingle;
            histogramPictureBox.Location = new Point(10, 704);
            histogramPictureBox.Margin = new Padding(3, 4, 3, 4);
            histogramPictureBox.Name = "histogramPictureBox";
            histogramPictureBox.Size = new Size(308, 262);
            histogramPictureBox.TabIndex = 3;
            histogramPictureBox.TabStop = false;
            // 
            // curvePictureBox
            // 
            curvePictureBox.BorderStyle = BorderStyle.FixedSingle;
            curvePictureBox.Location = new Point(324, 704);
            curvePictureBox.Margin = new Padding(3, 4, 3, 4);
            curvePictureBox.Name = "curvePictureBox";
            curvePictureBox.Size = new Size(272, 262);
            curvePictureBox.TabIndex = 4;
            curvePictureBox.TabStop = false;
            curvePictureBox.Paint += CurvePictureBox_Paint;
            curvePictureBox.MouseDown += CurvePictureBox_MouseDown;
            curvePictureBox.MouseMove += CurvePictureBox_MouseMove;
            curvePictureBox.MouseUp += CurvePictureBox_MouseUp;
            // 
            // buttonResetCurve
            // 
            buttonResetCurve.Location = new Point(703, 775);
            buttonResetCurve.Margin = new Padding(3, 4, 3, 4);
            buttonResetCurve.Name = "buttonResetCurve";
            buttonResetCurve.Size = new Size(120, 38);
            buttonResetCurve.TabIndex = 5;
            buttonResetCurve.Text = "Сбросить кривую";
            buttonResetCurve.UseVisualStyleBackColor = true;
            buttonResetCurve.Click += buttonResetCurve_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1032, 979);
            Controls.Add(buttonResetCurve);
            Controls.Add(curvePictureBox);
            Controls.Add(histogramPictureBox);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Редактор изображений";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)histogramPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)curvePictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox histogramPictureBox;
        private System.Windows.Forms.PictureBox curvePictureBox;
        private System.Windows.Forms.Button buttonResetCurve;
    }
}