﻿using System;
using System.Collections.Generic;
using AbyssLibrary;
using System.Threading;

namespace Abyss
{
    public class AbyssRunner
    {
        public const string SoundDressingRoomBGMFileName = "dressing_room_bgm_7-3.mp3";
        public const string SoundSecretRoomBGMFileName = "";
        public const string SoundAltarTouchFileName = "explosion_sfx.mp3";
        public const string SoundAltarWindSFXFileName = "Altar.mp3";
        public const string SoundAltarWhispersFileName = "";
        public const string SoundNoxSuccessNarrationFileName = "endgame_win.mp3";
        public const string SoundNoxFailureNarrationFileName = "EndgameLoseNew.mp3";

        public const float SoundDressingRoomBGMVolume = 400f;
        public const float SoundSecretRoomBGMVolume = 0f;
        public const float SoundAltarTouchVolume = 120f;
        public const float SoundAltarWindSFXVolume = 200f;
        public const float SoundAltarWhispersVolume = 0f;
        public const float SoundNoxSuccessNarrationVolume = 280f;
        public const float SoundNoxFailureNarrationVolume = 280f;

        public void Start()
        {
            Thread t = new Thread(Run);
            t.Start();
        }

        public void Run()
        {
            // ***********************
            // The Game Itself
            // ***********************
            AbyssGameController gameController = new AbyssGameController("Game: The Vanishing Act");
            SPGame sp_gameController = new SPGame()
            {
                Games = MakeList(gameController)
            };

            sp_gameController.Initialize();
            AbyssSystem.Instance.RegisterPhysicalObject(gameController);
            AbyssSystem.Instance.RegisterSubProcessor(sp_gameController);

            // ***********************
            // Countdown Clock
            // ***********************
            AbyssScreenController screenController = new AbyssScreenController("Clock");
            SPScreen sp_countdownScreen = new SPScreen
            {
                Screens = new List<AbyssScreenController>
                {
                    screenController
                }
            };

            AbyssSystem.Instance.RegisterPhysicalObject(screenController);
            AbyssSystem.Instance.RegisterSubProcessor(sp_countdownScreen);
            sp_countdownScreen.Initialize();

            // ***********************
            // VLC Servers on Raspberry PIs
            // ***********************
            VLCServerControl vlc01 = new VLCServerControl(
                "RPi Dress:50000",
                "74-DA-38-41-93-74",
                VLCServerControl.OSType.Linux,
                "50000");

            VLCServerControl vlc02 = new VLCServerControl(
                "RPi Secret:50000",
                "7C-DD-90-91-07-74",
                VLCServerControl.OSType.Linux,
                "50000");

            VLCServerControl vlc02_02 = new VLCServerControl(
                "RPi Secret:50001",
                "7C-DD-90-91-07-74",
                VLCServerControl.OSType.Linux,
                "50001");

            SPSoundControl sp_soundDressingRoom01 = new SPSoundControl()
            {
                Name = "Dressing Room 01",
                VLCControllers = MakeList(vlc01)
            };

            SPSoundControl sp_soundSecretRoom01 = new SPSoundControl()
            {
                Name = "Secret Room 01",
                VLCControllers = MakeList(vlc02)
            };

            SPSoundControl sp_soundSecretRoom02 = new SPSoundControl()
            {
                Name = "Secret Room 02",
                VLCControllers = MakeList(vlc02_02)
            }; 

            SPSoundControl sp_soundDressingRoomBGM = new SPSoundControl()
            {
                Name = "Dressing Room BGM",
                SongFileName = SoundDressingRoomBGMFileName,
                Volume = SoundDressingRoomBGMVolume,
                VLCControllers = MakeList(vlc01)
            };

            SPSoundControl sp_soundSecretRoomBGM = new SPSoundControl()
            {
                Name = "Secret Room BGM",
                SongFileName = SoundSecretRoomBGMFileName,
                Volume = SoundSecretRoomBGMVolume,
                VLCControllers = MakeList(vlc02)
            };

            SPSoundControl sp_soundAltarTouch = new SPSoundControl()
            {
                Name = "Altar Touch",
                SongFileName = SoundAltarTouchFileName,
                Volume = SoundAltarTouchVolume,
                VLCControllers = MakeList(vlc02)
            };

            SPSoundControl sp_soundAltarWindEffect = new SPSoundControl()
            {
                Name = "Altar Wind Effect",
                SongFileName = SoundAltarWindSFXFileName,
                Volume = SoundAltarWindSFXVolume,
                VLCControllers = MakeList(vlc02_02)
            };

            SPSoundControl sp_soundAltarWhispers = new SPSoundControl()
            {
                Name = "Altar Whispers",
                SongFileName = SoundAltarWhispersFileName,
                Volume = SoundAltarWhispersVolume,
                VLCControllers = MakeList(vlc02)
            };

            SPSoundControl sp_soundNoxSuccessNarration = new SPSoundControl()
            {
                Name = "Nox Success Narration",
                SongFileName = SoundNoxSuccessNarrationFileName,
                Volume = SoundNoxSuccessNarrationVolume,
                VLCControllers = MakeList(vlc02)
            };

            SPSoundControl sp_soundNoxFailureNarration = new SPSoundControl()
            {
                Name = "Nox Fail Narration",
                SongFileName = SoundNoxFailureNarrationFileName,
                Volume = SoundNoxFailureNarrationVolume,
                VLCControllers = MakeList(vlc02)
            };

            AbyssSystem.Instance.RegisterPhysicalObject(vlc01);
            AbyssSystem.Instance.RegisterPhysicalObject(vlc02);
            AbyssSystem.Instance.RegisterPhysicalObject(vlc02_02);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundDressingRoom01);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundSecretRoom01);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundSecretRoom02);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundDressingRoomBGM);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundSecretRoomBGM);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundAltarTouch);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundAltarWhispers);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundAltarWindEffect);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundNoxSuccessNarration);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundNoxFailureNarration);
            sp_soundDressingRoom01.Initialize();
            sp_soundSecretRoom01.Initialize();
            sp_soundSecretRoom02.Initialize();
            sp_soundDressingRoomBGM.Initialize();
            sp_soundSecretRoomBGM.Initialize();
            sp_soundAltarWhispers.Initialize();
            sp_soundAltarWindEffect.Initialize();
            sp_soundNoxSuccessNarration.Initialize();
            sp_soundNoxFailureNarration.Initialize();
            sp_soundAltarTouch.Initialize();

            // ***********************
            // Hint Text Messages
            // ***********************
            TextingController textingMotorola =
                new TextingController("Motorola Droid 2", "F8-7B-7A-88-04-D9");

            AbyssSystem.Instance.RegisterPhysicalObject(textingMotorola);

            // ***********************
            // ALTAR
            // ***********************
            XBeeEndpoint altarArduino = new XBeeEndpoint("ALTAR", "Altar");

            SPXBeeEndpoint sp_altarPowerOn = new SPXBeeEndpoint()
            {
                ExpectedMessage = "POWERON",
                Endpoints = MakeList(altarArduino)
            };

            SPXBeeEndpoint sp_altarTopStart = new SPXBeeEndpoint()
            {
                ExpectedMessage = "TOPSTART",
                Endpoints = MakeList(altarArduino)
            };

            SPXBeeEndpoint sp_altarTopSolved = new SPXBeeEndpoint()
            {
                ExpectedMessage = "TOPSOLVED",
                Endpoints = MakeList(altarArduino)
            };

            SPXBeeEndpoint sp_altarWordBegin = new SPXBeeEndpoint()
            {
                ExpectedMessage = "WORDBEGIN",
                Endpoints = MakeList(altarArduino)
            };

            SPXBeeEndpoint sp_altarTagPresent = new SPXBeeEndpoint()
            {
                ExpectedMessage = "WORDTAGPRESENT",
                Endpoints = MakeList(altarArduino)
            };

            SPXBeeEndpoint sp_altarWordSolved = new SPXBeeEndpoint()
            {
                ExpectedMessage = "WORDSOLVED",
                Endpoints = MakeList(altarArduino)
            };

            SPXBeeEndpoint sp_altarWordFailed = new SPXBeeEndpoint()
            {
                ExpectedMessage = "WORDFAILED",
                Endpoints = MakeList(altarArduino)
            };

            AbyssSystem.Instance.RegisterPhysicalObject(altarArduino);
            AbyssSystem.Instance.RegisterSubProcessor(sp_altarPowerOn);
            AbyssSystem.Instance.RegisterSubProcessor(sp_altarTopStart);
            AbyssSystem.Instance.RegisterSubProcessor(sp_altarTopSolved);
            AbyssSystem.Instance.RegisterSubProcessor(sp_altarWordBegin);
            AbyssSystem.Instance.RegisterSubProcessor(sp_altarTagPresent);
            AbyssSystem.Instance.RegisterSubProcessor(sp_altarWordSolved);
            sp_altarPowerOn.Initialize();
            sp_altarTopStart.Initialize();
            sp_altarTopSolved.Initialize();
            sp_altarWordBegin.Initialize();
            sp_altarTagPresent.Initialize();
            sp_altarWordSolved.Initialize();

            // ***********************
            // LED RGB+W Lightbulbs
            // ***********************
            LimitlessLEDBridge bridge = new LimitlessLEDBridge(
                    "MiLight Bridge 01",
                    "AC-CF-23-46-86-46");

            AbyssSystem.Instance.RegisterPhysicalObject(bridge);

            SPLimitlessLEDBridge sp_lightAllWhite = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetToWhite,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Bridges = MakeList(bridge)
            };

            SPLimitlessLEDBridge sp_lightAllFullBrightness = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetBrightness,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Brightness = 1,
                Bridges = MakeList(bridge)
            };

            AbyssSystem.Instance.RegisterSubProcessor(sp_lightAllWhite);
            AbyssSystem.Instance.RegisterSubProcessor(sp_lightAllFullBrightness);
            sp_lightAllWhite.Initialize();
            sp_lightAllFullBrightness.Initialize();

            // ***********************
            // Script Event Logic
            // ***********************

            // when the game is started
            // stop all music on all servers
            // play dressing room BGM
            // start game timer
            // TODO: enable secret room ambience
            sp_gameController.GameStarted += sp_soundDressingRoomBGM.Play;
            sp_gameController.GameStarted += sp_soundSecretRoom01.Stop;
            sp_gameController.GameStarted += sp_soundSecretRoom02.Stop;
            sp_gameController.GameStarted += sp_countdownScreen.Start;

            // when the top of the altar is solved
            // play some spooky MYST wind effects
            sp_altarTopSolved.ExpectedMessageReceived += sp_soundAltarWindEffect.Play;

            // every time a syllable of the spell is cast on the altar
            // play some sound effect
            sp_altarTagPresent.ExpectedMessageReceived += sp_soundAltarTouch.Play;

            // TODO:
            // when the altar-side word game is started, ramp up spooky whispers
            // when the altar-side word game is failed, turn off the whipsers.
            sp_altarWordBegin.ExpectedMessageReceived += sp_soundAltarWhispers.Play;
            sp_altarWordFailed.ExpectedMessageReceived += sp_soundAltarWhispers.Stop;

            // when the altar-side word game is completed,
            // WIN THE GAME!!
            sp_altarWordSolved.ExpectedMessageReceived += sp_gameController.Win;

            // This is the Win scenario:
            // stop the game timer
            // stop any sounds from the dressing room
            // play Nox narration
            // TODO: decide if the wind effects or other sounds should be turned off
            sp_gameController.GameWon += sp_countdownScreen.Stop;
            sp_gameController.GameWon += sp_soundDressingRoom01.Stop;
            sp_gameController.GameWon += sp_soundNoxSuccessNarration.Play;

            // When the time is up, 
            // LOSE!!
            sp_countdownScreen.CountdownExpired += sp_gameController.Lose;

            // emergency stop
            // pause all sounds
            // whiten all lights
            sp_gameController.GameHardPaused += sp_soundDressingRoom01.Pause;
            sp_gameController.GameHardPaused += sp_soundSecretRoom01.Pause;
            sp_gameController.GameHardPaused += sp_soundSecretRoom02.Pause;
            sp_gameController.GameHardPaused += sp_countdownScreen.Stop;
            sp_gameController.GameHardPaused += sp_lightAllWhite.Run;
            sp_gameController.GameHardPaused += sp_lightAllFullBrightness.Run;
            
            // soft pause:
            // pause all music and sounds, pause game timer
            sp_gameController.GameSoftPaused += sp_soundDressingRoom01.Pause;
            sp_gameController.GameSoftPaused += sp_soundSecretRoom01.Pause;
            sp_gameController.GameSoftPaused += sp_soundSecretRoom02.Pause;
            sp_gameController.GameSoftPaused += sp_countdownScreen.Stop;

            // game is resumed from paused state
            // TODO: 'pause' message is sent to music devices. this is both for pause and unpause. can be dangerous if spamming.
            sp_gameController.GameUnPaused += sp_soundDressingRoom01.Pause;
            sp_gameController.GameUnPaused += sp_soundSecretRoom01.Pause;
            sp_gameController.GameUnPaused += sp_soundSecretRoom02.Pause;
            sp_gameController.GameUnPaused += sp_countdownScreen.Start;

            // ***********************
            // Script Event Logic for LIGHTS
            // ***********************
            SPLimitlessLEDBridge sp_lightGameStart = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Color = LimitlessLEDBridge.ColorType.Green,
                Bridges = MakeList(bridge)
            };
            AbyssSystem.Instance.RegisterSubProcessor(sp_lightGameStart);
            sp_lightGameStart.Initialize();

            SPLimitlessLEDBridge sp_lightGameStartBrighness = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetBrightness,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Brightness = 1,
                Bridges = MakeList(bridge)
            };
            AbyssSystem.Instance.RegisterSubProcessor(sp_lightGameStartBrighness);
            sp_lightGameStartBrighness.Initialize();

            SPLimitlessLEDBridge sp_lightSolveAltarTop = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Color = LimitlessLEDBridge.ColorType.Royal_Blue,
                Bridges = MakeList(bridge)
            };
            AbyssSystem.Instance.RegisterSubProcessor(sp_lightSolveAltarTop);
            sp_lightSolveAltarTop.Initialize();

            SPLimitlessLEDBridge sp_lightTouchAltarWordBefore = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Color = LimitlessLEDBridge.ColorType.Red,
                Bridges = MakeList(bridge)
            };
            AbyssSystem.Instance.RegisterSubProcessor(sp_lightTouchAltarWordBefore);
            sp_lightTouchAltarWordBefore.Initialize();

            SPLimitlessLEDBridge sp_lightTouchAltarWordAfter = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Color = LimitlessLEDBridge.ColorType.Violet,
                Bridges = MakeList(bridge)
            };
            AbyssSystem.Instance.RegisterSubProcessor(sp_lightTouchAltarWordAfter);
            sp_lightTouchAltarWordAfter.Initialize();

            SPDelay sp_touchAltarWordDelay = new SPDelay()
            {
                DurationMs = 300
            };

            // turn on back lights when game is started
            sp_gameController.GameStarted += sp_lightGameStart.Run;
            sp_gameController.GameStarted += sp_lightGameStartBrighness.Run;

            // turn on light when top is solved
            sp_altarTopSolved.ExpectedMessageReceived += sp_lightSolveAltarTop.Run;

            // flicker light sequence when side is touched
            sp_altarTagPresent.ExpectedMessageReceived += sp_lightTouchAltarWordBefore.Run;
            sp_altarTagPresent.ExpectedMessageReceived += sp_touchAltarWordDelay.Start;
            sp_touchAltarWordDelay.Finished += sp_lightTouchAltarWordAfter.Run;

            // ****************************
            // ZONE 1
            SPLimitlessLEDBridge sp_lightWinZ1_Color = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor,
                Zone = LimitlessLEDBridge.ZoneType.Zone1,
                Color = LimitlessLEDBridge.ColorType.Green,
                Bridges = MakeList(bridge)
            };
            sp_lightWinZ1_Color.Initialize();

            SPLimitlessLEDBridge sp_lightWinZ1_On = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.TurnOn,
                Zone = LimitlessLEDBridge.ZoneType.Zone1,
                Bridges = MakeList(bridge)
            };
            sp_lightWinZ1_On.Initialize();

            SPLimitlessLEDBridge sp_lightWinZ1_Off = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.TurnOff,
                Zone = LimitlessLEDBridge.ZoneType.Zone1,
                Bridges = MakeList(bridge)
            };
            sp_lightWinZ1_Off.Initialize();

            SPDelay sp_winZone1_delay1 = new SPDelay()
            {
                DurationMs = 200
            };
            sp_winZone1_delay1.Initialize();

            SPDelay sp_winZone1_delay2 = new SPDelay()
            {
                DurationMs = 200
            };
            sp_winZone1_delay2.Initialize();

            sp_gameController.GameWon += sp_winZone1_delay1.Start;
            sp_gameController.GameWon += sp_lightWinZ1_Color.Run;
            sp_winZone1_delay1.Finished += sp_winZone1_delay2.Start;
            sp_winZone1_delay1.Finished += sp_lightWinZ1_Off.Run;
            sp_winZone1_delay2.Finished += sp_winZone1_delay1.Start;
            sp_winZone1_delay2.Finished += sp_lightWinZ1_On.Run;

            // ****************************
            // ZONE 2
            SPLimitlessLEDBridge sp_lightWinZ2_Color = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor,
                Zone = LimitlessLEDBridge.ZoneType.Zone2,
                Color = LimitlessLEDBridge.ColorType.Violet,
                Bridges = MakeList(bridge)
            };
            sp_lightWinZ2_Color.Initialize();
            SPLimitlessLEDBridge sp_lightWinZ2_On = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.TurnOn,
                Zone = LimitlessLEDBridge.ZoneType.Zone2,
                Bridges = MakeList(bridge)
            };
            sp_lightWinZ2_On.Initialize();
            SPLimitlessLEDBridge sp_lightWinZ2_Off = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.TurnOff,
                Zone = LimitlessLEDBridge.ZoneType.Zone2,
                Bridges = MakeList(bridge)
            };
            sp_lightWinZ2_Off.Initialize();

            SPDelay sp_winZone2_delay1 = new SPDelay()
            {
                DurationMs = 600
            };
            sp_winZone2_delay1.Initialize();

            SPDelay sp_winZone2_delay2 = new SPDelay()
            {
                DurationMs = 800
            };
            sp_winZone2_delay2.Initialize();

            sp_gameController.GameWon += sp_winZone2_delay1.Start;
            sp_gameController.GameWon += sp_lightWinZ2_Color.Run;
            sp_winZone2_delay1.Finished += sp_winZone2_delay2.Start;
            sp_winZone2_delay1.Finished += sp_lightWinZ2_Off.Run;
            sp_winZone2_delay2.Finished += sp_winZone2_delay1.Start;
            sp_winZone2_delay2.Finished += sp_lightWinZ2_On.Run;
            
            // ****************************
            // ZONE 3
            SPLimitlessLEDBridge sp_lightWinZ3_Color = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor,
                Zone = LimitlessLEDBridge.ZoneType.Zone3,
                Color = LimitlessLEDBridge.ColorType.Red,
                Bridges = MakeList(bridge)
            };
            sp_lightWinZ3_Color.Initialize();
            SPLimitlessLEDBridge sp_lightWinZ3_On = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetBrightness,
                Zone = LimitlessLEDBridge.ZoneType.Zone3,
                Brightness = 1,
                Bridges = MakeList(bridge)
            };
            sp_lightWinZ3_On.Initialize();
            SPLimitlessLEDBridge sp_lightWinZ3_Off = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetBrightness,
                Zone = LimitlessLEDBridge.ZoneType.Zone3,
                Brightness = 0.01,
                Bridges = MakeList(bridge)
            };
            sp_lightWinZ3_Off.Initialize();

            SPDelay sp_winZone3_delay1 = new SPDelay()
            {
                DurationMs = 1000
            };
            sp_winZone3_delay1.Initialize();

            SPDelay sp_winZone3_delay2 = new SPDelay()
            {
                DurationMs = 1000
            };
            sp_winZone3_delay2.Initialize();

            sp_gameController.GameWon += sp_winZone3_delay1.Start;
            sp_gameController.GameWon += sp_lightWinZ3_Color.Run;
            sp_winZone3_delay1.Finished += sp_winZone3_delay2.Start;
            sp_winZone3_delay1.Finished += sp_lightWinZ3_Off.Run;
            sp_winZone3_delay2.Finished += sp_winZone3_delay1.Start;
            sp_winZone3_delay2.Finished += sp_lightWinZ3_On.Run;

            // ****************************
            // ZONE 4
            SPLimitlessLEDBridge sp_lightWinZ4_Color = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor,
                Zone = LimitlessLEDBridge.ZoneType.Zone4,
                Color = LimitlessLEDBridge.ColorType.Red,
                Bridges = MakeList(bridge)
            };
            sp_lightWinZ4_Color.Initialize();
            SPLimitlessLEDBridge sp_lightWinZ4_On = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetBrightness,
                Zone = LimitlessLEDBridge.ZoneType.Zone4,
                Brightness = 1,
                Bridges = MakeList(bridge)
            };
            sp_lightWinZ4_On.Initialize();
            SPLimitlessLEDBridge sp_lightWinZ4_Off = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetBrightness,
                Zone = LimitlessLEDBridge.ZoneType.Zone4,
                Brightness = 0.01,
                Bridges = MakeList(bridge)
            };
            sp_lightWinZ4_Off.Initialize();

            SPDelay sp_winZone4_delay1 = new SPDelay()
            {
                DurationMs = 500
            };
            sp_winZone4_delay1.Initialize();

            SPDelay sp_winZone4_delay2 = new SPDelay()
            {
                DurationMs = 1000
            };
            sp_winZone4_delay2.Initialize();

            sp_gameController.GameWon += sp_winZone4_delay1.Start;
            sp_gameController.GameWon += sp_lightWinZ4_Color.Run;
            sp_winZone4_delay1.Finished += sp_winZone4_delay2.Start;
            sp_winZone4_delay1.Finished += sp_lightWinZ4_Off.Run;
            sp_winZone4_delay2.Finished += sp_winZone4_delay1.Start;
            sp_winZone4_delay2.Finished += sp_lightWinZ4_On.Run;

            // *****************************************
            // lose game sequence
            // *****************************************
            SPLimitlessLEDBridge sp_lightLoseHalfDimAll = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetBrightness,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Brightness = 0.5,
                Bridges = MakeList(bridge)
            };
            SPLimitlessLEDBridge sp_lightLoseRedAll = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Color = LimitlessLEDBridge.ColorType.Red,
                Bridges = MakeList(bridge)
            };
            SPLimitlessLEDBridge sp_lightLoseDimAll = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetBrightness,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Brightness = 0.01,
                Bridges = MakeList(bridge)
            };
            SPLimitlessLEDBridge sp_lightLoseOffAll = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.TurnOff,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Brightness = 0.01,
                Bridges = MakeList(bridge)
            };
            
            SPLimitlessLEDBridge sp_lightLoseWhiteAll = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetToWhite,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Brightness = 0.01,
                Bridges = MakeList(bridge)
            };

            SPLimitlessLEDBridge sp_lightLoseLightenAll = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetBrightness,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Brightness = 0.6,
                Bridges = MakeList(bridge)
            };

            SPDelay sp_delayLoseWaitForSoundStop = new SPDelay()
            {
                DurationMs = 1 * 1000
            };

            // 52s is just before Nox laughing
            SPDelay sp_delayLoseWaitForNox = new SPDelay()
            {
                DurationMs = 52 * 1000
            };

            SPDelay sp_delayLoseWaitForDim = new SPDelay()
            {
                DurationMs = 500
            };

            SPDelay sp_delayLoseWaitForOff = new SPDelay()
            {
                DurationMs = 7 * 1000
            };

            SPDelay sp_delayLoseWaitForWhite = new SPDelay()
            {
                DurationMs = 100
            };
            
            // When the game is lost, turn off all sounds, 
            // TODO: currently leaving wind effects playing, maybe should change
            sp_gameController.GameLost += sp_soundDressingRoom01.Stop;
            sp_gameController.GameLost += sp_soundSecretRoom01.Stop;

            // Wait for just a moment to sounds to stop
            sp_gameController.GameLost += sp_delayLoseWaitForSoundStop.Start;

            // Then, play the Nox soundtrack
            // turn all lights to red and dim all lights a bit
            sp_delayLoseWaitForSoundStop.Finished += sp_soundNoxFailureNarration.Play;
            sp_delayLoseWaitForSoundStop.Finished += sp_lightLoseRedAll.Run;
            sp_delayLoseWaitForSoundStop.Finished += sp_lightLoseHalfDimAll.Run;

            // wait for the Nox narration
            sp_delayLoseWaitForSoundStop.Finished += sp_delayLoseWaitForNox.Start;

            // dim the lights to as low as possible
            // then turn off the lights (hacky transition)
            sp_delayLoseWaitForNox.Finished += sp_lightLoseDimAll.Run;
            sp_delayLoseWaitForNox.Finished += sp_delayLoseWaitForDim.Start;
            sp_delayLoseWaitForDim.Finished += sp_lightLoseOffAll.Run;

            // wait for dramatic effect
            sp_delayLoseWaitForDim.Finished += sp_delayLoseWaitForOff.Start;

            // turn to white and turn back on
            sp_delayLoseWaitForOff.Finished += sp_lightLoseWhiteAll.Run;
            sp_delayLoseWaitForOff.Finished += sp_delayLoseWaitForWhite.Start;
            sp_delayLoseWaitForWhite.Finished += sp_lightLoseLightenAll.Run;
        }

        private List<T> MakeList<T>(params T[] listItems)
        {
            List<T> list = new List<T>();
            list.AddRange(listItems);
            return list;
        }
    }
}
