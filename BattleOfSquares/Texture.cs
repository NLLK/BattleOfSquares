using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleOfSquares
{
    public class Texture
    {//объект texture2d, но с именем отдельно
        public Texture2D texture;//текстура 
        public string name;//имя, без имени папки
        public int number;//номер, для перегрузки с интом
        public Texture(Texture2D t, string n)//конструктор для квадратиков
        {
            texture = t;
            name = n;
        }
        public Texture(Texture2D t, int n)//конструктор для костей
        {
            texture = t;
            number = n;
        }
    }
}
