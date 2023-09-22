using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private WeaponSample myWeapon;
    [SerializeField]
    private GameObject bullet;

    private void Shot()
    {
        myWeapon.currentReloadTime = myWeapon.reloadTime;
        StartCoroutine(Reload());
        GameObject newBullet = Instantiate(myWeapon.bullet, transform.position, Quaternion.Euler(0, 0, transform.parent.localEulerAngles.z));
        newBullet.GetComponent<Bullet>().Initialize(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
    }

    private void Update()
    {
        if (myWeapon.currentReloadTime == 0)
        {
            if (myWeapon.isAuto)
            {
                if (Input.GetMouseButton(0))
                    Shot();
            }
            else
            if (Input.GetMouseButtonDown(0))
                Shot();
        }
    }

    private IEnumerator Reload()
    {
        while (myWeapon.currentReloadTime > 0)
        {
            myWeapon.currentReloadTime -= Time.deltaTime;
            yield return null;
        }
        myWeapon.currentReloadTime = 0;
    }
}
