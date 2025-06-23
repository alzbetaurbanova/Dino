// MeteorBossController.cs
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
    [SerializeField] private TargetSpawner targetSpawner;

    private bool fightActive = false;
    private bool attackEnabled = true;

    [Header("Boss Animators")]
    [SerializeField] private Animator headAnimator;
    [SerializeField] private Animator eyesAnimator;
    [SerializeField] private Animator mouthAnimator;
    [SerializeField] private Animator handsAnimator;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void StartBossFightExternally()
    {
        StartCoroutine(StartBossFight());
    }

    IEnumerator StartBossFight()
    {
        fightActive = true;

        if (targetSpawner != null)
            targetSpawner.verticalOffset = 1000f;

        StartCoroutine(AttackRoutine());
        yield return null;
    }

    IEnumerator AttackRoutine()
    {
        while (fightActive && currentHealth > 0)
        {
            if (!attackEnabled)
            {
                yield return null;
                continue;
            }

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

        if (targetSpawner != null)
            targetSpawner.verticalOffset = 9f;

        yield return new WaitForSeconds(2f);

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

    public void SetAttackState(bool enabled)
    {
        attackEnabled = enabled;

        if (!enabled)
        {
            PlayIdleAnimations();
        }
    }
}
