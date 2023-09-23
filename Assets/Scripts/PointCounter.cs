using UnityEngine;
using UnityEngine.UI;

public class PointCounter : MonoBehaviour
{
    public static PointCounter instance;
    private Text counter;

    public void Initialize()
    {
        instance = this;
        counter = GetComponent<Text>();
    }

    public void UpdateText(int points)
    {
        counter.text = System.Convert.ToString(points);
    }
}
