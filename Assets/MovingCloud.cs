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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        float direction = moveRight ? 1f : -1f;
        Vector3 move = new Vector3(direction, 0f, 0f) * moveSpeed * Time.fixedDeltaTime;

        if (movingOut)
        {
            rb.MovePosition(transform.position + move);
            if (Vector3.Distance(startPos, transform.position) >= moveDistance)
                movingOut = false;
        }
        else
        {
            rb.MovePosition(transform.position - move);
            if (Vector3.Distance(startPos, transform.position) <= 0.1f)
                movingOut = true;
        }
    }
}
