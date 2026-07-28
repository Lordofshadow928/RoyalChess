using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCountTMP : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    private void Start()
    {
        coinText.text = CoinManager.Instance.Coins.ToString();
        CoinManager.Instance.OnCoinsChanged += UpdateCoins;
    }

    private void OnDestroy()
    {
        if (CoinManager.Instance != null)
            CoinManager.Instance.OnCoinsChanged -= UpdateCoins;
    }

    private void UpdateCoins(int totalCoins)
    {
        coinText.text = totalCoins.ToString();
    }
}

