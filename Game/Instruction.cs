using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace Game
{
    internal class Instruction
    {
        string textPath = Directory.GetCurrentDirectory().Replace("Game\\bin\\Debug\\net7.0", "Instruction\\instruction.txt");
        string fontPath = Directory.GetCurrentDirectory().Replace("Game\\bin\\Debug\\net7.0", "Instruction\\arial.ttf");
        string backgroundPath = Directory.GetCurrentDirectory().Replace("Game\\bin\\Debug\\net7.0", "IMG\\Instruction.png");
        GameMenager game;
        private Text textBar;
        private Texture instructionTex;
        private Sprite instrukction;
        public Instruction(GameMenager game)
        {
            this.game = game;
            instructionTex = new Texture(backgroundPath);
            instrukction = new Sprite(instructionTex)
            {
                Origin = new Vector2f(instructionTex.Size.X/2,instructionTex.Size.Y/2),
            };
            var text = File.ReadAllText(textPath);
            var font = new Font(fontPath);
            textBar = new Text(text, font, 16)
            {
                Color = Color.Black,
            };
        }
        public void Draw()
        {
            instrukction.Position = game.GetCenterPosition();
            textBar.Position = new Vector2f(instrukction.Position.X - 120, instrukction.Position.Y - 200);
            game.window.Draw(instrukction);
            game.window.Draw(textBar);
        }
    }
}
