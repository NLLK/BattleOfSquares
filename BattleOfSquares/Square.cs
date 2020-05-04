using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace BattleOfSquares
{
    //class for drawing squares
    public class Square
    {
        public static Point sizeOfGrid = new Point(54, 54);

        static Color wrongPlaceColor = new Color(255, 0, 0, 255);
        SpriteBatch spriteBatch;
        public Square(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }
        //Class for containing info about square
        public class SquareInfo
        {
            public Point position;
            public string name;
            public int team;
            public int rotate;
            int teamWas = 3;
            public bool wrong = false;
            /// <summary>
            /// Sets info
            /// </summary>
            /// <param name="position">Position point in relative points</param>
            /// <param name="name">name with template "widht-height"</param>
            /// <param name="rotate">if it rotate - 1, or 0 if its not</param>
            /// <param name="team">team: 0-blue, 1 - pink, 2 - error</param>
            public SquareInfo(Point position, string name, int rotate, int team)
            {
                this.position = position;
                this.name = name;
                this.team = team;
                this.rotate = rotate;
            }
            /// <summary>
            /// Sets info
            /// </summary>
            /// <param name="position">Position point in relative points</param>
            /// <param name="width">width</param>
            /// <param name="height">height</param>
            /// <param name="rotate">if it rotate - 1, or 0 if its not</param>
            /// <param name="team">team: 0-blue, 1 - pink, 2 - error</param>
            public SquareInfo(Point position, int width, int height, int rotate, int team)
            {
                this.position = position;
                name = height.ToString() + "-" + width.ToString();
                this.team = team;
                this.rotate = rotate;
            }
            /// <summary>
            /// Changes rotate
            /// </summary>
            public void ChangeRotate()
            {
                rotate = (rotate == 0) ? 1 : 0;
            }
            /// <summary>
            /// requares dices for set name in game
            /// </summary>
            /// <param name="dice1">number on fisrt dice</param>
            /// <param name="dice2">number on second dice</param>
            public void ChangeDices(int dice1, int dice2)
            {
                int x = dice1;
                int y = dice2;
                if (y < x)
                {
                    int t = x;
                    x = y;
                    y = t;
                }
                name = x.ToString() + "-" + y.ToString();
            }
            /// <summary>
            /// changes team
            /// </summary>
            public void ChangeTeam()
            {
                team = (team == 0) ? 1 : 0;
            }
            /// <summary>
            /// requares isIt int from external class for remember team and set it in wrong (for draw a error square)
            /// </summary>
            /// <param name="isIt">if it is wrong - 1, else 0</param>
            public void WrongPlace(int isIt)
            {
                if (isIt == 1)
                {
                    teamWas = team;
                    team = 2;
                    wrong = true;
                }
                else
                {
                    if (teamWas != 3)
                    {
                        team = teamWas;
                        teamWas = 3;
                        wrong = false;
                    }
                }
            }
        }
        /// <summary>
        /// Draws a square
        /// </summary>
        /// <param name="width">width of square</param>
        /// <param name="height">height of square</param>
        /// <param name="rotate">is it rotate? 0 - no, 1 - yes</param>
        /// <param name="team">team 0-blue, 1-pink, 2-error</param>
        /// <param name="x">X of posiiton</param>
        /// <param name="y">Y of posiiton</param>
        public void Draw(int width, int height, int rotate, int team, int x, int y)
        {
            Vector2 position;
            if (rotate == 1)
            {
                position = GetPoint(x, y + 1).ToVector2();
            }
            else position = GetPoint(x, y).ToVector2();
            Color teamColor;
            switch (team)
            {
                case 0: { teamColor = Game1.blueTeamColor; break; }
                case 1: { teamColor = Game1.pinkTeamColor; break; }
                case 2: { teamColor = wrongPlaceColor; break; }
                default: { teamColor = Color.Black; break; }
            }

            string name = width.ToString() + "-" + height.ToString();

            Texture2D sqTexture = Game1.GetSquareTexture(name);

            spriteBatch.Draw(sqTexture, position, null, teamColor, 4.712f * rotate, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

        }
        /// <summary>
        /// Draws a square
        /// </summary>
        /// <param name="name">name with template "widht-height"</param>
        /// <param name="rotate">is it rotate? 0 - no, 1 - yes</param>
        /// <param name="team">team 0-blue, 1-pink, 2-error</param>
        /// <param name="pos">X and Y in relative Point</param>
        public void Draw(string name, int rotate, int team, Point pos)
        {
            int w = Convert.ToInt16(name.Substring(0, 1));
            int h = Convert.ToInt16(name.Substring(2, 1));
            Draw(w, h, rotate, team, pos.X, pos.Y);
        }
        /// <summary>
        /// Draw square in pixel
        /// </summary>
        /// <param name="name">name with template "widht-height"</param>
        /// <param name="rotate">is it rotate? 0 - no, 1 - yes</param>
        /// <param name="team">team 0-blue, 1-pink, 2-error</param>
        /// <param name="pos">X and Y in absoulte Point</param>
        /// <param name="spriteBatch">Sprite Batch</param>
        public void DrawInPixel(string name, int rotate, int team, Point pos, SpriteBatch spriteBatch)
        {
            int w = Convert.ToInt16(name.Substring(0, 1));
            int h = Convert.ToInt16(name.Substring(2, 1));
            Vector2 position;
            if (rotate == 1) position = pos.ToVector2() + new Vector2(0, 54);
            else position = pos.ToVector2();

            Color teamColor;
            switch (team)
            {
                case 0: { teamColor = Game1.blueTeamColor; break; }
                case 1: { teamColor = Game1.pinkTeamColor; break; }
                case 2: { teamColor = wrongPlaceColor; break; }
                default: { teamColor = Color.Black; break; }
            }

            Texture2D sqTexture = Game1.GetSquareTexture(name);

            spriteBatch.Draw(sqTexture, position, null, teamColor, 4.712f * rotate, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }
        /// <summary>
        /// Returns a absolute point by x and y of relative
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>Point in absolute presentation</returns>
        public static Point GetPoint(int x, int y)
        {//возвращает point по координатам сетки
            return new Point(x * 54 + (int)Game1.startPoint.X, y * 54);
        }
    }
}
