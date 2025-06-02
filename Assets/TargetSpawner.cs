#pragma warning disable 0618 // vypne upozornenia
using UnityEngine;
using System.Collections;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform cameraTransform;
    public float cooldown = 1f;
    public float verticalOffset = 9f;

    public Sprite[] targetSprite;

    private float nextSpawnTime;
    private int destroyedTargets = 0;

    public int targetMilestone = 40;
    public float showerDuration = 10f;
    public float showerCooldown = 40f;

    //private bool isInMeteorShower = false;
    private float nextAllowedShowerTime = 0f;



    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnTarget();
            nextSpawnTime = Time.time + cooldown;
        }
    }

    public void TargetDestroyed()
    {
        destroyedTargets++;
        

        if (destroyedTargets >= targetMilestone && Time.time >= nextAllowedShowerTime)
        {
            
            StartCoroutine(MeteorShower());
        }
    }

    IEnumerator MeteorShower()
    {
        //isInMeteorShower = true;
        float originalCooldown = cooldown;
        cooldown = 0.3f; // viac meteorov
        nextAllowedShowerTime = Time.time + showerCooldown + showerDuration;

        Debug.Log("Meteor Shower STARTED!"); 
        UI.instance.ShowMeteorShowerText();


        yield return new WaitForSeconds(showerDuration);

        cooldown = originalCooldown;
        //isInMeteorShower = false;
        targetMilestone += 40; // ďalší shower pri +40

        Debug.Log("Meteor Shower ENDED. Next milestone: " + targetMilestone);
    }

    void SpawnTarget()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-7f, 7f), cameraTransform.position.y + verticalOffset, 0f);
        GameObject target = Instantiate(targetPrefab, spawnPos, Quaternion.identity);

        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();

        if (sr != null && targetSprite.Length > 0)
        {
            Sprite chosenSprite = targetSprite[Random.Range(0, targetSprite.Length)];
            sr.sprite = chosenSprite;

            TargetMovement tm = target.GetComponent<TargetMovement>();
            if (tm != null)
            {
                if (chosenSprite.name.ToLower().Contains("burning"))
                    tm.speed = 1.8f;
                else
                    tm.speed = 0.3f;
            }

        }
    }

}
