#pragma warning disable 0618
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingCloud : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private bool moveRight = true;

    private Rigidbody2D rb;
    private Vector3 startPos;
    private bool movingOut = true;
    private Rigidbody2D rider;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        float direction = (moveRight ? 1f : -1f) * (movingOut ? 1f : -1f);
        Vector2 delta = new Vector2(direction, 0f) * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + delta);

        if (movingOut)
        {
            if (Vector3.Distance(startPos, transform.position) >= moveDistance)
                movingOut = false;
        }
        else if (Vector3.Distance(startPos, transform.position) <= 0.1f)
        {
            movingOut = true;
        }

        // Carry the rider by the exact same delta, so it never drifts relative to the cloud
        // and its own velocity stays untouched (idle animation keeps playing).
        if (rider != null)
            rider.position += delta;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            rider = collision.rigidbody;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (rider != null && collision.rigidbody == rider)
            rider = null;
    }
}
