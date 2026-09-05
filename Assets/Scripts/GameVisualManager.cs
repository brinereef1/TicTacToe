using Unity.Netcode;
using UnityEngine;

public class GameVisualManager : NetworkBehaviour
{   
    private static readonly Vector2[,] GRID_POSITIONS =
    {
        { new Vector2(-1.5f, -1.8f), new Vector2(0f, -1.8f), new Vector2(1.5f, -1.8f) },
        { new Vector2(-1.5f, -0.15f), new Vector2(0f, -0.15f), new Vector2(1.5f, -0.15f) },
        { new Vector2(-1.5f, 1.5f), new Vector2(0f, 1.5f), new Vector2(1.5f, 1.5f) },
    };
    [SerializeField] private Transform crossPrefab;
    [SerializeField] private Transform circlePrefab;

    void Start()
    {
        GameManager.Instance.onClickedOnGridPosition += GameManager_OnClickedGridPosition;
    }

    // Requests that the server spawn the local player's symbol at the clicked square.
    private void GameManager_OnClickedGridPosition(object sender, GameManager.OnClickedOnPositionEventArgs e)
    {
        Debug.Log("GameManager_OnClickedGridPosition");
        SpawnObjectRpc(e.x, e.y, e.playerType);
    }

    // Selects and network-spawns the correct symbol at the requested grid position.
    [Rpc(SendTo.Server)]
    private void SpawnObjectRpc(int x, int y, GameManager.PlayerType playerType)
    {
        Debug.Log("Spawned Object");
        Transform prefab;

        switch(playerType)
        {   
            default:
            case GameManager.PlayerType.Circle:
                prefab = circlePrefab;
                break;     
            case GameManager.PlayerType.Cross:
                prefab = crossPrefab;
                break;   
        }

        Transform spawnedObject =  Instantiate(prefab, GetGridWorldPosition(x, y), Quaternion.identity);
        spawnedObject.GetComponent<NetworkObject>().Spawn(true);
    }

    // Converts grid coordinates into the matching world position.
    private Vector2 GetGridWorldPosition(int x, int y)
    {
        return GRID_POSITIONS[y, x];
    }
}
