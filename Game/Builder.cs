using Newtonsoft.Json;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Game
{
    public enum Items
    {
        Stone, BiggerStone, NextLevelButton
    }
    internal class Builder : Scene
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.None, PropertyName = "PlayerType")]
        public Player player { get; set; }
        //stat obj holder
        [JsonProperty(TypeNameHandling = TypeNameHandling.None, PropertyName = "StaticObjectsList")]
        public List<StaticObject> staticObjects { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.None, PropertyName = "ActiveObjectsList")]
        public List<ActiveObject> activeObjects { get; set; }

        //publicObjsStats
        private List<StaticObject> borders { get; set; }
        private List<FloatRect> staticObjectFloatReacts;
        //map holder
        private string mapPath = Directory.GetCurrentDirectory().Replace("Game\\bin\\Debug\\net7.0", "IMG\\mapBuilder.jpg");
        private Texture mapTexture;
        private Sprite map;
        private string pointerPath = Directory.GetCurrentDirectory().Replace("Game\\bin\\Debug\\net7.0", "IMG\\Pointer.png");
        private Texture pointerTexture;
        private Sprite pointer;

        //game and view holder
        private View view;
        private GameMenager game;
 

        //BUILDER SETTINGS
        private bool staticItemIsHold = true;
        private Items itemHolder;
        //CONSTS
        private const float PLAYER_SPEED = 15f;
        public override void Action(Keyboard.Key k)
        {
            Vector2f dirVector = new Vector2f();
            switch (k)
            {
                case Keyboard.Key.Left:
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                    {
                        TryDrawObject();
                    }
                    dirVector = new Vector2f(-PLAYER_SPEED, 0);
                    break;
                case Keyboard.Key.Right:
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                    {
                        TryDrawObject();
                    }
                    dirVector = new Vector2f(PLAYER_SPEED, 0);
                    break;
                case Keyboard.Key.Up:
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                    {
                        TryDrawObject();
                    }
                    dirVector = new Vector2f(0, -PLAYER_SPEED);
                    break;
                case Keyboard.Key.Down:
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                    {
                        TryDrawObject();
                    }
                    dirVector = new Vector2f(0, PLAYER_SPEED);
                    break;
                case Keyboard.Key.Space:
                    PlaceObject();
                    break;
                case Keyboard.Key.Num1: //Stone
                    itemHolder = Items.Stone;
                    staticItemIsHold = true;
                    break;
                case Keyboard.Key.Num2: //Stone
                    itemHolder = Items.BiggerStone;
                    staticItemIsHold = true;
                    break;
                case Keyboard.Key.Q:
                    itemHolder = Items.NextLevelButton;
                    staticItemIsHold = false;
                    break;
                default:
                    break;
            }
            var playerColl = player.CheckCollision(dirVector);
            foreach (var statObj in staticObjectFloatReacts)
            {
                if (statObj.Intersects(playerColl))
                {
                    return;
                }
            }
            player.SetMove(dirVector);
    }

        private void TryDrawObject()
        {
            if (staticItemIsHold && staticObjects != null)
            {
                var temp = staticObjects.Last().GetPosition();
                var calc = Math.Sqrt(Math.Pow((temp.X - player.playerPosition.X),2)+ Math.Pow((temp.Y - player.playerPosition.Y), 2));
                if(calc > 50)
                {
                    PlaceObject();
                }
                return;
                
            }
        }

        public override void Draw()
        {
            game.window.SetView(view);
            game.window.Draw(map);
            if (staticObjects != null)
                foreach (var stone in staticObjects)
                {
                    stone.Draw();
                }
            foreach (var stone in borders)
            {
                stone.Draw();
            }
            if (activeObjects != null)
                foreach (var Obj in activeObjects)
                {
                    Obj.Draw();
                }
            game.window.Draw(pointer);
        }

        public override void LoadContent(GameMenager game)
        {
            this.game = game;
            view = this.game.window.GetView();
            view.Viewport = new FloatRect(0f, 0f, 1f, 1f);
            mapTexture = new Texture(mapPath);
            map = new Sprite(mapTexture);
            staticObjects = new List<StaticObject>();
            activeObjects = new List<ActiveObject>();
            borders = new List<StaticObject>();
            for (int i = 0; i <= mapTexture.Size.X; i = i + 50)
            {
                borders.Add(new Stone() { position = new Vector2f(i, 0) });
                borders.Add(new Stone() { position = new Vector2f(i, mapTexture.Size.Y) });
            }
            for (int i = 0; i <= mapTexture.Size.Y; i = i + 50)
            {
                borders.Add(new Stone() { position = new Vector2f(0, i) });
                borders.Add(new Stone() { position = new Vector2f(map.Texture.Size.X, i) });
            }
            player = new Player();
            player.LoadContent(game);
            player.playerPosition = new Vector2f(100f, 100f);
            staticObjectFloatReacts = new List<FloatRect>();
            foreach (var Obj in borders)
            {
                Obj.LoadContent(this.game);
                staticObjectFloatReacts.Add(Obj.GetGlobalBounds());
            }
            itemHolder = Items.Stone;
            pointerTexture = new Texture(pointerPath);
            pointer = new Sprite(pointerTexture)
            {
                Origin = new Vector2f(pointerTexture.Size.X / 2, pointerTexture.Size.Y / 2),
                Position = new Vector2f(game.window.Size.X/2,game.window.Size.Y/2),
            };
        }

        public override void Update()
        {
            player.Update();
            view.Center = player.playerPosition;
            pointer.Position = player.playerPosition;
        }
        private void PlaceObject()
        {
            //cos sie jebie
            var mousePos = player.playerPosition; 
            //if czy w polu
            {
                if (staticItemIsHold)
                {
                    if(itemHolder == Items.Stone)
                    {
                        var stone = new Stone()
                        {
                            position = new Vector2f(mousePos.X, mousePos.Y),
                        };
                        stone.LoadContent(this.game);
                        staticObjects.Add(stone);
                    }
                    if (itemHolder == Items.BiggerStone)
                    {
                        var biggerStone = new BiggerStone()
                        {
                            position = new Vector2f(mousePos.X, mousePos.Y),
                        };
                        biggerStone.LoadContent(this.game);
                        staticObjects.Add(biggerStone);
                    }
                }
                else if (!staticItemIsHold)
                {
                    if(itemHolder == Items.NextLevelButton)
                    {
                        var nextLevelButton = new NextLevelButton()
                        {
                            position = new Vector2f(mousePos.X, mousePos.Y),
                        };
                        nextLevelButton.LoadContent(this.game);
                        activeObjects.Add(nextLevelButton);
                    }
                }
            }
        }

        public override List<StaticObject> GetStaticObjects()
        {
            return staticObjects;
        }

        public override List<ActiveObject> GetActiveObjects()
        {
            return activeObjects;
        }

        public override Player GetPlayer()
        {
            return player;
        }
    }
}
