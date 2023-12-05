using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReadableSign : MonoBehaviour
{
    // Fade text attributes
    public TMP_Text tmp_text;
    private float fadeTime = 1.0f;
    private bool showText = false;
    // Message attributes
    [SerializeField] public string defaultMessage;
    public bool useDefaultMessage;
    // Start is called before the first frame update
    void Start()
    {
        if (useDefaultMessage)
        {
            tmp_text.SetText(defaultMessage);
        } else
        {
            MessageHolder msgs = GameManager.instance.GetComponent<MessageHolder>();
            if (msgs != null)
            {
                tmp_text.SetText(msgs.GetRandomMessage());
            } else
            {
                // Set to generic message if there is no message holder in Gamemanager.
                tmp_text.SetText("Warning:\nSign has sharp edges.");
            }
        }
        // Set sign alph color to 
        tmp_text.fontMaterial.SetColor("_FaceColor", new Color(1, 1, 1, 0));
        tmp_text.fontMaterial.SetColor("_OutlineColor", new Color(0, 0, 0, 0));
        tmp_text.ForceMeshUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            showText = true;
            StartCoroutine(FadeText());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            showText = false;
        }
    }

    private IEnumerator FadeText()
    {
        float waitTime = 0.0f;
        while (waitTime > 0.0 || showText)
        {
            if ((waitTime < 1.0 && showText) || (waitTime > 0.0 && !showText))
            {
                if (showText)
                {
                    waitTime = waitTime + Time.deltaTime / fadeTime;
                }
                else
                {
                    waitTime = waitTime - Time.deltaTime / fadeTime;
                }
                tmp_text.ForceMeshUpdate();
                tmp_text.fontMaterial.SetColor("_FaceColor", new Color(1, 1, 1, waitTime));
                tmp_text.fontMaterial.SetColor("_OutlineColor", new Color(0, 0, 0, waitTime));
            }
            yield return null;
        }
        tmp_text.fontMaterial.SetColor("_FaceColor", new Color(1, 1, 1, 0));
        tmp_text.fontMaterial.SetColor("_OutlineColor", new Color(0, 0, 0, 0));
        StopAllCoroutines();
    }
}
