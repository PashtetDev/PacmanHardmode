using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private WeaponSample myWeapon;
    [SerializeField]
    private GameObject bullet;

    private IEnumerator Shot()
    {
        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float rotateAngle = transform.parent.localEulerAngles.z;
        myWeapon.currentReloadTime = myWeapon.reloadTime;
        StartCoroutine(Reload());
        for (int i = 0; i < myWeapon.count; i++)
        {
            GameObject newBullet = Instantiate(myWeapon.bullet, transform.position, Quaternion.Euler(0, 0, rotateAngle));
            newBullet.GetComponent<Bullet>().Initialize(direction - transform.position);
            yield return new WaitForSeconds(myWeapon.waitTime);
        }
    }

    private void Update()
    {
        if (myWeapon.currentReloadTime == 0)
        {
            if (myWeapon.isAuto)
            {
                if (Input.GetMouseButton(0))
                    StartCoroutine(Shot());
            }
            else
            if (Input.GetMouseButtonDown(0))
                StartCoroutine(Shot());
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
