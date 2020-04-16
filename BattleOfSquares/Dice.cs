using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BattleOfSquares
{
    public class Dice
    {
        public bool needToDraw;//нужно ли рисовать

        static int timeForSide = 300;//сколько времени на одну сторону кости
        static Vector2 startPoint = new Vector2(200, 100);//если в определенной точке, то нужна начальная точка

        static float scale = 0.7f;//масштаб
        static Vector2 addTo = new Vector2(0, 5 * scale);//вектор, на сколько изменять координаты

        int currentTime = 0;//прошедшее время
        int[] randoms = new int[6];//массив рандомных чисел
        int currentSide = 1;//текущая сторона

        Vector2 additionalVector = new Vector2(0, -100 * scale);//вектор для изменения координат

        public int NewRoll(int diceNumber, int prCount)
        {//запрос на отрисовку
            needToDraw = true;
            randoms = new int[6];
            currentSide = 1;
            currentTime = 0;
            if (diceNumber == 1) return doRandom(diceNumber, 0);
            else return doRandom(diceNumber, prCount);
        }
        public void Draw(SpriteBatch sb, int numberOfDices, Point mousePosition)
        {
            Vector2 mouseVector = mousePosition.ToVector2() + new Vector2(0, -200 * scale);

            numberOfDices--;
            Vector2 shiftPr = new Vector2(numberOfDices * 110 * scale, 100 * scale);
            Vector2 shift = new Vector2(numberOfDices * 110 * scale, 0);
            if (needToDraw == true)
            {
                currentTime += 16;
                if (currentSide <= 6)
                {
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
                            sb.Draw(dicePrevious, mouseVector + shiftPr, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0.5f);//отрисовка предыдущей
                            sb.Draw(diceTexture, mouseVector + additionalVector + shift, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);//отрисовка этой
                        }
                        else
                        {
                            sb.Draw(diceTexture, mouseVector + shiftPr, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);//отрисовка этой
                        }
                    }
                }

                if (currentSide == 7)
                {
                    if (currentTime <= 2 * timeForSide)
                    {
                        Texture2D diceTexture = Game1.GetDiceTexture(randoms[5]);//получение текстуры стороны
                        sb.Draw(diceTexture, mouseVector + shiftPr, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);//отрисовка
                    }
                    else
                    {
                        needToDraw = false;
                    }

                }
            }
        }
        int doRandom(int diceNumber, int c)
        {
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
