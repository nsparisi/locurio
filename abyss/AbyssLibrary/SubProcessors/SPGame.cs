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
        private enum SPGameEventType { Start, Win, Lose, SoftPause, HardPause, UnPause }
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
        public void SoftPause(object sender, EventArgs e)
        {
            eventType = SPGameEventType.SoftPause;
            StartProcess();
        }

        [AbyssInput]
        public void HardPause(object sender, EventArgs e)
        {
            eventType = SPGameEventType.HardPause;
            StartProcess();
        }

        [AbyssInput]
        public void UnPause(object sender, EventArgs e)
        {
            eventType = SPGameEventType.UnPause;
            StartProcess();
        }
        
        [AbyssOutput]
        public event AbyssEvent GameStarted;
        [AbyssOutput]
        public event AbyssEvent GameWon;
        [AbyssOutput]
        public event AbyssEvent GameLost;
        [AbyssOutput]
        public event AbyssEvent GameSoftPaused;
        [AbyssOutput]
        public event AbyssEvent GameHardPaused;
        [AbyssOutput]
        public event AbyssEvent GameUnPaused;

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
                game.GameSoftPaused += OnGameSoftPaused;
                game.GameHardPaused += OnGameHardPaused;
                game.GameUnPaused += OnGameUnPaused;
            }
        }

        protected override void Process()
        {
            Debug.Log("SPGame Start [{0}]", Name);

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
                else if (eventType == SPGameEventType.SoftPause)
                {
                    game.SoftPause();
                }
                else if (eventType == SPGameEventType.HardPause)
                {
                    game.HardPause();
                }
                else if (eventType == SPGameEventType.UnPause)
                {
                    game.UnPause();
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

        private void OnGameSoftPaused(object sender, EventArgs e)
        {
            if (GameSoftPaused != null)
            {
                GameSoftPaused(sender, e);
            }
        }

        private void OnGameHardPaused(object sender, EventArgs e)
        {
            if (GameHardPaused != null)
            {
                GameHardPaused(sender, e);
            }
        }

        private void OnGameUnPaused(object sender, EventArgs e)
        {
            if (GameUnPaused != null)
            {
                GameUnPaused(sender, e);
            }
        }
    }
}
