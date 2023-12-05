using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    // Text for perk achievement
    public GameObject AccessFloatingTextPrefab;

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
            // Need to get perk achievement
            if (PlayerPrefs.GetInt("CollectedPerk" + id, 0) == 0)
            {
                PlayerPrefs.SetInt("CollectedPerk" + id, 1);
                PlayerPrefs.Save();
                StartCoroutine(PerkFound(id));
            }
        }
    }

    IEnumerator PerkFound(int perkid)
    {
        string[] perkNames = {"Bounce", "Speed+", "Speed-", "Lifetime+", "Lifetime-", "Damage+","Damage-", "Explosive",
            "Size+", "Size-", "Burst", "Bite", "Dash", "Orbit", "Split", "Homing", "Boomerang", "Wiggle", "Health+"};
        // Display reward of access and stop particle effect
        Vector3 offset = new Vector3(0.0f, -1.0f, 0.0f);
        GameObject text = Instantiate(AccessFloatingTextPrefab, gameObject.transform.position + offset, Quaternion.identity);
        TextMeshProUGUI textMesh = text.GetComponentInChildren<TextMeshProUGUI>();
        string Text = perkNames[perkid] + " found!";
        textMesh.text = Text;
        Destroy(text, 3.0f);
        float time = 0.0f;
        float duration = 2.0f;
        float riseSpeed = 0.5f;
        Vector3 originalPosition = textMesh.transform.position;

        while (time < duration)
        {
            float alpha = Mathf.PingPong(time * 2.0f, 1.0f);
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
            textMesh.transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0.0f);
        textMesh.transform.position = originalPosition;
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
