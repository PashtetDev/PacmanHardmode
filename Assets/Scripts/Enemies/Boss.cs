using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private GameObject eay1, eay2;
    [SerializeField]
    private GameObject gun;
    [SerializeField]
    private GameObject[] bullet;
    private bool isLose;
    private int count;

    private void Awake()
    {
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
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        while (PlayerController.instance == null)
            yield return null;
        while (!PlayerController.instance.isLose && !isLose)
        {
            Instantiate(gun, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-2f, 2f), 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }

    private void Death()
    {
        Destroy(GetComponent<Collider2D>());
        isLose = true;
        PlayerController.instance.AddPoints();
        PlayerController.instance.AddPoints();
        PlayerController.instance.AddPoints();
        PlayerController.instance.AddPoints();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            Death();
        }
    }
}
