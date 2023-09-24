using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem particle;

    public void Initialize()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Play();
        StartCoroutine(Wait());
    }

    private void Update()
    {
        if (CompareTag("BigPoint"))
        {
            if (PlayerController.instance.isLose)
            {
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator Wait()
    {
        while (particle.isPlaying)
            yield return null;
        Destroy(gameObject);
    }
}
