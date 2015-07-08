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
                "RPi 105:50000", 
                VLCServerControl.OSType.Linux, 
                "192.168.0.105", 
                "50000");

            VLCServerControl vlc02 = new VLCServerControl(
                "RPi 100:50000",
                VLCServerControl.OSType.Linux,
                "192.168.0.100",
                "50000");

            SPSoundControl sp_soundDressingRoomBGM = new SPSoundControl()
            {
                Name = "Dressing Room BGM",
                SongFileName = @"MagicShow-NoSFX.mp3",
                Volume = 300f,
                VLCControllers = new List<VLCServerControl>() 
                {
                    vlc01
                }
            };

            SPSoundControl sp_soundSecretRoomBGM = new SPSoundControl()
            {
                Name = "Secret Room BGM",
                SongFileName = @"MagicShow-NoSFX.mp3",
                Volume = 300f,
                VLCControllers = new List<VLCServerControl>() 
                {
                    vlc02
                }
            };

            SPSoundControl sp_soundAltarWhispers = new SPSoundControl()
            {
                Name = "Altar Whispers",
                SongFileName = @"MagicShow-NoSFX.mp3",
                Volume = 300f,
                VLCControllers = new List<VLCServerControl>() 
                {
                    vlc02
                }
            };

            SPSoundControl sp_soundNoxSuccessNarration = new SPSoundControl()
            {
                Name = "Nox Success Narration",
                SongFileName = @"MagicShow-NoSFX.mp3",
                Volume = 300f,
                VLCControllers = new List<VLCServerControl>() 
                {
                    vlc02
                }
            };

            SPSoundControl sp_soundNoxFailureNarration = new SPSoundControl()
            {
                Name = "Nox Fail Narration",
                SongFileName = @"MagicShow-NoSFX.mp3",
                Volume = 300f,
                VLCControllers = new List<VLCServerControl>() 
                {
                    vlc02
                }
            };

            AbyssSystem.Instance.RegisterPhysicalObject(vlc01);
            AbyssSystem.Instance.RegisterPhysicalObject(vlc02);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundDressingRoomBGM);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundSecretRoomBGM);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundAltarWhispers);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundNoxSuccessNarration);
            AbyssSystem.Instance.RegisterSubProcessor(sp_soundNoxFailureNarration);
            sp_soundDressingRoomBGM.Initialize();
            sp_soundSecretRoomBGM.Initialize();
            sp_soundAltarWhispers.Initialize();
            sp_soundNoxSuccessNarration.Initialize();
            sp_soundNoxFailureNarration.Initialize();

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
            sp_countdownScreen.CountdownStarted += sp_soundSecretRoomBGM.Play;

            sp_countdownScreen.CountdownExpired += sp_soundDressingRoomBGM.Stop;
            sp_countdownScreen.CountdownExpired += sp_soundSecretRoomBGM.Stop;
            sp_countdownScreen.CountdownExpired += sp_soundAltarWhispers.Stop;
            sp_countdownScreen.CountdownExpired += sp_soundNoxFailureNarration.Play;

            // not yet implemented
            //sp_altarTagPresent.ExpectedMessageReceived += sp_soundAltarWhispers.Play;
            //sp_altarWordFailed.ExpectedMessageReceived += sp_soundAltarWhispers.Stop;

            sp_altarWordSolved.ExpectedMessageReceived += sp_countdownScreen.Stop;
            sp_altarWordSolved.ExpectedMessageReceived += sp_soundDressingRoomBGM.Stop;
            sp_altarWordSolved.ExpectedMessageReceived += sp_soundSecretRoomBGM.Stop;
            sp_altarWordSolved.ExpectedMessageReceived += sp_soundAltarWhispers.Stop;
            sp_altarWordSolved.ExpectedMessageReceived += sp_soundNoxFailureNarration.Stop;
            sp_altarWordSolved.ExpectedMessageReceived += sp_soundNoxSuccessNarration.Play;

            // not yet implemented
            // emergency stop pressed


            // *****************************************
            // TEST CODE
            // testing LIGHT BULBS
            LimitlessLEDBridge bridge = new LimitlessLEDBridge("Bridge01", "192.168.1.7");
            AbyssSystem.Instance.RegisterPhysicalObject(bridge);

            SPLimitlessLEDBridge turnOnAll = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.TurnOn,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };

            SPLimitlessLEDBridge turnOffAll = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.TurnOff,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };

            SPLimitlessLEDBridge turnYellow = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Color = LimitlessLEDBridge.ColorType.Yellow_Orange,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };

            SPLimitlessLEDBridge turnBlue = new SPLimitlessLEDBridge()
            {
                Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor,
                Zone = LimitlessLEDBridge.ZoneType.All,
                Color = LimitlessLEDBridge.ColorType.Baby_Blue,
                Bridges = new List<LimitlessLEDBridge>
                {
                    bridge
                }
            };

            //bridge.TurnOn(LimitlessLEDBridge.ZoneType.All);
            //bridge.TurnOff(LimitlessLEDBridge.ZoneType.All);
            //bridge.TurnOn(LimitlessLEDBridge.ZoneType.All);

            //turnOnAll.Run(null, EventArgs.Empty);
            //Thread.Sleep(1000 * 1);
            //turnOffAll.Run(null, EventArgs.Empty);
            //Thread.Sleep(1000 * 1);
            //turnOnAll.Run(null, EventArgs.Empty);
            //Thread.Sleep(1000 * 1);
            //turnYellow.Run(null, EventArgs.Empty);
            //Thread.Sleep(1000 * 1);
            //turnBlue.Run(null, EventArgs.Empty);
            //Thread.Sleep(1000 * 1);
        }
    }
}
