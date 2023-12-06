using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ShopAspiration : MonoBehaviour
{
    [SerializeField] bool OneUse = true;

    public int price = 100;

    private GameManager gameManager;

    public GameObject priceText;
    TextMeshPro pricetext;
    public GameObject itemName;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;

        // set price text
        pricetext = priceText.GetComponent<TextMeshPro>();
        pricetext.text = "" + price;

        // set item name text
        TextMeshPro itemText = itemName.GetComponent<TextMeshPro>();
        itemText.text = "Make Your Offering";
    }

    void Update()
    {
        if (price > gameManager.coinCount) { pricetext.color = Color.red; }
        else { pricetext.color = Color.white; }
    }

    public void InstantiateItem()
    {
        if (gameManager.coinCount >= price)
        {
            gameManager.DecreaseCoins(price);

            // toggle player crown
            GameObject player = gameManager.GetPlayer();
            player.GetComponentInChildren<Crown>().gameObject.SetActive(true);
            // toggle map crown
            string tag = "Minimap";
            Transform childTransform = player.transform.Find(tag);
            if (childTransform != null)
            {
                GameObject miniIcon = childTransform.gameObject;
                miniIcon.GetComponentInChildren<Crown>().gameObject.SetActive(true);
            }
            // toggle healthbar crown
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                GameObject healthbar = canvas.GetComponentInChildren<FillStatusBar>().gameObject;
                healthbar.GetComponentInChildren<Crown>().gameObject.SetActive(true);
            }

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
}
