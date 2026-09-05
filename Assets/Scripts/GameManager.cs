using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Notifies other scripts when a board position is clicked.
    public event EventHandler<OnClickedOnPositionEventArgs> onClickedOnGridPosition;

    // Stores the coordinates of the clicked board position.
    public class OnClickedOnPositionEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("More than one GameManager instance detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Sends the clicked coordinates to every script listening for the event.
    public void ClickedOnGridPosition(int x, int y)
    {
        Debug.Log("ClickedOnGridPosition: " + x + " " + y);
        onClickedOnGridPosition?.Invoke(this, new OnClickedOnPositionEventArgs
        {
            x = x,
            y = y
        });
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
