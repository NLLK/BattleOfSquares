using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace BattleOfSquares.Tests
{
    [TestFixture]
    public class SquareTest
    {
        [TestCase(0,0,370,0)]
        [TestCase(0, 1, 370, 54)]
        [TestCase(19,19,1396,1026)]
        public void GetPointTest(int x, int y, int xExpected, int yExpected)
        {
            Point result = Square.GetPoint(x, y);
            Point expected = new Point(xExpected, yExpected);

            Assert.AreEqual(result,expected);
        }
    }
}
