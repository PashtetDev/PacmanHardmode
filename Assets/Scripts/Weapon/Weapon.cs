using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private GameObject shot;
    [SerializeField]
    private Transform dulo;
    [SerializeField]
    private WeaponSample myWeapon;
    [SerializeField]
    private GameObject bullet;
    private Vector3 startPosition;


    private void OnEnable()
    {
        transform.localPosition = startPosition;
        StartCoroutine(Reload());
    }

    private void Awake()
    {
        startPosition = transform.localPosition;
        myWeapon.currentReloadTime = 0;
    }

    private void Shot()
    {
        Instantiate(shot).GetComponent<Sound>().Initialize();
        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float rotateAngle = transform.parent.localEulerAngles.z;
        myWeapon.currentReloadTime = myWeapon.reloadTime;
        StartCoroutine(Reload());
        StartCoroutine(CameraShaker.instance.Shake(0.1f, 0.5f));
        StartCoroutine(Output(1f));
        GameObject newBullet = Instantiate(myWeapon.bullet, dulo.position, Quaternion.Euler(0, 0, rotateAngle));
        newBullet.GetComponent<Bullet>().Initialize(direction - dulo.position);
        
        PlayerController.instance.myBoosts.gunBullet--;
        PlayerController.instance.BulletCheck();
    }

    public void HideWeapon()
    {
        StopAllCoroutines();
    }

    private IEnumerator Output(float magnitude)
    {
        Vector3 startRotation = transform.localEulerAngles;
        transform.localPosition = startPosition + new Vector3(Random.Range(-magnitude, 0), 0, 0);
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
            transform.localEulerAngles = new Vector3(0, 0, Random.Range(20f, 40f));
        else
            transform.localEulerAngles = new Vector3(0, 0, Random.Range(-20f,-40f));
        yield return new WaitForSeconds(myWeapon.reloadTime / 2);
        transform.localEulerAngles = startRotation;
        transform.localPosition = startPosition;
    }

    private void Update()
    {
        if (myWeapon.currentReloadTime == 0 && SceneManager.GetActiveScene().name != "Upgrade")
        {
            if (myWeapon.isAuto)
            {
                if (Input.GetMouseButton(0) && PlayerController.instance.myBoosts.gunBullet > 0 && !PlayerController.instance.isLose)
                    Shot();
            }
            else
            if (Input.GetMouseButtonDown(0) && PlayerController.instance.myBoosts.gunBullet > 0 && !PlayerController.instance.isLose)
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
