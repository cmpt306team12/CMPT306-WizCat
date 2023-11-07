using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public TMP_Text scoreText;
    private int scoreCount = 0;

    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        scoreText.text = "Score: " + scoreCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public void IncreaseScore(int amount){
        scoreCount += amount;
        scoreText.text = "Score: " + scoreCount.ToString();
    }

}
