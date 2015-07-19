using AbyssScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class AbyssGameController : AbstractPhysicalObject
    {
        public enum SPGameStateType { NotRunning, Running, SoftPaused, HardPaused }
        public SPGameStateType GameState { get; private set; }

        public event AbyssEvent GameStarted;
        public event AbyssEvent GameWon;
        public event AbyssEvent GameLost;
        public event AbyssEvent GameSoftPaused;
        public event AbyssEvent GameHardPaused;
        public event AbyssEvent GameUnPaused;
        
        public AbyssGameController(string name)
            : base(name)
        {
        }

        public void Start()
        {
            if (GameState == SPGameStateType.NotRunning)
            {
                GameState = SPGameStateType.Running;
                OnGameStarted(this, EventArgs.Empty);
            }
        }

        public void Win()
        {
            // win if game is running or even if paused
            if (GameState != SPGameStateType.NotRunning)
            {
                GameState = SPGameStateType.NotRunning;
                OnGameWon(this, EventArgs.Empty);
            }
        }

        public void Lose()
        {
            // lose if game is running or even if paused
            if (GameState != SPGameStateType.NotRunning)
            {
                GameState = SPGameStateType.NotRunning;
                OnGameLost(this, EventArgs.Empty);
            }
        }

        public void SoftPause()
        {
            if (GameState == SPGameStateType.Running)
            {
                GameState = SPGameStateType.SoftPaused;
                OnGameSoftPaused(this, EventArgs.Empty);
            }
        }

        public void HardPause()
        {
            // hard pause takes priority, can be done always
            GameState = SPGameStateType.HardPaused;
            OnGameHardPaused(this, EventArgs.Empty);
        }

        public void UnPause()
        {
            if (GameState == SPGameStateType.SoftPaused)
            {
                GameState = SPGameStateType.Running;
                OnGameUnPaused(this, EventArgs.Empty);
            }
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
