using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ShopCategory { Background, Tile }

public class ShopItemUI : MonoBehaviour
{
    [Header("Идентификация")]
    [SerializeField] private ShopCategory category;
    [SerializeField] private string itemId;
    [SerializeField] private string displayName;
    [SerializeField] private int price;

    [Header("UI внутри item")]
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private GameObject currentItemText;
    [SerializeField] private Button useItemButton;
    [SerializeField] private Button buyItemButton;

    private ShopUI _shopUI;

    private void Awake()
    {
        _shopUI = GetComponentInParent<ShopUI>();
        if (useItemButton) useItemButton.onClick.AddListener(OnUseClicked);
        if (buyItemButton) buyItemButton.onClick.AddListener(OnBuyClicked);
    }

    private void OnUseClicked()
    {
        var sm = SaveManager.Instance;
        if (!IsOwned(sm.Data)) return;

        Equip(sm);
        if (_shopUI != null) _shopUI.RefreshAll();
    }

    private void OnBuyClicked()
    {
        var sm = SaveManager.Instance;
        if (IsOwned(sm.Data)) return;
        if (!sm.SpendCoins(price)) return;

        Unlock(sm);
        if (_shopUI != null) _shopUI.RefreshAll();
    }

    public void Refresh()
    {
        var data = SaveManager.Instance.Data;

        if (itemNameText) itemNameText.text = displayName;

        bool owned    = IsOwned(data);
        bool equipped = IsEquipped(data);

        if (priceText) priceText.text = owned ? "" : price.ToString();
        if (currentItemText) currentItemText.SetActive(false);
        if (useItemButton)   useItemButton.gameObject.SetActive(false);
        if (buyItemButton)   buyItemButton.gameObject.SetActive(false);

        if (equipped)
        {
            if (currentItemText) currentItemText.SetActive(true);
        }
        else if (owned)
        {
            if (useItemButton) useItemButton.gameObject.SetActive(true);
        }
        else
        {
            if (buyItemButton) buyItemButton.gameObject.SetActive(true);
        }
    }

    private bool IsOwned(GameData data) =>
        category == ShopCategory.Background
            ? data.unlockedBackgroundSkins.Contains(itemId)
            : data.unlockedTileSkins.Contains(itemId);

    private bool IsEquipped(GameData data) =>
        category == ShopCategory.Background
            ? data.selectedBackgroundSkin == itemId
            : data.selectedTileSkin == itemId;

    private void Unlock(SaveManager sm)
    {
        if (category == ShopCategory.Background) sm.UnlockBackground(itemId);
        else                                     sm.UnlockTile(itemId);
    }

    private void Equip(SaveManager sm)
    {
        if (category == ShopCategory.Background) sm.EquipBackground(itemId);
        else                                     sm.EquipTile(itemId);
    }
}