using UnityEngine;
using UnityEngine.InputSystem;

public class GridPosition : MonoBehaviour
{
    [SerializeField] private int x;
    [SerializeField] private int y;

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Collider2D hit = Physics2D.OverlapPoint(worldPos);

            if (hit != null)
            {
                Debug.Log(hit.name); 
                GameManager.Instance.ClickedOnGridPosition(x, y);
            }
                
        }
    }
}