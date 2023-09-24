using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private GameObject win;
    [SerializeField]
    private GameObject winSound;
    [SerializeField]
    private TextGenerator generator;
    [SerializeField]
    private GameObject sprite;
    [SerializeField]
    private GameObject particle1, particle2;
    [SerializeField]
    private GameObject eay1, eay2;
    [SerializeField]
    private GameObject gun;
    [SerializeField]
    private GameObject[] bullet;
    private bool isLose;
    private int count;
    private int countLife;

    private void Awake()
    {
        win.SetActive(false);
        countLife = 2;
        StartCoroutine(Attack());
        StartCoroutine(SpawnGun());
    }

    private IEnumerator Attack()
    {
        while (PlayerController.instance == null)
            yield return null;
        while (!PlayerController.instance.isLose && !isLose)
        {
            Instantiate(bullet[Random.Range(0, bullet.Length - 1)], transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-2f, 2f), 0), Quaternion.identity);
            count++;
            yield return new WaitForSeconds(1 / Mathf.Log(count + 1));
        }
    }

    private IEnumerator SpawnGun()
    {
        yield return new WaitForSeconds(Random.Range(5f, 7f));
        while (PlayerController.instance == null)
            yield return null;
        while (!PlayerController.instance.isLose && !isLose)
        {
            Instantiate(gun, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-2f, 2f), 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(7f, 10f));
        }
    }

    private void Death()
    {
        if (countLife > 0)
            countLife--;
        else
        {
            if (!isLose)
            {
                PlayerController.instance.win = true;
                Instantiate(winSound).GetComponent<Sound>().Initialize();
                win.SetActive(true);
                generator.Initialize();
                isLose = true;
                Destroy(sprite);
                Instantiate(particle1, transform.position, Quaternion.identity).GetComponent<ParticleController>().Initialize();
                Instantiate(particle2, transform.position, Quaternion.identity).GetComponent<ParticleController>().Initialize();
                Destroy(GetComponent<Collider2D>());
                PlayerController.instance.AddPoints();
                PlayerController.instance.AddPoints();
                PlayerController.instance.AddPoints();
                PlayerController.instance.AddPoints();
            }
        }
        switch (countLife)
        {
            case 1:
                eay1.SetActive(true);
                eay2.SetActive(false);
                break;
            case 0:
                eay1.SetActive(true);
                eay2.SetActive(true);
                break;
            default: break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            Death();
        }
    }
}
