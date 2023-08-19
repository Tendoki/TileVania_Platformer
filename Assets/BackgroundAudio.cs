using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    [SerializeField] private List<AudioClip> soundtracks;

    private void Start()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(soundtracks[UnityEngine.Random.Range(0, soundtracks.Count)]);
    }
}
