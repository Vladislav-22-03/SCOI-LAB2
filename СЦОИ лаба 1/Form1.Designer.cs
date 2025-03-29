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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.histogramPictureBox = new System.Windows.Forms.PictureBox();
            this.curvePictureBox = new System.Windows.Forms.PictureBox();
            this.buttonResetCurve = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.histogramPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.curvePictureBox)).BeginInit();
            this.SuspendLayout();

            // pictureBox1
            this.pictureBox1.Location = new System.Drawing.Point(10, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 600);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;

            // button1
            this.button1.Location = new System.Drawing.Point(820, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(200, 60);
            this.button1.TabIndex = 1;
            this.button1.Text = "📂 Открыть";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);

            // flowLayoutPanel1
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(820, 80);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 400);
            this.flowLayoutPanel1.TabIndex = 2;
            this.flowLayoutPanel1.WrapContents = false;

            // histogramPictureBox
            this.histogramPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.histogramPictureBox.Location = new System.Drawing.Point(10, 620);
            this.histogramPictureBox.Name = "histogramPictureBox";
            this.histogramPictureBox.Size = new System.Drawing.Size(300, 150);
            this.histogramPictureBox.TabIndex = 3;
            this.histogramPictureBox.TabStop = false;

            // curvePictureBox
            this.curvePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.curvePictureBox.Location = new System.Drawing.Point(320, 620);
            this.curvePictureBox.Name = "curvePictureBox";
            this.curvePictureBox.Size = new System.Drawing.Size(300, 150);
            this.curvePictureBox.TabIndex = 4;
            this.curvePictureBox.TabStop = false;
            this.curvePictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.CurvePictureBox_Paint);
            this.curvePictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CurvePictureBox_MouseDown);
            this.curvePictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CurvePictureBox_MouseMove);
            this.curvePictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CurvePictureBox_MouseUp);

            // buttonResetCurve
            this.buttonResetCurve.Location = new System.Drawing.Point(630, 620);
            this.buttonResetCurve.Name = "buttonResetCurve";
            this.buttonResetCurve.Size = new System.Drawing.Size(120, 30);
            this.buttonResetCurve.TabIndex = 5;
            this.buttonResetCurve.Text = "Сбросить кривую";
            this.buttonResetCurve.UseVisualStyleBackColor = true;
            this.buttonResetCurve.Click += new System.EventHandler(this.buttonResetCurve_Click);

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 783);
            this.Controls.Add(this.buttonResetCurve);
            this.Controls.Add(this.curvePictureBox);
            this.Controls.Add(this.histogramPictureBox);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Редактор изображений";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.histogramPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.curvePictureBox)).EndInit();
            this.ResumeLayout(false);
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