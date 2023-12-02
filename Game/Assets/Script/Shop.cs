using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Shop : MonoBehaviour
{
    [SerializeField] bool OneUse = true;
    [SerializeField] bool PerkNames = true;

    public List<GameObject> perkPrefabs;
    public GameObject selectedPerk;

    private int price = 1;
    // price tiers
    public int tier1 = 1;
    public int tier2 = 2;
    public int tier3 = 3;

    // private CoinCounter counter;
    private GameManager gameManager;

    public GameObject priceText;
    TextMeshPro pricetext;
    public GameObject perkImage;
    public GameObject perkName;


    private void Start()
    {
        // set the perk
        gameManager = GameManager.instance;
        selectRandomPerk();

        // set perk image
        SpriteRenderer shopSprite = perkImage.GetComponent<SpriteRenderer>();
        SpriteRenderer perkSprite = selectedPerk.GetComponent<SpriteRenderer>();
        shopSprite.sprite = perkSprite.sprite;

        // set perk name text
        if (PerkNames)
        {
            TextMeshPro perktext = perkName.GetComponent<TextMeshPro>();
            int perkID = selectedPerk.GetComponent<Perk>().GetPerkID();
            switch (perkID)
            {
                case 0: // Bounce
                    perktext.text = "Bouncing Projectiles";
                    price = tier1;
                    break;

                case 1: // Faster proj
                    perktext.text = "Faster Projectiles";
                    price = tier1;
                    break;

                case 2: // Slower Proj
                    perktext.text = "Slower Projectiles";
                    price = tier1;
                    break;

                case 3: // Projectile duration up
                    perktext.text = "More Projectile Duration";
                    price = tier1;
                    break;

                case 4: // Projectile duration down
                    perktext.text = "Less Projectile Duration";
                    price = tier1;
                    break;

                case 5: // Damage up
                    perktext.text = "Increase Damage";
                    price = tier1;
                    break;

                case 6: // 
                    perktext.text = "";
                    break;
                case 7: // Explosive
                    perktext.text = "Explosive Projectiles";
                    price = tier2;
                    break;
                case 8: // Size up
                    perktext.text = "Projectile Size Up";
                    price = tier1;
                    break;
                case 9: // Size down
                    perktext.text = "Projectile Size Down";
                    price = tier1;
                    break;
                case 10: // Burst shot
                    perktext.text = "Burst Shot";
                    price = tier1;
                    break;
                case 11: // Bite Powerup
                    perktext.text = "Bite Powerup";
                    price = tier3;
                    break;
                case 12: // Dash Powerup
                    perktext.text = "Dash Powerup";
                    price = tier3;
                    break;
                case 13: // Orbit Powerup
                    perktext.text = "Orbit Powerup";
                    price = tier3;
                    break;
                case 14: // Split Perk
                    perktext.text = "Projectile Split";
                    price = tier2;
                    break;
                case 15: // Homing Perk
                    perktext.text = "Homing Projectiles";
                    price = tier2;
                    break;
                case 16: // Boomerang Perk
                    perktext.text = "Boomerang Projectiles";
                    price = tier1;
                    break;
                case 17: // Wiggle Perk
                    perktext.text = "Wiggling Projectiles";
                    price = tier1;
                    break;

                default:
                    Debug.LogError("Applying undefined PerkID:");

                    break;
            }
        }
        else
        {
            perkName.SetActive(false);
        }

        // set perk price text
        pricetext = priceText.GetComponent<TextMeshPro>();
        pricetext.text = "" + price;
    }

    void Update()
    {
        if (price > gameManager.coinCount) { pricetext.color = Color.red; }
        else { pricetext.color = Color.white;}
    }

    private void selectRandomPerk()
    {
        if (perkPrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, perkPrefabs.Count);
            selectedPerk = perkPrefabs[randomIndex];
            if (randomIndex == 10 && Bite.canBite) { selectRandomPerk(); }
            if (randomIndex == 11 && StaticData.canDash) { selectRandomPerk(); }
            if (randomIndex == 12 && OrbitProjectiles.canOrbit) { selectRandomPerk(); }
        }
        else { Debug.LogError("No perk prefabs"); }
    }

    public void InstantiatePerk()
    {
        if (selectedPerk != null) {
            if (gameManager.coinCount >= price)
            {
                gameManager.DecreaseCoins(price);
                Vector2 offset = new Vector2(0, -1);
                Vector2 coords = (Vector2)transform.position + offset;
                Instantiate(selectedPerk, coords, transform.rotation);

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
            Debug.LogError("No selected perk to instantiate.");
        }
    }


}
