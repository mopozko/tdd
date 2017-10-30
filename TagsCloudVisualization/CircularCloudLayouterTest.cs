using System.Drawing;
using System.Linq;

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
    }
}