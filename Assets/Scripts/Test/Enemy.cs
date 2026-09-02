using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EventsAndDelegates player;

    void OnEnable()
    {
        player.OnHealthChange += UpdateHealth;
        player.OnDamageTaken += ShowDamageTaken;
    }

    void UpdateHealth(int health)
    {
        Debug.Log("Enemy hit the player, player current health - " + health);
    }

    void ShowDamageTaken(int damage)
    {
        Debug.Log("Player took " + damage + " damage.");    
    }

    void OnDisable()
    {
        player.OnHealthChange -= UpdateHealth;
        player.OnDamageTaken -= ShowDamageTaken;
    }
}