using UnityEngine;

public abstract class EnemyBasic : MonoBehaviour
{
    public GameObject hand;
    [SerializeField]
    private int hp;

    public abstract void TurnOnPlayer();
    public abstract bool PlayerIsVisible();

    private void Update()
    {
        TurnOnPlayer();
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    public void GetDamage(int damage)
    {
        if (hp > damage)
            hp -= damage;
        else
        {
            hp = 0;
            Death();
        }
    }
}
