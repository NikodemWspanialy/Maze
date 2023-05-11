using Newtonsoft.Json;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class Level : Scene
    {
        //player holder
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
        private string mapPath = Directory.GetCurrentDirectory().Replace("Game\\bin\\Debug\\net7.0", "IMG\\map.jpg");
        private Texture mapTexture;
        private Sprite map;
        //game and view holder
        private View view;
        private GameMenager game;
        //CONSTS
        private const float PLAYER_SPEED = 7f;

        public override void Draw()
        {
            game.window.SetView(view);
            game.window.Draw(map);
            if(staticObjects != null)
            foreach (var stone in staticObjects)
            {
                stone.Draw();
            }
            foreach (var stone in borders) 
            {
                stone.Draw();
            }
            player.Draw();
            if(activeObjects != null)
            foreach (var Obj in activeObjects)
            {
                Obj.Draw();
            }
        }

        public override void LoadContent(GameMenager game)
        {
            this.game = game;
            view = this.game.window.GetView();
            view.Viewport = new FloatRect(0f, 0f, 1f, 1f);
            mapTexture = new Texture(mapPath);
            map = new Sprite(mapTexture);
            borders = new List<StaticObject>();
            for(int i = 0; i <= mapTexture.Size.X; i = i + 50)
            {
                borders.Add(new Stone() { position = new Vector2f(i, 0) });
                borders.Add(new Stone() { position = new Vector2f(i, mapTexture.Size.Y) });
            }
            for (int i = 0; i <= mapTexture.Size.Y; i = i + 50)
            {
                borders.Add(new Stone() { position = new Vector2f(0, i) });
                borders.Add(new Stone() { position = new Vector2f(map.Texture.Size.X, i) });
            }

            player.LoadContent(game);
            staticObjectFloatReacts = new List<FloatRect>();
            if(staticObjects != null)
            foreach (var Obj in staticObjects)
            {
                Obj.LoadContent(this.game);
                staticObjectFloatReacts.Add(Obj.GetGlobalBounds());
            }
            if(activeObjects != null)
                foreach (var Obj in activeObjects)
                {
                    Obj.LoadContent(this.game);
                }
            foreach (var Obj in borders)
            {
                Obj.LoadContent(this.game);
                staticObjectFloatReacts.Add(Obj.GetGlobalBounds());
            }
        }

        public override void Update()
        {
            //activeObjects Update
            player.Update();
            view.Center = player.playerPosition;
        }
        public override void Action(Keyboard.Key k)
        {
            Vector2f dirVector = new Vector2f();
            switch (k)
            {
                case Keyboard.Key.Left:
                    dirVector = new Vector2f(-PLAYER_SPEED, 0);
                    break;
                case Keyboard.Key.Right:
                    dirVector = new Vector2f(PLAYER_SPEED, 0);
                    break;
                case Keyboard.Key.Up:
                    dirVector = new Vector2f(0, -PLAYER_SPEED);
                    break;
                case Keyboard.Key.Down:
                    dirVector = new Vector2f(0,PLAYER_SPEED);
                    break;
                default:
                    break;
            }
            var playerColl = player.CheckCollision(dirVector);
            foreach(var statObj in staticObjectFloatReacts)
            {
                if (statObj.Intersects(playerColl))
                {
                    return;
                }
            }
            foreach(var Obj in activeObjects)
            {
                if (Obj.GetGlobalBounds().Intersects(playerColl))
                {
                    Obj.Action();
                    return;
                }
            }
            player.SetMove(dirVector);
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
