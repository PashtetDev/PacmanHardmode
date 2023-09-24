using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float radius;
    private Vector3 position;

    private void Update()
    {
        if (!PlayerController.instance.isLose && !PlayerController.instance.win)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position = (mousePosition - PlayerController.instance.transform.position) / 2;
            position.z = -10;
            transform.localPosition = -(transform.localPosition - position).normalized * 5;
        }
    }

}
