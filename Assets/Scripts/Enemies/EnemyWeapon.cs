using System.Collections;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject dulo, bullet;
    public float currentReloadTime, reloadTime;
    public bool active;

    public void Shot()
    {
        if (currentReloadTime == 0)
        {
            active = true;
            Vector3 direction = PlayerController.instance.transform.position;
            float rotateAngle = transform.parent.localEulerAngles.z;
            currentReloadTime = reloadTime;
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(Reload());
                StartCoroutine(Output(1f));
            }
            GameObject newBullet = Instantiate(bullet, dulo.transform.position, Quaternion.Euler(0, 0, rotateAngle));
            newBullet.GetComponent<Bullet>().Initialize(direction - dulo.transform.position);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Reload());
    }

    public void Hide()
    {
        StopAllCoroutines();
    }

    private IEnumerator Output(float magnitude)
    {
        Vector3 startPosition = transform.localPosition;
        Vector3 startRotation = transform.localEulerAngles;
        transform.localPosition = startPosition + new Vector3(Random.Range(-magnitude, 0), 0, 0);
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
            transform.localEulerAngles = new Vector3(0, 0, Random.Range(20f, 40f));
        else
            transform.localEulerAngles = new Vector3(0, 0, Random.Range(-20f, -40f));
        yield return new WaitForSeconds(0.2f);
        transform.localEulerAngles = startRotation;
        transform.localPosition = startPosition;
    }

    private IEnumerator Reload()
    {
        while (currentReloadTime > 0)
        {
            currentReloadTime -= Time.deltaTime;
            yield return null;
        }
        currentReloadTime = 0;
        active = false;
    }
}

