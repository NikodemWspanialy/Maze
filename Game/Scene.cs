using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    abstract internal class Scene
    {
        abstract public  void Draw();
        abstract public  void Update();
        abstract public  void LoadContent(GameMenager game);
        abstract public void Action(Keyboard.Key k);

        abstract public List<StaticObject> GetStaticObjects();
        abstract public List<ActiveObject> GetActiveObjects();
        abstract public Player GetPlayer();
    }
}
