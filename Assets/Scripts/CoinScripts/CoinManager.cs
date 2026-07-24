using System;
using UnityEngine;

[Serializable]
public struct CoinReward
{
    public int amount;
    public int stageIndex;

    public CoinReward(int amount, int stageIndex)
    {
        this.amount = amount;
        this.stageIndex = stageIndex;
    }
}

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    private const string CoinsKey = "Coins";
    private const string PendingCoinsKey = "PendingCoins";
    private const string PendingStageKey = "PendingStage";

    public event Action<int> OnCoinsChanged;

    public int Coins => PlayerPrefs.GetInt(CoinsKey, 0);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Coins

    public bool CanAfford(int amount)
    {
        return Coins >= amount;
    }

    public void AddCoins(int amount, int stageIndex)
    {
        if (amount <= 0)
            return;

        int total = Coins + amount;
        PlayerPrefs.SetInt(CoinsKey, total);
        // Save reward for menu animation
        PlayerPrefs.SetInt(PendingCoinsKey, amount);
        PlayerPrefs.SetInt(PendingStageKey, stageIndex);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(total);
        Debug.Log($"Earned {amount} coins. Total = {total}");
    }

    public bool SpendCoins(int amount)
    {
        if (!CanAfford(amount))
            return false;

        int total = Coins - amount;

        PlayerPrefs.SetInt(CoinsKey, total);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(total);
        Debug.Log($"Spent {amount} coins. Total = {total}");
        return true;
    }

    #endregion

    #region Pending Reward

    public bool HasPendingReward()
    {
        return PlayerPrefs.GetInt(PendingCoinsKey, 0) > 0;
    }

    public CoinReward GetPendingReward()
    {
        return new CoinReward( PlayerPrefs.GetInt(PendingCoinsKey, 0), PlayerPrefs.GetInt(PendingStageKey, 0));
    }

    public void ClearPendingReward()
    {
        PlayerPrefs.DeleteKey(PendingCoinsKey);
        PlayerPrefs.DeleteKey(PendingStageKey);
        PlayerPrefs.Save();
    }

    #endregion

    #region Debug

    [ContextMenu("Add 100 Coins")]
    private void DebugAddCoins()
    {
        AddCoins(100, 1);
    }

    [ContextMenu("Clear Coins")]
    private void DebugClearCoins()
    {
        PlayerPrefs.DeleteKey(CoinsKey);
        PlayerPrefs.DeleteKey(PendingCoinsKey);
        PlayerPrefs.DeleteKey(PendingStageKey);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(0);
        Debug.Log("Coins cleared.");
    }

    #endregion
}