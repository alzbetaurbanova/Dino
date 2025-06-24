#pragma warning disable 0618
using UnityEngine;
using System.Collections;

public class BossTrigger : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera mainCamera;
    public float zoomOutSize = 12f;
    public float yOffset = 8f;
    public float zoomDuration = 2f;

    private bool cameraZoomedIn = false;
    private float originalCamSize;
    private Vector3 originalCamPosition;

    private MeteorBossController boss;
    private CameraScroll cameraScroll;
    
    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        boss = FindObjectOfType<MeteorBossController>();
        cameraScroll = FindObjectOfType<CameraScroll>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (boss == null || cameraScroll == null) return;

        if (!cameraZoomedIn)
        {
            originalCamSize = mainCamera.orthographicSize;
            originalCamPosition = new Vector3(
                mainCamera.transform.position.x,
                cameraScroll.player.position.y + cameraScroll.verticalOffset,
                mainCamera.transform.position.z
            );

            cameraScroll.allowFollow = false;

            StopAllCoroutines();
            StartCoroutine(ZoomCameraOut());

            cameraZoomedIn = true;
            boss.StartBossFightExternally();
        }

        boss.SetAttackState(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (boss == null || cameraScroll == null) return;

        float playerY = other.transform.position.y;
        float triggerY = transform.position.y;

        if (playerY < triggerY) // len ak hráč odíde smerom dole
        {
            if (cameraZoomedIn)
            {
                StopAllCoroutines();
                StartCoroutine(ResetCameraCoroutine());
                cameraZoomedIn = false;
            }

            boss.SetAttackState(false);
        }
    }

    private IEnumerator ZoomCameraOut()
    {
        float elapsed = 0f;
        Vector3 targetPos = originalCamPosition + new Vector3(0f, yOffset, 0f);
        float startZoom = mainCamera.orthographicSize;
        Vector3 startPos = mainCamera.transform.position;

        while (elapsed < zoomDuration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / zoomDuration);
            mainCamera.orthographicSize = Mathf.Lerp(startZoom, zoomOutSize, t);
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = zoomOutSize;
        mainCamera.transform.position = targetPos;
    }

    private IEnumerator ResetCameraCoroutine()
    {
        float elapsed = 0f;
        float startZoom = mainCamera.orthographicSize;
        Vector3 startPos = mainCamera.transform.position;

        while (elapsed < zoomDuration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / zoomDuration);
            mainCamera.orthographicSize = Mathf.Lerp(startZoom, originalCamSize, t);
            mainCamera.transform.position = Vector3.Lerp(startPos, originalCamPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = originalCamSize;
        mainCamera.transform.position = originalCamPosition;

        cameraScroll.allowFollow = true;
    }
}
