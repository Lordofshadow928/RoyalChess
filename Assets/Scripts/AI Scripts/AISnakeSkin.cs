using UnityEngine;

public class AISnakeSkin : MonoBehaviour
{
    [SerializeField] private SnakeSkinDatabase database;

    private GameObject currentHead;

    private void Awake()
    {
        SnakeSkinController controller = GetComponent<SnakeSkinController>();

        SnakeSkinData selectedSkin = database.GetRandomAISkin();

        if (selectedSkin == null)
            return;

        controller.ApplySkin(selectedSkin);

        currentHead = Instantiate(selectedSkin.headPrefab, transform);

        currentHead.transform.localPosition = Vector3.zero;
        currentHead.transform.localRotation = Quaternion.identity;
        currentHead.transform.localScale = Vector3.one;
    }
}