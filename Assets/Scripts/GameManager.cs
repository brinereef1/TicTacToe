using System;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    // Notifies other scripts when a board position is clicked.
    public event EventHandler<OnClickedOnPositionEventArgs> onClickedOnGridPosition;

    // Stores the coordinates and player symbol for the clicked board position.
    public class OnClickedOnPositionEventArgs : EventArgs
    {
        public int x;
        public int y;
        public PlayerType playerType;
    }

    public enum PlayerType
    {
        None,
        Circle,
        Cross,
    }

    private PlayerType localPlayerType;
    private PlayerType currentPlayablePlayerType;

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

    public override void OnNetworkSpawn()
    {
        Debug.Log("Network id : " + NetworkManager.Singleton.LocalClientId);
        if (NetworkManager.Singleton.LocalClientId == 0)
        {
            localPlayerType = PlayerType.Cross;
        } else
        {
            localPlayerType = PlayerType.Circle;
        }

        if (IsServer)
        {
            currentPlayablePlayerType = PlayerType.Cross;
        }
    }

    // Sends the clicked coordinates and local player's symbol to every listener.
    [Rpc(SendTo.Server)]
    public void ClickedOnGridPositionRpc(int x, int y, PlayerType playerType)
    {
        Debug.Log("ClickedOnGridPosition: " + x + " " + y);

        if (playerType != currentPlayablePlayerType)
        {
            return;
        }
        
        onClickedOnGridPosition?.Invoke(this, new OnClickedOnPositionEventArgs
        {
            x = x,
            y = y,
            playerType = playerType,
        });

        switch(currentPlayablePlayerType)
        {
            default:
            case PlayerType.Cross:
                currentPlayablePlayerType = PlayerType.Circle;
                break;
            case PlayerType.Circle:
                currentPlayablePlayerType = PlayerType.Cross;
                break;
        }
    }

    // Returns the symbol assigned to the local network player.
    public PlayerType GetPlayerType()
    {
        return localPlayerType;
    }
}
