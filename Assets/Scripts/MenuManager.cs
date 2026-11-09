using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Панели")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject levelCompletePanel;

    [Header("Контроллеры")]
    [SerializeField] private LevelCompleteUI levelCompleteUI;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        if (GameResults.TryConsumeLevelCompleted(out int reward))
            ShowLevelComplete(reward);
        else
            ShowMain();
    }

    public void ShowMain()                => ShowOnly(mainPanel);
    public void ShowLevelSelect()         => ShowOnly(levelSelectPanel);
    public void ShowShop()                => ShowOnly(shopPanel);

    public void ShowLevelComplete(int rewardCoins)
    {
        ShowOnly(levelCompletePanel);
        if (levelCompleteUI != null) levelCompleteUI.Show(rewardCoins);
    }

    private void ShowOnly(GameObject target)
    {
        if (mainPanel)          mainPanel.SetActive(target == mainPanel);
        if (levelSelectPanel)   levelSelectPanel.SetActive(target == levelSelectPanel);
        if (shopPanel)          shopPanel.SetActive(target == shopPanel);
        if (levelCompletePanel) levelCompletePanel.SetActive(target == levelCompletePanel);
    }
}