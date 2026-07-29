using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    private LevelData level;
    private MapData map;

    private readonly List<GameObject> activePowerups = new();

    public void Initialize(LevelData levelData, MapData mapData)
    {
        level = levelData;
        map = mapData;

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(level.powerupSpawnInterval);

            CleanupDestroyedPowerups();

            if (activePowerups.Count >= level.maxPowerups)
                continue;

            SpawnPowerup();
        }
    }

    void SpawnPowerup()
    {
        if (level.powerupPrefabs == null || level.powerupPrefabs.Length == 0)
            return;

        Transform spawn = map.GetRandomPowerupSpawn();

        if (spawn == null)
            return;

        GameObject prefab =
            level.powerupPrefabs[Random.Range(0, level.powerupPrefabs.Length)];

        GameObject powerup = Instantiate(
            prefab,
            spawn.position,
            spawn.rotation);

        activePowerups.Add(powerup);
    }

    void CleanupDestroyedPowerups()
    {
        activePowerups.RemoveAll(powerup => powerup == null);
    }
}