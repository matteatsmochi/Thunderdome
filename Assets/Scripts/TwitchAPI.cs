using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using UnityEngine;
using SimpleJSON;

public class TwitchAPI : MonoBehaviour
{
    public string TwitchUsername;
    public string TwitchOAuth;
    List<string> DownloadedSlugs;

    public LowerThird l3;
    public Slugs slugs;


    string slugName;
    string broadcasterName;
    string broadcasterURL;
    string broadcasterIcon;
    string curatorName;
    string gameName;
    int views;
    string thumbnailURL;

    
    public string ClipURL(string slug)
    {
        string ClipHTML = getHTML("https://api.twitch.tv/kraken/clips/" + slug + "?api_version=5&oauth_token=" + TwitchOAuth);

        JSONNode n = JSON.Parse(ClipHTML);
        
        slugName = n["slug"].Value;
        broadcasterName = n["broadcaster"]["display_name"].Value;
        broadcasterURL = n["broadcaster"]["channel_url"].Value;
        broadcasterIcon = n["broadcaster"]["logo"].Value;
        curatorName = n["curator"]["display_name"].Value;
        gameName = n["game"].Value;
        views = n["views"].AsInt;
        thumbnailURL = n["thumbnails"]["medium"].Value;
        
        l3.NewLowerThird(broadcasterName, curatorName, views.ToString(), broadcasterIcon);
        
        int i = thumbnailURL.IndexOf("-preview");
        return thumbnailURL.Substring(0, i) + ".mp4";

    }

    public bool RealSlug(string slug)
    {
        string ClipHTML = getHTML("https://api.twitch.tv/kraken/clips/" + slug + "?api_version=5&oauth_token=" + TwitchOAuth);
        
        if (ClipHTML.Contains("Clip does not exist"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void GetNewSlugs()
    {
        string SlugsHTML = getHTML("https://api.twitch.tv/kraken/clips/top?game=Apex%20Legends&period=day&language=en&api_version=5&oauth_token=" + TwitchOAuth);

        JSONNode n = JSON.Parse(SlugsHTML);

        slugs.AddSlug(n["clips"][0]["slug"].Value, "Default");
        slugs.AddSlug(n["clips"][1]["slug"].Value, "Default");
        slugs.AddSlug(n["clips"][2]["slug"].Value, "Default");
        slugs.AddSlug(n["clips"][3]["slug"].Value, "Default");
        slugs.AddSlug(n["clips"][4]["slug"].Value, "Default");
        slugs.AddSlug(n["clips"][5]["slug"].Value, "Default");
        slugs.AddSlug(n["clips"][6]["slug"].Value, "Default");
        slugs.AddSlug(n["clips"][7]["slug"].Value, "Default");
        slugs.AddSlug(n["clips"][8]["slug"].Value, "Default");
        slugs.AddSlug(n["clips"][9]["slug"].Value, "Default");
    }
    

    public Tuple<string, int> GetSlugInfo(string slug)
    {
        string ClipHTML = getHTML("https://api.twitch.tv/kraken/clips/" + slug + "?api_version=5&oauth_token=" + TwitchOAuth);

        JSONNode n = JSON.Parse(ClipHTML);
        
        return Tuple.Create(n["vod"]["id"].Value, n["vod"]["offset"].AsInt);
    }
    
    public bool IsStreaming()
    {
        return true;
    }


    private string getHTML(string address)
    {
        using (WebClient client = new WebClient())
        {
            string htmlCode = client.DownloadString(address);
            return htmlCode;
        }
    }
}
