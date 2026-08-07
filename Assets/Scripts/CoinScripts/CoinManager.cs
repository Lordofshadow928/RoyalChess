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
    private const string FirstLaunchKey = "CoinSystemInitialized";
    private const int StartingCoins = 0;
    public event Action<int> OnCoinsChanged;

    public int Coins => PlayerPrefs.GetInt(CoinsKey, 0);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSave();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSave()
    {
        if (PlayerPrefs.HasKey(FirstLaunchKey))
            return;

        PlayerPrefs.SetInt(CoinsKey, StartingCoins);

        PlayerPrefs.SetInt(FirstLaunchKey, 1);
        PlayerPrefs.Save();
    }
    #region Coins

    public bool CanAfford(int amount)
    {
        return Coins >= amount;
    }

    public void AddCoins(int amount, int stageIndex)
    {
        SetPendingReward(amount, stageIndex);
    }

    public void SetPendingReward(int amount, int stageIndex)
    {
        if (amount <= 0)
            return;

        PlayerPrefs.SetInt(PendingCoinsKey, amount);
        PlayerPrefs.SetInt(PendingStageKey, stageIndex);
        PlayerPrefs.Save();
    }

    public void CommitCoins(int amount)
    {
        if (amount <= 0)
            return;

        int total = Coins + amount;
        PlayerPrefs.SetInt(CoinsKey, total);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(total);
    }

    public bool SpendCoins(int amount)
    {
        if (!CanAfford(amount))
            return false;

        int total = Coins - amount;

        PlayerPrefs.SetInt(CoinsKey, total);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(total);
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
        return new CoinReward(PlayerPrefs.GetInt(PendingCoinsKey, 0), PlayerPrefs.GetInt(PendingStageKey, 0));
    }

    public void ClearPendingReward()
    {
        PlayerPrefs.DeleteKey(PendingCoinsKey);
        PlayerPrefs.DeleteKey(PendingStageKey);
        PlayerPrefs.Save();
    }

    #endregion

    #region Debug

    [ContextMenu("Add 100000 Coins")]
    private void DebugAddCoins()
    {
        SetPendingReward(100000, 1);
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