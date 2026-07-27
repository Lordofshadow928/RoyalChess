using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanel : ResultHandler
{
    [SerializeField] private GameObject panel;
    [SerializeField] private ResultFoodUI resultFoodUI;
    public override void HandleResult()
    {
        Time.timeScale = 0f;
        int food = resultFoodUI.Storage.StoredFood;
        int coins = CoinConverter.ConvertFoodToCoins(food);

        CoinManager.Instance.SetPendingReward(coins, LevelManager.Instance.CurrentStageIndex);
        resultFoodUI.ShowLose();
        panel.SetActive(true);
    }
}
