﻿using System;
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
            TwitchManager.AddCommand("removeytps", RemoveYTPs, 4);
            TwitchManager.AddCommand("flicker", FlickerLights, 4);
            TwitchManager.AddCommand("giveitem", GiveItem, 5);
            TwitchManager.AddCommand("removeitem", RemoveItem, 8);
            TwitchManager.AddCommand("collectbook", CollectNotebook, 10);
            TwitchManager.AddCommand("removebook", RemoveNotebook, 15);
            TwitchManager.AddCommand("alert-baldi", AlertBaldi, 20);
            TwitchManager.AddCommand("congratulate", BaldiSaysCongrats, 10);
            TwitchManager.AddCommand("makesus", MakeSus, 15);
            TwitchManager.AddCommand("teleport", Teleport, 20); //someone please fix this
            TwitchManager.AddCommand("sellall", SellAll, 50);
            TwitchManager.AddCommand("sendallnpcs", OhGodOhFuck, 55);
            TwitchManager.AddCommand("mute", MuteGame, 50);
            TwitchManager.AddCommand("rotatecamera", rotatecamera, 20);

            //the following commands can only be executed in johnny's shop

            TwitchManager.AddCommand("shop-forcebuy", ForceBuy, 12);

        }

        public static bool rotatecamera (string person, string number) //Singleton<CoreGameManager>.Instance.GetCamera(0)
        {
            string[] param = number.Split(' ');

            if ((!int.TryParse(param[1], out int num)) || !Singleton<CoreGameManager>.Instance) //yes i did copy this, grow up
            {
                return false;
            }
            if (param[0] == "X")
            {
                Singleton<CoreGameManager>.Instance.GetCamera(0).cameraModifiers.Add(new CameraModifier(Vector3.zero, new Vector3(num, 0, 0)));
            }
            if (param[0] == "Y")
            {
                Singleton<CoreGameManager>.Instance.GetCamera(0).cameraModifiers.Add(new CameraModifier(Vector3.zero, new Vector3(0, num, 0)));
            }
            if (param[0] == "Z")
            {
                Singleton<CoreGameManager>.Instance.GetCamera(0).cameraModifiers.Add(new CameraModifier(Vector3.zero, new Vector3(0, 0, num)));
            }
            return true;

        }

        public static bool FlickerLights(string person, string number)
        {
            if (!Singleton<BaseGameManager>.Instance)
            {
                return false;
            }
            MonoLogicManager.Instance.FlickerTime = -3;
            MonoLogicManager.Instance.DeezNuts = true;
            return true;
        }

        public static bool AddYTPs(string person, string number)
        {
            if ((!int.TryParse(number, out int num)) || !Singleton<CoreGameManager>.Instance)
            {
                return false;
            }
            Singleton<CoreGameManager>.Instance.AddPoints(num, 0, true);
            return true;
        }

        public static bool RemoveYTPs(string person, string number)
        {
            if ((!int.TryParse(number, out int num)) || !Singleton<CoreGameManager>.Instance)
            {
                return false;
            }
            Singleton<CoreGameManager>.Instance.AddPoints(-num, 0, true);
            return true;
        }


        public static bool ForceBuy(string person, string number)
        {
            StoreScreen shop = GameObject.FindObjectOfType<StoreScreen>();
            if ((!int.TryParse(number, out int num)) || shop == null)
            {
                return false;
            }
            shop.BuyItem(num);
            return true;
        }

        public static bool OhGodOhFuck(string person, string number)
        {
            if (!Singleton<BaseGameManager>.Instance || !Singleton<CoreGameManager>.Instance)
            {
                return false;
            }
            EnvironmentController ec = Singleton<BaseGameManager>.Instance.CurrentEc;
            foreach (NPC npc in ec.Npcs)
            {
                npc.TargetPosition(Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position);
            }
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
            if (!Singleton<BaseGameManager>.Instance || !Singleton<CoreGameManager>.Instance)
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
                        Singleton<CoreGameManager>.Instance.AddPoints(-10, 0, true);
                        Singleton<BaseGameManager>.Instance.CollectNotebooks(-1);
                        return true;
                    }
                }
            }
            return true;
        }

        public static bool MuteGame(string person, string number)
        {
            if (!Singleton<CoreGameManager>.Instance)
            {
                return false;
            }
            AudioListener.volume = 0f;
            MonoLogicManager.Instance.TimeUntilUnmute = 0f;
            MonoLogicManager.Instance.Muted = true;
            return true;
        }

        public static bool AlertBaldi(string person, string number)
        {
            if (!Singleton<CoreGameManager>.Instance || !Singleton<BaseGameManager>.Instance)
            {
                return false;
            }



            Singleton<BaseGameManager>.Instance.CurrentEc.MakeNoise(Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position, 126);

            return true;
        }

        public static bool MakeSus(string person, string number)
        {
            if (!Singleton<CoreGameManager>.Instance)
            {
                return false;
            }

            Singleton<CoreGameManager>.Instance.GetPlayer(0).RuleBreak("No Reason",5f);

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

        public static bool SellAll(string person, string number)
        {
            if (!Singleton<CoreGameManager>.Instance)
            {
                return false;
            }
            if (GameObject.FindObjectOfType<StoreScreen>() != null) //could cause problems
            {
                return false;
            }
            ItemManager itemman = Singleton<CoreGameManager>.Instance.GetPlayer(0).itm;
            int TotalSellPrice = 0;
            for (int i=0; i < itemman.items.Length; i++)
            {
                if (itemman.items[i].itemType != Items.None)
                {
                    TotalSellPrice += itemman.items[i].cost;
                }
                if (itemman.items[i].nameKey.Contains(")")) //detect tampered with items(aka chat given ones)
                {
                    TotalSellPrice += 25;
                }
                itemman.RemoveItem(i);
            }
            return AddYTPs(person,TotalSellPrice.ToString());
        }

        public static bool BaldiSaysCongrats(string person, string number)
        {
            if (!Singleton<BaseGameManager>.Instance)
            {
                return false;
            }
            Singleton<BaseGameManager>.Instance.PleaseBaldi(5f);
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

            item = GameObject.Instantiate(item);
            item.nameKey = Singleton<LocalizationManager>.Instance.GetLocalizedText(item.nameKey) + "\n(" + person + ")";

            item.descKey = Singleton<LocalizationManager>.Instance.GetLocalizedText(item.descKey) + "\n(" + person + " from Twitch chat gifted this to you! How nice!)";
            Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.AddItem(item);


            return true;
        }

        //so close but i need to figure out how to detect "fake" fieldtrip entrances and have them give the items directly.
        /*public static bool StartFieldtrip(string person, string tr)
        {
            if ((!Enum.TryParse(tr, true, out FieldTrips tri)) || !Singleton<CoreGameManager>.Instance)
            {
                tri = FieldTrips.Camping;
            }
            FieldTripObject trip = GeneralBaldiStuff.FieldTrips.ToList().Find(x => x.trip == tri);
            if (trip == null)
            {
                trip = GeneralBaldiStuff.FieldTrips.ToList().Find(x => x.trip == FieldTrips.Camping);
                if (trip == null)
                {
                    UnityEngine.Debug.LogError("Something has gone horribly wrong here!");
                    return false;
                }
            }
            
            Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.AddItem(item);


            return true;
        }*/

        public static bool Teleport(string person, string enu)
        {
            ItemObject item = GeneralBaldiStuff.Items.ToList().Find(x => x.itemType == Items.Teleporter);
            if (item == null)
            {
                UnityEngine.Debug.LogError("Something has gone horribly wrong here!");
                return false;
            }
            item = GameObject.Instantiate(item);
            item.item.gameObject.SetActive(true);
            item.item.Use(Singleton<CoreGameManager>.Instance.GetPlayer(0));
            return true;
        }


    }
}
