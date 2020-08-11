using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Text treeText;
    public Text foodText;
    public Text workersText;
    public Text coinsText;

    public int tree;
    public int food;
    public int workers;
    public int coins;

    
    void Update()
    {
        treeText.text = "Trees: " + tree.ToString("0");
        foodText.text = "Food: " + food.ToString("0");
        workersText.text = "Workers: " +  workers.ToString("0");
        coinsText.text = "Coins: " +  coins.ToString("0");
    }
}
