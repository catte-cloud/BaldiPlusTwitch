﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Net;
using System.Linq;
//BepInEx stuff
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib;
//more stuff
using System.Collections.Generic;
using System.Collections;
using StolenYetHelpfulCode;

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


    public static class GeneralBaldiStuff
    {
        public static readonly ItemObject[] Items = Resources.FindObjectsOfTypeAll<ItemObject>();
        public static readonly FieldTripObject[] FieldTrips = Resources.FindObjectsOfTypeAll<FieldTripObject>();
        public static RandomEvent[] RandomEvents;



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

        public static Dictionary<string, List<string[]>> CommandVotes = new Dictionary<string, List<string[]>>();

        public static float CommandCooldown = 5f;

        public static bool CooldownEnabled = false;

        public static bool AddCommand(string cmd, Func<string, string, bool> func, int min = -1)
        {
            CommandVotes.Add(cmd, new List<string[]>());
            try
            {
                Commands.Add(cmd, new TwitchCommand(cmd, "description missing", func, min));
            }
            catch
            {
                return false;
            }
            return true;
        }
    }

    public static class NameMenuManager
    {
        public static NameMenuState CurrentState = NameMenuState.ModeSelector;
    }
}
