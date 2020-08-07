using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Text foodText;
    public Text workersText;
    public Text coinsText;

    public int food;
    public int workers;
    public int coins;

    
    void Update()
    {
        foodText.text = "Food: " + food.ToString("0");
        workersText.text = "Workers: " +  workers.ToString("0");
        coinsText.text = "Coins: " +  coins.ToString("0");
    }
}
