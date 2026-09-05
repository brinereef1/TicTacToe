using UnityEngine;
using System;

public class EventsAndDelegates : MonoBehaviour
{
    public event Action<int> OnHealthChange;
    public event Action<int> OnDamageTaken;
    public delegate void ScoreHandler(int score);
    public event ScoreHandler onScoreChange;

    public GameObject enemy;

    int currentHealth = 100;
    int damage = 10;
    int playerScore = 200;

    void Start()
    {
        Debug.Log("Player current health - " + currentHealth);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");
            TakeDamage(damage);
        }
    }

    void TakeDamage(int amount)
    {        
        currentHealth -= amount;
        OnHealthChange?.Invoke(currentHealth);
        OnDamageTaken?.Invoke(amount);

        playerScore += 10;
        onScoreChange?.Invoke(10);
    }
}
