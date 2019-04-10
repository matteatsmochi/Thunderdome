using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LowerThird : MonoBehaviour
{
    
    public TextMeshProUGUI broadcasterName;
    public TextMeshProUGUI curatorName;
    public TextMeshProUGUI views;
    public Image broadcasterIcon;  
    public RectTransform MoveMe;

    Bot bot;
    
    void Awake()
    {
        bot = GameObject.Find("Chat Bot").GetComponent<Bot>();
    }


    public void NewLowerThird(string bn, string cn, string v, string bi)
    {
        broadcasterName.text = bn;
        curatorName.text = cn;
        views.text = "[ " + v + " ]";
        LoadImage(bi);
        MoveMe.DOAnchorPos(new Vector2(0,0), 1);
        StartCoroutine(MoveMeBack());

        bot.TwitchMessage("⚡Clip from " + bn + ": https://twitch.tv/" + bn.ToLower());
    }

    IEnumerator MoveMeBack()
    {
        yield return new WaitForSeconds(6);
        MoveMe.DOAnchorPos(new Vector2(-1000,0), 1);
    }

    void LoadImage(string url)
    {
        StartCoroutine(LoadImageASYNC(url));
    }

    IEnumerator LoadImageASYNC(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        broadcasterIcon.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }
}
