using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace BattleOfSquares
{
    public class Square
    {
        public static Point sizeOfGrid = new Point(54, 54);

        static Color wrongPlaceColor = new Color(255, 0, 0, 255);
        SpriteBatch spriteBatch;
        public Square(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }
        public class SquareInfo
        {
            public Point position;
            public string name;
            public int team;
            public int rotate;
            int teamWas = 3;
            public bool wrong = false;
            public SquareInfo(Point position, string name, int rotate, int team)
            {
                this.position = position;
                this.name = name;
                this.team = team;
                this.rotate = rotate;
            }
            public SquareInfo(Point position, int width, int height, int rotate, int team)
            {
                this.position = position;
                name = height.ToString() + "-" + width.ToString();
                this.team = team;
                this.rotate = rotate;
            }
            public void ChangeRotate()
            {
                rotate = (rotate == 0) ? 1 : 0;
            }
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
            public void ChangeTeam()
            {
                team = (team == 0) ? 1 : 0;
            }
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
        public void Draw(int w, int h, int rotate, int team, int x, int y)
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

            string name = w.ToString() + "-" + h.ToString();

            Texture2D sqTexture = Game1.GetSquareTexture(name);

            spriteBatch.Draw(sqTexture, position, null, teamColor, 4.712f * rotate, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

        }
        public void Draw(string name, int rotate, int team, Point pos)
        {
            int w = Convert.ToInt16(name.Substring(0, 1));
            int h = Convert.ToInt16(name.Substring(2, 1));
            Draw(w, h, rotate, team, pos.X, pos.Y);
        }
        public void DrawInPixel(string name, int rotate, int team, Point pos, GraphicsDevice gd)
        {
            int w = Convert.ToInt16(name.Substring(0, 1));
            int h = Convert.ToInt16(name.Substring(2, 1));
            using (SpriteBatch sb = new SpriteBatch(gd))
            {
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
        }
        public static Point GetPoint(int x, int y)
        {//возвращает point по координатам сетки
            return new Point(x * 54 + (int)Game1.startPoint.X, y * 54);
        }
    }
}
