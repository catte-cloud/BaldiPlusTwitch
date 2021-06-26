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

        //Mute
        public float TimeUntilUnmute = 0f;
        public bool Muted = false;

        //CursorTroll
        public float TimeUntilLock = 0f;
        public bool Locked = false;

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

            if (Locked)
            {
                TimeUntilLock += Time.deltaTime;
                Singleton<CursorManager>.Instance.cursorLocked = false;
                Singleton<CursorManager>.Instance.UnlockCursor();
            }
            if (TimeUntilLock > 10f && Locked)
            {
                TimeUntilUnmute = 0f;
                Muted = false;
                AudioListener.volume = 1f;
                Singleton<CursorManager>.Instance.cursorLocked = true;
                Singleton<CursorManager>.Instance.LockCursor();
            }

        }
    }
}
