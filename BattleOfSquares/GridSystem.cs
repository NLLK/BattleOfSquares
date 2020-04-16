using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleOfSquares
{

    public class GridSystem
    {
        int[,] gridArray = new int[20, 20];

        List<Square.SquareInfo> squaresList = new List<Square.SquareInfo>();
        int sumOfSquaresBlue;
        int sumOfSquaresPink;
        public bool isItFit(int width, int height, int x, int y, int rotate, int team)
        {//вмещается ли?
            if (rotate == 1)
            {
                y += 1 - width;

                int temp = width;
                width = height;
                height = temp;
            }
            if (x + width > 20 || y + height > 20 || y < 0) return false;
            for (int j = y; j < y + height; j++)
            {
                int sum = 0;
                for (int i = x; i < x + width; i++)
                {
                    sum += gridArray[j, i];
                    if (sum > 0 || gridArray[j, i] != 0)
                        return false;
                }
            }

            return isOnRightPlace(width, height, x, y, team);
        }
        public bool isItFit(string name, int rotate, Point positionPoint, int team)
        {
            int w = Convert.ToInt16(name.Substring(0, 1));
            int h = Convert.ToInt16(name.Substring(2, 1));
            return isItFit(w, h, positionPoint.X, positionPoint.Y, rotate, team);
        }
        public bool isOnRightPlace(int width, int height, int x, int y, int team)//на правильное ли место ставим?
        {
            if (team == 0)//синие
            {
                if (x == 0 && y == 0)//для первого
                    return true;
                if (x == 0)//касается левой границы
                    x++;
                if (y == 0)//касается верхней границы
                    y++;
                if (x + width > 19)//касается правой границы
                    width--;
                if (y + height > 19)//касается нижней границы
                    height--;
                for (int j = y - 1; j < y + height; j++)//есть ли рядом что-то
                {
                    for (int i = x - 1; i < x + width; i++)
                    {
                        if (gridArray[j, i] % 2 == 1) return true;
                    }
                }
            }
            else if (team == 1)//розовые
            {

                if (x + width == 20 && y + height == 20)//для первого
                    return true;
                if (x == 0)//касается левой границы
                    x++;
                if (y == 0)//касается верхней границы
                    y++;
                if (x + width > 19)//касается правой границы
                    width--;
                if (y + height > 19)//касается нижней границы
                    height--;
                for (int j = y - 1; j < y + height + 1; j++)//есть ли рядом что-то
                {
                    for (int i = x - 1; i < x + width + 1; i++)
                    {
                        if (gridArray[j, i] != 0 && gridArray[j, i] % 2 == 0) return true;
                    }
                }
            }
            return false;
        }
        public bool isItTheEnd()
        {


            for (int j = 0; j <= 19; j++)
            {
                for (int i = 0; i <= 19; i++)
                {
                    if (gridArray[j, i] == 0)
                    {

                    }
                }
            }
            return false;
        }
        public void addSquare(int width, int height, int rotate, int team, int x, int y)
        {
            Point coords = new Point(x, y);

            if (isItFit(width, height, x, y, rotate, team))
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
                for (int i = 0; i <= 19; i++)
                {
                    for (int j = 0; j <= 19; j++)
                    {
                        if (gridArray[i, j] >= 10) Console.Write(gridArray[i, j].ToString() + ",");
                        else
                            Console.Write(gridArray[i, j].ToString() + " ,");
                    }
                    Console.WriteLine("\n");
                }
            }
        }
        public void addSquare(string name, int rotate, int team, Point positionPoint)
        {
            int w = Convert.ToInt16(name.Substring(0, 1));
            int h = Convert.ToInt16(name.Substring(2, 1));
            addSquare(w, h, rotate, team, positionPoint.X, positionPoint.Y);
        }
        public void DrawAll(SpriteBatch spriteBatch)
        {
            for (int n = 0; n < squaresList.Count; n++)
            {
                Square sq = new Square(spriteBatch);
                Square.SquareInfo el = squaresList[n];
                sq.Draw(el.name, el.rotate, el.team, el.position);
                sq = null;
            }
            DrawScore(spriteBatch);
        }
        private void DrawScore(SpriteBatch spriteBatch)
        {
            Color blueTeamColor = new Color(102, 153, 255, 255);
            Color pinkTeamColor = new Color(255, 51, 153, 255);

            SpriteFont scoreBlue = Game1.GetScoreSpriteFont(0);
            SpriteFont scorePink = Game1.GetScoreSpriteFont(1);
            spriteBatch.DrawString(scoreBlue, sumOfSquaresBlue.ToString(), new Vector2(1500, 100), blueTeamColor);
            spriteBatch.DrawString(scorePink, sumOfSquaresPink.ToString(), new Vector2(1500, 150), pinkTeamColor);
        }
        public void ClearSquares()
        {
            squaresList.Clear();
            gridArray = new int[20, 20];
            sumOfSquaresBlue = 0;
            sumOfSquaresPink = 0;
        }
    }
}
