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

        if (boss == null)
        {
            boss = FindObjectOfType<MeteorBossController>();
        }
        if (cameraScroll == null)
        {
            cameraScroll = FindObjectOfType<CameraScroll>();
        }

        if (boss == null || cameraScroll == null) return;


        if (!cameraZoomedIn)
        {
            originalCamSize = mainCamera.orthographicSize;

            if (cameraScroll.player == null)
            {
                PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
                if (playerHealth != null) cameraScroll.player = playerHealth.transform;
            }
            if (cameraScroll.player == null) return;

            originalCamPosition = new Vector3(
                mainCamera.transform.position.x,
                cameraScroll.player.position.y + cameraScroll.verticalOffset,
                mainCamera.transform.position.z
            );

            cameraScroll.allowFollow = false;

            boss.StartBossCameraZoom(mainCamera, originalCamPosition, zoomOutSize, yOffset, zoomDuration);

            cameraZoomedIn = true;
            boss.StartBossFightExternally();
        }

        boss.SetAttackState(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (boss == null)
        {
            boss = FindObjectOfType<MeteorBossController>();
        }
        if (boss == null || cameraScroll == null) return;

        float playerY = other.transform.position.y;
        float triggerY = transform.position.y;

        if (playerY < triggerY)
        {
            if (cameraZoomedIn)
            {
                boss.ResetBossCamera(mainCamera, cameraScroll, originalCamSize, originalCamPosition, zoomDuration);

                cameraZoomedIn = false;
            }

            boss.SetAttackState(false);
        }
    }
}