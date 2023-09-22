using System.Collections;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;
        StartCoroutine(VisibleChecker());
    }

    private void FixedUpdate()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 1) * 10;
    }

    private IEnumerator VisibleChecker()
    {
        while(true)
        {
            if (Cursor.visible)
                Cursor.visible = false;
            yield return new WaitForSeconds(1);
        }
    }
}
