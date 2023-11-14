using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitProjectiles : MonoBehaviour
{
    public float orbitRadius = 2.5f; 
    public float orbitSpeed = 6f;    
    private bool isOrbiting = false;
    private Rigidbody2D characterRigidbody;
    public static bool canOrbit = false;
    private float orbitDuration = 3.0f;
    private float currentOrbitTime = 0.0f;
    private float cooldownDuration = 2.0f; 
    private float lastCtrlPressTime = 0.0f;

    public AudioClip orbitSFX;
    public AudioClip noOrbitSFX;

    public GameObject orbitCirclePrefab;
    private GameObject orbitCircle;

    private void Start()
    {
        characterRigidbody = GetComponent<Rigidbody2D>();
        orbitCircle = Instantiate(orbitCirclePrefab, transform.position, Quaternion.identity);
        orbitCircle.SetActive(false);
    }

    private void Update()
    {
        if (canOrbit)
        {
            if ((Input.GetKeyDown(KeyCode.LeftControl)) && (Time.time - lastCtrlPressTime >= cooldownDuration))
            {
                isOrbiting = true;
                currentOrbitTime = 0.0f;
                lastCtrlPressTime = Time.time;
                gameObject.GetComponent<RandomSound>().PLayClipAt(orbitSFX, transform.position);
                //catSFX.PlayOneShot(orbitSFX);

                orbitCircle.SetActive(true);
                orbitCircle.transform.position = characterRigidbody.position;
            }

            else if ((Input.GetKeyDown(KeyCode.LeftControl)) && (Time.time - lastCtrlPressTime < cooldownDuration))
            {
                gameObject.GetComponent<RandomSound>().PLayClipAt(noOrbitSFX, transform.position);
                //catSFX.PlayOneShot(noOrbitSFX);
            }

            // if (Input.GetKeyUp(KeyCode.LeftControl))
            // {
            //     isOrbiting = false;
            // }

            if (isOrbiting)
            {
                if (currentOrbitTime < orbitDuration)
                {
                    Vector2 orbitCenter = characterRigidbody.position;
                    GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

                    foreach (var projectile in projectiles)
                    {
                        float distanceToProjectile = Vector2.Distance(orbitCenter, projectile.transform.position);

                        if (distanceToProjectile <= orbitRadius)
                        {
                            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

                            if (rb != null)
                            {
                                Vector2 directionToCenter = (orbitCenter - rb.position).normalized;
                                Vector2 orbitalDirection = new Vector2(-directionToCenter.y, directionToCenter.x); // perpendicular direction 
                                rb.velocity = orbitalDirection * orbitSpeed; // force for circular motion 
                            }
                        }
                    }
                    orbitCircle.transform.position = characterRigidbody.position;
                    currentOrbitTime += Time.deltaTime;
                }
                else
                {
                    isOrbiting = false;
                    orbitCircle.SetActive(false);
                }
            }
        }
    }
}
