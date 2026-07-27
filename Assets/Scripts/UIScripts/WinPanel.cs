using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : ResultHandler
{
    [SerializeField] private GameObject panel;
    [SerializeField] private ResultFoodUI resultFoodUI;
    public override void HandleResult()
    {
        Time.timeScale = 0f;
        int food = resultFoodUI.Storage.StoredFood;
        int coins = CoinConverter.ConvertFoodToCoins(food);

        CoinManager.Instance.SetPendingReward(coins, LevelManager.Instance.CurrentStageIndex);
        resultFoodUI.ShowWin();
        panel.SetActive(true);
    }
}
