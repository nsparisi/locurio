using System;
using System.Collections.Generic;
using AbyssLibrary;
using System.Threading;

namespace Abyss
{
    public class AbyssRunner
    {
        public void Start()
        {
            Thread t = new Thread(Run);
            t.Start();
        }

        public void Run()
        {
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
                "RPi 107:50000", 
                VLCServerControl.OSType.Linux, 
                "192.168.0.107", 
                "50000");

            VLCServerControl vlc02 = new VLCServerControl(
                "RPi 106:50000",
                VLCServerControl.OSType.Linux,
                "192.168.0.106",
                "50000");

            SPSoundControl sp_soundDressingRoomBGM = new SPSoundControl()
            {
                Name = "Dressing Room BGM",
                SongFileName = @"dressing_room_bgm_7-3.mp3",
                Volume = 400f,
                VLCControllers = new List<VLCServerControl>() 
                {
                    vlc01
                }
            };

            SPSoundControl sp_soundSecretRoomBGM = new SPSoundControl()
            {
                Name = "Secret Room BGM",
                SongFileName = @"MagicShow-NoSFX.mp3",
                Volume = 400f,
                VLCControllers = new List<VLCServerControl>() 
                {
                    vlc02
                }
            };

            SPSoundControl sp_soundAltarTouch = new SPSoundControl()
            {
                Name = "Altar Touch",
                SongFileName = @"explosion_sfx.mp3",
                Volume = 100f,
                VLCControllers = new List<VLCServerControl>() 
                {
                    vlc02
                }
            };

            SPSoundControl sp_soundAltarWhispers = new SPSoundControl()
            {
                Name = "Altar Whispers",
                SongFileName = @"Altar.mp3",
                Volume = 200f,
                VLCControllers = new List<VLCServerControl>() 
                {
                    vlc02
                }
            };

            SPSoundControl sp_soundNoxSuccessNarration = new SPSoundControl()
            {
                Name = "Nox Success Narration",
                SongFileName = @"endgame_win.mp3",
                Volume = 280f,
                VLCControllers = new List<VLCServerControl>() 
                {
                    vlc02
                }
            };

            SPSoundControl sp_soundNoxFailureNarration = new SPSoundControl()
            {
                Name = "Nox Fail Narration",
                SongFileName = @"endgame_lose.m4a",
                Volume = 280f,
                VLCControllers = new List<VLCServerControl>() 
                {
                    vlc02
                }
            };

