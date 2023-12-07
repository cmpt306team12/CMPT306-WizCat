using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnDestroy : MonoBehaviour
{
    public int tiersUsed;
    public string[] tierNames;
    public float[] weights;

    private GameObject[][] droppables;
    public GameObject[] tier1Drops;
    public GameObject[] tier2Drops;
    public GameObject[] tier3Drops;
    public GameObject[] tier4Drops;
    public GameObject[] tier5Drops;

    private void Start()
    {
        droppables = new GameObject[6][];
        droppables[0] = new GameObject[0];
        droppables[1] = tier1Drops;
        droppables[2] = tier2Drops;
        droppables[3] = tier3Drops;
        droppables[4] = tier4Drops;
        droppables[5] = tier5Drops;
    }

    public void Drop()
    {
        float totalWeight2 = 0f;
        for (int i = 0; i < tiersUsed; i++)
        {
            totalWeight2 += weights[i];
        }

        // Have total weight, now generate a random number to get tier of drop
        float p = Random.Range(0f, totalWeight2);
        float runningTotal = 0f;
        int selectedTier = -1;
        for (int i = 0; i < weights.Length; i++)
        {
            runningTotal += weights[i];
            if (p <= runningTotal)
            {
                selectedTier = i;
                break;
            }
        }

        // Selected tier of drop, now get an item to drop
        if (selectedTier == 0) // 0 tier is drop nothing
        {
            return;
        }
        else if (selectedTier == -1)
        {
            Debug.Log("Failed to select a loot item");
        } else
        {
            // actually drop an item
            int itemToDrop = Random.Range(0, droppables[selectedTier].Length);
            Debug.Log("Dropping: Tier: " + selectedTier + "Item: " + itemToDrop);
            Transform droppedTransform = gameObject.transform;
            droppedTransform.localScale = Vector3.one;
            GameObject loot = Instantiate(droppables[selectedTier][itemToDrop], droppedTransform);
            loot.transform.parent = null; // decouple from parent so that loot persists
            return;
        }
    }
}
