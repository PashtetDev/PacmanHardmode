using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Marshroom : MonoBehaviour
{
    [SerializeField]
    private Image image;
    private void Awake()
    {
        transform.parent = PlayerController.instance.transform;
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        float alpha = 0.2f;
        Color currentColor, newColor;
        image.color = Color.red;
        while (true)
        {
            currentColor = Color.red;
            while (image.color != Color.green)
            {
                newColor = (Color.green - currentColor) * Time.deltaTime;
                image.color = new Color(newColor.r, newColor.g, newColor.b, alpha );
                yield return null;
            }
            currentColor = Color.green;
            while (image.color != Color.blue)
            {
                newColor = (Color.blue - currentColor) * Time.deltaTime;
                image.color = new Color(newColor.r, newColor.g, newColor.b, alpha);
                yield return null;
            }
            currentColor = Color.blue;
            while (image.color != Color.red)
            {
                newColor = (Color.red - currentColor) * Time.deltaTime;
                image.color = new Color(newColor.r, newColor.g, newColor.b, alpha);
                yield return null;
            }

        }
    }
}