            AbyssSystem.Instance.RegisterPhysicalObject(vlc01);
            AbyssSystem.Instance.RegisterPhysicalObject(vlc02);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundDressingRoomBGM);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundSecretRoomBGM);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundAltarTouch);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundAltarWhispers);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundNoxSuccessNarration);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundNoxFailureNarration);
            sp_soundDressingRoomBGM.Initialize();
            sp_soundSecretRoomBGM.Initialize();
            sp_soundAltarWhispers.Initialize();
            sp_soundNoxSuccessNarration.Initialize();
            sp_soundNoxFailureNarration.Initialize();
            sp_soundAltarTouch.Initialize();

            // ***********************
            // Hint Text Messages
            // ***********************
            TextingController textingMotorola = new TextingController("192.168.1.12", "Motorola Droid 2");
            AbyssSystem.Instance.RegisterPhysicalObject(textingMotorola);

            // ***********************
            // ALTAR
            // ***********************
            XBeeEndpoint altarArduino = new XBeeEndpoint("ALTAR", "Altar");

            SPXBeeEndpoint sp_altarPowerOn = new SPXBeeEndpoint()
            {
                ExpectedMessage = "POWERON",
                Endpoints = new List<XBeeEndpoint>
                {
                    altarArduino
                }
            };

            SPXBeeEndpoint sp_altarTopStart = new SPXBeeEndpoint()
            {
                ExpectedMessage = "TOPSTART",
                Endpoints = new List<XBeeEndpoint>
                {
                    altarArduino
                }
            };

            SPXBeeEndpoint sp_altarTopSolved = new SPXBeeEndpoint()
            {
                ExpectedMessage = "TOPSOLVED",
                Endpoints = new List<XBeeEndpoint>
                {
                    altarArduino
                }
            };

            SPXBeeEndpoint sp_altarWordBegin = new SPXBeeEndpoint()
            {
                ExpectedMessage = "WORDBEGIN",
                Endpoints = new List<XBeeEndpoint>
                {
                    altarArduino
                }
            };

            SPXBeeEndpoint sp_altarTagPresent = new SPXBeeEndpoint()
            {
                ExpectedMessage = "WORDTAGPRESENT",
                Endpoints = new List<XBeeEndpoint>
                {
                    altarArduino
                }
            };

            SPXBeeEndpoint sp_altarWordSolved = new SPXBeeEndpoint()
            {
                ExpectedMessage = "WORDSOLVED",
                Endpoints = new List<XBeeEndpoint>
                {
                    altarArduino
                }
            };

            SPXBeeEndpoint sp_altarWordFailed = new SPXBeeEndpoint()
            {
                ExpectedMessage = "WORDFAILED",
                Endpoints = new List<XBeeEndpoint>
                {
                    altarArduino
                }
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
            // Script Event Logic
            // ***********************

            // countdown clock START -> start dressing room music
            // countdown clock START -> start secret room music

            // countdown clock EXPIRED -> stop dressing room music
            // countdown clock EXPIRED -> stop secret room music
            // countdown clock EXPIRED -> stop secret room whispers
            // countdown clock EXPIRED -> lose game music

            // altar WORDTAGPRESENT (first time only) -> start whispers
            // altar WORDFAILED (not present) -> stop whispers

            // altar WORDSOLVED -> pause clock
            // altar WORDSOLVED -> stop dressing room music
            // altar WORDSOLVED -> stop secret room music
            // altar WORDSOLVED -> stop secret room whispers
            // altar WORDSOLVED -> start success game music

            // emergency stop PRESSED -> pause dressing room music
            // emergency stop PRESSED -> pause secret room music
            // emergency stop PRESSED -> pause countdown clock 

            sp_countdownScreen.CountdownStarted += sp_soundDressingRoomBGM.Play;
            //sp_countdownScreen.CountdownStarted += sp_soundSecretRoomBGM.Play;

            sp_countdownScreen.CountdownExpired += sp_soundDressingRoomBGM.Stop;
            //sp_countdownScreen.CountdownExpired += sp_soundSecretRoomBGM.Stop;
            sp_countdownScreen.CountdownExpired += sp_soundAltarWhispers.Stop;
            sp_countdownScreen.CountdownExpired += sp_soundNoxFailureNarration.Play;

            // not yet implemented
            sp_altarTopSolved.ExpectedMessageReceived += sp_soundAltarWhispers.Play;
            sp_altarTagPresent.ExpectedMessageReceived += sp_soundAltarTouch.Play;
            //sp_altarWordFailed.ExpectedMessageReceived += sp_soundAltarWhispers.Stop;

            sp_altarWordSolved.ExpectedMessageReceived += sp_countdownScreen.Stop;
            sp_altarWordSolved.ExpectedMessageReceived += sp_soundDressingRoomBGM.Stop;
            //sp_altarWordSolved.ExpectedMessageReceived += sp_soundSecretRoomBGM.Stop;
            sp_altarWordSolved.ExpectedMessageReceived += sp_soundAltarWhispers.Stop;
            sp_altarWordSolved.ExpectedMessageReceived += sp_soundNoxFailureNarration.Stop;
            sp_altarWordSolved.ExpectedMessageReceived += sp_soundNoxSuccessNarration.Play;

            // not yet implemented
            // emergency stop pressed


            // *****************************************
            // TEST CODE
            // testing LIGHT BULBS
            LimitlessLEDBridge bridge = new LimitlessLEDBridge("Bridge01", "192.168.0.104");
            AbyssSystem.Instance.RegisterPhysicalObject(bridge);
            
            SPLimitlessLEDBridge sp_lightGameStart = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Color = LimitlessLEDBridge.ColorType.Royal_Blue,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };

            SPLimitlessLEDBridge sp_lightSolveAltarTop = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Color = LimitlessLEDBridge.ColorType.Green,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };

            SPLimitlessLEDBridge sp_lightTouchAltarWordBefore = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Color = LimitlessLEDBridge.ColorType.Red,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };

            SPLimitlessLEDBridge sp_lightTouchAltarWordAfter = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Color = LimitlessLEDBridge.ColorType.Violet,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };

            SPDelay sp_touchAltarWordDelay = new SPDelay()
            {
                DurationMs = 300
            };


            // turn on back lights when game is started
            sp_countdownScreen.CountdownStarted += sp_lightGameStart.Run;

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
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };
            sp_lightWinZ1_Color.Initialize();

            SPLimitlessLEDBridge sp_lightWinZ1_On = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.TurnOn,
                Zone = LimitlessLEDBridge.ZoneType.Zone1,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };
            sp_lightWinZ1_On.Initialize();

            SPLimitlessLEDBridge sp_lightWinZ1_Off = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.TurnOff,
                Zone = LimitlessLEDBridge.ZoneType.Zone1,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
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

            sp_altarWordSolved.ExpectedMessageReceived += sp_winZone1_delay1.Start;
            sp_altarWordSolved.ExpectedMessageReceived += sp_lightWinZ1_Color.Run;
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
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };
            sp_lightWinZ2_Color.Initialize();
            SPLimitlessLEDBridge sp_lightWinZ2_On = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.TurnOn,
                Zone = LimitlessLEDBridge.ZoneType.Zone2,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };
            sp_lightWinZ2_On.Initialize();
            SPLimitlessLEDBridge sp_lightWinZ2_Off = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.TurnOff,
                Zone = LimitlessLEDBridge.ZoneType.Zone2,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
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

            sp_altarWordSolved.ExpectedMessageReceived += sp_winZone2_delay1.Start;
            sp_altarWordSolved.ExpectedMessageReceived += sp_lightWinZ2_Color.Run;
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
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };
            sp_lightWinZ3_Color.Initialize();
            SPLimitlessLEDBridge sp_lightWinZ3_On = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetBrightness,
                Zone = LimitlessLEDBridge.ZoneType.Zone3,
                Brightness = 1,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };
            sp_lightWinZ3_On.Initialize();
            SPLimitlessLEDBridge sp_lightWinZ3_Off = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetBrightness,
                Zone = LimitlessLEDBridge.ZoneType.Zone3,
                Brightness = 0.01,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
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

            sp_altarWordSolved.ExpectedMessageReceived += sp_winZone3_delay1.Start;
            sp_altarWordSolved.ExpectedMessageReceived += sp_lightWinZ3_Color.Run;
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
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };
            sp_lightWinZ4_Color.Initialize();
            SPLimitlessLEDBridge sp_lightWinZ4_On = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetBrightness,
                Zone = LimitlessLEDBridge.ZoneType.Zone4,
                Brightness = 1,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };
            sp_lightWinZ4_On.Initialize();
            SPLimitlessLEDBridge sp_lightWinZ4_Off = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetBrightness,
                Zone = LimitlessLEDBridge.ZoneType.Zone4,
                Brightness = 0.01,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
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

            sp_altarWordSolved.ExpectedMessageReceived += sp_winZone4_delay1.Start;
            sp_altarWordSolved.ExpectedMessageReceived += sp_lightWinZ4_Color.Run;
            sp_winZone4_delay1.Finished += sp_winZone4_delay2.Start;
            sp_winZone4_delay1.Finished += sp_lightWinZ4_Off.Run;
            sp_winZone4_delay2.Finished += sp_winZone4_delay1.Start;
            sp_winZone4_delay2.Finished += sp_lightWinZ4_On.Run;

            // uncomment to win the game
            //sp_altarWordSolved.DebugReceivedExpectedMessage();
        }
    }
}
