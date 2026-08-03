using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISnakeRespawn : MonoBehaviour
{
    [SerializeField] private SnakeCrown crown;
    private SnakeSpawner spawner;
    private SnakeCrownManager crownManager;
    
    public void Initialize(SnakeSpawner snakeSpawner, SnakeCrownManager manager)
    {
        spawner = snakeSpawner;
        crownManager = manager;
        GetComponent<SnakeHealth>()
            .OnDeath
            .AddListener(OnDeath);
    }

    void OnDeath(DeathData data)
    {
        spawner.RespawnAI();

        if (crown != null)
            crownManager.UnregisterSnake(crown);

        Destroy(gameObject);
    }


}
