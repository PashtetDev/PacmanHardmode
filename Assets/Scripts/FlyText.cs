using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class FlyText : MonoBehaviour
{
    [SerializeField]
    private Text text2, text1;
    [SerializeField]
    private List<Color> randomColor;
    private char[] chars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    public void Initialize()
    {
        text1.color = randomColor[Random.Range(0, randomColor.Count - 1)];
        text2.color = randomColor[Random.Range(0, randomColor.Count - 1)];
        char myChar = chars[Random.Range(0, chars.Length - 1)];
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Text>().text = System.Convert.ToString(myChar);
        }
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        float time = 5;
        while (time > 0)
        {
            yield return null;
            transform.position += new Vector3(0, 250, 0) * Time.deltaTime;
            time -= Time.deltaTime;
        }
        Destroy(gameObject);
    }

}
