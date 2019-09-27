using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChartDrawer
{
    public partial class Dot : UserControl
    {
        private List<string> _LableX;
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
        private int W;
        private int H;

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
                new Rectangle(0, 0, W, (int)G.MeasureString(Title, Font).Height + 30),
                _SF);
        }

        public void Draw_Grid()
        {
            StringFormat _SF = new StringFormat()
            {
                LineAlignment = StringAlignment.Far,
                Alignment = StringAlignment.Far
            };

            int Max = ValueX.Max();
            int Min = ValueX.Min();
            int Grid_Step = Interval(Max - Min);
            int MaxPStep = (int)(Math.Ceiling(Max / 5.0) * 5) + Grid_Step;

            int max_height = (H - 80) - (int)G.MeasureString(Title, Font).Height + 10;
            int space = max_height / (MaxPStep / Grid_Step);
            int temp_y = (int)G.MeasureString(Title, Font).Height + 35;

            for (int _ = MaxPStep; _ >= 0; _ -= Grid_Step)
            {
                G.DrawString(_.ToString(), Font, new SolidBrush(ForeColor), new Rectangle(0, temp_y, (int)G.MeasureString(Max.ToString(), Font).Width + 5, (int)G.MeasureString(_.ToString(), Font).Height), _SF);


                G.DrawLine(new Pen(Color.FromArgb(217, 217, 217), 1), (int)G.MeasureString(Max.ToString(), Font).Width + 10, temp_y + ((int)G.MeasureString(_.ToString(), Font).Height) / 2, W - 10, temp_y + ((int)G.MeasureString(_.ToString(), Font).Height) / 2);

                temp_y += space;
            }
        }
        #region MiniVoid
        private int Interval(double Range)
        {
            double Temp_Range = Range;
            double Temp_X = Math.Pow(10.0, Math.Floor(Math.Log10(Temp_Range)));
            if (Temp_Range / Temp_X >= 5)
            {
                return (int)Temp_X;
            }
            else if (Temp_Range / (Temp_X / 2.0) >= 5)
            {
                return (int)(Temp_X / 2.0);
            }
            else
            {
                return (int)(Temp_X / 5.0);
            }
        }
        #endregion

        public void Draw_Bar()
        {

            StringFormat _SF = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };

            //
            int max_height = (H - 80) - (int)G.MeasureString(Title, Font).Height + 10;
            int Max = ValueX.Max();
            int Min = ValueX.Min();
            int Grid_Step = Interval(Max - Min);
            int MaxPStep = (int)(Math.Ceiling(Max / 5.0) * 5) + Grid_Step;
            int space = max_height / (MaxPStep / Grid_Step);
            int max_start = (int)G.MeasureString(Title, Font).Height + 35 + ((int)G.MeasureString(ValueX.Max().ToString(), Font).Height) / 2;

            max_height = max_height - (int)G.MeasureString("0", Font).Height / 2;
            max_height += 2;
            max_start += space;

            int validW = W - (int)G.MeasureString(ValueX.Max().ToString(), Font).Width - 30;
            int HSpace = (validW / ValueX.Count) - (ValueX.Count * 2);
            int start_x_temp = 0 + (int)G.MeasureString(ValueX.Max().ToString(), Font).Width + 60;

            int alpha = max_start / Max;
            double beta = ((max_height - (double)space)) / Max;

            foreach (int item in ValueX)
            {
                int tempoftemp = (max_height - space) - (int)(item * (beta));
                G.FillPie(new SolidBrush(Color.FromArgb(91, 155, 213)), new Rectangle(start_x_temp, max_start + tempoftemp - (((int)G.MeasureString("0", Font).Height) / 2), 10, 10), 0, 360);
                //G.DrawString(item.ToString(), Font, new SolidBrush(ForeColor), new Rectangle(start_x_temp, H - 25, 20, (int)(G.MeasureString("0", Font).Height)), _SF);
                start_x_temp += HSpace;

            }
        }

        public Dot()
        {
            InitializeComponent();
            BackColor = Color.FromArgb(255, 255, 255);
            BorderStyle = BorderStyle.FixedSingle;
            G = CreateGraphics();
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            W = Width;
            H = Height;
        }

        private void dot_Resize(object sender, EventArgs e)
        {
            W = Width;
            H = Height;
            G = CreateGraphics();
            Refresh();
        }

        private void dot_Paint(object sender, PaintEventArgs e)
        {
            Draw_Title();
            Draw_Grid();
            Draw_Bar();
        }
        #region MiniVoid
        private bool CheckProp()
        {
            if (LableX != null && ValueX != null && Title != null)
                return true;
            return false;
        }
        #endregion
    }
}
