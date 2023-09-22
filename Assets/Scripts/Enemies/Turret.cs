using UnityEngine;

public class Turret : EnemyBasic
{
    public override void TurnOnPlayer()
    {
        Vector2 mousePosition = (PlayerController.instance.transform.position - transform.position).normalized;
        if (PlayerController.instance.transform.position.x > transform.position.x)
            hand.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan(mousePosition.y / mousePosition.x) * Mathf.Rad2Deg);
        else
            hand.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan(mousePosition.y / mousePosition.x) * Mathf.Rad2Deg + 180);
    }
}
