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
        public enum SPGameStateType { NotRunning, Running, Paused, Testing }
        public SPGameStateType GameState { get; private set; }

        public event AbyssEvent GameStarted;
        public event AbyssEvent GameWon;
        public event AbyssEvent GameLost;
        public event AbyssEvent GamePaused;
        public event AbyssEvent GameUnPaused;
        public event AbyssEvent GameStopped;
        public event AbyssEvent GameEnteredTestMode;

        public bool CanStart { get { return GameState == SPGameStateType.NotRunning; } }
        public bool CanWin { get { return true; } }
        public bool CanLose { get { return true; } }
        public bool CanPause { get { return GameState == SPGameStateType.Running; } }
        public bool CanUnPause { get { return GameState == SPGameStateType.Paused; } }
        public bool CanStop { get { return true; } }
        public bool CanEnterTestMode { get { return GameState == SPGameStateType.NotRunning; } }

        public bool PreventFromLosing {get; set;}
        
        public AbyssGameController(string name)
            : base(name)
        {
        }

        public void Start()
        {
            if (CanStart)
            {
                GameState = SPGameStateType.Running;
                OnGameStarted(this, EventArgs.Empty);
            }
        }

        public void Win()
        {
            // win if game is running or even if paused
            if (CanWin)
            {
                GameState = SPGameStateType.NotRunning;
                OnGameWon(this, EventArgs.Empty);
            }
        }

        public void Lose()
        {
            // lose if game is running or even if paused
            if (CanLose && !PreventFromLosing)
            {
                GameState = SPGameStateType.NotRunning;
                OnGameLost(this, EventArgs.Empty);
            }
        }

        public void Pause()
        {
            if (CanPause)
            {
                GameState = SPGameStateType.Paused;
                OnGamePaused(this, EventArgs.Empty);
            }
        }

        public void UnPause()
        {
            if (CanUnPause)
            {
                GameState = SPGameStateType.Running;
                OnGameUnPaused(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            if (CanStop)
            {
                // stop takes priority, can be done always
                GameState = SPGameStateType.NotRunning;
                OnGameStopped(this, EventArgs.Empty);
            }
        }

        public void EnterTestMode()
        {
            if (CanEnterTestMode)
            {
                GameState = SPGameStateType.Testing;
                OnGameEnteredTestMode(this, EventArgs.Empty);
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
