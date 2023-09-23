using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer weaponSprite;

    private float HandAngle()
    {
        Vector2 mousePosition = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        return Mathf.Atan(mousePosition.y / mousePosition.x) * Mathf.Rad2Deg;
    }

    private void Update()
    {
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, HandAngle() + 180);
            if (!weaponSprite.flipY)
                weaponSprite.flipY = true;
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, HandAngle());
            if (weaponSprite.flipY)
                weaponSprite.flipY = false;
        }
    }
}
