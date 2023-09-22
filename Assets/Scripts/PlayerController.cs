using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    private Vector2 direction;
    [SerializeField]
    private float speed;
    private Rigidbody2D rb;

    private void Start()
    {
        Initialization();
    }

    private void Initialization()
    {
        SetInstance();
        rb = GetComponent<Rigidbody2D>();
    }

    private void SetInstance()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(CameraShaker.instance.Shake(0.1f, 0.4f));
        rb.velocity = direction * speed;
    }
}
