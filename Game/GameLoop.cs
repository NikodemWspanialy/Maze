using SFML.System;
using SFML.Window;
using SFML.Graphics;
using System.Drawing;

namespace gameLoop
{
    abstract public class GameLoop
    {
        const float MAX_FPS = 50f;
        float deltaTime = 1f / MAX_FPS;
        float correctTime = 0f, previousTime = 0f, timeSinceLastFrame = 0f;

        public GameTime gameTime { get; set; }
        public RenderWindow window { get; }
        public SFML.Graphics.Color windowColor { get; set; }


        public GameLoop(string title, SFML.Graphics.Color windowColor)
        {
            this.gameTime = new GameTime();
            this.windowColor = windowColor;
            window = new RenderWindow(new VideoMode(VideoMode.DesktopMode.Width,
            VideoMode.DesktopMode.Height), title, Styles.Fullscreen);

            //inicjujemy eventy
            window.Closed += WindowClose;
            window.KeyPressed += WindowKeyPressed;
        }

        private void WindowKeyPressed(object? sender, KeyEventArgs e)
        {
            if(e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
            if(e.Code == Keyboard.Key.F1)
            {
                increaseLVL();
            }
           if(e.Code == Keyboard.Key.F2)
            {
                BuilderMode();
            }
           if(e.Code == Keyboard.Key.F3)
            {
                BuilderModeEditMap();
            }
           if(e.Code == Keyboard.Key.LControl)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                {
                    SaveTheMap();
                }
            }
           if(e.Code == Keyboard.Key.I)
            {
                Instruction();
            }
            else
            {
                if(e.Code != Keyboard.Key.Down && e.Code != Keyboard.Key.Up && e.Code != Keyboard.Key.Left && e.Code != Keyboard.Key.Right)
                {
                Action(e.Code);
                }
            }
        }

        private void WindowClose(object? sender, EventArgs e)
        {
            window.Close();
        }

        public void Run()
        {
            Initialize();
            LoadContent();
            Clock clock = new Clock();
            while (window.IsOpen)
            {
                window.DispatchEvents();
                correctTime = clock.ElapsedTime.AsSeconds();
                timeSinceLastFrame = correctTime - previousTime;
                if (timeSinceLastFrame >= deltaTime)
                {
                    gameTime.Update(timeSinceLastFrame, clock.ElapsedTime.AsSeconds());
                    timeSinceLastFrame = 0f;
                    previousTime = correctTime;

                    if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                    {
                        Action(Keyboard.Key.Up);
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                    {
                        Action(Keyboard.Key.Down);
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
                    {
                        Action(Keyboard.Key.Left);
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
                    {
                        Action(Keyboard.Key.Right);
                    }

                    Update(gameTime);

                    window.Clear(windowColor);
                    Draw(gameTime);
                    window.Display();
                }
            }
        }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
        public abstract void Initialize();
        public abstract void LoadContent();
        public abstract void Action(Keyboard.Key k); 
        public abstract void increaseLVL();
        public abstract void BuilderMode();
        public abstract void SaveTheMap();
        public abstract void BuilderModeEditMap();
        public abstract void Instruction();
    }
}
