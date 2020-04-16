using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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
            public void ChangeTeam(int dice1, int dice2)
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
                Console.WriteLine("Generated: x y: " + name);
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
                position = GridCoords.GetPoint(x, y + 1).ToVector2();
            }
            else position = GridCoords.GetPoint(x, y).ToVector2();
            Color teamColor;
            switch (team)
            {
                case 0: { teamColor = blueTeamColor; break; }
                case 1: { teamColor = pinkTeamColor; break; }
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
                    case 0: { teamColor = blueTeamColor; break; }
                    case 1: { teamColor = pinkTeamColor; break; }
                    case 2: { teamColor = wrongPlaceColor; break; }
                    default: { teamColor = Color.Black; break; }
                }

                Texture2D sqTexture = Game1.GetSquareTexture(name);

                spriteBatch.Draw(sqTexture, position, null, teamColor, 4.712f * rotate, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            }
        }
    }
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
            if (x + width > 20 || y + height > 20|| y<0) return false;
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
                        if (gridArray[i,j]>=10) Console.Write(gridArray[i, j].ToString() + ",");
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
            //пиздец. Передавать массив другого дайса и сделать так, чтобы хотя бы первые 5 были другие. Ну или прочекать алгоритм с сайта лучше
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
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState lastMouseState;//положение мыши для сравнения на изменения ее пололжения

        Texture2D fieldTexture;//текстура поля
        Texture2D startMenuTexture;//текстура стартового меню
        Texture2D startMenuStButton;//текстура кнопки старт стартового меню
        Texture2D startMenuStPrButton;//текстура кнопки старт стартового меню в нажатом состоянии
        Texture2D startMenuCoursor;//текстура курсора стартового меню
        SpriteFont controlHelp;

        bool hideHelp = false;//скрывать подсказку

        int currentTimeKeyboard = 0; // сколько времени прошло, для клавиатуры
        int periodKeyboard = 150; // частота обновления в миллисекундах
        int currentTimeWrong = 0;
        int periodWrong = 300;

        bool pressed = false;

        Point mousePosition;
        Point positionPoint = new Point(0, 0);

        GridSystem gridSystem;//управление сеткой и прямоугольниками на ней
        Dice dice;//кости, анимация и рандом
        Dice dice2;//кости, анимация и рандом, 2 кость
        Square.SquareInfo placingSquare;//квадратик, который будем ставить, информация о нем

        static List<Texture> squareTextures = new List<Texture>();//текстуры прямоугольников
        static List<Texture> diceTextures = new List<Texture>();//текстуры костей
        static List<SpriteFont> scoreSpriteFont = new List<SpriteFont>();//spriteFont для очков

        public static Vector2 startPoint = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 - 590, 0);//начальная точка отрисовки поля

        int pageNumber = 0;//номер страницы - определяет рисовать меню - 0, или игру - 1

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

            gridSystem = new GridSystem();
            dice = new Dice();
            dice2 = new Dice();
            GraphicsDevice.Clear(Color.White);
            placingSquare = new Square.SquareInfo(new Point(0, 0), "1-1", 0, 1);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            fieldTexture = Content.Load<Texture2D>("field");
            startMenuTexture = Content.Load<Texture2D>("StartMenu\\startMenu");
            startMenuStButton = Content.Load<Texture2D>("StartMenu\\button");
            startMenuStPrButton = Content.Load<Texture2D>("StartMenu\\pressedButton");
            startMenuCoursor = Content.Load<Texture2D>("StartMenu\\coursor");
            controlHelp = Content.Load<SpriteFont>("controlHelp"); 
            for (int i = 1; i <= 6; i++) //заполняем список текстур квадратиков
            {
                for (int j = i; j <= 6; j++)
                {
                    string name = i.ToString() + "-" + j.ToString();
                    string reverseName = j.ToString() + "-" + i.ToString();
                    string place = "squares\\" + name;

                    squareTextures.Add(new Texture(Content.Load<Texture2D>(place), name + " " + reverseName)); //записываем в спискок текстуру и название прямоугольника в двух видах, чтобы успешно осуществлять поиск в GetSquareTexture
                }
            }
            for (int i = 1; i <= 6; i++)//заполняем список костей
            {
                string place = "dices\\" + i.ToString();
                diceTextures.Add(new Texture(Content.Load<Texture2D>(place), i));
            }
            scoreSpriteFont.Add(Content.Load<SpriteFont>("scoreBlue"));
            scoreSpriteFont.Add(Content.Load<SpriteFont>("scorePink"));
            controlHelp = Content.Load<SpriteFont>("controlHelp");
        }

        protected override void UnloadContent()
        { }
        protected override void Update(GameTime gameTime)
        {
            currentTimeKeyboard += gameTime.ElapsedGameTime.Milliseconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            switch (pageNumber)
            {
                case 0:
                    {
                        UpdateMenu();
                        break;
                    }
                case 1:
                    {
                        UpdateGame();
                        break;
                    }
            }
            base.Update(gameTime);
        }
        void UpdateMenu()
        {
            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.X != lastMouseState.X || currentMouseState.Y != lastMouseState.Y)//мышка сдвинулась вообще
            {
                mousePosition = new Point(currentMouseState.X, currentMouseState.Y);//возможно стоит переделать для удобства
            }
            if ((currentMouseState.X > 696 && currentMouseState.X < 1223) && (currentMouseState.Y > 605 && currentMouseState.Y < 703))
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (pressed == false)
                    {
                        pressed = true;
                    }
                }
                if (currentMouseState.LeftButton == ButtonState.Released)
                {
                    if (pressed == true)//клавиша была нажата
                    {
                        pressed = false;
                        pageNumber = 1;

                        int prCount = dice.NewRoll(1, 0);
                        int count = dice2.NewRoll(2, prCount);
                        placingSquare.ChangeTeam(dice.GetRandom(), dice2.GetRandom());
                    }

                }
            }
        }
        void UpdateGame()
        {
            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.X != lastMouseState.X || currentMouseState.Y != lastMouseState.Y)//мышка сдвинулась вообще
            {
                if ((currentMouseState.X > startPoint.X && currentMouseState.X < startPoint.X + 1080) && (currentMouseState.Y > 0 && currentMouseState.Y < 1080))
                {
                    positionPoint = new Point((int)((currentMouseState.X - startPoint.X) / Square.sizeOfGrid.X), (int)((currentMouseState.Y - startPoint.Y) / Square.sizeOfGrid.Y));//относительные координаты
                    mousePosition = new Point(currentMouseState.X - 27, currentMouseState.Y - 27);//возможно стоит переделать для удобства
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
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    int prCount = dice.NewRoll(1, 0);
                    int count = dice2.NewRoll(2, prCount);
                }
                if (keyboardState.IsKeyDown(Keys.H))
                {
                    if (hideHelp == false)
                        hideHelp = true;
                    else hideHelp = false;
                }
            }

            if (placingSquare.wrong)
            {
                currentTimeWrong += 16;
            }
            if (currentTimeWrong >= periodWrong)
            {
                currentTimeWrong = 0;
                placingSquare.WrongPlace(0);
            }

            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                if (pressed == false)
                {
                    pressed = true;
                }
            }
            if (currentMouseState.LeftButton == ButtonState.Released)
            {
                if (pressed == true)//клавиша была нажата
                {
                    if (gridSystem.isItFit(placingSquare.name, placingSquare.rotate, positionPoint, placingSquare.team))//место подходит для установки
                    {
                        if (dice.needToDraw == false)
                        {
                            gridSystem.addSquare(placingSquare.name, placingSquare.rotate, placingSquare.team, positionPoint);//добавляем в систему

                            int prCount = dice.NewRoll(1, 0);
                            int count = dice2.NewRoll(2, prCount);
                            placingSquare.ChangeTeam(dice.GetRandom(), dice2.GetRandom());
                            placingSquare.rotate = 0;

                            pressed = false;
                        }
                    }
                    else
                    {
                        if (placingSquare.wrong == false) placingSquare.WrongPlace(1);
                        pressed = false;
                    }
                }
            }
            if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                gridSystem.ClearSquares();
            }

        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            switch (pageNumber)
            {
                case 0:
                    {
                        DrawMenu();
                        break;
                    }
                case 1:
                    {
                        DrawGame();
                        break;
                    }
            }

            base.Draw(gameTime);
        }

        private void DrawMenu()
        {
            using (spriteBatch = new SpriteBatch(GraphicsDevice))
            {
                spriteBatch.Begin(SpriteSortMode.Immediate);

                spriteBatch.Draw(startMenuTexture, new Vector2(0, 0), Color.White); //фон

                if (pressed == false)
                {
                    spriteBatch.Draw(startMenuStButton, new Vector2(684, 600), Color.White); //кнопка не нажата
                }
                else spriteBatch.Draw(startMenuStPrButton, new Vector2(696, 605), Color.White); //кнопка нажата

                spriteBatch.Draw(startMenuCoursor, mousePosition.ToVector2(), Color.White); //курсор

                spriteBatch.End();
            }
        }

        private void DrawGame()
        {
            using (spriteBatch = new SpriteBatch(GraphicsDevice))
            {
                spriteBatch.Begin(SpriteSortMode.Immediate);

                spriteBatch.Draw(fieldTexture, startPoint, null, new Color(255, 255, 255, 120), 0, Vector2.Zero, 1f, SpriteEffects.None, 0); //поле
                gridSystem.DrawAll(spriteBatch);//все квадратики

                dice.Draw(spriteBatch, 1, mousePosition + new Point(0, 54));
                dice2.Draw(spriteBatch, 2, mousePosition + new Point(0, 54));

                if (dice.needToDraw == false)
                {
                    Square sq = new Square(spriteBatch);
                    sq.Draw(placingSquare.name, placingSquare.rotate, placingSquare.team, positionPoint);
                    sq.DrawInPixel("1-1", placingSquare.rotate, placingSquare.team, mousePosition, GraphicsDevice);
                }

                string helpInfo = "Press R to rotate a rectangle\n\n"
                                + "Press LMB to place a rectangle\n\n"
                                + "If you have 1*1 or 6*6 rectangle\n"
                                + "Use scroll wheel to increase or\n"
                                + "decrease your rectangle\n\n"
                                + "Press F for pay respect";

                if (hideHelp == false)
                {
                    spriteBatch.DrawString(controlHelp, "Press H for open help [+]", new Vector2(1500, 900), Color.Gray);
                }
                else spriteBatch.DrawString(controlHelp, helpInfo, new Vector2(1500,800), Color.Gray);
                spriteBatch.End();
            }
        }

        public static Texture2D GetSquareTexture(string sqName)
        {
            for (int i = 0; i < squareTextures.Count; i++)
            {
                if (squareTextures[i].name.IndexOf(sqName) != -1)
                {
                    return (squareTextures[i]).texture;
                }
            }
            return null;
        }
        public static Texture2D GetDiceTexture(int num)
        {
            return (diceTextures[num - 1]).texture;
        }
        public static SpriteFont GetScoreSpriteFont(int num)
        {
            return (scoreSpriteFont[num]);
        }

    }
}