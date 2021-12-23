using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dolphinSound : MonoBehaviour
{
    public Animator transition;
    private AudioLibrary audioController;

    private void Start()
    {
        audioController = GameObject.Find("Audio").GetComponent<AudioLibrary>();
    }

    public void startDiveAudio()
    {
        audioController.playAmbientSound("Dolphin");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transition.SetTrigger("Dive");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transition.SetTrigger("Idle");
        //audioController.stopAmbientSound();
    }
}

