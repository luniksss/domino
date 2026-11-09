using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Навигация (TopBar)")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button shopButton;

    [Header("Категории")]
    [SerializeField] private Button backgroundCategoryButton;
    [SerializeField] private Button tileCategoryButton;
    [SerializeField] private GameObject backgroundItemsPanel;
    [SerializeField] private GameObject tileItemsPanel;

    private ShopItemUI[] _allItems;

    private void Awake()
    {
        _allItems = GetComponentsInChildren<ShopItemUI>(true);

        if (backButton) backButton.onClick.AddListener(() => MenuManager.Instance.ShowMain());
        if (shopButton) shopButton.onClick.AddListener(() => MenuManager.Instance.ShowMain());
        if (backgroundCategoryButton) backgroundCategoryButton.onClick.AddListener(() => ShowCategory(ShopCategory.Background));
        if (tileCategoryButton)       tileCategoryButton.onClick.AddListener(() => ShowCategory(ShopCategory.Tile));
    }

    private void OnEnable()
    {
        ShowCategory(ShopCategory.Background);
        RefreshAll();
    }

    public void ShowCategory(ShopCategory cat)
    {
        if (backgroundItemsPanel) backgroundItemsPanel.SetActive(cat == ShopCategory.Background);
        if (tileItemsPanel)       tileItemsPanel.SetActive(cat == ShopCategory.Tile);
    }

    public void RefreshAll()
    {
        if (_allItems == null || _allItems.Length == 0)
            _allItems = GetComponentsInChildren<ShopItemUI>(true);

        foreach (var item in _allItems) item.Refresh();
    }
}