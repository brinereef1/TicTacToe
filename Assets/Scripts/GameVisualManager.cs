using Unity.Mathematics;
using UnityEngine;

public class GameVisualManager : MonoBehaviour
{   
    private const float GRID_SIZE = 3.1f;
    [SerializeField] private Transform crossPrefab;
    [SerializeField] private Transform circlePrefab;

    void Start()
    {
        GameManager.Instance.onClickedOnGridPosition += GameManager_OnClickedGridPosition;
    }

    // Creates a circle at the world position of the clicked grid square.
    private void GameManager_OnClickedGridPosition(object sender, GameManager.OnClickedOnPositionEventArgs e)
    {
        Instantiate(circlePrefab, GetGridWorldPosition(e.x, e.y), Quaternion.identity);
    }

    // Converts grid coordinates into the matching world position.
    private Vector2 GetGridWorldPosition(int x, int y)
    {
        return new Vector2(-GRID_SIZE + x * GRID_SIZE, -GRID_SIZE + y * GRID_SIZE);
    }

    void OnDisable()
    {
        GameManager.Instance.onClickedOnGridPosition -= GameManager_OnClickedGridPosition;
    }
}
