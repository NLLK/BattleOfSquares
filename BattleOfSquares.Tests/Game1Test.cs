using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
