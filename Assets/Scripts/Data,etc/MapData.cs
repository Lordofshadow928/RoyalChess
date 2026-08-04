using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    [Header("Food")]
    [SerializeField] private Transform[] foodSpawnPoints;

    [Header("Snake")]
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform[] botSpawnPoints;

    [Header("Powerup")]
    [SerializeField] private Transform[] powerupSpawnPoints;

    public Transform[] FoodSpawnPoints => foodSpawnPoints;
    public Transform PlayerSpawnPoint => playerSpawnPoint;
    public Transform[] BotSpawnPoints => botSpawnPoints;
    public Transform[] PowerupSpawnPoints => powerupSpawnPoints;

    private void Awake()
    {
        FindFoodSpawns();
        FindSnakeSpawns();
        FindPowerupSpawns();
    }

    void FindFoodSpawns()
    {
        Transform root = transform.Find("FoodSpawnPos");

        if (root == null)
        {
            foodSpawnPoints = new Transform[0];
            return;
        }

        List<Transform> list = new List<Transform>();

        foreach (Transform child in root)
            list.Add(child);

        foodSpawnPoints = list.ToArray();
    }

    void FindSnakeSpawns()
    {
        Transform root = transform.Find("SnakeSpawnPos");

        if (root == null)
        {
            playerSpawnPoint = null;
            botSpawnPoints = new Transform[0];
            return;
        }

        List<Transform> bots = new List<Transform>();

        foreach (Transform child in root)
        {
            if (child.name == "PlayerSpawn")
                playerSpawnPoint = child;
            else if (child.name.StartsWith("BotSpawn"))
                bots.Add(child);
        }
        
        botSpawnPoints = bots.ToArray();
    }

    void FindPowerupSpawns()
    {
        Transform root = transform.Find("PowerupSpawnPos");

        if (root == null)
        {
            powerupSpawnPoints = new Transform[0];
            return;
        }
        if (root.childCount == 0)
        {
            powerupSpawnPoints = new Transform[] { root };
            return;
        }
        List<Transform> list = new();

        foreach (Transform child in root)
            list.Add(child);

        powerupSpawnPoints = list.ToArray();
    }

    public Transform GetRandomFoodSpawn()
    {
        if (foodSpawnPoints.Length == 0)
            return null;

        return foodSpawnPoints[Random.Range(0, foodSpawnPoints.Length)];
    }

    public Transform GetRandomBotSpawn()
    {
        if (botSpawnPoints.Length == 0)
            return null;

        return botSpawnPoints[Random.Range(0, botSpawnPoints.Length)];
    }

    public Transform GetRandomPowerupSpawn()
    {
        if (powerupSpawnPoints.Length == 0)
            return null;

        return powerupSpawnPoints[Random.Range(0, powerupSpawnPoints.Length)];
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Transform t in foodSpawnPoints)
        {
            if (t != null)
                Gizmos.DrawSphere(t.position, 0.15f);
        }

        Gizmos.color = Color.blue;
        foreach (Transform t in botSpawnPoints)
        {
            if (t != null)
                Gizmos.DrawCube(t.position, Vector3.one * 0.3f);
        }

        Gizmos.color = Color.cyan;
        foreach (Transform t in powerupSpawnPoints)
        {
            if (t != null)
                Gizmos.DrawSphere(t.position, 0.25f);
        }

        if (playerSpawnPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(playerSpawnPoint.position, Vector3.one * 0.4f);
        }
    }
#endif
}