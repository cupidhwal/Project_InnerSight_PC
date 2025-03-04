using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D customCursor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
        
    }

}
