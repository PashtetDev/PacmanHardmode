using UnityEngine;

public abstract class EnemyBasic : MonoBehaviour
{
    public GameObject weaponHand;
    [SerializeField]
    private int hp;

    public abstract void TurnOnPlayer();
    public abstract bool PlayerIsVisible();

    private void Update()
    {
        TurnOnPlayer();
    }

    public void Death()
    {
        PlayerController.instance.AddPoints();
        MapGenerator.instance.DeleteEnemy(gameObject);
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
