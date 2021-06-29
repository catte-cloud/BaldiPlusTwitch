using System;
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
using BepInEx.Configuration;

namespace BBPlusTwitch
{
    public enum NameMenuState
    {
        ModeSelector,
        Options,
        SaveSelect,
        Loading
    }

    public enum TwitchMode
    {
        Vanilla,
        Speedy,
        Chaos,
        Offline
    }
    public static class SettingsManager
    {
        public static TwitchMode Mode = TwitchMode.Vanilla;
        public static bool ShowCommands = false;
        public static bool ShowVotes = true;
        public static bool OfflineUseWeighted = true;
    }


    public static class GeneralBaldiStuff
    {
        public static readonly ItemObject[] Items = Resources.FindObjectsOfTypeAll<ItemObject>();
        public static readonly FieldTripObject[] FieldTrips = Resources.FindObjectsOfTypeAll<FieldTripObject>();
        public static RandomEvent[] RandomEvents;


    }

    public class OfflineCommand
    {
        public string command;
        public string[] valid_params;

        public OfflineCommand(string cmd, params string[] parms)
        {
            command = cmd;
            valid_params = parms;
        }

    }

    public class WeightedOfflineCommand : WeightedSelection<OfflineCommand> //um????
    {
        public WeightedOfflineCommand(string cmd, int w, params string[] parms)
        {
            weight = w;
            selection = new OfflineCommand(cmd, parms);
        }

    }


    public class TwitchCommand
    {
        public string command;
        public string description;
        public Func<string, string, bool> functocall;
        public int MinVotes;
        public bool Enabled = true;

        public TwitchCommand(string name, string desc, Func<string, string, bool> func, int max, bool enabled)
        {
            command = name;
            description = desc;
            functocall = func;
            MinVotes = max;
            Enabled = enabled;
        }
    }


   

    public static class TwitchManager
    {

        public static Dictionary<string, TwitchCommand> Commands = new Dictionary<string, TwitchCommand>();

        public static Dictionary<string, List<string[]>> CommandVotes = new Dictionary<string, List<string[]>>();

        public static List<WeightedOfflineCommand> WeightedCommands = new List<WeightedOfflineCommand>();

        public static float CommandCooldown = 5f;

        public static bool CooldownEnabled = false;

        public static void AddWeightedCommand(string name,int weight, params string[] parms)
        {
            WeightedCommands.Add(new WeightedOfflineCommand(name,weight,parms));
        }

        public static bool AddCommand(string cmd, Func<string,string, bool> func, int min = -1)
        {
            UnityEngine.Debug.Log("Attempting to add command:" + cmd);
            CommandVotes.Add(cmd,new List<string[]>());
            try
            {
                Commands.Add(cmd, new TwitchCommand(cmd, "description missing", func, BaldiTwitch.Instance.Config.Bind(new ConfigDefinition(
                "Command: " + cmd,
                "Votes Needed"
                ), min).Value, BaldiTwitch.Instance.Config.Bind(new ConfigDefinition(
                "Command: " + cmd,
                "Enabled"
                ), true).Value)); //this is really fucking stupid, remind me to create a custom method of storing this shit
            }
            catch
            {
                return false;
            }
            return true;
        }
    }


}
