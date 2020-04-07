using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BattleOfSquares
{
    public class Texture
    {//объект texture2d, но с именем отдельно
        public Texture2D texture;
        public string name;
        public Texture(Texture2D t, string n)
        {
            texture = t;
            name = n;
        }
    }
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
    public class Square
    {
        public static Point sizeOfGrid = new Point(54, 54);

        static Color blueTeamColor = new Color(102, 153, 255, 255);
        static Color pinkTeamColor = new Color(255, 51, 153, 255);
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
            public void Random()
            {
                Random rnd = new Random();
                int x = rnd.Next(1, 6);
                int y = rnd.Next(1, 6);
                if (y < x)
                {
                    int t = x;
                    x = y;
                    y = t;
                }
                name = x.ToString() + "-" + y.ToString();
                Console.WriteLine("Generated: x y: " + name);
            }
            public void ChangeTeam()
            {
                Random();
                team = (team == 0) ? 1 : 0;
            }
        }
        public void Draw(int w, int h, int rotate, int team, int x, int y, GraphicsDevice gd)
        {
            Vector2 position;
            if (rotate == 1)
            {
                position = GridCoords.GetPoint(x, y+1).ToVector2();
            }
           else position = GridCoords.GetPoint(x, y).ToVector2();
            Color teamColor = (team == 0) ? blueTeamColor : pinkTeamColor;

            string name = w.ToString() + "-" + h.ToString();

            Texture2D sqTexture = Game1.GetTexture(name);

            spriteBatch.Draw(sqTexture, position, null, teamColor, 4.712f * rotate, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

        }
        public void Draw(string name, int rotate, int team, Point pos, GraphicsDevice gd)
        {
            int w = Convert.ToInt16(name.Substring(0, 1));
            int h = Convert.ToInt16(name.Substring(2, 1));
            Draw(w, h, rotate, team, pos.X, pos.Y, gd);
        }
        public void DrawInPixel(string name, int rotate, int team, Point pos, GraphicsDevice gd)
        {
            int w = Convert.ToInt16(name.Substring(0, 1));
            int h = Convert.ToInt16(name.Substring(2, 1));
            //Point p = GridCoords.GetXY(pos);
            using (SpriteBatch sb = new SpriteBatch(gd))
            {
                Vector2 position;
                if (rotate==1) position = pos.ToVector2()+new Vector2(0,54);
                else position = pos.ToVector2();

                Color teamColor = (team == 0) ? blueTeamColor : pinkTeamColor;

                Texture2D sqTexture = Game1.GetTexture(name);

                spriteBatch.Draw(sqTexture, position, null, teamColor, 4.712f * rotate, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            }
        }
    }
    public class GridSystem
    {
        GraphicsDevice gd;
        int[,] gridArray = new int[20, 20];

        List<Square.SquareInfo> squaresList = new List<Square.SquareInfo>();

        public GridSystem(GraphicsDevice gd)
        {
            this.gd = gd;
        }

        public bool isItFit(int width, int height, int x, int y, int rotate)
        {
            if (rotate == 1)
            {//перевернутое
                if (x > 19 || y  > 19 || y-width+1 <0) return false;
                for (int j=y-width+1; j<=y; j++)
                {
                    int sum = 0;
                    for (int i = x; i < x + height; i++)
                    {
                        sum += gridArray[j, i];
                        if (sum>0 || gridArray[j, i]!=0)
                            return false;
                    }
                }
            }
            else
            {//вертикальное
                if (x + width > 20 || y + height > 20 ) return false;
                for (int j=y;j<y+height; j++)
                {
                    int sum = 0;
                    for (int i = x; i < x + width; i++)
                    {
                        sum += gridArray[j, i];
                        if (sum > 0 || gridArray[j, i] != 0)
                            return false;
                    }
                }
            }

            return true;
        }

        public void addSquare(int width, int height, int rotate, int team, int x, int y)
        {
            Point coords = new Point(x, y);

            if (isItFit(width, height, x, y, rotate))
            {
                Square.SquareInfo el = new Square.SquareInfo(coords, height.ToString() + "-" + width.ToString(), rotate, team);
                squaresList.Add(el);

                if (rotate == 1)
                {//горизонтальное
                    for (int j=y-width+1;j<=y; j++)
                    {
                        for (int i=x; i<x+height; i++)
                        {
                            gridArray[j, i] = squaresList.Count;
                        }
                    }
                }
                else
                {//вертикальное
                    for (int j=y; j<y+height; j++)
                    {
                        for (int i=x; i<x+width;i++)
                        {
                            gridArray[j, i] = squaresList.Count;
                        }
                    }
                }
                el = null;
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
                sq.Draw(el.name, el.rotate, el.team, el.position, gd);
                sq = null;
            }
        }
        public void ClearSquares()
        {
            squaresList.Clear();
            gridArray = new int[20, 20];
        }
    }
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState lastMouseState;

        Texture2D fieldTexture;

        int currentTimeKeyboard = 0; // сколько времени прошло, для клавиатуры
        int periodKeyboard = 150; // частота обновления в миллисекундах

        int currentTimeMouse = 0; // сколько времени прошло, для клавиатуры
        int periodMouse = 50; // частота обновления в миллисекундах

        Point mousePosition;
        Point positionPoint = new Point(0, 0);

        GridSystem gridSystem;

        Square.SquareInfo placingSquare;

        static ArrayList listOfTextures = new ArrayList();
        public static Vector2 startPoint = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 - 590, 0);
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.ApplyChanges();

          //  graphics.ToggleFullScreen();  

            gridSystem = new GridSystem(GraphicsDevice);

            GraphicsDevice.Clear(Color.White);
            placingSquare = new Square.SquareInfo(new Point(0, 0), "1-1", 0, 0);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            fieldTexture = Content.Load<Texture2D>("field");
            for (int i = 1; i <= 6; i++)
            {
                for (int j = i; j <= 6; j++)
                {
                    string name = i.ToString() + "-" + j.ToString();
                    string reverseName = j.ToString() + "-" + i.ToString();
                    string place = "squares\\" + name;

                    listOfTextures.Add(new Texture(Content.Load<Texture2D>(place), name + " " + reverseName));
                }
            }

        }

        protected override void UnloadContent()
        { }

        protected override void Update(GameTime gameTime)
        {
            currentTimeKeyboard += gameTime.ElapsedGameTime.Milliseconds;
            currentTimeMouse += gameTime.ElapsedGameTime.Milliseconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.X != lastMouseState.X || currentMouseState.Y != lastMouseState.Y)//мышка сдвинулась вообще
            {
                if ((currentMouseState.X > startPoint.X && currentMouseState.X < startPoint.X + 1080) && (currentMouseState.Y > 0 && currentMouseState.Y < 1080))
                {
                    positionPoint = new Point((int)((currentMouseState.X - startPoint.X) / Square.sizeOfGrid.X), (int)((currentMouseState.Y - startPoint.Y) / Square.sizeOfGrid.Y));
                    mousePosition = new Point(currentMouseState.X-27, currentMouseState.Y-27);//возможно стоит переделать для удобства
                }

            }

            lastMouseState = currentMouseState;
            if (currentTimeKeyboard > periodKeyboard)
            {
                currentTimeKeyboard -= periodKeyboard;
                KeyboardState keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.R))
                {
                    placingSquare.ChangeRotate();
                }
                if (keyboardState.IsKeyDown(Keys.F))
                {
                    placingSquare.Random();
                }
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    placingSquare.ChangeTeam();
                }
            }
            if (currentTimeMouse > periodMouse)
            {
                currentTimeMouse -= periodMouse;
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    Console.WriteLine("pressed:" +positionPoint.X.ToString() + "; " + positionPoint.Y.ToString());
                    gridSystem.addSquare(placingSquare.name, placingSquare.rotate, placingSquare.team, positionPoint);
                }
                if (currentMouseState.RightButton == ButtonState.Pressed)
                {
                    gridSystem.ClearSquares();
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            using (spriteBatch = new SpriteBatch(GraphicsDevice))
            {

                spriteBatch.Begin(SpriteSortMode.Immediate);

                spriteBatch.Draw(fieldTexture, startPoint, null, new Color(255, 255, 255, 120), 0, Vector2.Zero, 1f, SpriteEffects.None, 0); //поле
                gridSystem.DrawAll(spriteBatch);//все квадратики
                Square sq = new Square(spriteBatch);

                sq.Draw(placingSquare.name, placingSquare.rotate, placingSquare.team, positionPoint, GraphicsDevice);
                sq.DrawInPixel("1-1", placingSquare.rotate, placingSquare.team, mousePosition , GraphicsDevice);

                spriteBatch.End();

            }

            base.Draw(gameTime);
        }
        public static Texture2D GetTexture(string sqName)
        {
            for (int i = 0; i < listOfTextures.Count; i++)
            {
                if (((Texture)listOfTextures[i]).name.IndexOf(sqName) != -1)
                {
                    return ((Texture)listOfTextures[i]).texture;
                }
            }
            return null;
        }
    }
}
