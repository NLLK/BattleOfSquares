using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleOfSquares
{
    public class GridCoords
    {//координаты в сетке
        public static Point GetPoint(int x, int y)
        {//возвращает point по координатам сетки
            return new Point(x * 54 + (int)Game1.startPoint.X, y * 54);
        }
        public static Point GetXY(Point point)
        {
            return new Point((point.X - (int)Game1.startPoint.X) / 54, (point.Y / 54));
        }
    }
}
