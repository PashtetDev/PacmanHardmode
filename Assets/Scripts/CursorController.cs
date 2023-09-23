using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorController : MonoBehaviour
{
    public static CursorController instance;

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "Upgrade")
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
        else
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
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
