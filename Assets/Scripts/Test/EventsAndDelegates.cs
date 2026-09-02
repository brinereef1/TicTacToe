using UnityEngine;
using System;

public class EventsAndDelegates : MonoBehaviour
{
    public event Action<int> OnHealthChange;
    public event Action<int> OnDamageTaken;
    
    public GameObject enemy;

    int currentHealth = 100;
    int damage = 10;

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
    }
}
