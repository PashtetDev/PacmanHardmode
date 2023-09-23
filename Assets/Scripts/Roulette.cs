using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette : MonoBehaviour
{
    private Transform centerPosition;
    private List<GameObject> morePickUps;
    private GameObject pipe, mashroom;
    private List<GameObject> pool;
    private GameObject lenta;
    private Transform spawnPosition;

    private void Initialize()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < morePickUps.Count * 3; i++)
        {
            pool.Add(Instantiate(morePickUps[i % morePickUps.Count], Vector2.zero, Quaternion.identity));
        }
    }

    private IEnumerator _Start()
    {
        float speed = 0;
        while (speed < 100)
        {
            speed += 100 * Time.deltaTime;
            yield return null;
        }
        while (speed > 0)
        {
            speed -= 20 * Time.deltaTime;
            yield return null;
        }

    }
}
