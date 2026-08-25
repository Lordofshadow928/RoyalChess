using UnityEngine;
public class SnakeInvincible : MonoBehaviour
{
    //[SerializeField] private Collider snakeHeadCollider;

    public bool IsInvincible { get; private set; }

    public void EnableInvincible()
    {
        IsInvincible = true;

        //if (snakeHeadCollider != null)
        //{
        //    snakeHeadCollider.isTrigger = true;
        //    Debug.Log("HEAD COLLIDER = TRIGGER");
        //}
    }

    public void DisableInvincible()
    {
        IsInvincible = false;

        //if (snakeHeadCollider != null)
        //{
        //    snakeHeadCollider.isTrigger = false;
        //    Debug.Log("HEAD COLLIDER = SOLID");
        //}
    }
}

