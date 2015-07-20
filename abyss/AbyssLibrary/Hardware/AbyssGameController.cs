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
        public enum SPGameStateType { NotRunning, Running, Paused }
        public SPGameStateType GameState { get; private set; }

        public event AbyssEvent GameStarted;
        public event AbyssEvent GameWon;
        public event AbyssEvent GameLost;
        public event AbyssEvent GamePaused;
        public event AbyssEvent GameUnPaused;
        public event AbyssEvent GameStopped;
        
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

        public void Pause()
        {
            if (GameState == SPGameStateType.Running)
            {
                GameState = SPGameStateType.Paused;
                OnGamePaused(this, EventArgs.Empty);
            }
        }

        public void UnPause()
        {
            if (GameState == SPGameStateType.Paused)
            {
                GameState = SPGameStateType.Running;
                OnGameUnPaused(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            // stop takes priority, can be done always
            GameState = SPGameStateType.NotRunning;
            OnGameStopped(this, EventArgs.Empty);
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
    }
}
