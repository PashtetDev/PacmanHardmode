using UnityEngine;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private GameObject sound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Instantiate(sound).GetComponent<Sound>().Initialize();
    }
}
