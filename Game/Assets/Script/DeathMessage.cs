using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathMessage : MonoBehaviour
{
    TMP_Text message;
    string writer;

    [SerializeField] private List<string> deathMessage;
    [SerializeField] float delayBeforeStart = 0.5f;
    [SerializeField] float timeBetweenChars = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        message = GetComponent<TMP_Text>();

        int randomMessage = Random.Range(0, deathMessage.Count);
        writer = deathMessage[randomMessage];
        message.text = "";

    }

    public void TypeMessage()
    {
        StartCoroutine("ShowMessage");
    }

    IEnumerator ShowMessage()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        foreach (char c in writer)
        {
            if (message.text.Length > 0)
            {
                message.text = message.text.Substring(0, message.text.Length);
            }
            message.text += c;
            yield return new WaitForSeconds(timeBetweenChars);
        }

    }

}
