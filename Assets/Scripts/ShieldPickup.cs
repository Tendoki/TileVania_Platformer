using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
    [SerializeField] private AudioClip shieldPickupSFX;

    private bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            if (other.gameObject.GetComponent<PlayerMovement>().GetIsMovable())
            {
                wasCollected = true;
                FindObjectOfType<GameSession>().AddShield();
                GameObject audioHolder = GameObject.Find("AudioHolder");
                AudioSource audioSource = audioHolder.GetComponent<AudioSource>();
                audioSource.PlayOneShot(shieldPickupSFX, 0.7F);
                Destroy(gameObject);
            }
        }
    }
}
