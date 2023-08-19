using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f;
    private AudioClip killAudioClip;
    private AudioClip fireAudioClip;
    private Rigidbody2D myRigidbody;
    private PlayerMovement player;
    private float xSpeed;
    private GameObject audioHolder;
    private AudioSource audioSource;

    void Start()
    {
        audioHolder = GameObject.Find("AudioHolder");
        audioSource = audioHolder.GetComponent<AudioSource>();

        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = Mathf.Sign(player.transform.localScale.x) * bulletSpeed;
        transform.localScale = new Vector2((Mathf.Sign(xSpeed)) * transform.localScale.x, transform.localScale.y);
        killAudioClip = player.GetKillAudioClip();
        fireAudioClip = player.GetFireAudioClip();

        audioSource.PlayOneShot(fireAudioClip, 0.7F);
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2(xSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            audioSource.PlayOneShot(killAudioClip, 0.7F);
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
