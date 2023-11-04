using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<GameObject> perkPrefabs;
    public GameObject selectedPerk;

    public int price = 1;

    private CoinCounter counter;


    private void Start()
    {
        counter = GameManager.instance.GetComponent<CoinCounter>();
        selectRandomPerk();
    }

    private void selectRandomPerk()
    {
        if (perkPrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, perkPrefabs.Count);
            selectedPerk = perkPrefabs[randomIndex];
        }
        else { Debug.LogError("No perk prefabs"); }
    }

    public void InstantiatePerk()
    {
        if (selectedPerk != null) { 
            if (counter.coinCount >= price)
            {
                counter.DecreaseCoins(price);
                Instantiate(selectedPerk, transform.position, transform.rotation);
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
