using gameLoop;
using Newtonsoft.Json;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Game
{
    internal class GameMenager : gameLoop.GameLoop
    {
        private string[] stages;
        private Scene scene;
        private int currentLevelIP = 0;
        private Instruction instruction = null;
        private bool showInstruction = false;

        private bool buildingModeBool = false;
        private string actuallEditingPath = null;
        public GameMenager() : base("title", Color.Black) {}

        public override void Initialize()
        {
                stages = null;
                var path = Directory.GetCurrentDirectory();
                stages = Directory.GetFiles(path.Replace("Game\\bin\\Debug\\net7.0", "STAGES\\"));
            //JSON_Template();
        }
        private void JSON_Template()
        {

            Level temlLevel = new Level()
            {
                player = new Player()
                {
                    playerPosition = new SFML.System.Vector2f(100, 100),
                },
                staticObjects = new List<StaticObject> {
                    new Stone() { position = new SFML.System.Vector2f(1500, 980) },
                    new BiggerStone() { position = new SFML.System.Vector2f(1700, 1700) },
                },
                activeObjects = null,
            };



            string file = @"C:\PRJs\CS_PRJs\PRJs\Labirynt\test.json";
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented,
            };

            string json = JsonConvert.SerializeObject(temlLevel, settings);
            //if (File.Exists(file))
            //{
            //    File.Delete(file);
            //}
            File.WriteAllText(file, json);

        }
        
        public override void increaseLVL()
        {
            currentLevelIP++;
            if (currentLevelIP >= stages.Length){ 
                currentLevelIP = 0;
            }
            LoadContent();
        }
        public override void LoadContent()
        {
            try
            {
                scene = new Level();
                string file = stages[currentLevelIP];
                var json = File.ReadAllText(file);
                var options = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    Formatting = Formatting.Indented
                };
            scene = JsonConvert.DeserializeObject<Level>(json, options);
            scene.LoadContent(this);
            }
            catch
            {
                 Console.WriteLine("MapPath: " + stages[currentLevelIP] + " does not response!");
                 increaseLVL();
            }
            return;
        }

        public override void Update(GameTime gameTime)
        {
            scene.Update();
        }
        public override void Draw(GameTime gameTime)
        {
            scene.Draw();
            if (showInstruction)
            {
                instruction.Draw();
            }
        }

        public override void Action(Keyboard.Key k)
        {
            scene.Action(k);
        }
        public override void BuilderMode()
        {
           if(buildingModeBool)
            {
                LoadContent();
                buildingModeBool = !buildingModeBool;
            }
            else
            {
                LoadContentBuilderMode();
                buildingModeBool = !buildingModeBool;
            }
        }
        private void LoadContentBuilderMode() 
        {
            scene = new Builder();
            scene.LoadContent(this);
        }

        public override void SaveTheMap()
        {
            if (!buildingModeBool)
            {
                Console.WriteLine("not in building mode");
                return;
            }
            string path;
            if(actuallEditingPath == null) 
            { 
            path = Directory.GetCurrentDirectory();
            var data = (DateTime.Now).ToString();
            data = data.Replace(" ", "_");
            data = data.Replace(":", ".");
            path = (path.Replace("Game\\bin\\Debug\\net7.0", "STAGES\\Save_")) +data + ".json";
            }
            else
            {
                path = actuallEditingPath;
                actuallEditingPath = null;
            }
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented,
            };
            Level temp_scene = new Level()
            {
                staticObjects = scene.GetStaticObjects(),
                activeObjects = scene.GetActiveObjects(),
                player = scene.GetPlayer(),
            };
            string json = JsonConvert.SerializeObject(temp_scene, settings);
            File.WriteAllText(path, json);
            Console.WriteLine("Done, map saved, PATH:\n" + path);
            Initialize();
        }
        public override void BuilderModeEditMap()
        {
            if (buildingModeBool)
            {
                LoadContent();
                buildingModeBool = !buildingModeBool;
            }
            else
            {
                actuallEditingPath = stages[currentLevelIP];
                LoadSceneBuilderMode();
                buildingModeBool = !buildingModeBool;
            }
        }
        private void LoadSceneBuilderMode()
        {
            var temp_scene = new Builder();
            temp_scene.LoadContent(this);
            temp_scene.activeObjects = scene.GetActiveObjects();
            temp_scene.staticObjects = scene.GetStaticObjects();
            scene = temp_scene;
        }

        public override void Instruction()
        {
            if(instruction == null)
            {
                instruction = new Instruction(this);
                showInstruction = true;
            }
            else
            {
                showInstruction = !showInstruction;
            }
        }
        public Vector2f GetCenterPosition()
        {
            return new Vector2f(scene.GetPlayer().playerPosition.X, scene.GetPlayer().playerPosition.Y);
        }
    }
}
