using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Net;
//BepInEx stuff
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib; //god im hoping i got the right version of harmony
using BepInEx.Configuration;
//this code is reused from BaldiMP, lol

namespace BBPlusTwitch
{
    [BepInPlugin("mtm101.rulerp.bbplus.balditwitch", "BB+ Twitch Intergration", "0.0.0.0")]



    public class BaldiTwitch : BaseUnityPlugin
    {
        public static ConfigEntry<string> Name;

        public static ConfigEntry<string> UserName;

        public static ConfigEntry<string> Oath;
        void Awake()
        {
            Logger.LogDebug("hey wassup bb+ twitch mod here to say snas");
            Name = Config.Bind(new ConfigDefinition(
                "Auth Info",
                "Name"

                ), "generic_name");

            UserName = Config.Bind(new ConfigDefinition(
                "Auth Info",
                "Username"

                ), "generic_username");

            Oath = Config.Bind(new ConfigDefinition(
                "Auth Info",
                "OATH"

                ), "oath:https://twitchapps.com/tmi/");

            Harmony harmony = new Harmony("mtm101.rulerp.bbplus.balditwitch");

            harmony.PatchAll();

        }
    }
}
