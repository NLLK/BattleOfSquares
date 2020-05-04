using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;

namespace BattleOfSquares.Tests
{
    class Game1Test
    {
        [TestCase("1-0",null)]
        [TestCase("0-1", null)]
        [TestCase("0-0", null)]
        public void GetSquareTexture(string number, Texture2D expected)
        {
            Texture2D result= Game1.GetSquareTexture(number);
            Assert.AreEqual(result, expected);
        }
        [TestCase("1-1", 1,1)]
        public void GetSquareTextureTest(string number, int expectedWidth, int expectedHeight)
        {
            Texture2D resultTexture = Game1.GetSquareTexture(number);
            int width = resultTexture.Width;
            int height = resultTexture.Height;
            Point result = new Point(width, height);
            Point expected = new Point(expectedWidth*54, expectedHeight*54);
            Assert.AreEqual(number, expected);
        }
        [TestCase(0, null)]
        [TestCase(-1, null)]
        [TestCase(7, null)]
        public void GetDiceTexture(int number, Texture2D expected)
        {
            Texture2D result = Game1.GetDiceTexture(number);
            Assert.AreEqual(result, expected);
        }
    }
}
