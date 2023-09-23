using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int damage;
    public void Initialize(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction.normalized * 50;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
            Destroy(gameObject);
        else
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
