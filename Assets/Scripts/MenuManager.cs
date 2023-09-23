using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerController.instance != null)
            Destroy(PlayerController.instance.gameObject);
        PlayerController.instance = null;
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }
}
