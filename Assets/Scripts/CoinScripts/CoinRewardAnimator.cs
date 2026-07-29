using System;
using System.Collections;
using UnityEngine;

public class CoinRewardAnimator : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform coinPrefab;
    [SerializeField] private RectTransform coinSpawnPoint;
    [SerializeField] private RectTransform coinTargetPoint;
    [SerializeField] private RectTransform coinContainer;

    [Header("Animation")]
    [SerializeField] private float spawnInterval = 0.04f;
    [SerializeField] private float flyDuration = 0.45f;
    [SerializeField] private float arcHeight = 140f;

    private struct AnimationPlan
    {
        public int flyingCoins;
        public int[] increments;
    }
    private bool isPlaying;

    private void Start()
    {
        TryPlayPendingReward();
    }

    public void TryPlayPendingReward()
    {
        if (isPlaying)
            return;

        if (CoinManager.Instance == null)
            return;

        if (!CoinManager.Instance.HasPendingReward())
            return;

        StartCoroutine(PlayPendingRewardRoutine());
    }
    private AnimationPlan BuildAnimationPlan(int reward)
    {
        AnimationPlan plan = new AnimationPlan();

        // 1 - 10 coins: fly every coin
        if (reward <= 10)
        {
            plan.flyingCoins = reward;
            plan.increments = new int[reward];

            for (int i = 0; i < reward; i++)
                plan.increments[i] = 1;

            return plan;
        }

        // 11 - 40 coins: always fly 20 coins
        plan.flyingCoins = 20;
        plan.increments = new int[20];

        int baseValue = reward / 20;
        int remainder = reward % 20;

        // Everyone gets the base value
        for (int i = 0; i < 20; i++)
            plan.increments[i] = baseValue;

        // Spread the remaining coins over the first few flying coins
        for (int i = 0; i < remainder; i++)
            plan.increments[i]++;

        return plan;
    }
    private IEnumerator PlayPendingRewardRoutine()
    {
        isPlaying = true;

        CoinReward reward = CoinManager.Instance.GetPendingReward();

        if (reward.amount <= 0)
        {
            CoinManager.Instance.ClearPendingReward();
            isPlaying = false;
            yield break;
        }

        AnimationPlan plan = BuildAnimationPlan(reward.amount);

        int activeCoins = 0;

        for (int i = 0; i < plan.flyingCoins; i++)
        {
            int value = plan.increments[i];

            activeCoins++;
            StartCoroutine(FlyOneCoin(value, () => activeCoins--));

            yield return new WaitForSecondsRealtime(spawnInterval);
        }

        while (activeCoins > 0)
            yield return null;

        CoinManager.Instance.ClearPendingReward();
        isPlaying = false;
    }

    private IEnumerator FlyOneCoin(int coinValue, Action onFinished)
    {
        RectTransform coin = Instantiate(coinPrefab, coinContainer);

        coin.position = coinSpawnPoint.position;
        coin.localScale = Vector3.one;

        Vector3 start = coinSpawnPoint.position;
        Vector3 end = coinTargetPoint.position;
        Vector3 control = (start + end) * 0.5f + Vector3.up * arcHeight;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / flyDuration;

            float u = 1f - t;

            coin.position = (u * u * start) + (2f * u * t * control) + (t * t * end);
            yield return null;
        }

        coin.position = end;

        CoinManager.Instance.CommitCoins(coinValue);

        Destroy(coin.gameObject);

        onFinished?.Invoke();
    }
}