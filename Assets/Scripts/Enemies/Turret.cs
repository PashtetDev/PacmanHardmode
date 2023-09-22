using UnityEngine;

public class Turret : EnemyBasic
{
    [SerializeField]
    private float radiusActivated;

    public override bool PlayerIsVisible()
    {
        throw new System.NotImplementedException();
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
}
