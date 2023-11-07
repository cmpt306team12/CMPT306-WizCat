using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeFloor : MonoBehaviour
{
    private Animator anim;
    [SerializeField] float delayTime = 0.25f;
    [SerializeField] float spikeUpTime = 0.5f;
    private bool animating = false;
    public GameObject spikeHitbox;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        spikeHitbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Fire(float delay)
    {
        // wait for specified time
        yield return new WaitForSeconds(delay);
        // Make the spikes play one animation
        anim.SetTrigger("ShootSpikes");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length / 4);
        spikeHitbox.SetActive(true);
        yield return new WaitForSeconds(spikeUpTime);

        // remove trigger that hurst Player
        spikeHitbox.SetActive(false);
        anim.SetTrigger("RetractSpikes");
        animating = false;
        gameObject.GetComponent<Collider2D>().enabled = true;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Spikes animating: " + animating);
            if (!animating)
            {
                // disable trigger collider
                gameObject.GetComponent<Collider2D>().enabled = false;
                animating = true;
                StartCoroutine(Fire(delayTime));
            }
        }
    }
}
