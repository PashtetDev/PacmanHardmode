using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject music;
    [SerializeField]
    private Text counter;

    private void Awake()
    {
        Instantiate(music).GetComponent<Sound>().Initialize();
        if (PlayerPrefs.GetInt("TheBest") != 0)
            counter.text = "The best: " + System.Convert.ToString(PlayerPrefs.GetInt("TheBest"));
        else
            counter.text = "";
        if (PlayerController.instance != null)
            Destroy(PlayerController.instance.gameObject);
        PlayerController.instance = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }
}
