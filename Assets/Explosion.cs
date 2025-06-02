#pragma warning disable 0618
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.3f; // cca dĺžka animácie

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}