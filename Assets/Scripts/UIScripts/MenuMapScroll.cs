using UnityEngine;
using System.Collections;
public class MenuMapScroll : MonoBehaviour
{
    [SerializeField] private MenuProgressManager progressManager;
    [SerializeField] private SnakeUITeleport snakeTeleportEffect;
    [SerializeField] private MenuIsland[] islands;
    [SerializeField] private RequirementUI requirementUI;

    private int currentLevel;
    private int previousLevel;

    [SerializeField] private float scrollSpeed = 0.1f;
    [SerializeField] private float minZ = -5f;
    [SerializeField] private float maxZ = 210f;
    [SerializeField] private float smoothSpeed = 4f;

    [SerializeField] private float stepSize = 15f;
    [SerializeField] private float snapOffset = -5f;

    private Vector3 lastMousePos;
    private float targetZ;
    private bool waitingToShowRequirement;

    private IEnumerator Start()
    {
        yield return null;

        currentLevel = Mathf.Clamp(
            StageSelection.SelectedStage - 1,
            0,
            progressManager.MaxReachableIndex);

        previousLevel = currentLevel;

        targetZ = currentLevel * stepSize + snapOffset;

        Vector3 pos = transform.position;
        pos.z = targetZ;
        transform.position = pos;

        snakeTeleportEffect.SetPosition(islands[currentLevel]);

        if (requirementUI != null)
        {
            if (islands[currentLevel].IsLocked)
                requirementUI.Show(islands[currentLevel]);
            else
                requirementUI.Hide();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;

            if (requirementUI != null)
                requirementUI.Hide();
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;

            targetZ -= delta.y * scrollSpeed;
            targetZ = Mathf.Clamp(targetZ, minZ, maxZ);

            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            float level = Mathf.Round((targetZ - snapOffset) / stepSize);
            currentLevel = Mathf.Clamp((int)level, 0, progressManager.MaxReachableIndex);

            targetZ = currentLevel * stepSize + snapOffset;
            targetZ = Mathf.Clamp(targetZ, minZ, maxZ);

            if (requirementUI != null)
                requirementUI.Hide();

            waitingToShowRequirement = true;

            if (currentLevel != previousLevel)
            {
                if (snakeTeleportEffect.TeleportTo(islands[currentLevel]))
                {
                    previousLevel = currentLevel;
                }
            }
        }

        Vector3 pos = transform.position;
        pos.z = Mathf.Lerp(pos.z, targetZ, Time.deltaTime * smoothSpeed);
        transform.position = pos;

        if (waitingToShowRequirement && Mathf.Abs(transform.position.z - targetZ) < 0.05f)
        {
            waitingToShowRequirement = false;

            if (islands[currentLevel].IsLocked)
            {
                requirementUI.Show(islands[currentLevel]);
            }
            else
            {
                requirementUI.Hide();
            }
        }
    }
    public int CurrentStageIndex
    {
        get
        {
            return islands[currentLevel].StageData.stageIndex;
        }
    }
    public bool CurrentIslandLocked
    {
        get
        {
            return islands[currentLevel].IsLocked;
        }
    }
}