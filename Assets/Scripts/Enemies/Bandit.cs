using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bandit : EnemyBasic
{
    [SerializeField]
    private GameObject eyas;
    [SerializeField]
    private SpriteRenderer mySprite;
    private Sprite startSprite;
    [SerializeField]
    private Sprite ghost;
    [SerializeField]
    private float followDistance;
    [SerializeField]
    private float speed;
    private Rigidbody2D rb;
    private bool init;
    private bool isWalk;
    private float walkTime;
    private bool playerFollower;
    [SerializeField]
    private SpriteRenderer weaponSprite;

    public void Initialize()
    {
        startSprite = mySprite.sprite;
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        init = true;
        isWalk = false;
        rb = GetComponent<Rigidbody2D>();
    }

    public override void TurnOnPlayer()
    {
        if (PlayerController.instance.myBoosts.eating)
        {
            weaponHand.SetActive(false);
        }
        else
        {
            if (playerFollower)
            {
                weaponHand.SetActive(true);
                if (PlayerController.instance.transform.position.x < transform.position.x)
                {
                    weaponHand.transform.localRotation = Quaternion.Euler(0f, 0f, HandAngle() + 180);
                    if (!weaponSprite.flipY)
                        weaponSprite.flipY = true;
                }
                else
                {
                    weaponHand.transform.localRotation = Quaternion.Euler(0f, 0f, HandAngle());
                    if (weaponSprite.flipY)
                        weaponSprite.flipY = false;
                }
            }
            else
            {
                weaponHand.SetActive(false);
            }
        }
    }


    private float HandAngle()
    {
        Vector2 mousePosition = (PlayerController.instance.transform.position - transform.position).normalized;
        return Mathf.Atan(mousePosition.y / mousePosition.x) * Mathf.Rad2Deg;
    }

    public override bool PlayerIsVisible()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, (PlayerController.instance.transform.position - transform.position).normalized * 100);
        Debug.DrawLine(transform.position, PlayerController.instance.transform.position, Color.red);

        int counter = 0;
        for (int i = 0; i < hit.Length; i++)
        {
            if (!hit[i].transform.GetComponent<Collider2D>().isTrigger)
            {
                counter++;
                if (counter == 2 && hit[i].transform.CompareTag("Player"))
                {
                    return true;
                }
                if (counter > 2)
                    return false;
            }
        }
        return false;
    }

    private void WalkToPlayer()
    {
        if (playerFollower)
        {
            if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) > 0.5f)
            {
                rb.velocity = (PlayerController.instance.transform.position - transform.position).normalized * speed;
            }
        }
        else
            rb.velocity = Vector2.zero;
    }

    private void FreeWalk()
    {
        Debug.Log("Walk");
        Vector2 targetPosition = MapGenerator.instance.RandomCell();
        List<RaycastHit2D> hits = Physics2D.RaycastAll(transform.position, targetPosition).ToList();

        Debug.DrawLine(transform.position, targetPosition, Color.yellow);

        if (hits.Count > 0)
        {
            if (Vector2.Distance(transform.position, targetPosition) > 5f)
            {
                isWalk = true;
                StartCoroutine(WalkToCell(targetPosition));
            }
        }
    }

    private IEnumerator WalkToCell(Vector2 targetPosition)
    {
        walkTime = Vector2.Distance(transform.position, targetPosition) / speed;
        Debug.DrawLine(transform.position, targetPosition, Color.green, walkTime);
        rb.velocity = (targetPosition - (Vector2)transform.position).normalized * speed;
        while (walkTime > 0)
        {
            yield return null;
            walkTime -= Time.deltaTime;
        }
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);
        isWalk = false;
    }


    private void Update()
    {
        TurnOnPlayer();
        if (init)
        {
            if (PlayerController.instance.myBoosts.eating)
            {
                eyas.SetActive(false);
                mySprite.sprite = ghost;
                RunAway();
            }
            else
            {
                eyas.SetActive(true);
                mySprite.sprite = startSprite;
                if (!PlayerController.instance.isLose && PlayerIsVisible() && Vector2.Distance(transform.position, PlayerController.instance.transform.position) < followDistance)
                {
                    playerFollower = true;
                    isWalk = true;
                    WalkToPlayer();
                }
                else
                {
                    playerFollower = false;
                    if (!isWalk)
                    {
                        FreeWalk();
                    }
                }
            }
        }
    }

    private void RunAway()
    {
        rb.velocity = -(PlayerController.instance.transform.position - transform.position).normalized * speed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (init)
        {
            if (!collision.transform.CompareTag("Player"))
            {
                walkTime = 0;
                StopAllCoroutines();
                isWalk = false;
                playerFollower = false;
            }
            if (collision.transform.CompareTag("Player"))
            {
                isWalk = false;
                playerFollower = false;
                if (PlayerController.instance.myBoosts.eating)
                    Death();
                else
                    collision.transform.GetComponent<PlayerController>().Death();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Point"))
        {
            Destroy(collision.gameObject);
        }
    }
}
