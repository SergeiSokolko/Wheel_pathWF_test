using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace COURSE_PROJECT
{
    public partial class Form1 : Form
    {
#region Variables
        int velocity;
        int diameter;
        int coordY;
        float rotationSpeed = 0.01f;
        long totalMemory = GC.GetTotalMemory(false);
        Color colors; Brush brushes;
        List<Point> points = new List<Point>();
#endregion

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            Wheel wheel = new Wheel(ClientSize, e.Graphics, diameter, velocity, rotationSpeed, colors);

            Graphics gra = e.Graphics;
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime; //приоритет выполняемому процессу

            int x = (int)(diameter / 2 * Math.Sin(-velocity * rotationSpeed) + (diameter / 2));
            int y = (int)(diameter / 2 * Math.Cos(-velocity * rotationSpeed) + (diameter / 2));

            points.Add(new Point(velocity + x + 5, this.Height - diameter + y - 43));

            Track track = new Track(e.Graphics, points, colors, brushes);
            coordY = diameter - y-1;

            textBox3.Text = (velocity + x + 5).ToString();
            textBox4.Text = coordY.ToString();
        }

        private void Radius_TextChanged(object sender, EventArgs e)
        {

            try
            {
                Start_Stop.Enabled = true;
                var diam = Convert.ToInt32(Radius.Text);    //диаметр
                
                if (diam >= this.Width || diam >= this.Height)
                {
                    MessageBox.Show("Введённое значение диаметра превышает границы области");
                    Start_Stop.Enabled = false;
                }
                diameter = diam;
            }
            catch (Exception ex)
            {
                    //MessageBox.Show("Ошибка! " + ex);
            }
        }

        private void Radius_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)     //8 - код клавиши backspace
                e.Handled = true;
        }

        private void Start_Stop_MouseClick(object sender, MouseEventArgs e)
        {
            if (Start_Stop.Text == "Старт" && Radius.Text != "")
            {
                Start_Stop.Text = ("Стоп");
                if (velocity < 1)
                    points.Clear();
                Radius.Enabled = false;
                timer1.Start();
            }
            else
            {
                Start_Stop.Text = ("Старт");
                Radius.Enabled = true;
                timer1.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (velocity >= (Width - diameter - 30)) //заканчивает движение круга, когда он доходит до правого края поля
            {
                timer1.Stop();
                Start_Stop.Text = ("Старт");
                Start_Stop.Enabled = false;
            }

            velocity += 1; // velocity отвечает за координату Х. Интервал задается в миллисекундах, т.е. 1000 = 1 секунда
            this.Invalidate(); //отрисовка круга с каждым тиком таймера

        }

        private void Reset_MouseClick(object sender, MouseEventArgs e)
        {
            velocity = 0;
            points.Clear();
            Start_Stop.Enabled = true;
            Radius.Enabled = true;
            if (diameter >= this.Width || diameter >= this.Height)
            {
                Radius.Text = "";
                points.Clear();
            }
            GC.Collect();
            //GC.WaitForPendingFinalizers();
            this.Invalidate();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            switch (trackBar1.Value)
            {
                case 0:
                    rotationSpeed = 0.01f; break;
                case 1:
                    rotationSpeed = 0.05f; break;
                case 2:
                    rotationSpeed = 0.1f; break;
                case 3:
                    rotationSpeed = 0.15f; break;
                case 4:
                    rotationSpeed = 0.5f; break;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    { colors = Color.Red; brushes = Brushes.Red;  this.Invalidate(); break; }
                case 1:
                    { colors = Color.Blue; brushes = Brushes.Blue; this.Invalidate(); break; }
                case 2:
                    { colors = Color.Green; brushes = Brushes.Green; this.Invalidate(); break; }
                case 3:
                    { colors = Color.Orange; brushes = Brushes.Orange; this.Invalidate(); break; }
                case 4:
                    { colors = Color.Indigo; brushes = Brushes.Indigo; this.Invalidate(); break; }
            }
        }
    }
}