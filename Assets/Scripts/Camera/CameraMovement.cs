using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float radius;
    private Vector3 position;

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position = (mousePosition - PlayerController.instance.transform.position) / 2;
        position.z = -10;
        float distance = Vector2.Distance(mousePosition, transform.position);
        transform.localPosition = -(transform.localPosition - position).normalized * 5;
    }
}