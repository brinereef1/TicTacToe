using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EventsAndDelegates player;

    void OnEnable()
    {
        player.OnHealthChange += UpdateHealth;
        player.OnDamageTaken += ShowDamageTaken;
        player.onScoreChange += UpdateScore;
    }

    void UpdateHealth(int health)
    {
        Debug.Log("Enemy hit the player, player current health - " + health);
    }

    void ShowDamageTaken(int damage)
    {
        Debug.Log("Player took " + damage + " damage.");    
    }

    void UpdateScore(int amount)
    {
        Debug.Log("Added " + amount + " to the player score");
    }

    void OnDisable()
    {
        player.OnHealthChange -= UpdateHealth;
        player.OnDamageTaken -= ShowDamageTaken;
        player.onScoreChange -= UpdateScore;
    }
}