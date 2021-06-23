using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Net;
using System.IO;
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
using System.Runtime.Serialization.Formatters.Binary;
using StolenYetHelpfulCode;


namespace BBPlusTwitch
{
    public static class IngameCommands
    {
        public static void AddCommands()
        {
            TwitchManager.AddCommand("addytps", AddYTPs, 4);
            TwitchManager.AddCommand("giveitem", GiveItem, 5);
            TwitchManager.AddCommand("removeitem", RemoveItem, 8);
            TwitchManager.AddCommand("collectbook", CollectNotebook,10);
            TwitchManager.AddCommand("removebook", RemoveNotebook,15);
        }

        public static bool AddYTPs(string person, string number)
        {
            if ((!int.TryParse(number, out int num)) || !Singleton<CoreGameManager>.Instance)
            {
                return false;
            }
            Singleton<CoreGameManager>.Instance.AddPoints(Math.Clamp(num, -200, 200), 0, true);
            return true;
        }

        public static bool CollectNotebook(string person, string number)
        {
            if (!Singleton<CoreGameManager>.Instance)
            {
                return false;
            }
            Notebook[] Notebooks = GameObject.FindObjectsOfType<Notebook>();
            System.Random rng = new System.Random();
            foreach (Notebook note in Notebooks.OrderBy(a => rng.Next()))
            {
                if (note.icon != null)
                {
                    if (note.icon.sprite.enabled)
                    {
                        note.Clicked(0);
                        return true;
                    }
                }
            }
            return true;
        }

        public static bool RemoveNotebook(string person, string number)
        {
            if (!Singleton<BaseGameManager>.Instance)
            {
                return false;
            }
            Notebook[] Notebooks = GameObject.FindObjectsOfType<Notebook>();
            System.Random rng = new System.Random();
            foreach (Notebook note in Notebooks.OrderBy(a => rng.Next()))
            {
                if (note.icon != null)
                {
                    if (!note.icon.sprite.enabled)
                    {
                        note.Hide(false);
                        Singleton<CoreGameManager>.Instance.AddPoints(-10,0,true);
                        Singleton<BaseGameManager>.Instance.CollectNotebooks(-1);
                        return true;
                    }
                }
            }
            return true;
        }

        public static bool RemoveItem(string person, string number)
        {
            if (!Singleton<CoreGameManager>.Instance)
            {
                return false;
            }
            Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.RemoveRandomItem();
            return true;
        }

        public static bool GiveItem(string person, string enu)
        {
            if ((!Enum.TryParse(enu, true, out Items nu)) || !Singleton<CoreGameManager>.Instance)
            {
                nu = Items.Quarter;
            }
            ItemObject item = GeneralBaldiStuff.Items.ToList().Find(x => x.itemType == nu);
            if (item == null)
            {
                item = GeneralBaldiStuff.Items.ToList().Find(x => x.itemType == Items.Quarter);
                if (item == null)
                {
                    UnityEngine.Debug.LogError("Something has gone horribly wrong here!");
                    return false;
                }
            }
            item.nameKey = Singleton<LocalizationManager>.Instance.GetLocalizedText(item.nameKey) + "\n(" + person + ")";
            Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.AddItem(item);


            return true;
        }


    }
}