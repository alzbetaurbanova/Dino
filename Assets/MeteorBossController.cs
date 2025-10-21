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
    private Coroutine attackRoutineInstance;
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

    [Header("Minis")]
    [SerializeField] private GameObject miniMeteorPrefab;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float minSpeed = 15f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private Transform[] shootPoints;

    [Header("References")]
    [SerializeField] private UI ui;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private TargetSpawner targetSpawner;
    private Transform playerTransform;
    [SerializeField] private DeadZone deadZone;

    [Header("Boss Animators")]
    [SerializeField] private Animator headAnimator;
    [SerializeField] private Animator eyesAnimator;
    [SerializeField] private Animator mouthAnimator;
    [SerializeField] private Animator handsAnimator;

    [Header("Hover Movement")]
    [SerializeField] private float hoverDistance = 0.5f;
    [SerializeField] private float hoverSpeed = 1f;
    private Vector3 startPosition;

    private bool fightActive = false;
    private bool attackEnabled = true;

    private Coroutine cameraCoroutine;


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
        startPosition = transform.position;
        StartCoroutine(HoverRoutine());
    }


    public void StartBossFightExternally()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayBossMusic();
        }

        StartCoroutine(StartBossFight());
    }

    IEnumerator StartBossFight()
    {
        yield return new WaitForSeconds(attackCooldown);
        if (fightActive)
            yield break;
        fightActive = true;

        if (deadZone != null)
        {
            deadZone.gameObject.SetActive(false);
            Debug.Log("DeadZone OFF");
        }

        PlayerHealth player = FindObjectOfType<PlayerHealth>();

        playerTransform = player.transform;
        bossHealthBar.gameObject.SetActive(true);
        bossHeartUI.SetActive(true);
        targetSpawner.verticalOffset = 1000f;




        StartCoroutine(AttackRoutine());
        yield return null;
    }
    public void PauseBossFight()
    {
        fightActive = false;
        StopAllCoroutines();
        Debug.Log("Pause");
        if (deadZone != null)
        { deadZone.gameObject.SetActive(true); }
        Debug.Log("DeadZone ON");
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
        if (playerTransform == null)
        {
            Debug.LogWarning("Player transform not found!");
            return;
        }

        GameObject meteor = Instantiate(miniMeteorPrefab, pos, Quaternion.identity);

        Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on MiniMeteor!");
            return;
        }


        Vector2 direction = (playerTransform.position - pos).normalized;

        float randomAngle = Random.Range(-5f, 5f);
        direction = Quaternion.Euler(0, 0, randomAngle) * direction;

        float speed = Random.Range(minSpeed, maxSpeed);

        rb.gravityScale = 0f;
        rb.drag = 0f;
        rb.angularDrag = 0f;
        rb.velocity = direction * speed;

        Destroy(meteor, 5f);
    }


    public void TakeDamage(int damage)
    {
        if (!fightActive && !attackEnabled) return;

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
    IEnumerator HoverRoutine()
    {
        while (true)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverDistance;
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);

            yield return null;
        }
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

        PlayerPrefs.SetInt("Level2Unlocked", 1);
        PlayerPrefs.Save();

        Destroy(gameObject);
        ui.OpenEndScreen();
    }

    public void StartBossCameraZoom(Camera cam, Vector3 originalPos, float zoomSize, float offset, float duration)
    {
        if (cameraCoroutine != null)
            StopCoroutine(cameraCoroutine);

        cameraCoroutine = StartCoroutine(ZoomCameraOutCoroutine(cam, originalPos, zoomSize, offset, duration));
    }

    public void ResetBossCamera(Camera cam, CameraScroll cameraScroll, float originalSize, Vector3 originalPos, float duration)
    {
        if (cameraCoroutine != null)
            StopCoroutine(cameraCoroutine);

        cameraCoroutine = StartCoroutine(ResetCameraCoroutine(cam, cameraScroll, originalSize, originalPos, duration));
    }


    private IEnumerator ZoomCameraOutCoroutine(Camera cam, Vector3 originalPos, float zoomSize, float offset, float duration)
    {
        float elapsed = 0f;
        Vector3 targetPos = originalPos + new Vector3(0f, offset, 0f);
        float startZoom = cam.orthographicSize;
        Vector3 startPos = cam.transform.position;

        while (elapsed < duration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            cam.orthographicSize = Mathf.Lerp(startZoom, zoomSize, t);
            cam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = zoomSize;
        cam.transform.position = targetPos;
    }

    private IEnumerator ResetCameraCoroutine(Camera cam, CameraScroll cameraScroll, float originalSize, Vector3 originalPos, float duration)
    {
        float elapsed = 0f;
        float startZoom = cam.orthographicSize;
        Vector3 startPos = cam.transform.position;

        while (elapsed < duration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            cam.orthographicSize = Mathf.Lerp(startZoom, originalSize, t);
            cam.transform.position = Vector3.Lerp(startPos, originalPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = originalSize;
        cam.transform.position = originalPos;

        if (cameraScroll != null)
        {
            cameraScroll.allowFollow = true;
        }
    }
}