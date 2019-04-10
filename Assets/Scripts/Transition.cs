using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    VideoManager videoManager;
    Animator anim;
    AudioSource uiAudio;
    

    void Awake()
    {
        videoManager = GameObject.Find("Video Player").GetComponent<VideoManager>();
        anim = GameObject.Find("Lightning Transition").GetComponent<Animator>();
        uiAudio = GameObject.Find("Audio Player").GetComponent<AudioSource>();

        PlayTransition();
    }

    public void PlayTransition()
    {
        anim.Play("Lightning Transition", 0, 0);
    }

    public void EndOfTransition()
    {
        videoManager.PlayNextVideo();
    }

    public void TransitionSound()
    {
        uiAudio.Play();
    }
}
