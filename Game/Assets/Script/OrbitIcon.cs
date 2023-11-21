using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class OrbitIcon : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text countdownText;
    private Coroutine countdownCoroutine;

    void Update()
    {
        if (OrbitProjectiles.canOrbit)
        {
            if (!OrbitProjectiles.onCooldown)
            {
                iconImage.color = Color.white;
            }
            else
            {
                iconImage.color = Color.grey;

                if (countdownCoroutine == null)
                {
                    countdownCoroutine = StartCoroutine(CountdownTime());
                }
            }
        }
        else
        {
            iconImage.color = Color.grey;
        }
    }

    IEnumerator CountdownTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < OrbitProjectiles.cooldownDuration)
        {
            float remainingTime = OrbitProjectiles.cooldownDuration - elapsedTime;
            countdownText.text = Mathf.CeilToInt(remainingTime).ToString();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StopCountdownCoroutine();
    }


    void StopCountdownCoroutine()
    {
        StopCoroutine(countdownCoroutine);
        countdownCoroutine = null;

        // Reset countdown text
        if (countdownText != null)
        {
            countdownText.text = "";
        }
    }
}
