using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryBanner : MonoBehaviour
{

    [SerializeField] private CanvasGroup banner;

    [SerializeField] private bool hideBanner = false;

    [SerializeField] private int hideBannerIn = 3;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HideBanner());
    }

    // Update is called once per frame
    void Update()
    {
        if (hideBanner)
        {
            if (banner.alpha >= 0)
            {
                banner.alpha -= Time.deltaTime;
                if (banner.alpha == 0)
                {
                    hideBanner = false;
                }
            }
        }
    }

    public IEnumerator HideBanner()
    {
        yield return new WaitForSeconds(hideBannerIn);
        hideBanner = true;
    }
}
