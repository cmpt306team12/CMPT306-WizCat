using System.Collections;
using UnityEngine;

public class Bite : MonoBehaviour
{
    private GameObject[] enemies;
    private Transform targetEnemy;
    private bool isTeleporting = false;
    private float lastTeleportTime = 0.0f;
    private float lastCPressTime = 0.0f;

    public float cooldown = 5.0f;
    public bool onCooldown = false;

    public static bool canBite = false;

    public AudioClip biteSFX;
    public AudioClip noBiteSFX;
    public GameObject biteEffect;
    private Animator biteAnimator;

    private void Start()
    {
        biteAnimator = biteEffect.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (canBite && !onCooldown)
            {
                float currentTime = Time.time;

                if (!isTeleporting && currentTime - lastTeleportTime >= cooldown)
                {
                    isTeleporting = true;
                    lastTeleportTime = currentTime;
                    StartCoroutine(TeleportToNearestEnemies());
                    gameObject.GetComponent<RandomSound>().PLayClipAt(biteSFX, transform.position);
                    onCooldown = true;
                }
                else if (currentTime - lastCPressTime > cooldown)
                {
                    lastTeleportTime = currentTime - cooldown;
                    lastCPressTime = currentTime;
                    gameObject.GetComponent<RandomSound>().PLayClipAt(noBiteSFX, transform.position);
                }
            }
        }

        if (onCooldown && Time.time - lastTeleportTime >= cooldown)
        {
            onCooldown = false;
        }
    }

    private IEnumerator TeleportToNearestEnemies()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length > 0)
        {
            int maxEnemiesToTeleport = Mathf.Min(enemies.Length, 2);

            for (int i = 0; i < maxEnemiesToTeleport; i++)
            {
                targetEnemy = enemies[i].transform;

                if (IsEnemyValid(targetEnemy.gameObject))
                {
                    Vector3 direction = targetEnemy.position - transform.position;
                    direction.Normalize();
                    transform.position = targetEnemy.position - direction; // distance between player and enemy after tele
                    
                    yield return new WaitForSeconds(0.4f);
                    StartCoroutine(PlayBiteAnimation(targetEnemy.position));
                    yield return new WaitForSeconds(biteAnimator.GetCurrentAnimatorClipInfo(0).Length);
                }
            }
        }
        else{
            StartCoroutine(PlayBiteAnimation(transform.position));

        }

        isTeleporting = false;
    }

    private bool IsEnemyValid(GameObject enemy)
    {
        return enemy != null;
    }

    private IEnumerator PlayBiteAnimation(Vector3 bitePosition)
    {

        if (enemies.Length > 0)
        {
        // Lock the camera to the bite position if on enemy
        Camera.main.transform.position = new Vector3(bitePosition.x, bitePosition.y, Camera.main.transform.position.z);
        }

        // Play bite
        GameObject effectInstance = Instantiate(biteEffect, bitePosition, Quaternion.identity);
        biteAnimator = effectInstance.GetComponent<Animator>();
        biteAnimator.SetTrigger("Bite");
        yield return new WaitForSeconds(biteAnimator.GetCurrentAnimatorClipInfo(0).Length);
        
        Destroy(effectInstance);
        Camera.main.transform.parent = null;
    }


}
