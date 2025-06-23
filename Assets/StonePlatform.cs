#pragma warning disable 0618
using UnityEngine;

public class StonePlatform : MonoBehaviour
{
    [SerializeField] private float torqueAmount = 30f;
    [SerializeField] private float returnSpeed = 10f; // ako rýchlo sa narovná
    [SerializeField] private string playerTag = "Player";

    private Rigidbody2D rb;
    private bool playerOnPlatform = false;
    [SerializeField] private float slideForce = 30f;
    [SerializeField] private float slideThreshold = 10f; // min. uhol natočenia
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.angularDrag = 7f; // tlmenie otáčania
    }

    void FixedUpdate()
    {
        if (!playerOnPlatform)
        {
            // jemne vráti platformu do pôvodnej rotácie (0°)
            float angleDifference = -rb.rotation;
            float correctionTorque = angleDifference * returnSpeed * Time.fixedDeltaTime;
            rb.AddTorque(correctionTorque);

        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                float tilt = transform.eulerAngles.z;
                if (tilt > 180f) tilt -= 360f; // Normalizácia: -180 až 180

                if (Mathf.Abs(tilt) < slideThreshold)
                    return; // Platforma je skoro rovná

                float xDiff = collision.transform.position.x - transform.position.x;

                if (Mathf.Abs(xDiff) < 0.2f)
                    return; // Stojí v strede -> nešmýka

                // Použi pozíciu hráča (nie tilt!) na určenie smeru
                float slideDirection = Mathf.Sign(xDiff); // +1 ak je vpravo, -1 ak vlavo
                Vector2 push = new Vector2(slideDirection * slideForce, 0f);

                playerRb.AddForce(push, ForceMode2D.Force);
            }
        }
    }






    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            playerOnPlatform = true;

            Vector2 contactPoint = collision.GetContact(0).point;
            float xOffset = contactPoint.x - transform.position.x;

            if (Mathf.Abs(xOffset) < 0.1f)
            {
                // Hráč dopadol takmer presne do stredu → žiadny torque
                return;
            }

            float direction = xOffset < 0 ? 1f : -1f;

            // Čím ďalej od stredu, tým silnejšie (limitované na max 1f)
            float strength = Mathf.Clamp01(Mathf.Abs(xOffset) / 1f); // 1f = max vzdialenosť, uprav ak máš väčšiu platformu

            rb.AddTorque(torqueAmount * direction * strength, ForceMode2D.Impulse);
        }
    }



    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            playerOnPlatform = false;
        }
    }
}
