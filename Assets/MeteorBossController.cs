#pragma warning disable 0618
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MeteorBossController : MonoBehaviour
{
    [Header("Boss Settings")]
    public int maxHealth = 200;
    private int currentHealth;
    private Coroutine regenCoroutine;
    [SerializeField] private float regenRate = 2f;
    [SerializeField] private int regenAmount = 1;

    [Header("UI References")]
    [SerializeField] private Slider bossHealthBar;
    [SerializeField] private GameObject bossHeartUI;

    [Header("Heart UI Images")]
    [SerializeField] private GameObject heartFull;
    [SerializeField] private GameObject heartCracked;
    [SerializeField] private GameObject heartMid;
    [SerializeField] private GameObject heartLow;
    [SerializeField] private GameObject heartEmpty;

    [Header("Attack Settings")]
    [SerializeField] private GameObject miniMeteorPrefab;
    [SerializeField] private Transform[] shootPoints;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("References")]
    [SerializeField] private UI ui;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private TargetSpawner targetSpawner;

    [Header("Boss Animators")]
    [SerializeField] private Animator headAnimator;
    [SerializeField] private Animator eyesAnimator;
    [SerializeField] private Animator mouthAnimator;
    [SerializeField] private Animator handsAnimator;

    private bool fightActive = false;
    private bool attackEnabled = true;

    void Start()
    {
        currentHealth = maxHealth;

        if (bossHealthBar != null)
        {
            bossHealthBar.maxValue = maxHealth;
            bossHealthBar.value = currentHealth;
            bossHealthBar.gameObject.SetActive(false);
        }

        if (bossHeartUI != null)
            bossHeartUI.SetActive(false);

        UpdateHeartUI();
    }

    public void StartBossFightExternally()
    {
        StartCoroutine(StartBossFight());
    }

    IEnumerator StartBossFight()
    {
        fightActive = true;

        if (bossHealthBar != null)
            bossHealthBar.gameObject.SetActive(true);

        if (bossHeartUI != null)
            bossHeartUI.SetActive(true);

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
                SpawnMiniMeteor(shootPoint.position);

            yield return new WaitForSeconds(attackCooldown);
            PlayIdleAnimations();
        }
    }

    void SpawnMiniMeteor(Vector3 pos)
    {
        GameObject meteor = Instantiate(miniMeteorPrefab, pos, Quaternion.identity);
        Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.down * Random.Range(2f, 4f);
    }

    public void TakeDamage(int damage)
    {
        if (!fightActive || !attackEnabled) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (bossHealthBar != null)
            bossHealthBar.value = currentHealth;

        UpdateHeartUI();

        if (currentHealth <= 0)
            StartCoroutine(EndBossFight());
    }

    void UpdateHeartUI()
    {
        if (bossHeartUI == null) return;

        heartFull.SetActive(false);
        heartCracked.SetActive(false);
        heartMid.SetActive(false);
        heartLow.SetActive(false);
        heartEmpty.SetActive(false);

        if (currentHealth > maxHealth * 0.75)
            heartFull.SetActive(true);
        else if (currentHealth > maxHealth * 0.5)
            heartCracked.SetActive(true);
        else if (currentHealth > maxHealth * 0.3)
            heartMid.SetActive(true);
        else if (currentHealth > maxHealth * 0.03)
            heartLow.SetActive(true);
        else
            heartEmpty.SetActive(true);
    }

    IEnumerator EndBossFight()
    {
        fightActive = false;

        if (bossHealthBar != null)
            bossHealthBar.gameObject.SetActive(false);

        if (bossHeartUI != null)
            bossHeartUI.SetActive(false);

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

    private IEnumerator RegenerateHealth()
    {
        while (currentHealth < maxHealth)
        {
            currentHealth += regenAmount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);

            if (bossHealthBar != null)
                bossHealthBar.value = currentHealth;

            UpdateHeartUI();
            yield return new WaitForSeconds(1f / regenRate);
        }

        regenCoroutine = null;
    }

    public void SetAttackState(bool enabled)
    {
        attackEnabled = enabled;

        if (bossHealthBar != null)
            bossHealthBar.gameObject.SetActive(enabled);

        if (bossHeartUI != null)
            bossHeartUI.SetActive(enabled);

        if (!enabled)
        {
            PlayIdleAnimations();
            if (regenCoroutine == null)
                regenCoroutine = StartCoroutine(RegenerateHealth());
        }
        else
        {
            if (regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
                regenCoroutine = null;
            }
        }
    }
}
