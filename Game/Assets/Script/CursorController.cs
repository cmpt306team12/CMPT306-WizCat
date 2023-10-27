using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController instance;
    public Texture2D crosshairs;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ActivateCrosshair();
    }

    public void ActivateCrosshair()
    {
        Cursor.SetCursor(crosshairs, new Vector2(crosshairs.width / 2, crosshairs.height / 2), CursorMode.Auto);
    }
}
