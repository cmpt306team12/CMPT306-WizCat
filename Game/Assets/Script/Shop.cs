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

    public int price = 1;

    // private CoinCounter counter;
    private GameManager gameManager;

    public GameObject priceText;
    public GameObject perkImage;
    public GameObject perkName;


    private void Start()
    {
        // set the perk
        // counter = GameManager.instance.GetComponent<CoinCounter>();
        gameManager = GameManager.instance;
        selectRandomPerk();

        // set perk price text
        TextMeshPro pricetext = priceText.GetComponent<TextMeshPro>();
        pricetext.text = "" + price;

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
                    break;

                case 1: // Faster proj
                    perktext.text = "Faster Projectiles";
                    break;

                case 2: // Slower Proj
                    perktext.text = "Slower Projectiles";
                    break;

                case 3: // Projectile duration up
                    perktext.text = "More Projectile Duration";
                    break;

                case 4: // Projectile duration down
                    perktext.text = "Less Projectile Duration";
                    break;

                case 5: // Damage up
                    perktext.text = "Increase Damage";
                    break;

                case 6: // 
                    perktext.text = "";
                    break;
                case 7: // Explosive
                    perktext.text = "Explosive Projectiles";
                    break;
                case 8: // Size up
                    perktext.text = "Projectile Size Up";
                    break;
                case 9: // Size down
                    perktext.text = "Projectile Size Down";
                    break;
                case 10: // Burst shot
                    perktext.text = "Burst Shot";
                    break;
                case 11: // Bite Powerup
                    perktext.text = "Bite Powerup";
                    break;
                case 12: // Dash Powerup
                    perktext.text = "Dash Powerup";
                    break;
                case 13: // Orbit Powerup
                    perktext.text = "Orbit Powerup";
                    break;
                case 14: // Split Perk
                    perktext.text = "Projectile Split";
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
            // if (counter.coinCount >= price)
            if (gameManager.coinCount >= price)
            {
                // counter.DecreaseCoins(price);
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
