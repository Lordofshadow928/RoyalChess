using UnityEngine;

public class AISnakeBrain : MonoBehaviour
{
    [Header("Steering")]
    [SerializeField] private float steeringSensitivity = 120f;
    [SerializeField] private float steeringSmoothness = 2f;
    [SerializeField] private float steeringDeadZone = 12f;

    [Header("Targeting")]
    [SerializeField] private float targetRefreshRate = 0.75f;

    [Header("Target Stuck Detection")]
    [SerializeField] private float stuckDistance = 2f;
    [SerializeField] private float stuckTime = 1f;
    [SerializeField] private float rejectedFoodCooldown = 2f;

    private SnakeMovement movement;
    private AISnakeObstacleSensor sensor;

    private Transform currentTarget;
    private Transform rejectedTarget;

    private float targetTimer;
    private float stuckTimer;
    private float rejectedTargetTimer;

    private void Awake()
    {
        movement = GetComponentInParent<SnakeMovement>();
        sensor = GetComponentInParent<AISnakeObstacleSensor>();
    }

    private void FixedUpdate()
    {
        if (FoodManager.Instance == null)
            return;

        UpdateRejectedTargetCooldown();

        UpdateTarget();

        if (currentTarget == null)
        {
            movement.SteeringInput = 0f;
            return;
        }

        CheckIfStuck();

        if (currentTarget == null)
        {
            movement.SteeringInput = 0f;
            return;
        }

        SteerTowardsTarget();
    }

    private void UpdateRejectedTargetCooldown()
    {
        if (rejectedTarget == null)
            return;

        rejectedTargetTimer += Time.fixedDeltaTime;

        if (rejectedTargetTimer >= rejectedFoodCooldown)
        {
            rejectedTarget = null;
            rejectedTargetTimer = 0f;
        }
    }

    private void UpdateTarget()
    {
        targetTimer += Time.fixedDeltaTime;

        if (currentTarget == null || targetTimer >= targetRefreshRate)
        {
            targetTimer = 0f;

            currentTarget = FoodManager.Instance.GetNearestFood(transform.position, rejectedTarget);

            stuckTimer = 0f;
        }
    }

    private void CheckIfStuck()
    {
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

        if (distanceToTarget <= stuckDistance)
        {
            stuckTimer += Time.fixedDeltaTime;

            if (stuckTimer >= stuckTime)
            {
                RejectCurrentTarget();
            }
        }
        else
        {
            stuckTimer = 0f;
        }
    }

    private void RejectCurrentTarget()
    {
        rejectedTarget = currentTarget;
        rejectedTargetTimer = 0f;

        currentTarget = FoodManager.Instance.GetNearestFood(transform.position, rejectedTarget);

        targetTimer = 0f;
        stuckTimer = 0f;
    }

    private void SteerTowardsTarget()
    {
        Vector3 foodDirection = (currentTarget.position - transform.position).normalized;

        Vector3 finalDirection = foodDirection;

        if (sensor != null && sensor.AvoidanceDirection.sqrMagnitude > 0.1f)
        {
            finalDirection += sensor.AvoidanceDirection;
        }

        finalDirection.y = 0f;

        if (finalDirection.sqrMagnitude < 0.01f)
            return;

        finalDirection.Normalize();

        float angle = Vector3.SignedAngle(transform.forward, finalDirection, Vector3.up);

        float targetSteering = 0f;

        if (Mathf.Abs(angle) > steeringDeadZone)
        {
            targetSteering = Mathf.Clamp( angle / steeringSensitivity, -1f, 1f);
        }

        movement.SteeringInput = Mathf.Lerp(movement.SteeringInput, targetSteering, steeringSmoothness * Time.fixedDeltaTime);
    }
}