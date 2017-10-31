using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTest
    {
        private CircularCloudLayouter cloudLayouter;
        [SetUp]
        public void Initialization()
        {
            cloudLayouter = new CircularCloudLayouter(new Point(10, 10));
        }

        [Test]
        public void PutNextRectangle_GetFirstRectange()
        {
            cloudLayouter.PutNextRectangle(new Size(4, 4)).Should().Be(new Rectangle(8, 8, 4, 4));
        }

        [Test]
        public void PutNextRectangle_After5IdenticalRectangles()
        {
            Enumerable.Range(0, 5).Select(x => cloudLayouter.PutNextRectangle(new Size(2, 2)))
                .Should().BeEquivalentTo(new[]
                {
                    new Rectangle(9, 9, 2, 2),
                    new Rectangle(9, 7, 2, 2),
                    new Rectangle(9, 11, 2, 2),
                    new Rectangle(7, 9, 2, 2),
                    new Rectangle(11, 9, 2, 2),
                });
        }

        [Test]
        public void PutNextRectangle_After()
        {
            new[]
                {
                    new Size(4, 2),
                    new Size(2, 2),
                    new Size(2, 2),
                }.Select(x => cloudLayouter.PutNextRectangle(x))
                .Should().BeEquivalentTo(new[]
                {
                    new Rectangle(8, 9, 4, 2),
                    new Rectangle(9, 7, 2, 2),
                    new Rectangle(9, 11, 2, 2),
                });
        }

        [Test, Timeout(1000)]
        public void AddALotOfRectangles()
        {
            var size = new Size(1,1);
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

        [Test]
        public void DoSomething_WhenSomething()
        {
            var resultRectangles = Enumerable.Range(0, 100).Select(x => cloudLayouter.PutNextRectangle(new Size(2, 2))).ToArray();
            resultRectangles.Should().HaveCount(100);
            resultRectangles.Where(rectangle => resultRectangles
                .Any(x => x != rectangle && x.IntersectsWith(rectangle)))
                .Should()
                .HaveCount(0);
        }

    }
}