using Newtonsoft.Json;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    abstract internal class ActiveObject : Object 
    {
        abstract public void Action();
    }

    internal class NextLevelButton : ActiveObject
    {
        public string texturePath = Directory.GetCurrentDirectory().Replace("Game\\bin\\Debug\\net7.0", "IMG\\Brama.png");

        [JsonProperty(TypeNameHandling = TypeNameHandling.None, PropertyName = "NextLevelButtonPosition")]
        public SFML.System.Vector2f position { get; set; }

        private Texture texture;
        private Sprite stone;
        private RectangleShape Collider;
        private GameMenager game;


        public override void Draw()
        {
            //game.window.Draw(Collider);
            game.window.Draw(stone);
        }

        public override FloatRect GetGlobalBounds()
        {
            return Collider.GetGlobalBounds();
        }

        public override Vector2f GetPosition()
        {
            return position;
        }

        public override void LoadContent(GameMenager gameMenager)
        {
            game = gameMenager;
            texture = new Texture(texturePath);
            stone = new Sprite(texture)
            {
                Origin = new SFML.System.Vector2f(texture.Size.X / 2, texture.Size.Y / 2),
                Position = position,
            };
            Collider = new RectangleShape(new SFML.System.Vector2f(texture.Size.X / 2, texture.Size.Y / 2))
            {
                Origin = new SFML.System.Vector2f(texture.Size.X / 2, texture.Size.Y / 2),
                Position = stone.Position + new SFML.System.Vector2f(texture.Size.X / 4, texture.Size.Y / 4),
                FillColor = Color.Red,
            };
        }

        public override void Action()
        {
            game.increaseLVL();
        }
    }
}
