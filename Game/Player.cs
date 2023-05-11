using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class Player
    {
        public Vector2f playerPosition { get; set; }
        private Vector2f moveValocity;


        private GameMenager game;
        private string path = Directory.GetCurrentDirectory().Replace("Game\\bin\\Debug\\net7.0", "IMG\\player.png");
        private Texture texture;
        private Sprite player;
        private RectangleShape Collider;
        private bool faceToRight = true;
        public  void Draw()
        {
            //game.window.Draw(Collider);
            game.window.Draw(player);
        }
        public  void LoadContent(GameMenager gameMenager)
        {
            texture = new Texture(path);
            player = new Sprite(texture)
            {
                Origin = new Vector2f(texture.Size.X / 2, texture.Size.Y / 2),
                Position = new Vector2f(100f, 100f),
            };
            game = gameMenager;
            Collider = new RectangleShape(new Vector2f(texture.Size.X / 2, texture.Size.Y))
            {
                FillColor = Color.White,
                Position = player.Position + new Vector2f(texture.Size.X / 4, texture.Size.Y / 4),
                Origin = new Vector2f(texture.Size.X / 2, texture.Size.Y / 2 + texture.Size.Y/4),
            };
            playerPosition = new Vector2f(100f, 100f);
        }

        public  void Update()
        {
            player.Position = playerPosition ;
            if (!faceToRight)
            {
                player.Scale = new Vector2f(-1, 1);
            }
            else
            {
                player.Scale = new Vector2f(1, 1);
            }
            Collider.Position = playerPosition + new Vector2f(texture.Size.X / 4, texture.Size.Y / 4);
        }
        public FloatRect CheckCollision(Vector2f przesuniecie)
        {
            Collider.Position += przesuniecie;
            return Collider.GetGlobalBounds();
        }
        public void SetMove(SFML.System.Vector2f vector2F)
        {
            playerPosition += vector2F;
            if(vector2F.X < 0)
            {
                faceToRight = false;
            }
            else if (vector2F.X > 0)
            {
                faceToRight = true;
            }
        }
        public FloatRect GetGlobalBounds()
        {
            return player.GetGlobalBounds();
        }
    }
}
