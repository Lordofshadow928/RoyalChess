using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSpawner : MonoBehaviour
{
    [SerializeField] private FoodSpawner foodSpawner;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private DeathResultObserver deathObserver;
    [SerializeField] private ResultFoodUI resultFoodUI;
    [SerializeField] private SnakeProgressUI progressUI;
    [SerializeField] private LayerMask spawnBlockingLayer;
    private Transform player;
    private LevelData level;
    private MapData map;

    private void Start()
    {
        level = LevelManager.Instance.CurrentLevelData;
        map = LevelManager.Instance.CurrentMapData;

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(level.firstSpawnDelay);

        SpawnPlayer();

        for (int i = 0; i < level.aiCount; i++)
        {
            SpawnAI();
            yield return new WaitForSeconds(level.aiSpawnInterval);
        }
    }

    void SpawnPlayer()
    {
        player =Instantiate(level.playerPrefab, map.PlayerSpawnPoint.position, map.PlayerSpawnPoint.rotation).transform;
        foodSpawner.Initialize(level, map, player);
        cameraFollow.Initialize(player);
        deathObserver.Initialize(player.GetComponent<SnakeHealth>());
        resultFoodUI.Initialize(player.GetComponent<SnakeFoodStorage>());
        progressUI.Initialize(player.GetComponent<SnakeEnergy>());
    }
    Transform GetSafeBotSpawn()
    {
        Transform[] spawns = map.BotSpawnPoints;

        if (spawns.Length == 0)
            return null;

        List<Transform> validSpawns = new List<Transform>();

        foreach (Transform spawn in spawns)
        {
            bool occupied = Physics.CheckSphere(
                spawn.position,
                level.safeSpawnRadius,
                spawnBlockingLayer);

            if (!occupied)
                validSpawns.Add(spawn);
        }

        if (validSpawns.Count > 0)
        {
            return validSpawns[Random.Range(0, validSpawns.Count)];
        }

        return null;
    }
    public void SpawnAI()
    {
        Transform spawn = GetSafeBotSpawn();

        if (spawn == null)
        {
            StartCoroutine(RespawnRoutine());
            return;
        }

        GameObject snake = Instantiate(
            level.aiSnakePrefab,
            spawn.position,
            spawn.rotation);

        AISnakeRespawn respawn = snake.GetComponent<AISnakeRespawn>();

        if (respawn != null)
            respawn.Initialize(this);
    }

    public void RespawnAI()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(level.aiRespawnDelay);

        SpawnAI();
    }
}