using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Linq;

namespace СЦОИ_лаба_1
{
    public partial class Form1 : Form
    {
        private class Layer
        {
            public Bitmap OriginalImage { get; set; }
            public Bitmap Image { get; set; }
            public float Opacity { get; set; } = 1.0f;
            public string BlendMode { get; set; } = "Нет";
            public Color? RemovedColor { get; set; } = null; // Храним какой цвет был удален
        }

        private List<Layer> layers = new List<Layer>();
        private Bitmap canvas;
        private Graphics g;
        private int h, w;

        private Point[] curvePoints = new Point[256];
        private Point[] controlPoints = new Point[5]; // 5 контрольных точек
        private int selectedPointIndex = -1;
        private bool isDragging = false;

        public Form1()
        {
            InitializeComponent();
            h = pictureBox1.Height;
            w = pictureBox1.Width;
            canvas = new Bitmap(w, h);
            g = Graphics.FromImage(canvas);
            pictureBox1.Image = canvas;

            InitializeCurve();
            UpdateHistogramAndCurve();
        }


        private void InitializeCurve()
        {
            // Явная инициализация всех 5 точек
            controlPoints = new Point[5];
            controlPoints[0] = new Point(0, 255);    // Левая верхняя (X фиксирован)
            controlPoints[1] = new Point(63, 191);   // Изменил 64 на 63 для лучшего отображения
            controlPoints[2] = new Point(127, 127);  // Центр (изменил 128 на 127)
            controlPoints[3] = new Point(191, 63);   // Изменил 64 на 63
            controlPoints[4] = new Point(255, 0);    // Правая нижняя (X фиксирован)

            UpdateCurveInterpolation();
        }
        private void UpdateCurveInterpolation()
        {
            // Сортируем точки по X для корректной интерполяции
            var sortedPoints = controlPoints.OrderBy(p => p.X).ToArray();

            List<Point> fullCurve = new List<Point>();

            for (int i = 0; i < sortedPoints.Length - 1; i++)
            {
                Point start = sortedPoints[i];
                Point end = sortedPoints[i + 1];

                if (start.X >= end.X) continue; // Защита от некорректных интервалов

                int steps = end.X - start.X;

                for (int x = start.X; x <= end.X; x++)
                {
                    float t = (float)(x - start.X) / steps;
                    int y = (int)(start.Y + (end.Y - start.Y) * t);
                    fullCurve.Add(new Point(x, Math.Max(0, Math.Min(255, y))));
                }
            }

            curvePoints = fullCurve.ToArray();
        }
        private void UpdateHistogramAndCurve()
        {
            if (layers.Count == 0) return;

            UpdateHistogram();
            ApplyCurveToLayers();
            ApplyLayers();
            curvePictureBox.Invalidate();
        }

        private void UpdateHistogram()
        {
            if (layers.Count == 0) return;

            Bitmap currentImage = layers[layers.Count - 1].Image;
            int[] redHistogram = new int[256];
            int[] greenHistogram = new int[256];
            int[] blueHistogram = new int[256];

            for (int y = 0; y < currentImage.Height; y++)
            {
                for (int x = 0; x < currentImage.Width; x++)
                {
                    Color pixel = currentImage.GetPixel(x, y);
                    redHistogram[pixel.R]++;
                    greenHistogram[pixel.G]++;
                    blueHistogram[pixel.B]++;
                }
            }

            int maxRed = redHistogram.Max();
            int maxGreen = greenHistogram.Max();
            int maxBlue = blueHistogram.Max();
            int maxOverall = Math.Max(maxRed, Math.Max(maxGreen, maxBlue));

            Bitmap histogramImage = new Bitmap(histogramPictureBox.Width, histogramPictureBox.Height);
            using (Graphics g = Graphics.FromImage(histogramImage))
            {
                g.Clear(Color.White);
                Pen redPen = new Pen(Color.Red, 2);
                Pen greenPen = new Pen(Color.Green, 2);
                Pen bluePen = new Pen(Color.Blue, 2);

                float scaleX = histogramImage.Width / 256f;
                float scaleY = histogramImage.Height / (float)maxOverall;

                for (int i = 0; i < 255; i++)
                {
                    g.DrawLine(redPen, i * scaleX, histogramImage.Height, i * scaleX, histogramImage.Height - redHistogram[i] * scaleY);
                    g.DrawLine(greenPen, i * scaleX, histogramImage.Height, i * scaleX, histogramImage.Height - greenHistogram[i] * scaleY);
                    g.DrawLine(bluePen, i * scaleX, histogramImage.Height, i * scaleX, histogramImage.Height - blueHistogram[i] * scaleY);
                }
            }

            histogramPictureBox.Image = histogramImage;
        }



