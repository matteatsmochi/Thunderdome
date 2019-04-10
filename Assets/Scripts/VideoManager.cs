using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    VideoPlayer videoPlayer;
    Slugs slugs;
    TwitchAPI tapi;
    Transition trans;
    Bot bot;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        slugs = GameObject.Find("Slugs").GetComponent<Slugs>();
        tapi = GameObject.Find("API Manager").GetComponent<TwitchAPI>();
        trans = GameObject.Find("Lightning Transition").GetComponent<Transition>();
        bot = GameObject.Find("Chat Bot").GetComponent<Bot>();
    }

    void Start()
    {
        videoPlayer.loopPointReached += EndReached;

    }

    public void PlayVideo(string url)
    {
        videoPlayer.url = url;
        
    }

    public void PlayNextVideo()
    {
        PlayVideo(tapi.ClipURL(slugs.NextSlug(bot.TempSpread)));
        bot.TempSpread = 0;
        bot.ClearVotePlus();
        bot.ClearVoteMinus(); //if large numbers of people, reset every hour
    }

    void EndReached(VideoPlayer vp)
    {
        trans.PlayTransition();
    }
}
