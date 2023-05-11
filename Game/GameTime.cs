using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameLoop
{
    public class GameTime
    {
        private float _DeltaTime;
        private float _TimeScale;
        public float TimeScale
        {
            get { return _TimeScale; }
            set { _TimeScale = value; }
        }
        public float DeltaTime
        {
            get { return _DeltaTime * _TimeScale; }
            set { _DeltaTime = value; }
        }
        public float DeltaTimeUnscaled
        {
            get { return _DeltaTime; }
            private set { _DeltaTime = value; }
        }
        public float TotalTimeElapsed
        {
            get; private set;
        }
        public void Update (float deltaTime, float totalTimeEpased) 
        {
            _DeltaTime = deltaTime;
            TotalTimeElapsed = totalTimeEpased;
        }

    }
}