        private void ApplyCurveToLayers()
        {
            foreach (var layer in layers)
            {
                Bitmap source = new Bitmap(layer.Image); // Работаем с текущим изображением
                Bitmap adjusted = new Bitmap(source.Width, source.Height);

                for (int y = 0; y < source.Height; y++)
                {
                    for (int x = 0; x < source.Width; x++)
                    {
                        Color pixel = source.GetPixel(x, y);
                        int r = 255 - curvePoints[pixel.R].Y;
                        int g = 255 - curvePoints[pixel.G].Y;
                        int b = 255 - curvePoints[pixel.B].Y;

                        // Применяем удаление цвета, если оно было
                        if (layer.RemovedColor.HasValue)
                        {
                            if (layer.RemovedColor.Value == Color.Red) r = 0;
                            else if (layer.RemovedColor.Value == Color.Green) g = 0;
                            else if (layer.RemovedColor.Value == Color.Blue) b = 0;
                        }

                        r = Math.Max(0, Math.Min(255, r));
                        g = Math.Max(0, Math.Min(255, g));
                        b = Math.Max(0, Math.Min(255, b));

                        adjusted.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                    }
                }

                layer.Image = adjusted;
            }
        }

        private void CurvePictureBox_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.Clear(Color.White);

                // Рисуем сетку
                using (Pen gridPen = new Pen(Color.LightGray))
                {
                    for (int i = 0; i <= 255; i += 32)
                    {
                        e.Graphics.DrawLine(gridPen, i, 0, i, 255);
                        e.Graphics.DrawLine(gridPen, 0, i, 255, i);
                    }
                }

                // Рисуем оси
                e.Graphics.DrawLine(Pens.Black, 0, 255, 255, 255); // X ось
                e.Graphics.DrawLine(Pens.Black, 0, 255, 0, 0);     // Y ось

                // Рисуем кривую
                if (curvePoints.Length > 1)
                {
                    e.Graphics.DrawLines(Pens.Blue, curvePoints);
                }

                // Рисуем ВСЕ 5 контрольных точек с увеличенным размером
                using (Brush pointBrush = new SolidBrush(Color.Red))
                {
                    // Проверяем, что controlPoints инициализирован и содержит 5 точек
                    if (controlPoints != null && controlPoints.Length == 5)
                    {
                        foreach (var point in controlPoints)
                        {
                            // Увеличиваем размер точек и делаем их более заметными
                            int pointSize = 10;
                            int x = Math.Max(pointSize / 2, Math.Min(255 - pointSize / 2, point.X));
                            int y = Math.Max(pointSize / 2, Math.Min(255 - pointSize / 2, point.Y));

                            e.Graphics.FillEllipse(pointBrush, x - pointSize / 2, y - pointSize / 2, pointSize, pointSize);
                            e.Graphics.DrawEllipse(Pens.Black, x - pointSize / 2, y - pointSize / 2, pointSize, pointSize);
                        }
                    }
                    else
                    {
                        // Если что-то не так, переинициализируем точки
                        InitializeCurve();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отрисовки: {ex.Message}");
                InitializeCurve();
            }
        }


        private void CurvePictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            int minDistance = 10;
            selectedPointIndex = -1;

            // Проверяем все контрольные точки
            for (int i = 0; i < controlPoints.Length; i++)
            {
                int distance = (int)Math.Sqrt(
                    Math.Pow(e.X - controlPoints[i].X, 2) +
                    Math.Pow(e.Y - controlPoints[i].Y, 2));

                if (distance < minDistance)
                {
                    selectedPointIndex = i;
                    isDragging = true;
                    break;
                }
            }
        }


