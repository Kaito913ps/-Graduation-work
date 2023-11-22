using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private bool isDragged = false;

    void Update()
    {
        if (isDragged)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
        }
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        isDragged = true;
    }

    void OnMouseUp()
    {
        isDragged = false;
    }
}
