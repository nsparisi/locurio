using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class SPGame : AbstractSubProcessor
    {
        private enum SPGameEventType { Start, Win, Lose, Pause, UnPause, Stop, Test }
        private SPGameEventType eventType;

        [AbyssParameter]
        public List<AbyssGameController> Games
        {
            get;
            set;
        }

        [AbyssInput]
        public void Start(object sender, EventArgs e)
        {
            eventType = SPGameEventType.Start;
            StartProcess();
        }

        [AbyssInput]
        public void Win(object sender, EventArgs e)
        {
            eventType = SPGameEventType.Win;
            StartProcess();
        }

        [AbyssInput]
        public void Lose(object sender, EventArgs e)
        {
            eventType = SPGameEventType.Lose;
            StartProcess();
        }

        [AbyssInput]
        public void Pause(object sender, EventArgs e)
        {
            eventType = SPGameEventType.Pause;
            StartProcess();
        }

        [AbyssInput]
        public void UnPause(object sender, EventArgs e)
        {
            eventType = SPGameEventType.UnPause;
            StartProcess();
        }

        [AbyssInput]
        public void Stop(object sender, EventArgs e)
        {
            eventType = SPGameEventType.Stop;
            StartProcess();
        }

        [AbyssInput]
        public void Test(object sender, EventArgs e)
        {
            eventType = SPGameEventType.Test;
            StartProcess();
        }
        
        [AbyssOutput]
        public event AbyssEvent GameStarted;
        [AbyssOutput]
        public event AbyssEvent GameWon;
        [AbyssOutput]
        public event AbyssEvent GameLost;
        [AbyssOutput]
        public event AbyssEvent GamePaused;
        [AbyssOutput]
        public event AbyssEvent GameStopped;
        [AbyssOutput]
        public event AbyssEvent GameUnPaused;
        [AbyssOutput]
        public event AbyssEvent GameEnteredTestMode;

        public SPGame()
            : base()
        {
            this.Name = "SPAbyssGame";
            this.Games = new List<AbyssGameController>();
        }

        public override void Initialize()
        {
            foreach (AbyssGameController game in Games)
            {
                game.GameStarted += OnGameStarted;
                game.GameWon += OnGameWon;
                game.GameLost += OnGameLost;
                game.GamePaused += OnGamePaused;
                game.GameStopped += OnGameStopped;
                game.GameUnPaused += OnGameUnPaused;
                game.GameEnteredTestMode += OnGameEnteredTestMode;
            }
        }

        protected override void Process()
        {
            Debug.Log("SPGame Start [{0}] [type: {1}]", Name, eventType);

            foreach (AbyssGameController game in Games)
            {
                if (eventType == SPGameEventType.Start)
                {
                    game.Start();
                }
                else if (eventType == SPGameEventType.Win)
                {
                    game.Win();
                }
                else if (eventType == SPGameEventType.Lose)
                {
                    game.Lose();
                }
                else if (eventType == SPGameEventType.Pause)
                {
                    game.Pause();
                }
                else if (eventType == SPGameEventType.Stop)
                {
                    game.Stop();
                }
                else if (eventType == SPGameEventType.UnPause)
                {
                    game.UnPause();
                }
                else if (eventType == SPGameEventType.Test)
                {
                    game.EnterTestMode();
                }
            }

            ProcessEnded();
        }

        protected override void ProcessEnded()
        {
            Debug.Log("SPGame Ended [{0}]", Name);
        }


        private void OnGameStarted(object sender, EventArgs e)
        {
            if (GameStarted != null)
            {
                GameStarted(sender, e);
            }
        }

        private void OnGameWon(object sender, EventArgs e)
        {
            if (GameWon != null)
            {
                GameWon(sender, e);
            }
        }

        private void OnGameLost(object sender, EventArgs e)
        {
            if (GameLost != null)
            {
                GameLost(sender, e);
            }
        }

        private void OnGamePaused(object sender, EventArgs e)
        {
            if (GamePaused != null)
            {
                GamePaused(sender, e);
            }
        }

        private void OnGameUnPaused(object sender, EventArgs e)
        {
            if (GameUnPaused != null)
            {
                GameUnPaused(sender, e);
            }
        }

        private void OnGameStopped(object sender, EventArgs e)
        {
            if (GameStopped != null)
            {
                GameStopped(sender, e);
            }
        }

        private void OnGameEnteredTestMode(object sender, EventArgs e)
        {
            if (GameEnteredTestMode != null)
            {
                GameEnteredTestMode(sender, e);
            }
        }
    }
}
