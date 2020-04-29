using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BattleOfSquares
{
    /// <summary>
    /// CLass for draw and manage dices
    /// </summary>
    public class Dice
    {
        public bool needToDraw;//нужно ли рисовать

        static int timeForSide = 300;//сколько времени на одну сторону кости

        static float scale = 0.7f;//масштаб
        static Vector2 addTo = new Vector2(0, 5 * scale);//вектор, на сколько изменять координаты

        int currentTime = 0;//прошедшее время
        public int[] randoms = new int[6];//массив рандомных чисел
        //public для тестов
        int currentSide = 1;//текущая сторона

        Vector2 additionalVector = new Vector2(0, -100 * scale);//вектор для изменения координат
        /// <summary>
        /// Request for generate new roll
        /// </summary>
        /// <param name="diceNumber">Number of dice</param>
        /// <param name="prCount">previous count for randomizer</param>
        /// <returns>return random number</returns>
        public int NewRoll(int diceNumber, int prCount)
        {   
            randoms = new int[6];
            currentSide = 1;
            currentTime = 0;
            if (diceNumber == 1) return doRandom(diceNumber, 0);
            else return doRandom(diceNumber, prCount);
        }
        /// <summary>
        /// Draws a dices
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        /// <param name="numberOfDices">How many dices</param>
        /// <param name="team">team 0-blue, 1-pink</param>
        public void Draw(SpriteBatch sb, int numberOfDices, int team)
        {
            Vector2 placeVector;
            if (team == 0)
            {
                placeVector = new Vector2(1480, 30);
            }
            else if (team == 1) placeVector = new Vector2(1480, 560);
            else return;

            numberOfDices--;
            Vector2 shiftPr = new Vector2(numberOfDices * 110 * scale, 100 * scale);
            Vector2 shift = new Vector2(numberOfDices * 110 * scale, 0);

            currentTime += 16;
            if (currentSide <= 6)
            {
                needToDraw = true;
                if (currentTime >= timeForSide)//перелистывать, когда пришло время
                {
                    currentTime = 0;
                    currentSide++;//перелистывание
                    additionalVector = Vector2.Zero;
                }
                if (currentSide != 7)
                {
                    additionalVector += addTo;
                    Texture2D diceTexture = Game1.GetDiceTexture(randoms[currentSide - 1]);//получение текстуры стороны
                    if (currentSide != 1)
                    {
                        Texture2D dicePrevious = Game1.GetDiceTexture(randoms[currentSide - 2]);//получение текстуры предыдущей стороны
                        sb.Draw(dicePrevious, placeVector + shiftPr, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0.5f);//отрисовка предыдущей
                        sb.Draw(diceTexture, placeVector + additionalVector + shift, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);//отрисовка этой
                    }
                    else
                    {
                        sb.Draw(diceTexture, placeVector + shiftPr, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);//отрисовка этой
                    }
                }
            }

            if (currentSide == 7)
            {
                needToDraw = false;
                Texture2D diceTexture = Game1.GetDiceTexture(randoms[5]);//получение текстуры стороны
                sb.Draw(diceTexture, placeVector + shiftPr, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);//отрисовка
            }

        }
        /// <summary>
        /// Randoms a numbers for dices
        /// </summary>
        /// <param name="diceNumber">first or second number randoms</param>
        /// <param name="c">previous count of randoms</param>
        /// <returns>count of randoms</returns>
        public int doRandom(int diceNumber, int c)
        {//public для тестов
            Random rnd = new Random();
            int count = 0;
            if (diceNumber == 2)
            {
                for (int i = 0; i < c; i++)
                {
                    rnd.Next(1, 7);
                }
            }
            for (int i = 0; i < 6; i++)
            {
                bool write = true;
                int r;
                r = rnd.Next(1, 7);
                count++;
                for (int j = 0; j <= i; j++)
                {
                    if (randoms[j] == r)
                    {
                        write = false;
                        i--;
                        break;
                    }
                }
                if (write == true)
                {
                    randoms[i] = r;
                }
            }
            return count;
        }
        /// <summary>
        /// returns a last element of randoms array - final number
        /// </summary>
        /// <returns>a last element of randoms array - final number</returns>
        public int GetRandom()
        {
            if (randoms[5] == 0)
            {
                return 1;
            }
            else return randoms[5];
        }
    }
}
