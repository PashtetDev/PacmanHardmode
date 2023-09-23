using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;

    public void Initialize(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Fire"))
            Destroy(gameObject);
        else
        {
            if (collision.CompareTag("Player") && !CompareTag("Bullet"))
            {
                Destroy(gameObject);
                PlayerController.instance.Death();
            }
            else
            {
                if (!CompareTag("Fire"))
                {
                    EnemyBasic enemy;
                    if (collision.TryGetComponent(out enemy))
                    {
                        Destroy(gameObject);
                        enemy.GetDamage(damage);
                    }
                }
            }
        }
    }
}
