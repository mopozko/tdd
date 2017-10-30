using System;
using System.Drawing;
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
                for (var i = 0; i < 100; i++)
                {
                    var rect = cloudLoyouter2.PutNextRectangle(new Size(rnd.Next(4, 30), rnd.Next(5, 10)));
                    //var rect = cloudLoyouter2.PutNextRectangle(new Size(10, 10));
                    if (rect == default(Rectangle))
                        break;
                    g.DrawRectangle(Pens.Green, new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));
                }
                g.DrawEllipse(Pens.Green, center.X - 5, center.Y - 5, 10, 10);
            }
            pb.Image = drawArea;
            pb.Invalidate();
        }
    }
}