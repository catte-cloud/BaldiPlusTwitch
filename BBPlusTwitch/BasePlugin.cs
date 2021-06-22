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
//this code is reused from BaldiMP, lol

namespace BaldiTwitch
{
    [BepInPlugin("mtm101.rulerp.bbplus.balditwitch", "BB+ Twitch Intergration", "0.0.0.0")]
    public class BaldiTwitch : BaseUnityPlugin
    {
        void Awake()
        {
            Logger.LogDebug("hey wassup bb+ twitch mod here to say snas");
            Harmony harmony = new Harmony("mtm101.rulerp.bbplus.balditwitch");

            harmony.PatchAll();
        }
    }
}
