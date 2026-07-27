using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultFoodUI : MonoBehaviour
{
    [Header("Win")]
    [SerializeField] private TMP_Text winFoodText;
    [SerializeField] private TMP_Text winCoinText;

    [Header("Lose")]
    [SerializeField] private TMP_Text loseFoodText;
    [SerializeField] private TMP_Text loseCoinText;
    private SnakeFoodStorage storage;
    public SnakeFoodStorage Storage => storage;
    public void Initialize(SnakeFoodStorage playerStorage)
    {
        storage = playerStorage;
    }
    public void ShowWin()
    {
        int food = storage.StoredFood;
        int coins = CoinConverter.ConvertFoodToCoins(food);

        winFoodText.text = food.ToString();
        winCoinText.text = coins.ToString();
    }

    public void ShowLose()
    {
        int food = storage.StoredFood;
        int coins = CoinConverter.ConvertFoodToCoins(food);

        loseFoodText.text = food.ToString();
        loseCoinText.text = coins.ToString();
    }
}
