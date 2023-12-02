using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class OpenableChest : MonoBehaviour
{
    public Sprite openedSprite;
    public GameObject interactor;

    public void openChest() {
        // disable unwanted stuff when chest is opened
        gameObject.GetComponent<SpriteRenderer>().sprite = openedSprite;
        gameObject.GetComponent<Obstacle>().damaged = openedSprite;
        gameObject.GetComponent<Obstacle>().veryDamaged = openedSprite;
        gameObject.GetComponent<Health>().dropsLoot = false;
        interactor.SetActive(false);
        // drop the item
        gameObject.GetComponent<DropOnDestroy>().Drop();
    }
}
