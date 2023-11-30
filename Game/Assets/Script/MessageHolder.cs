using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageHolder : MonoBehaviour
{
    [SerializeField] public string[] messages;

    public string GetRandomMessage()
    {
        return messages[Random.Range(0, messages.Length)];
    }
}
