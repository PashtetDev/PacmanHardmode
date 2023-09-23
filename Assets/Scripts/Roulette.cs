using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Roulette : MonoBehaviour
{
    [SerializeField]
    private GameObject marshroom;
    [SerializeField]
    private Transform centerPosition;
    [SerializeField]
    private List<GameObject> morePickUps;
    [SerializeField]
    private GameObject pipe, mashroom;
    [SerializeField]
    private GameObject lenta;
    [SerializeField]
    private Transform spawnPosition;
    private int counter;
    private float speed;
    private bool play;
    [SerializeField]
    private GameObject button;
    [SerializeField]
    private List<GameObject> pool;
    private Vector2 lentaStartVector;

    private void Awake()
    {
        lentaStartVector = lenta.transform.position;
        pool = new List<GameObject>();
        button.SetActive(true);
        play = false;
    }

    public void Initialize()
    {
        lenta.transform.position = lentaStartVector;
        button.SetActive(false);
        play = true;
        StartCoroutine(_Start());
    }

    private IEnumerator _Start()
    {
        counter = 0;
        float maxSpeed = Random.Range(30f, 50f);
        while (speed < maxSpeed)
        {
            speed += maxSpeed / 2 * Time.deltaTime;
            yield return null;
        }
        while (speed > 0)
        {
            speed -= maxSpeed / 5 * Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Winner());
    }

    private IEnumerator Winner()
    {
        GameObject winner = pool[0];
        for (int i = 0; i < pool.Count - 1; i++)
        {
            yield return null;
            if (pool[i] != null)
                if (Mathf.Abs(winner.transform.position.x) > Mathf.Abs(pool[i].transform.position.x))
                    winner = pool[i];
        }
        Debug.Log(winner.name);
        float alpha = 1;
        Vector3 startScale = winner.transform.localScale;
        while (alpha < 1.5f)
        {
            yield return null;
            alpha += 0.5f * Time.deltaTime;
            winner.transform.localScale = alpha * startScale;
        }
        switch (winner.GetComponent<PickUp>().myType)
        {
            case PickUp.Type.fire:
                PlayerController.instance.myBoosts.isFire += 1;
                break;
            case PickUp.Type.gun:
                PlayerController.instance.myBoosts.gunBullet += 10;
                break;
            case PickUp.Type.pipe:
                PlayerController.instance.myBoosts.boss = true;
                break;
            case PickUp.Type.mashroom:
                Instantiate(marshroom);
                PlayerController.instance.myBoosts.mashroom = true;
                break;
            case PickUp.Type.bird:
                PlayerController.instance.myBoosts.bird += 1;
                break;
            case PickUp.Type.coin:
                PlayerController.instance.myBoosts.points += 100;
                break;
            case PickUp.Type.bullet:
                PlayerController.instance.myBoosts.enemyBullet += 1;
                break;
            case PickUp.Type.gameboy:
                play = false;
                Initialize();
                break;
        }
        yield return new WaitForSeconds(1f);
        if (!play)
            SceneManager.LoadScene("Game");
    }

    private void Update()
    {
        GameObject newObject = null;
        if (play)
        {
            lenta.transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
            if ((int)(lenta.transform.position.x * 2 / 3) > counter)
            {
                if (Random.Range(0, 10) == 0)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        if (!PlayerController.instance.myBoosts.mashroom)
                        {
                            newObject = Instantiate(mashroom, spawnPosition.position, Quaternion.identity); newObject.transform.parent = lenta.transform;
                            counter++;
                        }
                        else
                        {
                            newObject = Instantiate(morePickUps[Random.Range(0, morePickUps.Count)], spawnPosition.position, Quaternion.identity);
                            counter++;
                        }
                    }
                    else
                    {
                        if (!PlayerController.instance.myBoosts.boss)
                        {
                            newObject = Instantiate(pipe, spawnPosition.position, Quaternion.identity);
                            counter++;
                        }
                        else
                        {
                            newObject = Instantiate(morePickUps[Random.Range(0, morePickUps.Count)], spawnPosition.position, Quaternion.identity);
                            counter++;
                        }
                    }
                }
                else
                {
                    newObject = Instantiate(morePickUps[Random.Range(0, morePickUps.Count - 1)], spawnPosition.position, Quaternion.identity);
                    counter++;
                }
            }
            if (newObject != null)
                newObject.transform.parent = lenta.transform;
            if (!pool.Contains(newObject) && newObject != null)
                pool.Add(newObject);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Initialize();
        }
        if (pool.Count >= 10)
        {
            Destroy(pool[0]);
            pool.RemoveAt(0);
        }
    }
}
