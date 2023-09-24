using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabletControler : MonoBehaviour
{
    private Image image;
    [SerializeField]
    private List<Sprite> fill;

    public void Inititialize()
    {
        image = GetComponent<Image>();
    }

    public void Update_()
    {
        int count = PlayerController.instance.myBoosts.powerUp;
        if (count < 25 && count >= 0)
            image.sprite = fill[0];
        if (count < 50 && count >= 25)
            image.sprite = fill[1];
        if (count < 75 && count >= 50)
            image.sprite = fill[2];
        if (count < 90 && count >= 75)
            image.sprite = fill[3];
        if (count < 100 && count >= 90)
            image.sprite = fill[4];
    }

}
