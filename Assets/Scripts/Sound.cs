using System.Collections;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource source;

    public void Initialize()
    {
        source = GetComponent<AudioSource>();
        source.Play();
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
        while (source.isPlaying)
            yield return null;
        Destroy(gameObject);
    }
}
