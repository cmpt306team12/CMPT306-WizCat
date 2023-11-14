using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Sprite damaged;
    public Sprite veryDamaged;

    private float currHealth;
    private float maxhealth;

    private bool isDamaged;
    private bool isVeryDamaged;

    private void Awake()
    {
        isDamaged = false;
        isVeryDamaged = false;
        StartCoroutine(UpdateSprite());
    }

    IEnumerator UpdateSprite()
    {
        for(; ;)
        {
            yield return new WaitForSeconds(0.1f);
            maxhealth = gameObject.GetComponent<Health>().maxHealth;
            currHealth = gameObject.GetComponent<Health>().currentHealth;
            if (!isDamaged && currHealth < (maxhealth / 2.0f))
            {
                Damaged();
            }
            if (!isVeryDamaged && currHealth < (maxhealth / 4.0f))
            {
                VeryDamaged();
            }
            if (isDamaged && isVeryDamaged)
            {
                break;
            }

        }
        yield break;
    }
    public void Damaged()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = damaged;
        isDamaged=true;
    }

    public void VeryDamaged()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = veryDamaged;
        isVeryDamaged=true;
    }
}
