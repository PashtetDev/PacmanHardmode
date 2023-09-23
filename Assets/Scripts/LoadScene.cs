using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public static LoadScene instance;
    private bool upgrade;
    private bool boss;

    private void Awake()
    {
        instance = this;
    }

    public void LoadUpgrade()
    {
        upgrade = true;
    }

    public void LoadBoss()
    {
        boss = true;
    }

    public void Load()
    {
        if (upgrade || boss)
        {
            if (upgrade)
            {
                SceneManager.LoadScene("Upgrade");
            }
            if (boss)
            {
                SceneManager.LoadScene("Boss");
            }
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }
}
