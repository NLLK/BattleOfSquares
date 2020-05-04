using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace BattleOfSquares
{/// <summary>
/// A main Game class
/// </summary>
    public class Game1 : Game
    {
        public static Color blueTeamColor = new Color(102, 153, 255, 255);
        public static Color pinkTeamColor = new Color(255, 51, 153, 255);

        GraphicsDeviceManager graphics;//главный графический менеджер
        SpriteBatch spriteBatch;//упаковщик спрайтов
        MouseState lastMouseState;//положение мыши для сравнения на изменения ее пололжения

        Texture2D fieldTexture;//текстура поля
        Texture2D fieldStartPointsTexture;//текстура поля

        Texture2D startMenuTexture;//текстура стартового меню
        Texture2D startMenuStButton;//текстура кнопки старт стартового меню
        Texture2D startMenuStPrButton;//текстура кнопки старт стартового меню в нажатом состоянии
        Texture2D startMenuCoursor;//текстура курсора стартового меню
        Texture2D startMenuhTButton;//текстура кнопки howTo стартового меню
        Texture2D startMenuhTPrButton;//текстура кнопки howTo стартового меню в нажатом состоянии
        Texture2D startMenuExButton;//текстура кнопки выход стартового меню
        Texture2D startMenuExPrButton;//текстура кнопки выход стартового меню в нажатом состоянии

        Texture2D endMenuBackground;//текстура фона меню end
        Texture2D endMenuPlButton;//текстура кнопки play afgain меню end
        Texture2D endMenuPlPrButton;//текстура кнопки play afgain меню end в нажатом состоянии
        Texture2D endMenuExButton;//текстура кнопки старт стартового меню
        Texture2D endMenuExPrButton;//текстура кнопки старт стартового меню в нажатом состоянии

        Texture2D howToMenu;//текстура меню how to
        Texture2D howTobackButton;//текстура кнопки back меню how to
        Texture2D howToBackPrButton;//текстура кнопки back меню how to в нажатом состоянии

        SpriteFont controlHelp;//SprtieFont для хелпа
        SpriteFont turnDisplay;//SpriteFont для your turn!
        SpriteFont whoWins;//SpriteFont для определения победителя
        public static SpriteFont scoreDisplay;//SpriteFont для отображение очков
        bool hideHelp = false;//скрывать подсказку

        int currentTimeKeyboard = 0; // сколько времени прошло, для клавиатуры
        int periodKeyboard = 150; // частота обновления в миллисекундах
        int currentTimeWrong = 0;//текущее время в отрисовке ошибочного квадрата
        int periodWrong = 300;//время отрисовки ошибочного квадрата

        int pressed = 0;//мышка нажата? 0 - нет. 1 - какая-то первая кнопка, 2 - какая-то вторая и тп

        Point mousePosition;//абсолютные координаты мыши
        Point positionPoint = new Point(0, 0);//относительные координаты мыши

        GridSystem gridSystem;//управление сеткой и прямоугольниками на ней
        Dice dice;//кости, анимация и рандом
        Dice dice2;//кости, анимация и рандом, 2 кость
        CousorLoadingAnimation coursorLoading = new CousorLoadingAnimation();
        Square.SquareInfo placingSquare;//квадратик, который будем ставить, информация о нем

        static List<Texture> squareTextures = new List<Texture>();//текстуры прямоугольников
        static List<Texture> diceTextures = new List<Texture>();//текстуры костей
        static List<SpriteFont> scoreSpriteFont = new List<SpriteFont>();//spriteFont для очков

        public static Vector2 startPoint = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 - 590, 0);//начальная точка отрисовки поля

        int pageNumber = 0;//номер страницы - определяет рисовать меню - 0, игру - 1, выход - 2
        /// <summary>
        /// constructor of class
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        /// <summary>
        /// initialize variables and some display settings
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.ApplyChanges();
            //graphics.ToggleFullScreen();
            gridSystem = new GridSystem();
            dice = new Dice();
            dice2 = new Dice();
            GraphicsDevice.Clear(Color.White);
            placingSquare = new Square.SquareInfo(new Point(0, 0), "1-1", 0, 0);
            base.Initialize();
        }
        /// <summary>
        /// loading a content
        /// </summary>
        protected override void LoadContent()
        {
            fieldTexture = Content.Load<Texture2D>("field");
            fieldStartPointsTexture = Content.Load<Texture2D>("fieldStartPoints");

            startMenuTexture = Content.Load<Texture2D>("StartMenu\\startMenu");
            startMenuStButton = Content.Load<Texture2D>("StartMenu\\button");
            startMenuStPrButton = Content.Load<Texture2D>("StartMenu\\pressedButton");
            startMenuCoursor = Content.Load<Texture2D>("StartMenu\\coursor");

            startMenuhTButton = Content.Load<Texture2D>("StartMenu\\howTo");
            startMenuhTPrButton = Content.Load<Texture2D>("StartMenu\\howToPr");
            startMenuExButton = Content.Load<Texture2D>("StartMenu\\ex");
            startMenuExPrButton = Content.Load<Texture2D>("StartMenu\\exPr");

            endMenuBackground = Content.Load<Texture2D>("EndMenu\\background");
            endMenuPlButton = Content.Load<Texture2D>("EndMenu\\pl");
            endMenuPlPrButton = Content.Load<Texture2D>("EndMenu\\plPr");
            endMenuExButton = Content.Load<Texture2D>("EndMenu\\ex");
            endMenuExPrButton = Content.Load<Texture2D>("EndMenu\\exPr");

            howToMenu = Content.Load<Texture2D>("HowToMenu\\howToMenu");
            howTobackButton = Content.Load<Texture2D>("HowToMenu\\back");
            howToBackPrButton = Content.Load<Texture2D>("HowToMenu\\backPr");

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
            scoreDisplay = Content.Load<SpriteFont>("score");
            controlHelp = Content.Load<SpriteFont>("controlHelp");
            turnDisplay = Content.Load<SpriteFont>("turn");
            whoWins = Content.Load<SpriteFont>("whoWins");
        }
        /// <summary>
        /// unloading a content
        /// </summary>
        protected override void UnloadContent()
        {/*it is actually empty*/ }
        /// <summary>
        /// main update method. It's choose exactly what we need to update
        /// </summary>
        /// <param name="gameTime">Just a GameTime</param>
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
                case 2:
                    {
                        UpdateEnd();
                        break;
                    }
                case 3:
                    {
                        UpdateHowTo();
                        break;
                    }
            }
            base.Update(gameTime);
        }
        /// <summary>
        /// Update method for menu
        /// </summary>
        void UpdateMenu()
        {
            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.X != lastMouseState.X || currentMouseState.Y != lastMouseState.Y)//мышка сдвинулась вообще
            {
                mousePosition = new Point(currentMouseState.X, currentMouseState.Y);//возможно стоит переделать для удобства
            }
            if ((currentMouseState.X > 696 && currentMouseState.X < 1223) && (currentMouseState.Y > 574 && currentMouseState.Y < 683))
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (pressed == 0)
                    {
                        pressed = 1;
                    }
                }
                if (currentMouseState.LeftButton == ButtonState.Released)
                {
                    if (pressed == 1)//клавиша start была  нажата
                    {
                        pressed = 0;
                        pageNumber = 1;

                        int prCount = dice.NewRoll(1, 0);
                        int count = dice2.NewRoll(2, prCount);
                        placingSquare.ChangeDices(dice.GetRandom(), dice2.GetRandom());
                    }
                }
            }

            if ((currentMouseState.X > 696 && currentMouseState.X < 1223) && (currentMouseState.Y > 725 && currentMouseState.Y < 834))
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (pressed == 0)
                    {
                        pressed = 2;
                    }
                }
                if (currentMouseState.LeftButton == ButtonState.Released)
                {
                    if (pressed == 2)//клавиша how to была  нажата
                    {
                        pressed = 0;
                        pageNumber = 3;
                    }
                }
            }

            if ((currentMouseState.X > 696 && currentMouseState.X < 1223) && (currentMouseState.Y > 878 && currentMouseState.Y <978))
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (pressed == 0)
                    {
                        pressed = 3;
                    }
                }
                if (currentMouseState.LeftButton == ButtonState.Released)
                {
                    if (pressed == 3)//клавиша exit была нажата
                    {
                        pressed = 0;
                        Exit();
                    }
                }
            }


        }
        /// <summary>
        /// Update method for game
        /// </summary>
        void UpdateGame()
        {
            if (placingSquare.team!=2 && gridSystem.isItTheEnd(placingSquare.name, placingSquare.team) && dice.needToDraw == false)
            {
                pageNumber = 2;
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
                    pageNumber = 2;
                }
                if (keyboardState.IsKeyDown(Keys.H))
                {
                    if (hideHelp == false)
                        hideHelp = true;
                    else hideHelp = false;
                }
            }
            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                if (pressed == 0)
                {
                    pressed = 1;
                }
            }
            if (currentMouseState.LeftButton == ButtonState.Released)
            {
                if (pressed == 1)//клавиша была нажата
                {
                    if (gridSystem.isItFit(placingSquare.name, placingSquare.rotate, positionPoint, placingSquare.team) == 0)//место подходит для установки
                    {
                        if (dice.needToDraw == false)
                        {
                            gridSystem.addSquare(placingSquare.name, placingSquare.rotate, placingSquare.team, positionPoint);//добавляем в систему

                            int prCount = dice.NewRoll(1, 0);
                            int count = dice2.NewRoll(2, prCount);
                            placingSquare.ChangeDices(dice.GetRandom(), dice2.GetRandom());
                            placingSquare.ChangeTeam();
                            placingSquare.rotate = 0;

                            pressed = 0;
                        }
                    }
                    else
                    {
                        if (placingSquare.wrong == false) placingSquare.WrongPlace(1);
                        pressed = 0;
                    }
                }
            }
            if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                gridSystem.Clear();
            }

        }
        /// <summary>
        /// Update method for ending screen
        /// </summary>
        private void UpdateEnd()
        {
            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.X != lastMouseState.X || currentMouseState.Y != lastMouseState.Y)//мышка сдвинулась вообще
            {
                mousePosition = new Point(currentMouseState.X, currentMouseState.Y);
            }
            if ((currentMouseState.X > 696 && currentMouseState.X < 1223) && (currentMouseState.Y > 605 && currentMouseState.Y < 703))//кнопка again
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (pressed == 0)
                    {
                        pressed = 1;
                    }
                }
                if (currentMouseState.LeftButton == ButtonState.Released)
                {
                    if (pressed == 1)//клавиша была нажата
                    {
                        pressed = 0;
                        pageNumber = 1;
                        gridSystem.Clear();
                        int prCount = dice.NewRoll(1, 0);
                        int count = dice2.NewRoll(2, prCount);
                        placingSquare.ChangeDices(dice.GetRandom(), dice2.GetRandom());
                    }

                }
            }
            if ((currentMouseState.X > 696 && currentMouseState.X < 1223) && (currentMouseState.Y > 755 && currentMouseState.Y < 853))//кнопка exit
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (pressed == 0)
                    {
                        pressed = 2;
                    }
                }
                if (currentMouseState.LeftButton == ButtonState.Released)
                {
                    if (pressed == 2)//клавиша была нажата
                    {
                        pressed = 0;
                        Exit();
                    }
                }
            }
        }
        /// <summary>
        /// Update method for help screen
        /// </summary>
        private void UpdateHowTo()
        {
            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.X != lastMouseState.X || currentMouseState.Y != lastMouseState.Y)//мышка сдвинулась вообще
            {
                mousePosition = new Point(currentMouseState.X, currentMouseState.Y);//возможно стоит переделать для удобства
            }
            if ((currentMouseState.X > 696 && currentMouseState.X < 1223) && (currentMouseState.Y > 834 && currentMouseState.Y < 932))
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (pressed == 0)
                    {
                        pressed = 1;
                    }
                }
                if (currentMouseState.LeftButton == ButtonState.Released)
                {
                    if (pressed == 1)//клавиша была нажата
                    {
                        pressed = 0;
                        pageNumber = 0;
                    }

                }
            }
        }
        /// <summary>
        /// main draw method. It's choose exactly what we need to draw
        /// </summary>
        /// <param name="gameTime">Just a GameTime</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);//clears screen
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
                case 2:
                    {
                        DrawEnd();
                        break;
                    }
                case 3:
                    {
                        DrawHowTo();
                        break;
                    }
            }

            base.Draw(gameTime);
        }
        /// <summary>
        /// Draw method for menu
        /// </summary>
        private void DrawMenu()
        {
            using (spriteBatch = new SpriteBatch(GraphicsDevice))
            {
                spriteBatch.Begin(SpriteSortMode.Immediate);

                spriteBatch.Draw(startMenuTexture, new Vector2(0, 0), Color.White); //фон

                switch (pressed)
                {
                    case 0:
                        {
                            spriteBatch.Draw(startMenuStButton, new Vector2(684, 574), Color.White); //кнопка start не нажата
                            spriteBatch.Draw(startMenuhTButton, new Vector2(684, 725), Color.White);//кнопка how to не нажата
                            spriteBatch.Draw(startMenuExButton, new Vector2(684, 878), Color.White);//кнопка exit не нажата
                            break;
                        }
                    case 1:
                        {
                            spriteBatch.Draw(startMenuStPrButton, new Vector2(696, 579), Color.White); //кнопка start нажата
                            spriteBatch.Draw(startMenuhTButton, new Vector2(684, 725), Color.White);//кнопка how to не нажата
                            spriteBatch.Draw(startMenuExButton, new Vector2(684, 878), Color.White);//кнопка exit не нажата
                            break;
                        }
                    case 2:
                        {
                            spriteBatch.Draw(startMenuStButton, new Vector2(684, 574), Color.White); //кнопка start не нажата
                            spriteBatch.Draw(startMenuhTPrButton, new Vector2(696, 730), Color.White);//кнопка how to нажата
                            spriteBatch.Draw(startMenuExButton, new Vector2(684, 878), Color.White);//кнопка exit не нажата
                            break;
                        }
                    case 3:
                        {
                            spriteBatch.Draw(startMenuStButton, new Vector2(684, 574), Color.White); //кнопка start не нажата
                            spriteBatch.Draw(startMenuhTButton, new Vector2(684, 725), Color.White);//кнопка how to не нажата
                            spriteBatch.Draw(startMenuExPrButton, new Vector2(696, 883), Color.White);//кнопка exit нажата
                            break;
                        }
                }

                spriteBatch.Draw(startMenuCoursor, mousePosition.ToVector2(), Color.White); //курсор

                spriteBatch.End();
            }
        }
        /// <summary>
        /// Draw method for help screen
        /// </summary>
        private void DrawHowTo()
        {
            using (spriteBatch = new SpriteBatch(GraphicsDevice))
            {
                spriteBatch.Begin(SpriteSortMode.Immediate);

                spriteBatch.Draw(howToMenu, new Vector2(0, 0), Color.White); //фон
                if (pressed == 0)
                {
                    spriteBatch.Draw(howTobackButton, new Vector2(684, 834), Color.White); //кнопка не нажата
                }
                else spriteBatch.Draw(howToBackPrButton, new Vector2(696, 839), Color.White); //кнопка нажата

                spriteBatch.Draw(startMenuCoursor, mousePosition.ToVector2(), Color.White); //курсор

                spriteBatch.End();
            }
        }
        /// <summary>
        /// Draw method for game
        /// </summary>
        private void DrawGame()
        {
            using (spriteBatch = new SpriteBatch(GraphicsDevice))
            {
                spriteBatch.Begin(SpriteSortMode.Immediate);

                spriteBatch.Draw(fieldTexture, startPoint, null, new Color(255, 255, 255, 120), 0, Vector2.Zero, 1f, SpriteEffects.None, 0); //поле
                spriteBatch.Draw(fieldStartPointsTexture, startPoint, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0); //поле, точки старта

                gridSystem.DrawAll(spriteBatch);//все квадратики

                dice.Draw(spriteBatch, 1, placingSquare.team);
                dice2.Draw(spriteBatch, 2, placingSquare.team);

                if (dice.needToDraw == false)
                {
                    Square sq = new Square(spriteBatch);
                    sq.Draw(placingSquare.name, placingSquare.rotate, placingSquare.team, positionPoint);
                    sq.DrawInPixel("1-1", placingSquare.rotate, placingSquare.team, mousePosition, spriteBatch);
                }
                else
                {
                    coursorLoading.Draw(spriteBatch, mousePosition);
                }

                DrawText();//help, your turn!

                spriteBatch.End();
            }
        }
        /// <summary>
        /// Draws text for DrawGame
        /// </summary>
        private void DrawText()
        {
            string helpInfo = "Press R to rotate a rectangle\n\n"
                               + "Press LMB to place a rectangle\n\n"
                               + "If you have 1*1 or 6*6 rectangle\n"
                               + "Use scroll wheel to increase or\n"
                               + "decrease your rectangle\n\n"
                               + "Press F for pay respect";

            if (hideHelp == false)
            {
                spriteBatch.DrawString(controlHelp, "Press H for open help [+]", new Vector2(0, 1050), Color.Gray);
            }
            else spriteBatch.DrawString(controlHelp, helpInfo, new Vector2(0, 840), Color.Gray);
            if (placingSquare.team == 0)
            {
                spriteBatch.DrawString(turnDisplay, "Your turn!", new Vector2(1850, 350), blueTeamColor, 4.712f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
            else if (placingSquare.team == 1)
            {
                spriteBatch.DrawString(turnDisplay, "Your turn!", new Vector2(1850, 910), pinkTeamColor, 4.712f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }

        }
        /// <summary>
        /// Draw method for end screen
        /// </summary>
        private void DrawEnd()
        {
            using (spriteBatch = new SpriteBatch(GraphicsDevice))
            {
                spriteBatch.Begin(SpriteSortMode.Immediate);

                spriteBatch.Draw(fieldTexture, startPoint, null, new Color(255, 255, 255, 120), 0, Vector2.Zero, 1f, SpriteEffects.None, 0); //поле
                spriteBatch.Draw(fieldStartPointsTexture, startPoint, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0); //поле, точки старта

                gridSystem.DrawAll(spriteBatch);//все квадратики и очки
                dice.Draw(spriteBatch, 1, placingSquare.team);
                dice2.Draw(spriteBatch, 2, placingSquare.team);

                spriteBatch.Draw(endMenuBackground, new Vector2(684, 352), Color.White); //фон

                if (placingSquare.team == 1)
                {
                    spriteBatch.DrawString(whoWins, "Blue wins!", new Vector2(732, 400), blueTeamColor);
                }
                if (placingSquare.team == 0)
                {
                    spriteBatch.DrawString(whoWins, "Pink wins!", new Vector2(732, 400), pinkTeamColor);
                }

                if (pressed == 0)
                {
                    spriteBatch.Draw(endMenuPlButton, new Vector2(684, 600), Color.White); //кнопка again не нажата
                    spriteBatch.Draw(endMenuExButton, new Vector2(684, 750), Color.White); //кнопка exit не нажата
                }
                if (pressed == 1)
                {
                    spriteBatch.Draw(endMenuPlPrButton, new Vector2(696, 605), Color.White); //кнопка again нажата
                    spriteBatch.Draw(endMenuExButton, new Vector2(684, 750), Color.White); //кнопка exit не нажата
                }
                if (pressed == 2)
                {
                    spriteBatch.Draw(endMenuPlButton, new Vector2(684, 600), Color.White); //кнопка again не нажата
                    spriteBatch.Draw(endMenuExPrButton, new Vector2(696, 755), Color.White); //кнопка exit нажата
                }

                spriteBatch.Draw(startMenuCoursor, mousePosition.ToVector2(), Color.White); //курсор

                spriteBatch.End();
            }
        }
        /// <summary>
        /// Returns Texture2D by its name
        /// </summary>
        /// <param name="sqName">squares name with the template "width-height"</param>
        /// <returns>Texture2D by its name</returns>
        public static Texture2D GetSquareTexture(string sqName)
        {
            for (int i = 0; i < squareTextures.Count; i++)
            {
                if (squareTextures[i].name.IndexOf(sqName) != -1)
                {
                    Texture2D texture = (squareTextures[i]).texture;
                    Console.WriteLine(texture.Name);
                    return (squareTextures[i]).texture;
                }
            }
            return null;
        }
        /// <summary>
        /// Returns Texture2D by its number
        /// </summary>
        /// <param name="num">dice's number</param>
        /// <returns>Texture2D by its number</returns>
        public static Texture2D GetDiceTexture(int num)
        {
            if (num > 0 && num<7)
                return (diceTextures[num - 1]).texture;
            else return null;
        }
    }
}