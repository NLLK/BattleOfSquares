using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
namespace BattleOfSquares
{
    /// <summary>
    /// A main class for controll all things that connected with game field and placing squares stuff
    /// </summary>
    public class GridSystem
    {
        public int[,] gridArray = new int[20, 20];
        //public is only for tests
        List<Square.SquareInfo> squaresList = new List<Square.SquareInfo>();
        int sumOfSquaresBlue;
        int sumOfSquaresPink;
        /// <summary>
        /// Checks is square in this position able to place
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="x">X of posiiton</param>
        /// <param name="y">Y of posiiton</param>
        /// <param name="rotate">is it rotate? 0 - no, 1 - yes</param>
        /// <param name="team">team 0-blue, 1-pink, 2-error</param>
        /// <returns>returns negative if it is impossible to fit, positive if there is only enemies squares, and zero if its all ok</returns>
        public int isItFit(int width, int height, int x, int y, int rotate, int team)
        {//вмещается ли?
            //если <0, то не вмещается вообще, если >0, то возле другой команды ставится, 0 если все впорядке 
            if (rotate == 1)
            {
                y += 1 - width;

                int temp = width;
                width = height;
                height = temp;
            }
            if (x + width > 20 || y + height > 20 || y < 0) return -1;
            for (int j = y; j < y + height; j++)
            {
                int sum = 0;
                for (int i = x; i < x + width; i++)
                {
                    sum += gridArray[j, i];
                    if (sum > 0 || gridArray[j, i] != 0)
                        return -1;
                }
            }

            return isOnRightPlace(width, height, x, y, team);
        }
        /// <summary>
        /// Another method for Checks is square in this position able to place, but it requares for squares name and position like a Point
        /// </summary>
        /// <param name="name">squares name in pattern "1-1"</param>
        /// <param name="rotate">is it rotate? 0 - no, 1 - yes</param>
        /// <param name="positionPoint">relative point where we place</param>
        /// <param name="team">team 0-blue, 1-pink, 2-error</param>
        /// <returns>returns negative if it is impossible to fit, positive if there is only enemies squares, and zero if its all ok</returns>
        public int isItFit(string name, int rotate, Point positionPoint, int team)
        {
            int w = Convert.ToInt16(name.Substring(0, 1));
            int h = Convert.ToInt16(name.Substring(2, 1));
            return isItFit(w, h, positionPoint.X, positionPoint.Y, rotate, team);
        }
        /// <summary>
        /// addition for isItFit that describes can you place your square in that position according on its coords and another squares
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="x">X of posiiton</param>
        /// <param name="y">Y of posiiton</param>
        /// <param name="team">team 0-blue, 1-pink, 2-error</param>
        /// <returns>zero is for right place and 1 for it is not</returns>
        public int isOnRightPlace(int width, int height, int x, int y, int team)
        {//на правильное ли место ставим?
            //если 0 - на верное место. Если 1 - нет
            if (team == 0)//синие
            {
                int incJ = 0;//если на границе, надо учитывать на 1 строку меньше
                int incI = 0;//если на границе, надо учитывать на 1 строку меньше
                if (x == 0 && y == 0)//для первого
                    return 0;
                if (x == 0)//касается левой границы
                    incI++;
                if (y == 0)//касается верхней границы
                    incJ++;
                if (x + width > 19)//касается правой границы
                    width--;
                if (y + height > 19)//касается нижней границы
                    height--;
                for (int j = y - 1 + incJ; j < y + height + 1; j++)//есть ли рядом что-то
                {
                    for (int i = x - 1 + incI; i < x + width + 1; i++)
                    {
                        if (gridArray[j, i] % 2 == 1)
                            return 0;
                    }
                }
            }
            else if (team == 1)//розовые
            {
                int incJ=0;//если на границе, надо учитывать на 1 строку меньше
                int incI=0;//если на границе, надо учитывать на 1 строку меньше
                if (x + width == 20 && y + height == 20)//для первого
                    return 0;
                if (x == 0)//касается левой границы
                    incI++;
                if (y == 0)//касается верхней границы
                    incJ++;
                if (x + width > 19)//касается правой границы
                    width--;
                if (y + height > 19)//касается нижней границы
                    height--;
                for (int j = y - 1+incJ; j < y + height + 1; j++)//есть ли рядом что-то
                {
                    for (int i = x - 1 + incI; i < x + width + 1; i++)
                    {
                        if (gridArray[j, i] != 0 && gridArray[j, i] % 2 == 0)
                            return 0;
                    }
                }
            }
            return 1;
        }
        /// <summary>
        /// identify can you place your square in any position near your squares
        /// </summary>
        /// <param name="name">squares name in patten "1-1"</param>
        /// <param name="team">team 0-blue, 1-pink, 2-error</param>
        /// <returns>true if it's the end, false if it is not</returns>
        public bool isItTheEnd(string name, int team)
        {
            bool isIt = true;
            int w = Convert.ToInt16(name.Substring(0, 1));
            int h = Convert.ToInt16(name.Substring(2, 1));
            for (int j = 0; j <= 19; j++)
            {
                for (int i = 0; i <= 19; i++)
                {
                    if (gridArray[j, i] == 0)
                    {
                        if (isItFit(w, h, i, j, 0, team) == 0 || isItFit(w, h, i, j, 1, team) == 0)//если в точку помещается повернутый или не повернутый прямоугольник, то еще можно продолжать
                        {
                            if (isIt == true)
                                isIt = false;
                        }
                    }
                }
            }
            return isIt;
        }
        /// <summary>
        /// Main method for adding squares in system
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="x">X of posiiton</param>
        /// <param name="y">Y of posiiton</param>
        /// <param name="team">team 0-blue, 1-pink, 2-error</param>
        /// <param name="rotate">is it rotate? 0 - no, 1 - yes</param>
        public void addSquare(int width, int height, int rotate, int team, int x, int y)
        {
            Point coords = new Point(x, y);

            if (isItFit(width, height, x, y, rotate, team) == 0)
            {
                Square.SquareInfo el = new Square.SquareInfo(coords, height.ToString() + "-" + width.ToString(), rotate, team);
                squaresList.Add(el);
                el = null;
                if (squaresList.Count % 2 == 1)
                {
                    sumOfSquaresBlue += height * width;
                }
                else sumOfSquaresPink += height * width;

                if (rotate == 1)
                {
                    y += 1 - width;

                    int temp = width;
                    width = height;
                    height = temp;
                }
                for (int j = y; j < y + height; j++)
                {
                    for (int i = x; i < x + width; i++)
                    {
                        gridArray[j, i] = squaresList.Count;
                    }
                }
                Console.Write("{");
                for (int i=0;i<20;i++)//вывод в консоль массива
                {
                    Console.Write("{");
                    for (int j=0;j<19;j++)
                    {
                        Console.Write(gridArray[i,j].ToString()+",");
                    }
                    Console.Write(gridArray[i, 19].ToString()+"},\n");
                }
                Console.Write("}\n");


            }
        }
        /// <summary>
        /// Another method for adding squares in system
        /// </summary>
        /// <param name="name">squares name in pattern "1-1"</param>
        /// <param name="rotate">is it rotate? 0 - no, 1 - yes</param> 
        /// <param name="team">team 0-blue, 1-pink, 2-error</param>
        /// <param name="positionPoint">relative point where we place</param>
        public void addSquare(string name, int rotate, int team, Point positionPoint)
        {
            int w = Convert.ToInt16(name.Substring(0, 1));
            int h = Convert.ToInt16(name.Substring(2, 1));
            addSquare(w, h, rotate, team, positionPoint.X, positionPoint.Y);
        }
        /// <summary>
        /// Draw both squares and score
        /// </summary>
        /// <param name="spriteBatch">Sprite Batch</param>
        public void DrawAll(SpriteBatch spriteBatch)
        {
            DrawSquares(spriteBatch);
            DrawScore(spriteBatch);
        }
        /// <summary>
        /// Draw squares from squares list
        /// </summary>
        /// <param name="spriteBatch">Sprite Batch</param>
        public void DrawSquares(SpriteBatch spriteBatch)
        {
            for (int n = 0; n < squaresList.Count; n++)
            {
                Square sq = new Square(spriteBatch);
                Square.SquareInfo el = squaresList[n];
                sq.Draw(el.name, el.rotate, el.team, el.position);
                sq = null;
            }
        }
        /// <summary>
        /// Draw only score
        /// </summary>
        /// <param name="spriteBatch">Sprite Batch</param>
        private void DrawScore(SpriteBatch spriteBatch)
        {
            Color blueTeamColor = new Color(102, 153, 255, 255);
            Color pinkTeamColor = new Color(255, 51, 153, 255);

            SpriteFont score = Game1.scoreDisplay;

            spriteBatch.DrawString(score, "Score: " + sumOfSquaresBlue.ToString(), new Vector2(1480, 10), blueTeamColor);
            spriteBatch.DrawString(score, "Score: " + sumOfSquaresPink.ToString(), new Vector2(1480, 540 + 10), pinkTeamColor);
        }
        /// <summary>
        /// Clears all Grid System includes 
        /// </summary>
        public void Clear()
        {
            squaresList.Clear();
            gridArray = new int[20, 20];
            sumOfSquaresBlue = 0;
            sumOfSquaresPink = 0;
        }
    }
}
