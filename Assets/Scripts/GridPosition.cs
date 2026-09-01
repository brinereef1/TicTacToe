using UnityEngine;
using UnityEngine.InputSystem;

public class GridPosition : MonoBehaviour
{
    [SerializeField] private int x;
    [SerializeField] private int y;

    void OnMouseDown()
    {
        Debug.Log("Clicked " + x + " " + y);
        GameManager.Instance.ClickedOnGridPosition(x, y);
    }
}