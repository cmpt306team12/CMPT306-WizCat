using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearBanner : MonoBehaviour
{
    [SerializeField] private CanvasGroup banner;

    [SerializeField] private bool showBanner = false;
    [SerializeField] private bool hideBanner = false;

    [SerializeField] private int showBannerFor = 5;

    public void ShowBanner()
    {
        showBanner = true;
    }

    public IEnumerator HideBanner()
    {
        yield return new WaitForSeconds(showBannerFor);
        hideBanner = true;
    }

    private void Update()
    {

        if (showBanner)
        {
            if (banner.alpha < 1)
            {
                banner.alpha += Time.deltaTime; 
                if (banner.alpha >= 1)
                {
                    showBanner = false;
                }
            }
        }

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



}
