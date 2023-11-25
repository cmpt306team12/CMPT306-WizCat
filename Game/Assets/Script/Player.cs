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
        // wait for specified time
        yield return new WaitForSeconds(rt);
        stunned = false;
        yield return null;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !stunned && wand.enabled)
        {
            wand.Shoot();
            // play player shoot sound effect
            gameObject.GetComponent<RandomSound>().PLayClipAt(shootSoundEffect, gameObject.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Perk"))
        {
            GetComponentInChildren<AudioSource>().clip = perkSound;
            GetComponentInChildren<AudioSource>().Play();
            int id = collision.gameObject.GetComponent<Perk>().GetPerkID();
            collision.gameObject.GetComponent<Perk>().Despawn();
            gameObject.GetComponent<Perks>().AddPerk(id);
        }
    }

}
