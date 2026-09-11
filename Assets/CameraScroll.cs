using System.Collections;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 2f;
    public float verticalOffset = 3f;

    [Header("Falling")]
    public float fallFollowSpeed = 12f;
    public float fallSpeedForMaxFollow = 15f;
    public float fallVerticalOffset = -1.5f;
    private Rigidbody2D playerRb;

    private Vector3 shakeOffset = Vector3.zero;
    private Vector3 originalPos;
    public float shakeDuration = 11f;
    public float shakeMagnitude = 1f;
    public float returnSmoothTime = 0.5f;

    private bool isReturning = false;
    private float returnElapsed = 0f;
    public bool enableShake = true;
    private Coroutine shakeCoroutine;

    public bool allowFollow = true; // 🔥 Nové – ovládanie sledovania

    void Start()
    {
        if (player != null)
            playerRb = player.GetComponent<Rigidbody2D>();
    }

    // 0 while rising or standing, 1 at full fall speed. Drives both how fast the camera
    // catches up and how far it looks down, so the player can see what is below.
    private float FallAmount()
    {
        if (playerRb == null || playerRb.linearVelocity.y >= 0f)
            return 0f;

        return Mathf.InverseLerp(0f, fallSpeedForMaxFollow, -playerRb.linearVelocity.y);
    }

    void Update()
    {
        if (player == null) return;

        if (!enableShake)
        {
            shakeOffset = Vector3.zero;
            isReturning = false;
        }

        if (allowFollow)
        {
            float fall = FallAmount();

            Vector3 targetPosition = new Vector3(
                transform.position.x,
                player.position.y + Mathf.Lerp(verticalOffset, fallVerticalOffset, fall),
                transform.position.z
            );

            if (isReturning)
            {
                returnElapsed += Time.deltaTime;
                float t = Mathf.Clamp01(returnElapsed / returnSmoothTime);
                shakeOffset = Vector3.Lerp(shakeOffset, Vector3.zero, t);

                if (t >= 1f)
                {
                    isReturning = false;
                    shakeOffset = Vector3.zero;
                }
            }

            transform.position = Vector3.Lerp(transform.position, targetPosition + shakeOffset, Mathf.Lerp(followSpeed, fallFollowSpeed, fall) * Time.deltaTime);
        }
    }

    public void ShakeCamera()
    {
        if (!enableShake) return;

        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(CameraShakeCoroutine());
    }

    public void StopShake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        shakeOffset = Vector3.zero;
        isReturning = false;
    }

    public void SetShakeEnabled(bool isEnabled)
    {
        enableShake = isEnabled;
        if (!enableShake) StopShake();
    }

    private IEnumerator CameraShakeCoroutine()
    {
        float elapsed = 0f;
        isReturning = false;

        while (elapsed < shakeDuration)
        {
            if (!enableShake) yield break;

            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            shakeOffset = new Vector3(offsetX, offsetY, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        returnElapsed = 0f;
        isReturning = true;
    }
}
