#pragma warning disable 0618 //vypne upozornenia
using UnityEngine;

public class GunController : MonoBehaviour
{
    private Animator gunAnim;
    [SerializeField] private Transform gun;
    [SerializeField] private float gunDistance = 1.5f;
    private bool gunFacingRight = false;
    private Camera mainCamera;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float muzzleOffset = 0.6f;
    [SerializeField] private float muzzleSideOffset = 0.05f;
    [SerializeField] private int currentBullets;
    [SerializeField] private int maxBullets=15;

    
    
    private void Start()
    {
        mainCamera = Camera.main;
        Reload();
        gunAnim = gun.GetComponent<Animator>();
    }

    void Update()
    {
        if (mainCamera == null) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gun.rotation = Quaternion.Euler(0, 0, angle);
        gun.position = transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(gunDistance, 0, 0);

        if (UI.isGameOver) return; //aby nestrielal ked skončila hra
        if (VolumeUIController.isMenuOpen) return;



        if (Input.GetKeyDown(KeyCode.Mouse0) && HaveBullets())
            Shoot(direction);

        GunFlipController(mousePos);
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    private void GunFlipController(Vector3 mousePos)
    {
        
        if (mousePos.x < gun.position.x && !gunFacingRight)
        {
            GunFlip();

        }
        else if (mousePos.x > gun.position.x && gunFacingRight)
            GunFlip();
    }

    private void GunFlip()
    {
        gunFacingRight = !gunFacingRight;
        gun.localScale = new Vector3(gun.localScale.x, gun.localScale.y * -1, gun.localScale.z);
    }

    public void Shoot(Vector3 direction)
    {
        gunAnim.SetTrigger("Shoot");
        if (UI.instance != null)
            UI.instance.UpdateAmmoInfo(currentBullets, maxBullets);
        Vector3 aim = direction.normalized;
        float angle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;

        // Perpendicular to the aim, flipped with the gun sprite so it stays on the same side of the barrel.
        Vector3 side = new Vector3(-aim.y, aim.x, 0f) * Mathf.Sign(gun.localScale.y);

        // Spawn along the aim direction so the bullet always leaves the muzzle, never behind the gun,
        // and already rotated so it is not visibly misaligned on its first frame.
        Vector3 spawnPos = gun.position + aim * muzzleOffset + side * muzzleSideOffset;
        GameObject newBullet = Instantiate(bulletPrefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
        newBullet.GetComponent<Rigidbody2D>().velocity = aim * bulletSpeed;
        Destroy(newBullet, 7);
    }
    private void Reload()
    {
        currentBullets = maxBullets;
        if (UI.instance != null)
            UI.instance.UpdateAmmoInfo(currentBullets, maxBullets);
    }
    public bool HaveBullets()
    {
        if (currentBullets <= 0)
        {
            return false;
        }
        currentBullets--;
        return true;

    }
}