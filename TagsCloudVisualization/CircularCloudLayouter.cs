using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point center;
        private bool[,] feeld;
        private int width;
        private int height;
        private List<Rectangle> cloudRectangles;
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            this.width = center.X * 2;
            this.height = center.Y * 2;
            feeld = new bool[width, height];
            cloudRectangles = new List<Rectangle>();
        }

        private bool IsBorderPoint(int x, int y)
        {
            if (feeld[x, y]) return false;
            for (var dx = -1; dx <= 1; dx++)
                for (var dy = -1; dy <= 1; dy++)
                    if (feeld[x + dx, y + dy]) return true;
            return false;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle result;
            var w = rectangleSize.Width;
            var h = rectangleSize.Height;
            if (!cloudRectangles.Any())
                result = new Rectangle(center.X - w / 2, center.Y - h / 2, w, h);
            else
            {
                var potentialRectangles = new List<Rectangle>();

                for (var x = w; x < width - w - 1; x++)
                    for (var y = h; y < height - h - 1; y++)
                        if (IsBorderPoint(x, y))
                        {
                            potentialRectangles.Add(new Rectangle(x, y, w, h));
                            potentialRectangles.Add(new Rectangle(x - w + 1, y - h + 1, w, h));
                            potentialRectangles.Add(new Rectangle(x - w + 1, y, w, h));
                            potentialRectangles.Add(new Rectangle(x, y - h + 1, w, h));
                        }

                potentialRectangles = potentialRectangles
                                      .Where(x => !cloudRectangles.Any(r => r.IntersectsWith(x)))
                                      .ToList();

                result = FindTheNearestArea(potentialRectangles);
            }
            if (result != default(Rectangle))
            {
                cloudRectangles.Add(result);
                UpdateFeeld(result);
            }
            return result;
        }

        private Rectangle FindTheNearestArea(IEnumerable<Rectangle> potentialRectangles)
        {
            Rectangle result = default(Rectangle);
            var minDistance = -1.0;
            foreach (var rectangle in potentialRectangles)
            {
                var distance = GetDistanceBetweenPoints(rectangle.GetCenter(), center);
                if (minDistance == -1) minDistance = distance;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    result = rectangle;
                }
            }
            return result;
        }
        private void UpdateFeeld(Rectangle area)
        {
            for (var i = area.X; i < area.Width + area.X; i++)
                for (var j = area.Y; j < area.Height + area.Y; j++)
                    feeld[i, j] = true;
        }

        private double GetDistanceBetweenPoints(Point point1, Point point2)
        {
            return Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) +
                             (point1.Y - point2.Y) * (point1.Y - point2.Y));
        }
    }
    public static class MyExtensions
    {
        public static Point GetCenter(this Rectangle rect)
        {
            return new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }
    }
}