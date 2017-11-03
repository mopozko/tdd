using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    public sealed class MainForm : Form
    {
        private Bitmap drawArea;
        public MainForm()
        {
            var center = new Point(Width / 2, Height / 2);
            var cloudLoyouter2 = new CircularCloudLayouter(center);
            var pb = new PictureBox
            {
                Width = this.Width,
                Height = this.Height,
            };
            drawArea = new Bitmap(pb.Size.Width, pb.Size.Height);
            Controls.Add(pb);
            using (var g = Graphics.FromImage(drawArea))
            {
                var rnd = new Random(1);
                var allRectangles = new List<Rectangle>();

                for (var i = 0; i < 30; i++)
                {
                    var rect = cloudLoyouter2.PutNextRectangle(new Size(30, 30));
                    g.DrawRectangle(Pens.Green, rect);
                    allRectangles.Add(rect);
                }
                for (var i = 0; i < 30; i++)
                {
                    var rect = cloudLoyouter2.PutNextRectangle(new Size(20, 20));
                    g.DrawRectangle(Pens.Green, rect);
                    allRectangles.Add(rect);
                }

                //for (var i = 0; i < 100; i++)
                //{
                //    //var rect = cloudLoyouter2.PutNextRectangle(new Size(rnd.Next(4, 30), rnd.Next(5, 10)));
                //    var rect = cloudLoyouter2.PutNextRectangle(new Size(10, 10));
                //    allRectangles.Add(rect);
                //    if (rect == default(Rectangle))
                //        break;
                //    //g.FillRectangle(Brushes.Green, new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));
                //    g.DrawRectangle(Pens.Green, new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));
                //}
                var totalArea = allRectangles.Select(x => x.Width * x.Height).Sum();
                var currentCircularRadius = (int)(1.2 * Math.Sqrt(totalArea / Math.PI));
                g.DrawEllipse(Pens.Green, center.X - currentCircularRadius, center.Y - currentCircularRadius,2 * currentCircularRadius, 2 * currentCircularRadius);
            }
            pb.Image = drawArea;
            pb.Invalidate();
        }
        public static IEnumerable<Point> ToBorderPoints(Rectangle rect)
        {
            var x = rect.Location.X;
            var y = rect.Location.Y;
            var w = rect.Width;
            var h = rect.Height;
            return new[]
            {
                new Point(x,y),
                new Point(x + w, y + h),
                new Point(x + w, y),
                new Point(x, y + h),
            };
        }
    }
}