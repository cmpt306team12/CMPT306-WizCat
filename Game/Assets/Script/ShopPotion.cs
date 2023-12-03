using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopPotion : MonoBehaviour
{
    [SerializeField] bool OneUse = true;
    public GameObject selectedItem;

    public int price = 1;

    private GameManager gameManager;

    public GameObject priceText;
    TextMeshPro pricetext;
    public GameObject itemImage;
    public GameObject itemName;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;

        // set price text
        pricetext = priceText.GetComponent<TextMeshPro>();
        pricetext.text = "" + price;

        // set item image
        SpriteRenderer shopSprite = itemImage.GetComponent<SpriteRenderer>();
        SpriteRenderer itemSprite = selectedItem.GetComponentInChildren<SpriteRenderer>();
        shopSprite.sprite = itemSprite.sprite;

        // set item name text
        TextMeshPro itemText = itemName.GetComponent<TextMeshPro>();
        itemText.text = "Healing Potion";
    }

    void Update()
    {
        if (price > gameManager.coinCount) { pricetext.color = Color.red; }
        else { pricetext.color = Color.white; }
    }

    public void InstantiateItem()
    {
        Debug.Log("Not enough coins");
        if (selectedItem != null)
        {
            if (gameManager.coinCount >= price)
            {
                gameManager.DecreaseCoins(price);
                Vector2 offset = new Vector2(0, -1);
                Vector2 coords = (Vector2)transform.position + offset;
                Instantiate(selectedItem, coords, transform.rotation);

                if (OneUse)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.Log("Not enough coins");
            }
        }
        else
        {
            Debug.LogError("No selected item to instantiate.");
        }
    }
}
