using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Net;
using System.Text;
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
using TMPro;

namespace BBPlusTwitch
{
    public class MonoLogicManager : MonoBehaviour //for anything that needs an update function
    {
        public float TimeUntilUnmute = 0f;
        public bool Muted = false;
        public float FlickerTime;
        public bool DeezNuts; //haha ben i'm SOOO funny :DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD


        public static MonoLogicManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if (Instance != this)
                {
                    GameObject.Destroy(Instance);
                    Instance = this;
                }

            }
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneChange;
        }

        private void OnSceneChange(Scene scene, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Single)
            {
                TimeUntilUnmute = 6f;
                Muted = false;
                AudioListener.volume = 1f;
            }
        }

        void Update()
        {
            bool singletonexist = (bool)Singleton<CoreGameManager>.Instance;
            if (!singletonexist) return;
            if (Muted)
            {
                TimeUntilUnmute += Time.deltaTime;
                AudioListener.volume = 0f;
            }
            if (TimeUntilUnmute > 5f && Muted)
            {
                TimeUntilUnmute = 0f;
                Muted = false;
                AudioListener.volume = 1f;
            }

            if (DeezNuts)
            {
                FlickerTime += Time.deltaTime;
                Singleton<BaseGameManager>.Instance.CurrentEc.FlickerLights(true);
            }
            if (FlickerTime > 5f && DeezNuts)
            {
                FlickerTime = 0f;
                DeezNuts = false;
                Singleton<BaseGameManager>.Instance.CurrentEc.FlickerLights(false);
            }
        }
    }
}
