using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Player : MonoBehaviour
{

    public float fireRate;
    public AudioClip shootSoundEffect;
    private bool stunned;

    // pickup sound
    public AudioClip perkSound;

    // Wand ref
    private Wand wand;

    // Stun gameobject reference
    public GameObject stun;

    // Fire rate stuff
    public float cooldown = 0.5f;
    public bool canFire = true;

    // Start is called before the first frame update
    void Start()
    {
        // Apply perks Saved in StaticData
        gameObject.GetComponent<Perks>().SetPerks(StaticData.perks);
        gameObject.GetComponent<ProjectileProperties>().ApplyPerks(StaticData.perks);
        wand = gameObject.GetComponentInChildren<Wand>();
        stunned = false;
    }

    public void Stun(float stunTime)
    {
        stunned = true;
        StartCoroutine(Recover(stunTime));
    }

    public bool IsStunned()
    {
        return stunned;
    }

    IEnumerator Recover(float rt)
    {
        // Display stunned icon
        if (stun == null) yield break;
        else
        {
            stun.SetActive(true);
        }
        yield return new WaitForSeconds(rt);
        if (stun == null) yield break;
        else
        {
            stun.SetActive(false);
        }
        // recover from stun
        stunned = false;
    }

    IEnumerator FireCooldown(float c)
    {
        canFire = false;
        yield return new WaitForSeconds(c);
        canFire = true;
    }



    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused && Input.GetMouseButton(0) && !stunned && wand.enabled && canFire)
        {
            wand.Shoot();
            StartCoroutine(FireCooldown(cooldown));
            // play player shoot sound effect
            gameObject.GetComponent<RandomSound>().PLayClipAt(shootSoundEffect, gameObject.transform.position);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Perk") && gameObject.CompareTag("Player"))
        {
            int id = collision.gameObject.GetComponent<Perk>().GetPerkID();
            collision.gameObject.GetComponent<Perk>().Despawn();
            gameObject.GetComponent<Perks>().AddPerk(id);
            GetComponentInChildren<AudioSource>().clip = perkSound;
            StartCoroutine(softerPerk());
        }
    }

    IEnumerator softerPerk()
    {
        GetComponentInChildren<AudioSource>().clip = perkSound;
        GetComponentInChildren<AudioSource>().volume = 0.25f;
        GetComponentInChildren<AudioSource>().Play();
        yield return new WaitForSeconds(1.8f);
        GetComponentInChildren<AudioSource>().volume = 1;
    }

}
