using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private AudioClip coinPickupSFX;
    [SerializeField] private int pointsForCoinPickup = 100;

    private bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            if (other.gameObject.GetComponent<PlayerMovement>().GetIsMovable())
            {
                wasCollected = true;
                FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);
                GameObject audioHolder = GameObject.Find("AudioHolder");
                AudioSource audioSource = audioHolder.GetComponent<AudioSource>();
                audioSource.PlayOneShot(coinPickupSFX, 0.7F);
                Destroy(gameObject);
            }
        }
    }
}
