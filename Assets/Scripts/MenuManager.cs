using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Панели")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject levelSelectPanel;
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
        if (PlayerPrefs.GetInt("ShowLevelComplete", 0) == 1)
        {
            PlayerPrefs.SetInt("ShowLevelComplete", 0);

            int reward = PlayerPrefs.GetInt("LastReward", 0);
            ShowLevelComplete(reward);
        }
        else
        {
            ShowMain();
        }
    }

    public void ShowMain()                => ShowOnly(mainPanel);
    public void ShowLevelSelect()         => ShowOnly(levelSelectPanel);

    public void ShowLevelComplete(int rewardCoins)
    {
        ShowOnly(levelCompletePanel);
        if (levelCompleteUI != null) levelCompleteUI.Show(rewardCoins);
    }

    private void ShowOnly(GameObject target)
    {
        if (mainPanel)          mainPanel.SetActive(target == mainPanel);
        if (levelSelectPanel)   levelSelectPanel.SetActive(target == levelSelectPanel);
        if (levelCompletePanel) levelCompletePanel.SetActive(target == levelCompletePanel);
    }
}