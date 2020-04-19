using NUnit.Framework;


namespace BattleOfSquares.Tests
{
    [TestFixture]
    public class DiceTest
    {
        [Test]
        public void doRandomTest()
        {
            Dice test1 = new Dice();
            Dice test2 = new Dice();

            int count1 = test1.doRandom(1, 0);
            int count2 = test2.doRandom(2, count1);

            Assert.AreNotEqual(test1.randoms, test2.randoms);
        }
        [Test]
        public void GetRandomTest()
        {
            Dice test1 = new Dice();
            test1.doRandom(1, 0);
            Assert.AreNotEqual(test1.randoms[5], 0);
        }
    }
}
