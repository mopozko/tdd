using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;


namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTest
    {
        private CircularCloudLayouter cloudLayouter;
        private Point center;
        [SetUp]
        public void Initialization()
        {
            center = new Point(10,10);
            cloudLayouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void PutNextRectangle_GetFirstRectange()
        {
            cloudLayouter.PutNextRectangle(new Size(4, 4))
                .Should()
                .Be(new Rectangle(8, 8, 4, 4));
        }

        [Test]
        public void PutNextRectangle_AllRectanglesFormACircle_After100IdenticalRectangles()
        {
            var allRectangles = new List<Rectangle>();

            for (var i = 0; i < 100; i++)
            {
                var rect = cloudLayouter.PutNextRectangle(new Size(10,10));
                allRectangles.Add(rect);
            }
            PutNextRectangle_AllRectanglesFormACircle(allRectangles);
        }
        [Test]
        public void PutNextRectangle_AllRectanglesFormACircle_After100RectanglesWithARandomSize()
        {
            var allRectangles = new List<Rectangle>();
            var rnd = new Random();
            for (var i = 0; i < 100; i++)
            {
                var rect = cloudLayouter.PutNextRectangle(new Size(rnd.Next(10,30),rnd.Next(10,30)));
                allRectangles.Add(rect);
            }
            PutNextRectangle_AllRectanglesFormACircle(allRectangles);
        }
        [Test]
        public void PutNextRectangle_AllRectanglesFormACircle_After50LargeAnd50SmallRectangles()
        {
            var allRectangles = new List<Rectangle>();

            for (var i = 0; i < 50; i++)
            {
                var rect = cloudLayouter.PutNextRectangle(new Size(50, 50));
                allRectangles.Add(rect);
            }
            for (var i = 0; i < 50; i++)
            {
                var rect = cloudLayouter.PutNextRectangle(new Size(10, 10));
                allRectangles.Add(rect);
            }
            PutNextRectangle_AllRectanglesFormACircle(allRectangles);
        }
        [Test]
        public void PutNextRectangle_AllRectanglesDoNotIntersect()
        {
            var resultRectangles = Enumerable.Range(0, 100).Select(x => cloudLayouter.PutNextRectangle(new Size(2, 2))).ToArray();
            resultRectangles.Should().HaveCount(100);
            resultRectangles.Where(rectangle => resultRectangles
                    .Any(x => x != rectangle && x.IntersectsWith(rectangle)))
                .Should()
                .HaveCount(0);
        }
        
        [Test, Timeout(1000)]
        public void AddALotOfRectangles_TimeoutTest()
        {
            var size = new Size(4,4);
            for (var i = 0; i < 100; i++)
                cloudLayouter.PutNextRectangle(size);
        }

        [TestCase(0, 1, TestName = "ZeroWidth")]
        [TestCase(-1, 1, TestName = "NegativeWidth")]
        [TestCase(1, 0, TestName = "ZeroHeight")]
        [TestCase(1, -1, TestName = "NegativeHeight")]

        public void PutNextRectangle_Throw_WhenIncorrectSize(int width, int height)
        {
            new Action(() => cloudLayouter.PutNextRectangle(new Size(width, height))).Should()
                .ThrowExactly<ArgumentException>();
        }

        private void PutNextRectangle_AllRectanglesFormACircle(List<Rectangle> allRectangles)
        {
            var totalArea = allRectangles.Select(x => x.Width * x.Height).Sum();

            var currentCircularRadius = 1.2 * Math.Sqrt(totalArea / Math.PI);

            allRectangles.Select(ToBorderPoints).SelectMany(x => x).Distinct()
                .Select(x => new Point(center.X - x.X, center.Y - x.Y))
                .Where(x => x.X * x.X + x.Y * x.Y > currentCircularRadius * currentCircularRadius)
                .Should()
                .HaveCount(0);
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