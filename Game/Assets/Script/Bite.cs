using System.Collections;
using UnityEngine;
using System.Collections.Generic;


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
    public GameObject biteEffect;
    private Animator biteAnimator;

    public float damage = 10.0f;

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
                    onCooldown = true;
                }
                else if (currentTime - lastCPressTime > cooldown)
                {
                    lastTeleportTime = currentTime - cooldown;
                    lastCPressTime = currentTime;
                    gameObject.GetComponent<RandomSound>().PLayClipAt(biteSFX, transform.position);
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
        List<GameObject> validEnemies = new List<GameObject>();

        if (enemies.Length > 0)
        {
            int maxEnemiesToTeleport = Mathf.Min(enemies.Length, 2);
           
            for (int i = 0; i < enemies.Length; i++)
            {
                if (IsEnemyValid(enemies[i]))
                {
                    validEnemies.Add(enemies[i]);
                }
            }

            maxEnemiesToTeleport = Mathf.Min(validEnemies.Count, 2);

            for (int i = 0; i < maxEnemiesToTeleport; i++)
            {
                targetEnemy = validEnemies[i].transform;

                Vector3 direction = targetEnemy.position - transform.position;
                direction.Normalize();
                transform.position = targetEnemy.position - direction; // distance between player and enemy after tele

                yield return new WaitForSeconds(0.4f);
                targetEnemy.GetComponent<Health>().ApplyDamage(damage); // deal damage to enemy
                StartCoroutine(PlayBiteAnimation(targetEnemy.position, validEnemies));
                yield return new WaitForSeconds(biteAnimator.GetCurrentAnimatorClipInfo(0).Length);
            }

            if(validEnemies.Count==0){
                StartCoroutine(PlayBiteAnimation(transform.position, validEnemies));
            }
        }

        else{
            StartCoroutine(PlayBiteAnimation(transform.position, validEnemies));
        }
       
        

        isTeleporting = false;
    }

    private bool IsEnemyValid(GameObject enemy)
    {
        if (enemy != null)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            Rigidbody2D enemyRigidbody = enemy.GetComponent<Rigidbody2D>();

            if (enemyCollider != null && enemyCollider.enabled && enemyRigidbody != null)
            {
                return true;
            }
        }

        return false;
    }


    private IEnumerator PlayBiteAnimation(Vector3 bitePosition, List<GameObject> validEnemies)
    {

        float yOffset = 1.0f;
        float xOffset = 0.5f;
        bitePosition += new Vector3(xOffset, yOffset, 0);

        if (validEnemies.Count>0)
        {
        // Lock the camera to the bite position if on enemy
        Camera.main.transform.position = new Vector3(bitePosition.x, bitePosition.y, Camera.main.transform.position.z);
        }
        
        gameObject.GetComponent<RandomSound>().PLayClipAt(biteSFX, transform.position);
        
        // Play bite
        GameObject effectInstance = Instantiate(biteEffect, bitePosition, Quaternion.identity);
        biteAnimator = effectInstance.GetComponent<Animator>();
        biteAnimator.SetTrigger("Bite");
        yield return new WaitForSeconds(biteAnimator.GetCurrentAnimatorClipInfo(0).Length);
        
        Destroy(effectInstance);
        Camera.main.transform.parent = null;
    }


}
