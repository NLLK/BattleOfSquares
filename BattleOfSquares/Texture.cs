using Microsoft.Xna.Framework.Graphics;

namespace BattleOfSquares
{
    /// <summary>
    /// an addition class that helps contain Texture2D and its name or number
    /// </summary>
    public class Texture
    {//объект texture2d, но с именем отдельно
        public Texture2D texture;//текстура 
        public string name;//имя, без имени папки
        public int number;//номер, для перегрузки с интом
        /// <summary>
        /// Init for squares
        /// </summary>
        /// <param name="t">Texture2D</param>
        /// <param name="n">name of square</param>
        public Texture(Texture2D t, string n)//конструктор для квадратиков
        {
            texture = t;
            name = n;
        }
        /// <summary>
        /// Init for dices
        /// </summary>
        /// <param name="t">Texture2D</param>
        /// <param name="n">number of dice</param>
        public Texture(Texture2D t, int n)//конструктор для костей
        {
            texture = t;
            number = n;
        }
    }
}
