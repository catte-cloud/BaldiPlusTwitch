using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net.Sockets;
using System.IO;
using BBPlusTwitch;

public class TwitchConnectionHandler : MonoBehaviour
{

    public static TwitchConnectionHandler Instance;

    TcpClient Twitch;
    StreamReader Reader;
    StreamWriter Writer;

    const string URL = "irc.chat.twitch.tv";
    const int PORT = 6667;
    const float PingInterval = 60f;
    const string Prefix = "!";

    public float PingCounter;




    public void ConnectToTwitch()
    {
        Debug.Log("Connecting...");
        Twitch = new TcpClient(URL, PORT);
        Reader = new StreamReader(Twitch.GetStream());
        Writer = new StreamWriter(Twitch.GetStream());

        Writer.WriteLine("PASS " + BaldiTwitch.Oath.Value);
        Writer.WriteLine("NICK " + BaldiTwitch.Name.Value);
        Writer.WriteLine("JOIN #" + BaldiTwitch.UserName.Value.ToLower());
        Writer.Flush();
    }

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
        ConnectToTwitch();
    }

    void Update()
    {
        PingCounter += Time.unscaledDeltaTime;

        if (PingCounter > PingInterval)
        {
            Writer.WriteLine("PING " + URL);
            Writer.Flush();
            PingCounter = 0f;
        }

        if (!Twitch.Connected)
        {
            ConnectToTwitch();
        }
        if (Twitch.Available > 0)
        {
            string message = Reader.ReadLine();

            if (message.Contains("PRIVMSG"))
            {
                int splitPoint = message.IndexOf("!");
                string chatter = message.Substring(1, splitPoint - 1);

                splitPoint = message.IndexOf(":",1);
                string msg = message.Substring(splitPoint + 1);

                if (msg.StartsWith(Prefix))
                {
                    //this is a command, do shit
                    Debug.Log("Command!");
                    int indexof = msg.IndexOf(" ");
                    string cmd = indexof == -1 ? msg : msg.Substring(0, indexof);
                    int cutoff = cmd.Length + 1;
                    cmd = cmd.Substring(Prefix.Length);
                    Debug.Log("cmd:" + cmd);
                    string param = "";
                    if (cutoff < msg.Length)
                    {
                        param = msg.Substring(cutoff);
                    }
                    Debug.Log("parm:" + param);
                    TwitchCommand com;
                    if (TwitchManager.Commands.TryGetValue(cmd, out com))
                    {
                        com.functocall(chatter,param);
                    }


                }
            }

            Debug.Log(message);
        }
    }
}

