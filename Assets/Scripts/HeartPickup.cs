using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] private AudioClip heartPickupSFX;

    private bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            if (other.gameObject.GetComponent<PlayerMovement>().GetIsMovable())
            {
                wasCollected = true;
                FindObjectOfType<GameSession>().AddLife();
                GameObject audioHolder = GameObject.Find("AudioHolder");
                AudioSource audioSource = audioHolder.GetComponent<AudioSource>();
                audioSource.PlayOneShot(heartPickupSFX, 0.7F);
                Destroy(gameObject);
            }
        }
    }
}
