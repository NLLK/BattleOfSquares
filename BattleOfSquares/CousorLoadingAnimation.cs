using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleOfSquares
{
    /// <summary>
    /// A Class for draw a coursor loading animation
    /// </summary>
    class CousorLoadingAnimation
    {
        public bool needToDraw=true;//нужно ли рисовать

        static float scale = 0.7f;//масштаб
        static Vector2 size = new Vector2(54, 54) * scale;
        static Vector2 addTo = new Vector2(5, 5) * scale;//вектор, на сколько изменять координаты
        Vector2 additionVector1 = Vector2.Zero;//куда добавлять для первого
        Vector2 additionVector2 = Vector2.Zero;//куда добавлять для второго

        int currentCorner = 1;//текущая сторона у голубого
        //1-------->2
        //|         |
        //3<________4
        /// <summary>
        /// Main method that draws a animation
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        /// <param name="mousePoint">Point where needs to draw</param>
        public void Draw(SpriteBatch sb, Point mousePoint)
        {
            Vector2 placeVector = mousePoint.ToVector2();
            Texture2D square = Game1.GetSquareTexture("1-1");
            if (needToDraw == true)
            {
                switch (currentCorner)
                {
                    case 1:
                        {
                            if (additionVector1.X <= size.X)
                            {
                                additionVector1 += new Vector2(addTo.X, 0);
                                additionVector2 += new Vector2(-addTo.X, 0);
                            }
                            else currentCorner++;
                            break;
                        }
                    case 2:
                        {
                            if (additionVector1.Y <= size.Y)
                            {
                                additionVector1 += new Vector2(0, addTo.Y);
                                additionVector2 += new Vector2(0, -addTo.Y);
                            }
                            else currentCorner++;
                            break;
                        }
                    case 3:
                        {
                            if (additionVector1.X > 0)
                            {
                                additionVector1 += new Vector2(-addTo.X, 0);
                                additionVector2 += new Vector2(addTo.X, 0);
                            }
                            else currentCorner++;
                            break;
                        }
                    case 4:
                        {
                            if (additionVector1.Y >0)
                            {
                                additionVector1 += new Vector2(0, -addTo.Y);
                                additionVector2 += new Vector2(0, addTo.Y);
                            }
                            else currentCorner=1;
                            break;
                        }
                }
                
                sb.Draw(square, placeVector + additionVector1, null, Game1.blueTeamColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
                sb.Draw(square, placeVector + size + additionVector2, null, Game1.pinkTeamColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
            }
        }
    }
}
