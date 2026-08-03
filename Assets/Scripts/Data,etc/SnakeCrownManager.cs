using UnityEngine;
using System.Collections.Generic;

public class SnakeCrownManager : MonoBehaviour
{
    private List<SnakeCrown> snakes = new List<SnakeCrown>();
    private SnakeCrown currentLeader;

    public void Initialize()
    {
        snakes.Clear();
        SnakeCrown[] found = FindObjectsOfType<SnakeCrown>();
        foreach (SnakeCrown snake in found)
        {
            RegisterSnake(snake);
        }
        UpdateCrowns();
    }
    public void RegisterSnake(SnakeCrown snake)
    {
        if (snake == null)
            return;

        if (snakes.Contains(snake))
            return;

        snakes.Add(snake);

        snake.FoodStorage.OnFoodChanged += UpdateCrowns;
    }

    public void UnregisterSnake(SnakeCrown snake)
    {
        if (snake == null)
            return;

        if (!snakes.Remove(snake))
            return;

        snake.FoodStorage.OnFoodChanged -= UpdateCrowns;

        if (currentLeader == snake)
            currentLeader = null;

        UpdateCrowns();
    }

    private void OnDestroy()
    {
        if (snakes == null)
            return;

        foreach (SnakeCrown snake in snakes)
        {
            if (snake == null)
                continue;

            snake.FoodStorage.OnFoodChanged -= UpdateCrowns;
        }
    }

    private void UpdateCrowns()
    {
        if (currentLeader == null)
        {
            foreach (SnakeCrown snake in snakes.ToArray())
            {
                if (snake == null)
                    continue;
                if (snake.FoodStorage.StoredFood > 0)
                {
                    currentLeader = snake;
                    break;
                }
            }
        }

        foreach (SnakeCrown snake in snakes.ToArray())
        {
            if (snake == null)
                continue;
            if (currentLeader == null)
                continue;

            if (snake == currentLeader)
                continue;

            if (snake.FoodStorage.StoredFood >
                currentLeader.FoodStorage.StoredFood)
            {
                currentLeader = snake;
            }
        }

        foreach (SnakeCrown snake in snakes.ToArray())
        {
            if (snake == null)
                continue;
            snake.SetCrown(snake == currentLeader);
        }
    }
}