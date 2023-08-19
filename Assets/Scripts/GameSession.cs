using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int playerLives = 3;
    [SerializeField] private int score = 0;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI shieldLivesText;
    [SerializeField] private int maxPlayerLives = 10;
    [SerializeField] private int shieldLives = 0;
    [SerializeField] private int maxShieldLives = 6;

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = "Score: " + score.ToString();
        shieldLivesText.text = shieldLives.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    private void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }

    public void TakeShieldLife()
    {
        shieldLives--;
        shieldLivesText.text = shieldLives.ToString();
    }

    public void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Score: " + score.ToString();
    }

    public void AddLife()
    {
        if (playerLives < maxPlayerLives)
        {
            playerLives++;
            livesText.text = playerLives.ToString();
        }
    }

    public void AddShield()
    {
        if (shieldLives < maxShieldLives)
        {
            shieldLives += 2;
            shieldLivesText.text = shieldLives.ToString();
        }
    }

    public int GetPlayerLives()
    {
        return playerLives;
    }

    public int GetShieldLives()
    {
        return shieldLives;
    }

}
