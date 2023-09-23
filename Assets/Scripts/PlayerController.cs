using System.Collections;
using UnityEngine;

public class Boosts
{
    public bool gun;
    public bool isFire;
    public int level;
    public int points;
    public bool eating;
    public int bulletCount;
}


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject hand;
    [SerializeField]
    private CameraShaker cameraShaker;
    [SerializeField]
    private GameObject sprite;
    public static PlayerController instance;
    private Vector2 direction;
    [SerializeField]
    private float speed;
    private Rigidbody2D rb;
    public bool isLose;
    public Boosts myBoosts;

    public void Initialize()
    {
        myBoosts = new Boosts();
        myBoosts.bulletCount = 5;
        isLose = false;
        SetInstance();
        rb = GetComponent<Rigidbody2D>();
        cameraShaker.transform.parent = transform;
        cameraShaker.Initialize();
    }

    private void OnLevelWasLoaded(int level)
    {
        if (!myBoosts.gun)
            hand.SetActive(false);
        else
            hand.SetActive(true);
    }

    private void SetInstance()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        DontDestroyOnLoad(instance);
    }

    private void Update()
    {
        if (!isLose)
        {
            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            rb.velocity = direction * speed;
            if (direction.x != 0)
            {
                if (direction.x < 0)
                {
                    if (direction.y != 0)
                    {
                        if (direction.y > 0)
                            sprite.transform.localEulerAngles = new Vector3(0, 0, 135f);
                        if (direction.y < 0)
                            sprite.transform.localEulerAngles = new Vector3(0, 0, 225f);
                    }
                    else
                        sprite.transform.localEulerAngles = new Vector3(0, 0, 180f);
                }
                if (direction.x > 0)
                {
                    if (direction.y != 0)
                    {
                        if (direction.y > 0)
                            sprite.transform.localEulerAngles = new Vector3(0, 0, 45f);
                        if (direction.y < 0)
                            sprite.transform.localEulerAngles = new Vector3(0, 0, -45f);
                    }
                    else
                        sprite.transform.localEulerAngles = new Vector3(0, 0, 0);
                }
            }
            else
            {
                if (direction.y != 0)
                {
                    if (direction.y > 0)
                        sprite.transform.localEulerAngles = new Vector3(0, 0, 90f);
                    if (direction.y < 0)
                        sprite.transform.localEulerAngles = new Vector3(0, 0, -90f);
                }
            }
        }
    }

    private IEnumerator Eating()
    {
        myBoosts.eating = true;
        float eatingTime = 5;
        while (eatingTime > 0)
        {
            eatingTime -= Time.deltaTime;
            yield return null;
        }
        myBoosts.eating = false;
    }

    public void Death()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isLose = true;
        Debug.Log("Game over!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Point"))
        {
            Destroy(collision.gameObject);
            myBoosts.points++;
        }
        if (collision.CompareTag("BigPoint"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(Eating());
        }
        if (collision.CompareTag("Portal"))
        {
            LoadScene.instance.Load();
        }
        if (collision.CompareTag("1up"))
        {
            LoadScene.instance.LoadUpgrade();
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("1up"))
        {
            Death();
        }
    }
}
