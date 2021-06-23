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
    //copied from BB+ multiplayer AGAIN i love re-using code
    [HarmonyPatch(typeof(NameManager))]
    [HarmonyPatch("Load")]
    class HijackDefaultLoad
    {
        static bool Prefix(NameManager __instance, ref NameButton[] ___buttons, ref string[] ___nameList, ref int ___nameCount) //Initialize initial connect menu
        {
            if (NameMenuManager.CurrentState == NameMenuState.ModeSelector)
            {
                UnityEngine.Debug.Log("loading mode selector");
                ___nameList[0] = "Vanilla";
                ___nameList[1] = "Speedy";
                ___nameList[2] = "Chaos";
            }
            else
            {
                UnityEngine.Debug.Log("not loading mode selector");
            }

            return NameMenuManager.CurrentState == NameMenuState.SaveSelect;
        }
    }

    [HarmonyPatch(typeof(NameManager))]
    [HarmonyPatch("Awake")]
    class HijackNameAwake
    {
        static bool Prefix(NameManager __instance)
        {
            //UnityEngine.Debug.Log(BaldiTwitch.Oath.Value);
            GameObject newgam = new GameObject();
            newgam.name = "TwitchHandlerObject";
            newgam.AddComponent<TwitchConnectionHandler>();
            GameObject newlod = new GameObject();
            newlod.name = "MonoLogicObject";
            newlod.AddComponent<MonoLogicManager>();

            IngameCommands.AddCommands();
            //TwitchManager.AddCommand("test",TestFunction,3);
            return true;
        }

        public static bool TestFunction(string person, string param)
        {
            UnityEngine.Debug.Log(person);
            UnityEngine.Debug.Log(param);
            return true;
        }
    }

    [HarmonyPatch(typeof(NameManager))]
    [HarmonyPatch("NameClicked")]
    class ModifyNameClick
    {
        static bool Prefix(NameManager __instance, int fileNo, ref int ___nameCount, ref string ___newName, ref string[] ___nameList, ref NameButton[] ___buttons, ref bool ___enteringNewName)
        {
            if (NameMenuManager.CurrentState == NameMenuState.ModeSelector)
            {
                if (fileNo > 2)
                {
                    return false;
                }
                NameMenuManager.CurrentState = NameMenuState.SaveSelect;
                SettingsManager.Mode = (TwitchMode)fileNo;
                //THANK YOU STACK OVERFLOW YOU HAVE SAVED MY LIFE
                __instance.InvokeMethod<NameManager>("Load");
                __instance.UpdateState();
                return false;
            }
            return NameMenuManager.CurrentState == NameMenuState.SaveSelect;
        }

    }

    [HarmonyPatch(typeof(NameManager))]
    [HarmonyPatch("Update")]
    class NoNameUpdate
    {
        static bool Prefix(NameManager __instance, ref NameButton[] ___buttons, ref string[] ___nameList, ref bool ___enteringNewName, ref string ___newName, ref int ___nameCount)
        {
            
            return NameMenuManager.CurrentState == NameMenuState.SaveSelect;
        }
    }
}
