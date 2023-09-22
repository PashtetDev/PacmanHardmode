using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : EnemyBasic
{
    [SerializeField]
    private float speed;
    private Rigidbody2D rb;
    [SerializeField]
    private float radiusActivated;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void TurnOnPlayer()
    {
        if (Vector2.Distance(PlayerController.instance.transform.position, transform.position) < radiusActivated)
        {
            Vector2 mousePosition = (PlayerController.instance.transform.position - transform.position).normalized;
            if (PlayerController.instance.transform.position.x > transform.position.x)
                hand.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan(mousePosition.y / mousePosition.x) * Mathf.Rad2Deg);
            else
                hand.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan(mousePosition.y / mousePosition.x) * Mathf.Rad2Deg + 180);
        }
    }

    public override bool PlayerIsVisible()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, (PlayerController.instance.transform.position - transform.position).normalized * 100);
        Debug.DrawLine(transform.position, (PlayerController.instance.transform.position - transform.position).normalized * 100, Color.red);

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
        if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) > 5)
        {
            rb.velocity = (PlayerController.instance.transform.position - transform.position).normalized * speed;
        }
        else
            rb.velocity = Vector2.zero;
    }

    private void Update()
    {
        if (PlayerIsVisible())
        {
            if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) < 10)
                WalkToPlayer();
            else
                rb.velocity = Vector2.zero;
        }
        else
            rb.velocity = Vector2.zero;
    }
}
