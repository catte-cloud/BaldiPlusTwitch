using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Net;
using System.IO;
//BepInEx stuff
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib;
//more stuff
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using StolenYetHelpfulCode;


namespace BBPlusTwitch
{
    public static class IngameCommands
    {
        public static void AddCommands()
        {
            TwitchManager.AddCommand("addytps",AddYTPs);
        }

        public static bool AddYTPs(string person, string number)
        {
            if ((!int.TryParse(number, out int num)) || !Singleton<CoreGameManager>.Instance)
            {
                return false;
            }
            Singleton<CoreGameManager>.Instance.AddPoints(num,0,true);
            return true;
        }


    }
}