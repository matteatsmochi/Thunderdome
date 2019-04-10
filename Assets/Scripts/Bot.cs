using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Unity;
using TwitchLib.Client.Models;

public class Bot : MonoBehaviour
{
    //Twitch
    Client client;
    public TwitchAPI tapi;
    Slugs slugs;
    Popups popups;

    public int TempSpread;
    List<string> VotePlus = new List<string>();
    List<string> VoteMinus = new List<string>();
    
    
    void Awake()
    {
        slugs = GameObject.Find("Slugs").GetComponent<Slugs>();
        popups = GameObject.Find("Popups").GetComponent<Popups>();
    }
    
    void Start()
    {
        Application.runInBackground = true;
        client = new Client();
        ConnectToTwitchChat();
        StartCoroutine(HelpStart());
    }

    IEnumerator HelpStart()
    {
        yield return new WaitForSeconds(5);
        Help();
    }


    #region "Twitch"


    void ConnectToTwitchChat()
    {
        ConnectionCredentials credentials = new ConnectionCredentials(tapi.TwitchUsername, tapi.TwitchOAuth);
        client.Initialize(credentials, tapi.TwitchUsername);

       
        client.OnConnected += onConnected;
        client.OnMessageReceived += globalChatMessageRecieved;
        client.OnDisconnected += onDisconnected;

        if (!client.IsConnected)
        {
            client.Connect();
        }

        Debug.Log("<< Connecting >>");
    }

    void DisconnectFromTwitchChat()
    {
        client.LeaveChannel(client.JoinedChannels[0]);
        if (client.IsConnected)
        {
            client.Disconnect();
        }
        Debug.Log("<< Disconnecting . . . >>");
    }

    void onConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
    {

        Debug.Log("<< Connected >>");
        
    }

    public void onDisconnected(object sender, TwitchLib.Client.Events.OnDisconnectedArgs e)

    {
        Debug.Log("<< Disconnected from chat server >>");
        
    }
    

        void globalChatMessageRecieved(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
    {
        Debug.Log(e.ChatMessage.Username + ": " + e.ChatMessage.Message);

        if (!e.ChatMessage.IsBroadcaster)//only perform checks if not the bot
        {
            string MESSAGE = e.ChatMessage.Message;

            if (MESSAGE.Contains("!submit"))
            {
                MESSAGE = MESSAGE.Replace("!submit", "");
                MESSAGE = MESSAGE.Replace(" ", "");
                slugs.AddSlug(MESSAGE, e.ChatMessage.Username);
            }
            else if (MESSAGE.Contains("+1"))
            {
                TryVotePlus(e.ChatMessage.Username);
            }
            else if (MESSAGE.Contains("-1"))
            {
                TryVoteMinus(e.ChatMessage.Username);
            }
        }
        
        
            
            
        
    }

    public void TwitchMessage(string msg)
    {
        client.SendMessage(client.JoinedChannels[0], msg);
    }

    void TryVotePlus(string user)
    {
        bool addme = true;
        for (int i = 0; i < VotePlus.Count; i ++)
        {
            if (VotePlus[i] == user)
            {
                addme = false;
            }
        }

        if (addme)
        {
            VotePlus.Add(user);
            TempSpread ++;
            popups.SpawnPlusOne();
        }
    }

    void TryVoteMinus(string user)
    {
        bool addme = true;
        for (int i = 0; i < VoteMinus.Count; i ++)
        {
            if (VoteMinus[i] == user)
            {
                addme = false;
            }
        }

        if (addme)
        {
            VoteMinus.Add(user);
            TempSpread --;
            popups.SpawnMinusOne();
        }
    }

    public void ClearVotePlus()
    {
        if (VotePlus.Count > 0)
        {
            VotePlus.Clear();
        }
    }

    public void ClearVoteMinus()
    {
        if (VoteMinus.Count > 0)
        {
            VoteMinus.Clear();
        }
        
    }

    void Help()
    {
        StartCoroutine(HelpDelay());
    }

    IEnumerator HelpDelay()
    {
        TwitchMessage("HOW TO: 📝Type +1 to like a clip. 📝Type -1 to dislike a clip. 📝Type !submit [Clip Slug] to add a clip to the queue. Slugs are the random words at the end of the clip's URL, like SaltBaeRealAdorableLlama");
        yield return new WaitForSeconds(180);
        Help();
    }

    #endregion
}
