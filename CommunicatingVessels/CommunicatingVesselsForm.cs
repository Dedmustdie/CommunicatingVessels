using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommunicatingVessels
{
    public partial class CommunicatingVesselsForm : Form
    {
        private Graphics graph;
        private Pen vesselsPen;
        private Pen markPen;
        private Pen waterPen;
        private Timer timer;

        private double width;
        private double height;

        private double k = 20;

        private double waterLevel = 0;
        private double dropWaterLevel = 25;

        private static PointF generalCoords = new PointF(0, -10);
        private static PointF bigVesCoords = new PointF(generalCoords.X - 20, generalCoords.Y);
        private static PointF smallVesCoords = new PointF(generalCoords.X + 10, generalCoords.Y);
        private static PointF intermediateVesCoords = new PointF(generalCoords.X, generalCoords.Y);
        private static PointF globalWater = new PointF(bigVesCoords.X + 5, 25);

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (Regex.IsMatch(VTextBox.Text, @"^(([1-9][0-9])|([1-9]))$"))
            {
                StartButton.Enabled = false;
                InitDropWaterTimer(1000 / 5 / Convert.ToInt32(VTextBox.Text));
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            dropWaterLevel = 25;
            waterLevel = 0;
            Invalidate();
            StartButton.Enabled = true;
        }

        private void DropOne(Graphics graph)
        {
            globalWater = new PointF(bigVesCoords.X + 5, generalCoords.Y + (float)dropWaterLevel);

            graph.DrawLine(waterPen, ScreenCoords(globalWater.X, globalWater.Y + 5),
                ScreenCoords(globalWater.X + 5, globalWater.Y + 5));

            graph.DrawLine(waterPen, ScreenCoords(globalWater.X, globalWater.Y),
                ScreenCoords(globalWater.X + 5, globalWater.Y));

            graph.DrawLine(waterPen, ScreenCoords(globalWater.X, globalWater.Y + 5),
                ScreenCoords(globalWater.X, globalWater.Y));

            graph.DrawLine(waterPen, ScreenCoords(globalWater.X + 5, globalWater.Y + 5),
                ScreenCoords(globalWater.X + 5, globalWater.Y));
        }

        private void FillVessels(Graphics graph)
        {
            PaintBox(graph);
            if (waterLevel < 5)
            {
                graph.DrawLine(waterPen, ScreenCoords(intermediateVesCoords.X - 5, intermediateVesCoords.Y + waterLevel),
                    ScreenCoords(intermediateVesCoords.X + 10, intermediateVesCoords.Y + waterLevel));
            }

            graph.DrawLine(waterPen, ScreenCoords(smallVesCoords.X, smallVesCoords.Y + waterLevel),
                ScreenCoords(smallVesCoords.X + 10, smallVesCoords.Y + waterLevel));

            graph.DrawLine(waterPen, ScreenCoords(bigVesCoords.X, bigVesCoords.Y + waterLevel),
                ScreenCoords(bigVesCoords.X + 15, bigVesCoords.Y + waterLevel));
        }

        private void PaintBox(Graphics graph)
        {
            for (int i = 0; i <= 15; i = i + 5)
            {
                graph.DrawLine(markPen, ScreenCoords(bigVesCoords.X + i, bigVesCoords.Y + 20),
                    ScreenCoords(bigVesCoords.X + i, bigVesCoords.Y));
            }

            for (int i = 0; i <= 20; i = i + 5)
            {
                graph.DrawLine(markPen, ScreenCoords(bigVesCoords.X, bigVesCoords.Y + i),
                    ScreenCoords(bigVesCoords.X + 15, bigVesCoords.Y + i));

                graph.DrawLine(markPen, ScreenCoords(smallVesCoords.X + 10, smallVesCoords.Y + i),
                    ScreenCoords(smallVesCoords.X, smallVesCoords.Y + i));
            }

            for (int i = 0; i <= 10; i = i + 5)
            {
                graph.DrawLine(markPen, ScreenCoords(smallVesCoords.X + i, smallVesCoords.Y + 20),
                    ScreenCoords(smallVesCoords.X + i, smallVesCoords.Y));
                graph.DrawLine(markPen, ScreenCoords(intermediateVesCoords.X + i, bigVesCoords.Y + 5),
                    ScreenCoords(intermediateVesCoords.X + i, bigVesCoords.Y));
            }

            // Большой сосуд:
            // Левая грань.
            graph.DrawLine(vesselsPen, ScreenCoords(bigVesCoords.X, bigVesCoords.Y + 20),
                ScreenCoords(bigVesCoords.X, bigVesCoords.Y));
            // Правая грань.
            graph.DrawLine(vesselsPen, ScreenCoords(bigVesCoords.X + 15, bigVesCoords.Y + 20),
                ScreenCoords(bigVesCoords.X + 15, bigVesCoords.Y + 5));
            //// Нижняя грань.
            graph.DrawLine(vesselsPen, ScreenCoords(bigVesCoords.X, bigVesCoords.Y),
                ScreenCoords(bigVesCoords.X + 15, bigVesCoords.Y));

            //// Перемычка:
            //// Верхняя грань.
            graph.DrawLine(vesselsPen, ScreenCoords(intermediateVesCoords.X - 5, intermediateVesCoords.Y + 5),
                ScreenCoords(intermediateVesCoords.X + 10, intermediateVesCoords.Y + 5));
            //// Нижняя грань.
            graph.DrawLine(vesselsPen, ScreenCoords(intermediateVesCoords.X - 5, intermediateVesCoords.Y),
                ScreenCoords(intermediateVesCoords.X + 10, intermediateVesCoords.Y));

            //// Малый сосуд:
            //// Левая грань.
            graph.DrawLine(vesselsPen, ScreenCoords(smallVesCoords.X, smallVesCoords.Y + 20),
                ScreenCoords(smallVesCoords.X, smallVesCoords.Y + 5));
            ////// Правая грань.
            graph.DrawLine(vesselsPen, ScreenCoords(smallVesCoords.X + 10, smallVesCoords.Y + 20),
                ScreenCoords(smallVesCoords.X + 10, smallVesCoords.Y));
            ////// Нижняя грань.
            graph.DrawLine(vesselsPen, ScreenCoords(smallVesCoords.X, smallVesCoords.Y),
                ScreenCoords(smallVesCoords.X + 10, smallVesCoords.Y));
        }

        private PointF ScreenCoords(double x, double y)
        {
            return new PointF((float)(width / 2 + x * k), (float)(height / 2 - y * k));
        }

        public CommunicatingVesselsForm()
        {
            InitializeComponent();
            graph = CreateGraphics();
            graph.SmoothingMode = SmoothingMode.HighQuality;
            vesselsPen = new Pen(Color.Black, 2);
            markPen = new Pen(Color.Red, 1);
            waterPen = new Pen(Color.Blue, 2);
            width = ClientSize.Width;
            height = ClientSize.Height;
        }

        private void InitDropWaterTimer(int interval)
        {
            timer = new Timer();
            timer.Interval = interval;
            timer.Tick += DropWaterTimer_Tick;
            timer.Enabled = true;
        }

        private void DropWaterTimer_Tick(object sender, EventArgs e)
        {
            dropWaterLevel -= 5;
            if (waterLevel == 20)
            {
                timer.Stop();
            }
            if (dropWaterLevel < waterLevel)
            {
                if (waterLevel < 5)
                {
                    waterLevel += 0.625;
                }
                else if (waterLevel < 20)
                {
                    waterLevel += 1;
                }
                dropWaterLevel = 25 + waterLevel;
            }
            Invalidate();
        }

        private void CommunicatingVesselsForm_Paint(object sender, PaintEventArgs e)
        {
            PaintBox(e.Graphics);
            FillVessels(e.Graphics);
            DropOne(e.Graphics);
        }
    }
}
