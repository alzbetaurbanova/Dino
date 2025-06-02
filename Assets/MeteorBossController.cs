#pragma warning disable 0618
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeteorBossController : MonoBehaviour
{
    [Header("Boss Settings")]
    public int maxHealth = 50;
    private int currentHealth;

    [Header("Attack Settings")]
    [SerializeField] private GameObject miniMeteorPrefab;
    [SerializeField] private Transform[] shootPoints;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("References")]
    [SerializeField] private UI ui;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TargetSpawner targetSpawner;

    [Header("Camera Zoom")]
    [SerializeField] private float zoomOutSize = 10f;
    private float originalCamSize;

    private bool fightActive = false;

    [Header("Boss Animators")]
    [SerializeField] private Animator headAnimator;
    [SerializeField] private Animator eyesAnimator;
    [SerializeField] private Animator mouthAnimator;
    [SerializeField] private Animator handsAnimator;

    void Start()
    {
        currentHealth = maxHealth;
        originalCamSize = mainCamera.orthographicSize;
        

    }

    // Toto pridaj:
    public void StartBossFightExternally()
    {
        StartCoroutine(StartBossFight());
    }


    IEnumerator StartBossFight()
    {
        fightActive = true;

        originalCamSize = mainCamera.orthographicSize;

        // Zoom out kamery
        float elapsed = 0f;
        float zoomDuration = 1f;
        while (elapsed < zoomDuration)
        {
            mainCamera.orthographicSize = Mathf.Lerp(originalCamSize, zoomOutSize, elapsed / zoomDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mainCamera.orthographicSize = zoomOutSize;

        if (targetSpawner != null)
            targetSpawner.verticalOffset = 1000f;

        // Boss music? -> musicManager.PlayBossMusic();
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (fightActive && currentHealth > 0)
        {
            PlayAttackAnimations();

            foreach (Transform shootPoint in shootPoints)
            {
                SpawnMiniMeteor(shootPoint.position);
            }

            yield return new WaitForSeconds(attackCooldown);
            PlayIdleAnimations();
        }
    }

    void SpawnMiniMeteor(Vector3 pos)
    {
        GameObject meteor = Instantiate(miniMeteorPrefab, pos, Quaternion.identity);
        Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.down * Random.Range(2f, 4f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!fightActive) return;

        currentHealth -= damage;
        ui.UpdateHPHearts(currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(EndBossFight());
        }
    }

    IEnumerator EndBossFight()
    {
        fightActive = false;

        //PlayDeathAnimations();

        float elapsed = 0f;
        float zoomDuration = 1f;
        while (elapsed < zoomDuration)
        {
            mainCamera.orthographicSize = Mathf.Lerp(zoomOutSize, originalCamSize, elapsed / zoomDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mainCamera.orthographicSize = originalCamSize;

        if (targetSpawner != null)
            targetSpawner.verticalOffset = 9f;

        // musicManager.PlayNormalMusic();
        yield return new WaitForSeconds(2f); // chvíľka na prehratie animácie smrti

        Destroy(gameObject);
        ui.OpenEndScreen();
    }

    void PlayIdleAnimations()
    {
        headAnimator.Play("BossHead_Idle");
        eyesAnimator.Play("BossEyes_Idle");
        mouthAnimator.Play("BossMouth_Idle");
        handsAnimator.Play("BossHands_Idle");
    }

    void PlayAttackAnimations()
    {
        headAnimator.Play("BossHead_Idle");
        eyesAnimator.Play("BossEyes_Attack");
        mouthAnimator.Play("BossMouth_Attack");
        handsAnimator.Play("BossHands_Attack");
    }

   /* void PlayDeathAnimations()
    {
        headAnimator.Play("BossHead_Death");
        eyesAnimator.Play("BossEyes_Death");
        mouthAnimator.Play("BossMouth_Death");
        handsAnimator.Play("BossHands_Death");
    }*/
}
