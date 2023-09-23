using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private SpriteRenderer sprite;
    float direction = 0;

    private void Awake()
    {
        if (PlayerController.instance.transform.position.x < transform.position.x)
            direction = -1;
        else
            direction = 1;
        if (direction == -1)
            sprite.flipX = true;
        else
            sprite.flipX = false;
    }

    private void Update()
    {
        transform.position += new Vector3(direction, 0, 0) * speed * Time.deltaTime;
        if (Mathf.Abs(PlayerController.instance.transform.position.x - transform.position.x) > 30)
            Destroy(gameObject);
    }
}
