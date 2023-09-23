using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boosts
{
    public int powerUp;
    public int isFire;
    public int level;
    public int points;
    public bool eating;
    public int gunBullet;
    public int bird;
    public bool boss;
    public bool mashroom;
    public int enemyBullet;
}


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject cursorTexture;
    [SerializeField]
    private GameObject losePanel, backGround;
    [SerializeField]
    private PointCounter counter;
    [SerializeField]
    private GameObject hand;
    [SerializeField]
    private CameraShaker cameraShaker;
    [SerializeField]
    private GameObject sprite;
    public static PlayerController instance;
    private Vector2 direction;
    public float speed;
    private Rigidbody2D rb;
    public bool isLose;
    public Boosts myBoosts;

    public void Initialize()
    {
        backGround.SetActive(false);
        losePanel.SetActive(false);
        myBoosts = new Boosts();
        myBoosts.gunBullet = 100;
        isLose = false;
        SetInstance();
        rb = GetComponent<Rigidbody2D>();
        cameraShaker.transform.parent = transform;
        cameraShaker.Initialize();
        counter.Initialize();
        BulletCheck();
    }

    public void BulletCheck()
    {
        if (myBoosts.gunBullet == 0)
        {
            hand.transform.GetChild(0).GetComponent<Weapon>().HideWeapon();
            hand.SetActive(false);
        }
        else
            hand.SetActive(true);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "Upgrade")
        {
            cursorTexture.SetActive(false);
            cameraShaker.transform.GetChild(0).gameObject.SetActive(false);
            transform.position = new Vector3(0, 0, -18);
        }
        else
        {
            cursorTexture.SetActive(true);
            transform.position = new Vector3(0, 0, 0);
            cameraShaker.transform.GetChild(0).gameObject.SetActive(true);
            BulletCheck();
        }
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
        if (myBoosts.powerUp >= 100)
        {
            myBoosts.powerUp = 0;
            StopAllCoroutines();
            StartCoroutine(Eating());
        }
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

    public void AddPoints()
    {
        myBoosts.points += 25;
    }

    public void Death()
    {
        if (PlayerPrefs.GetInt("TheBest") < myBoosts.points)
            PlayerPrefs.SetInt("TheBest", myBoosts.points);
        backGround.SetActive(true);
        losePanel.SetActive(true);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isLose = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Point"))
        {
            Destroy(collision.gameObject);
            myBoosts.points++;
            PointCounter.instance.UpdateText(myBoosts.points);
            myBoosts.powerUp++;
        }
        if (collision.CompareTag("BigPoint"))
        {
            StopAllCoroutines();
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
        if (collision.CompareTag("Fire") )
        {
            Death();
        }
        if (collision.CompareTag("Pipe"))
        {
            LoadScene.instance.LoadBoss();
            LoadScene.instance.Load();
        }
        if (collision.CompareTag("Gun"))
        {
            myBoosts.gunBullet += 5;
            Destroy(collision.gameObject);
            BulletCheck();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Boss"))
        {
            Death();
        }
    }
}
