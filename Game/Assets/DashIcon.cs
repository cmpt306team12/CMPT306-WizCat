using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DashIcon : MonoBehaviour
{
    public Image iconImage;
    public PlayerMovement playerMovement;
    public TMP_Text countdownText;
    private Coroutine countdownCoroutine;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public void UpdateDashIcon()
    {
        if (playerMovement.canDash)
        {
            if (!playerMovement.onCooldown)
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
        float cooldownDuration = playerMovement.dashCooldown;

        while (elapsedTime < cooldownDuration)
        {
            float remainingTime = cooldownDuration - elapsedTime;
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
