using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObject/Weapon")]
public class WeaponSample : ScriptableObject
{
    public Sprite sprite;
    public float reloadTime;
    [HideInInspector]
    public float currentReloadTime;
    public bool isAuto;
    public GameObject bullet;

}
