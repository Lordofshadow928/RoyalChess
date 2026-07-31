using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopEquipController : MonoBehaviour
{
    [SerializeField] private ShopSelectionManager selectionManager;

    [Header("UI")]
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unlockButton;
    [SerializeField] private Image coinIcon;
    [SerializeField] private TMP_Text equipText;
    [SerializeField] private TMP_Text unlockText;
    [SerializeField] private TMP_Text coinPriceText;
    [SerializeField] private SnakeUIPreview menuPreview;
    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        SkinButton selected = selectionManager.CurrentSelection;
        
        if (selected == null)
        {
            Debug.Log("CurrentSelection = NULL");
            return;
        }
        SnakeSkinData skin = selected.SkinData;

        if (!PlayerSkinManager.Instance.IsUnlocked(skin.skinID))
        {
            equipButton.interactable = false;
            equipText.text = "LOCKED";
            return;
        }

        if (PlayerSkinManager.Instance.IsEquipped(skin.skinID))
        {
            equipButton.interactable = false;
            equipText.text = "EQUIPPED";
            return;
        }
        equipButton.interactable = true;
        equipText.text = "EQUIP";
    }

    public void EquipSelected()
    {
        SkinButton selected = selectionManager.CurrentSelection;

        if (selected == null)
            return;

        PlayerSkinManager.Instance.EquipSkin(selected.SkinData.skinID);
        menuPreview.ShowSkin(PlayerSkinManager.Instance.EquippedSkin);
        Refresh();
    }
}