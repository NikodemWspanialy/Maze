using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    abstract internal class Object
    {
        abstract public void LoadContent(GameMenager gameMenager);
        abstract public void Draw();
        abstract public FloatRect GetGlobalBounds();
        abstract public Vector2f GetPosition();
    }
}