        private void CurvePictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedPointIndex >= 0 && selectedPointIndex < controlPoints.Length)
            {
                // Ограничиваем координаты
                int newX = Math.Max(0, Math.Min(255, e.X));
                int newY = Math.Max(0, Math.Min(255, e.Y));

                // Для крайних точек фиксируем X (0 для первой, 255 для последней)
                if (selectedPointIndex == 0)
                {
                    newX = 0;
                }
                else if (selectedPointIndex == controlPoints.Length - 1)
                {
                    newX = 255;
                }
                else // Для средних точек ограничиваем X соседями
                {
                    newX = Math.Max(controlPoints[selectedPointIndex - 1].X + 1,
                                  Math.Min(controlPoints[selectedPointIndex + 1].X - 1, newX));
                }

                // Обновляем позицию точки
                controlPoints[selectedPointIndex] = new Point(newX, newY);

                try
                {
                    UpdateCurveInterpolation();
                    curvePictureBox.Invalidate();
                    UpdateHistogramAndCurve();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обновлении: {ex.Message}");
                }
            }
        }


        private void InterpolatePoints(int startIndex, int endIndex)
        {
            int steps = endIndex - startIndex;
            if (steps <= 1) return;

            int startY = curvePoints[startIndex].Y;
            int endY = curvePoints[endIndex].Y;

            for (int i = 1; i < steps; i++)
            {
                float t = (float)i / steps;
                int interpolatedY = (int)(startY + (endY - startY) * t);
                curvePoints[startIndex + i].Y = interpolatedY;
            }
        }

        private void CurvePictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            selectedPointIndex = -1;
        }

        private void buttonResetCurve_Click(object sender, EventArgs e)
        {
            // Сбрасываем контрольные точки к исходным позициям
            controlPoints[0] = new Point(0, 255);   // Левый верхний угол
            controlPoints[1] = new Point(64, 191);
            controlPoints[2] = new Point(128, 128); // Центр
            controlPoints[3] = new Point(191, 64);
            controlPoints[4] = new Point(255, 0);   // Правый нижний угол

            // Обновляем интерполяцию кривой
            UpdateCurveInterpolation();

            // Сбрасываем изображения всех слоев к оригиналу
            foreach (var layer in layers)
            {
                // Сохраняем информацию об удаленном цвете
                Color? removedColor = layer.RemovedColor;

                // Восстанавливаем оригинальное изображение
                layer.Image = new Bitmap(layer.OriginalImage);

                // Если был удален цвет, применяем это снова
                if (removedColor.HasValue)
                {
                    RemoveColor(layer, removedColor.Value);
                }
            }

            // Обновляем отображение
            UpdateHistogramAndCurve();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Выберите изображения";
                openFileDialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in openFileDialog.FileNames)
                    {
                        Bitmap originalImg = new Bitmap(file);
                        Bitmap scaledImg = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                        using (Graphics graphics = Graphics.FromImage(scaledImg))
                        {
                            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            graphics.DrawImage(originalImg, 0, 0, pictureBox1.Width, pictureBox1.Height);
                        }

                        var layer = new Layer { Image = scaledImg, OriginalImage = new Bitmap(scaledImg) };
                        layers.Add(layer);
                        AddImageLayer(layer);
                    }
                    UpdateHistogramAndCurve();
                }
            }
        }

        private void ApplyLayers()
        {
            g.Clear(Color.Transparent);

            for (int i = layers.Count - 1; i >= 0; i--)
            {
                var layer = layers[i];

                using (ImageAttributes attr = new ImageAttributes())
                {
                    ColorMatrix matrix = new ColorMatrix { Matrix33 = layer.Opacity };
                    attr.SetColorMatrix(matrix);

                    Rectangle destRect = new Rectangle(0, 0, w, h);

                    if (layer.BlendMode != "Нет")
                    {
                        canvas = BlendBitmaps(canvas, layer.Image, layer.BlendMode, layer.Opacity);
                        g = Graphics.FromImage(canvas);
                    }
                    else
                    {
                        g.DrawImage(layer.Image, destRect, 0, 0, w, h, GraphicsUnit.Pixel, attr);
                    }
                }
            }

            pictureBox1.Image = canvas;
            pictureBox1.Refresh();
        }

        private Bitmap BlendBitmaps(Bitmap baseImg, Bitmap overlayImg, string mode, float opacity)
        {
            int width = Math.Min(baseImg.Width, overlayImg.Width);
            int height = Math.Min(baseImg.Height, overlayImg.Height);

            Bitmap result = new Bitmap(baseImg);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color baseColor = baseImg.GetPixel(x, y);
                    Color overlayColor = overlayImg.GetPixel(x, y);

                    int r, g, b;

                    switch (mode)
                    {
                        case "Сумма":
                            r = Math.Min(baseColor.R + overlayColor.R, 255);
                            g = Math.Min(baseColor.G + overlayColor.G, 255);
                            b = Math.Min(baseColor.B + overlayColor.B, 255);
                            break;
                        case "Разность":
                            r = Math.Abs(baseColor.R - overlayColor.R);
                            g = Math.Abs(baseColor.G - overlayColor.G);
                            b = Math.Abs(baseColor.B - overlayColor.B);
                            break;
                        case "Умножение":
                            r = (baseColor.R * overlayColor.R) / 255;
                            g = (baseColor.G * overlayColor.G) / 255;
                            b = (baseColor.B * overlayColor.B) / 255;
                            break;
                        case "Среднее":
                            r = (baseColor.R + overlayColor.R) / 2;
                            g = (baseColor.G + overlayColor.G) / 2;
                            b = (baseColor.B + overlayColor.B) / 2;
                            break;
                        default:
                            r = baseColor.R;
                            g = baseColor.G;
                            b = baseColor.B;
                            break;
                    }

                    Color blendedColor = Color.FromArgb((int)(opacity * 255), r, g, b);
                    result.SetPixel(x, y, blendedColor);
                }
            }

            return result;
        }

        private void AddImageLayer(Layer layer)
        {
            FlowLayoutPanel layerPanel = new FlowLayoutPanel
            {
                Width = 200,
                Height = 500,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            PictureBox mini = new PictureBox
            {
                Image = layer.Image,
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 180,
                Height = 180
            };

            ComboBox blendMode = new ComboBox { Width = 180 };
            blendMode.Items.AddRange(new string[] { "Нет", "Сумма", "Разность", "Умножение", "Среднее" });
            blendMode.SelectedIndex = 0;
            blendMode.SelectedIndexChanged += (s, ev) =>
            {
                layer.BlendMode = blendMode.SelectedItem.ToString();
                ApplyLayers();
            };

            ComboBox removeColorMenu = new ComboBox { Width = 180 };
            removeColorMenu.Items.AddRange(new string[] { "Восстановить цвета", "Убрать красный", "Убрать зеленый", "Убрать синий" });
            removeColorMenu.SelectedIndex = 0;
            removeColorMenu.SelectedIndexChanged += (s, ev) =>
            {
                string selected = removeColorMenu.SelectedItem.ToString();
                if (selected == "Восстановить цвета")
                {
                    RestoreColors(layer);
                }
                else
                {
                    Color colorToRemove = Color.Red;
                    if (selected == "Убрать зеленый") colorToRemove = Color.Green;
                    else if (selected == "Убрать синий") colorToRemove = Color.Blue;

                    RemoveColor(layer, colorToRemove);
                }
                ApplyLayers();
            };

            TrackBar opacityTrack = new TrackBar
            {
                Minimum = 0,
                Maximum = 100,
                Value = (int)(layer.Opacity * 100),
                TickFrequency = 10,
                Width = 180
            };
            opacityTrack.Scroll += (s, ev) =>
            {
                layer.Opacity = opacityTrack.Value / 100f;
                ApplyLayers();
            };

            Button moveUpButton = new Button { Text = "↑ Переместить вверх", Width = 180, Height = 40 };
            moveUpButton.Click += (s, ev) => { MoveLayerUp(layer); ApplyLayers(); };

            Button moveDownButton = new Button { Text = "↓ Переместить вниз", Width = 180, Height = 40 };
            moveDownButton.Click += (s, ev) => { MoveLayerDown(layer); ApplyLayers(); };

            Button deleteButton = new Button { Text = "🗑 Удалить слой", Width = 180, Height = 40, ForeColor = Color.Red };
            deleteButton.Click += (s, ev) =>
            {
                layers.Remove(layer);
                flowLayoutPanel1.Controls.Remove(layerPanel);
                ApplyLayers();
            };

            layerPanel.Controls.Add(mini);
            layerPanel.Controls.Add(blendMode);
            layerPanel.Controls.Add(removeColorMenu);
            layerPanel.Controls.Add(opacityTrack);
            layerPanel.Controls.Add(moveUpButton);
            layerPanel.Controls.Add(moveDownButton);
            layerPanel.Controls.Add(deleteButton);

            flowLayoutPanel1.Controls.Add(layerPanel);
        }

        private void RemoveColor(Layer layer, Color colorToRemove)
        {
            layer.RemovedColor = colorToRemove;

            Bitmap img = new Bitmap(layer.Image); // Работаем с текущим изображением

            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    Color original = img.GetPixel(x, y);
                    int r = original.R, g = original.G, b = original.B;

                    if (colorToRemove == Color.Red) r = 0;
                    else if (colorToRemove == Color.Green) g = 0;
                    else if (colorToRemove == Color.Blue) b = 0;

                    img.SetPixel(x, y, Color.FromArgb(original.A, r, g, b));
                }
            }

            layer.Image = img;
        }
        private void RestoreColors(Layer layer)
        {
            layer.RemovedColor = null;
            layer.Image = new Bitmap(layer.OriginalImage);
            UpdateHistogramAndCurve();
        }

        private void MoveLayerUp(Layer layer)
        {
            int index = layers.IndexOf(layer);
            if (index > 0)
            {
                layers.RemoveAt(index);
                layers.Insert(index - 1, layer);
                flowLayoutPanel1.Controls.Clear();
                foreach (var l in layers) AddImageLayer(l);
            }
        }

        private void MoveLayerDown(Layer layer)
        {
            int index = layers.IndexOf(layer);
            if (index < layers.Count - 1)
            {
                layers.RemoveAt(index);
                layers.Insert(index + 1, layer);
                flowLayoutPanel1.Controls.Clear();
                foreach (var l in layers) AddImageLayer(l);
            }
        }
    }
}