using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bite : MonoBehaviour
{


    private GameObject[] enemies;
    private Transform targetEnemy;
    private bool isTeleporting = false;
    private float lastTeleportTime = 0.0f;
    private float lastCPressTime = 0.0f;
    public float delayBetweenEnemies = 0.5f;
    public float cooldown = 5.0f;
    public static bool canBite = false;

    public AudioClip biteSFX;
    public AudioClip noBiteSFX;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (canBite){
                float currentTime = Time.time;

                if (!isTeleporting && currentTime - lastTeleportTime >= cooldown)
                {
                    isTeleporting = true;
                    lastTeleportTime = currentTime;
                    StartCoroutine(TeleportToNearestEnemies());
                    gameObject.GetComponent<RandomSound>().PLayClipAt(biteSFX, transform.position);
                }
                else if (currentTime - lastCPressTime > cooldown)
                {
                    lastTeleportTime = currentTime - cooldown;
                    lastCPressTime = currentTime;
                    gameObject.GetComponent<RandomSound>().PLayClipAt(noBiteSFX, transform.position);
                }
            }
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

                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

        isTeleporting = false;
    }

    private bool IsEnemyValid(GameObject enemy)
    {
        if (enemy == null)
        {
            return false;
        }
        return true;
    }
}
