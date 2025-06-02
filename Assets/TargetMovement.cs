#pragma warning disable 0618 //vypne upozornenia
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public float speed = 1f;

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
}
