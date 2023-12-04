using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App62
{
    public partial class CircularButton : Button
    {
        private bool isMouseOver;
        public CircularButton()
        {
            Size = new Size(100, 100); 
            BackColor = Color.Transparent; 
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            MouseEnter += (sender, e) => isMouseOver = true;
            MouseLeave += (sender, e) => isMouseOver = false;
            Paint += CircularButton_Paint;
        }
        private void CircularButton_Paint(object sender, PaintEventArgs e)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(ClientRectangle);
            Region = new Region(path);
            using (SolidBrush brush = new SolidBrush(isMouseOver ? Color.FromArgb(50, BackColor) : BackColor))
            {
                e.Graphics.FillEllipse(brush, ClientRectangle);
            }
            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }

    public partial class Form1 : Form
    {
        private Timer timer = new Timer();
        private double timeElapsed = 0;
        
        private List<CircularButton> planets = new List<CircularButton>();
        private List<double> Radius = new List<double>() { 0, 200, 350, 500, 650 };
        private List<double> Speeds = new List<double>() {0,0.7,0.6,0.5,0.4};
        private List<string> Names = new List<string>(){ "Sun", "Mercury", "Earth" , "Venus" , "Mars"};
        private List<Color> Colors = new List<Color>() {Color.Yellow,Color.Gray,Color.Blue,Color.Brown,Color.Red };
        private List<int> Sizes = new List<int>() { 200, 100, 75, 75, 75 };


        public Form1()
        {
            InitializeComponent();
            InitializePlanets();
            InitializeTimer();
            WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void InitializePlanets()
        {
            for (int i = 0;i<5;++i)
            {
                planets.Add(new CircularButton());
                planets[i].Text = Names[i];
                planets[i].BackColor = Colors[i];
                planets[i].Size = new Size(Sizes[i], Sizes[i]);
                Controls.Add(planets[i]);
                SetPlanetPosition(planets[i], Radius[i], 0);
            }
        }
        private void InitializeTimer()
        {
            timer.Interval = 16;
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; ++i)
            {
                UpdatePlanetPosition(planets[i], Radius[i], Speeds[i]);
            }
        }
        private void SetPlanetPosition(CircularButton planet, double radius, double angle)
        {
            int centerX = ClientSize.Width / 2;
            int centerY = ClientSize.Height / 2;
            int x = (int)(centerX + radius * Math.Cos(angle));
            int y = (int)(centerY + radius * Math.Sin(angle));
            foreach (Control control in Controls)
            {
                if (control is CircularButton otherPlanet && otherPlanet != planet)
                {
                    double distance = Math.Sqrt(Math.Pow(x - otherPlanet.Location.X, 2) + Math.Pow(y - otherPlanet.Location.Y, 2));
                    double minDistance = planet.Width / 2 + otherPlanet.Width / 2 + 10; 

                    if (distance < minDistance)
                    {
                        angle += Math.PI / 180;
                        x = (int)(centerX + radius * Math.Cos(angle));
                        y = (int)(centerY + radius * Math.Sin(angle));
                    }
                }
            }
            planet.Location = new Point(x - planet.Width / 2, y - planet.Height / 2);
        }
        private void UpdatePlanetPosition(CircularButton planet, double radius, double speed)
        {
            timeElapsed += 0.016;
            double angle = timeElapsed * speed;
            SetPlanetPosition(planet,radius,angle);
        }
    }
}
