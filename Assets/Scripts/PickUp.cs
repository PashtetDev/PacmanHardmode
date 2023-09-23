using UnityEngine;
public class PickUp: MonoBehaviour
{
    public Type myType;

    public enum Type
    {
        fire,
        gun,
        pipe,
        mashroom,
        gameboy,
        bird,
        coin,
        bullet
    }
}
