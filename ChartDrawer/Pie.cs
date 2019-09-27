using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace ChartDrawer
{
    public partial class Pie : UserControl
    {
        private List<string> _LableX = new List<string> { "Alireza", "Keshavarz", "LaNa", "DeL", "ReY" };
        public List<string> LableX
        {
            get
            {
                return _LableX;
            }

            set
            {
                _LableX = value; Refresh();
            }
        }
        private List<int> _ValueX = new List<int> { 10, 20, 30, 40, 60 };
        public List<int> ValueX
        {
            get
            {
                return _ValueX;
            }
            set
            {
                _ValueX = value; Refresh();
            }
        }
        private string _Title = "Chart Title";
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value; Refresh();
            }
        }

        private Graphics G;

        public Pie()
        {
            InitializeComponent();
            BackColor = Color.FromArgb(255, 255, 255);
            BorderStyle = BorderStyle.FixedSingle;
            G = CreateGraphics();
        }
        public void Draw_Title()
        {
            StringFormat _SF = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            Font ff = new Font("Calibri", 18, FontStyle.Regular);


            G.DrawString(Title, ff,
                new SolidBrush(Color.FromArgb(89, 89, 89)),
                new Rectangle(0, 0, Width, 2 * (Height / 10)),
                _SF);
        }
        private void Pie_Paint(object sender, PaintEventArgs e)
        {
            Draw_Title();
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Random rnd = new Random();
            double temp_start = 0;
            double sums = ValueX.Sum();
            string[] colorlist = new string[5];
            colorlist[0] = "#FF4472C4";
            colorlist[1] = "#FF5B9BD5";
            colorlist[2] = "#FFED7D31";
            colorlist[3] = "#FFFFC000";
            colorlist[4] = "#FFA5A5A5";
            StringFormat _SF = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near
            };
            Font ff = new Font("Calibri", 8, FontStyle.Regular);

            int n = 0;
            int lbi = 0;
            float xstep = 3 * (Width / 10);
            int space = Width / LableX.Count;

            foreach (int item in ValueX)
            {


                double val = ((item / sums) * 360);
                Color _color = ColorTranslator.FromHtml(colorlist[n]);

                e.Graphics.FillPie(new SolidBrush(_color), new Rectangle(3 * (Width / 10), 2 * (Height / 10), 4 * (Width / 10), 6 * (Height / 10)), (float)temp_start, (float)val);
                e.Graphics.DrawPie(new Pen(Color.White, 2), new Rectangle(3 * (Width / 10), 2 * (Height / 10), 4 * (Width / 10), 6 * (Height / 10)), (float)temp_start, (float)val);
                e.Graphics.DrawString(LableX[lbi], ff,
                new SolidBrush(_color),
                new Rectangle((int)xstep, 8 * (Height / 10), 4 * (Width / 10), 2 * (Height / 10))
                , _SF);
                xstep += e.Graphics.MeasureString(LableX[lbi], Font).Width;
                temp_start += val; n++; lbi++;
                if (n == 5)
                    n = 0;
            }
        }
        private PointF DegreesToXY(float degrees, float radius, Point origin)
        {
            PointF xy = new PointF();
            double radians = degrees * Math.PI / 180.0;

            xy.X = (float)Math.Cos(radians) * radius + origin.X;
            xy.Y = (float)Math.Sin(-radians) * radius + origin.Y;

            return xy;
        }
        private void Pie_Resize(object sender, EventArgs e)
        {
            G = CreateGraphics();
            Refresh();
        }
    }
}
