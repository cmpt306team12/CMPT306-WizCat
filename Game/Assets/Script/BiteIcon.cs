using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class BiteIcon : MonoBehaviour
{
    public Image iconImage;
    public Bite bite;
    public TMP_Text countdownText;

    private Coroutine countdownCoroutine;

    void Start()
    {
        bite = FindObjectOfType<Bite>();
    }

    void Update()
    {
        if (Bite.canBite)
        {
            if (!bite.onCooldown)
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

        while (elapsedTime < bite.cooldown)
        {
            float remainingTime = bite.cooldown - elapsedTime;
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
