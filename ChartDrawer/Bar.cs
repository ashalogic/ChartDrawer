using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ChartDrawer
{
    public partial class Bar : UserControl
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
        private List<int> _ValueX = new List<int> { 10, 20, 30, 40, 60,23,19 };
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
        private int NumberHeight = 0;
        public void Draw_Title()
        {
            G.DrawString(Title, new Font("Calibri", 18, FontStyle.Regular),
                new SolidBrush(Color.FromArgb(89, 89, 89)),
                new Rectangle(0, 0, W, (int)G.MeasureString(Title, base.Font).Height + 30),
                new StringFormat()
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                });
        }

        public void Draw_Grid()
        {
            int Y_Start_Point = (int)G.MeasureString(Title, Font).Height + 35;
            for (int _ = (int)(Math.Ceiling(ValueX.Max() / 5.0) * 5) + Interval(ValueX.Max() - ValueX.Min()); _ >= 0; _ -= Interval(ValueX.Max() - ValueX.Min()))
            {
                G.DrawString(_.ToString(), base.Font, new SolidBrush(base.ForeColor), new Rectangle(0, Y_Start_Point, (int)G.MeasureString(ValueX.Max().ToString(), base.Font).Width + 5, (int)G.MeasureString(_.ToString(), base.Font).Height), new StringFormat()
                {
                    LineAlignment = StringAlignment.Far,
                    Alignment = StringAlignment.Far
                });
                G.DrawLine(new Pen(Color.FromArgb(217, 217, 217), 1), (int)G.MeasureString(ValueX.Max().ToString(), base.Font).Width + 10, Y_Start_Point + ((int)G.MeasureString(_.ToString(), base.Font).Height) / 2, W - 10, Y_Start_Point + ((int)G.MeasureString(_.ToString(), base.Font).Height) / 2);
                Y_Start_Point += ((H - 80) - (int)G.MeasureString(Title, base.Font).Height + 10) / (((int)(Math.Ceiling(ValueX.Max() / 5.0) * 5) + Interval(ValueX.Max() - ValueX.Min())) / Interval(ValueX.Max() - ValueX.Min()));
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

            //int Y_Start_Point = (int)G.MeasureString(Title, Font).Height + 35 + NumberHeight / 2;

            //int Available_height = H - 80 - NumberHeight / 2;
            //int Grid_Space = Available_height / (((int)(Math.Ceiling(ValueX.Max() / 5.0) * 5) + Interval(ValueX.Max() - ValueX.Min())) / Interval(ValueX.Max() - ValueX.Min()));
            //double Beta = ((Available_height - Grid_Space)) / ValueX.Max();
            //MessageBox.Show(Height.ToString() + "|" + Available_height.ToString() + "|" + Grid_Space.ToString() + "|" + Beta.ToString());

            //G.FillRectangle(Brushes.Black, 60, Y_Start_Point, 20, Available_height - (int)(ValueX[0] * 3.31));
            //G.FillRectangle(Brushes.Black, 80, Y_Start_Point, 20, Available_height - (int)(ValueX[1] * 3.31));
            //G.FillRectangle(Brushes.Black, 100, Y_Start_Point, 20, Available_height - (int)(ValueX[2] * 3.31));
            //
            int Available_height = (H - 80) - (int)G.MeasureString(Title, Font).Height + 10;
            int Grid_Space = Available_height / (((int)(Math.Ceiling(ValueX.Max() / 5.0) * 5) + Interval(ValueX.Max() - ValueX.Min())) / Interval(ValueX.Max() - ValueX.Min()));
            int Y_Start_Point = (int)G.MeasureString(Title, Font).Height + 35 + ((int)G.MeasureString(ValueX.Max().ToString(), Font).Height) / 2;

            Available_height -= (int)G.MeasureString("0", Font).Height / 2;
            Available_height += 2;

            Y_Start_Point += Grid_Space;

            int validW = W - (int)G.MeasureString(ValueX.Max().ToString(), Font).Width - 30;
            int X_Start_Point = 0 + (int)G.MeasureString(ValueX.Max().ToString(), Font).Width + 60;

            int HSpace = (validW / ValueX.Count) - (ValueX.Count * 2);
            double Beta = (Available_height - (double)Grid_Space) / ValueX.Max();

            foreach (int item in ValueX)
            {

                int Not_Height_OF_Bar = (Available_height - Grid_Space) - (int)(item * Beta);
                G.FillRectangle(new SolidBrush(Color.FromArgb(91, 155, 213)), new Rectangle(X_Start_Point, Y_Start_Point + Not_Height_OF_Bar, 20, (Available_height - Grid_Space) - Not_Height_OF_Bar));
                G.DrawString(item.ToString(), Font, new SolidBrush(ForeColor), new Rectangle(X_Start_Point, H - 25, 20, (int)(G.MeasureString("0", Font).Height)), _SF);
                X_Start_Point += HSpace;
            }
        }

        public Bar()
        {
            InitializeComponent();
            BackColor = Color.FromArgb(255, 255, 255);
            BorderStyle = BorderStyle.FixedSingle;
            G = CreateGraphics();
            W = Width;
            H = Height;
            NumberHeight = (int)G.MeasureString("0", Font).Height;
        }

        private void Bar_Resize(object sender, EventArgs e)
        {
            W = Width;
            H = Height;
            G = CreateGraphics();
            Refresh();
        }

        private void Bar_Paint(object sender, PaintEventArgs e)
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
