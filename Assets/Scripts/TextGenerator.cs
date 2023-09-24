using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TextGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private GameObject text;

    public void Initialize()
    {
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            GameObject newText = Instantiate(text, Camera.main.WorldToScreenPoint(Camera.main.transform.position + new Vector3(Random.Range(-13, 13), -6, 0)), Quaternion.identity);
            newText.GetComponent<FlyText>().Initialize();
            newText.transform.parent = canvas.transform;
        }
    }
}
