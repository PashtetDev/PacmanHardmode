using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker instance;

    public void Initialize()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 startPosition = Camera.main.transform.localPosition;
        float myDuration = 0;
        while (myDuration < duration)
        {
            yield return null;
            myDuration += Time.deltaTime;
            Vector2 offset = new Vector2(Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude));
            Camera.main.transform.localPosition = new Vector3(offset.x, offset.y, -10f);
        }
        Camera.main.transform.localPosition = startPosition;
    }
}
