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
using HarmonyLib;
//more stuff
using System.Collections.Generic;
using System.Collections;

namespace BBPlusTwitch
{
    public enum NameMenuState
    {
        ModeSelector,
        Options,
        SaveSelect
    }

    public enum TwitchMode
    {
        Vanilla,
        Speedy,
        Chaos
    }
    public static class SettingsManager
    {
        public static TwitchMode Mode = TwitchMode.Vanilla;
    }



    public class TwitchCommand
    {
        public string command;
        public string description;
        public Func<string, string, bool> functocall;
        public int MinVotes;

        public TwitchCommand(string name, string desc, Func<string, string, bool> func, int max)
        {
            command = name;
            description = desc;
            functocall = func;
            MinVotes = max;
        }
    }


    public static class TwitchManager
    {

        public static Dictionary<string, TwitchCommand> Commands = new Dictionary<string, TwitchCommand>();

        public static bool AddCommand(string cmd, Func<string,string, bool> func, int min = -1)
        {
            return Commands.TryAdd(cmd,new TwitchCommand(cmd,"description missing",func,min));
        }
    }

    public static class NameMenuManager
    {
        public static NameMenuState CurrentState = NameMenuState.ModeSelector;
    }
}
