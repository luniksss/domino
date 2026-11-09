using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteUI : MonoBehaviour
{
    [Header("Тексты")]
    [SerializeField] private TMP_Text winText;         // WinText
    [SerializeField] private TMP_Text coinsAmountText; // LevelAwardPanel/CoinsAmountText
    [SerializeField] private TMP_Text addSignText;     // LevelAwardPanel/AddSign (опционально)

    [Header("Кнопки")]
    [SerializeField] private Button continueButton;    // GameButtons/ContinueButton — «Далее»
    [SerializeField] private Button startButton;       // GameButtons/StartButton — «На главную»

    [Header("Навигация (TopBar)")]
    [SerializeField] private Button backButton;        // BackButton
    [SerializeField] private Button shopButton;        // ShopButton

    private void Awake()
    {
        if (continueButton) continueButton.onClick.AddListener(OnContinue);
        if (startButton)    startButton.onClick.AddListener(OnHome);
        if (backButton)     backButton.onClick.AddListener(OnHome);
        if (shopButton)     shopButton.onClick.AddListener(() => MenuManager.Instance.ShowShop());
    }

    public void Show(int rewardCoins)
    {
        var data = SaveManager.Instance.Data;
        int completedLevel = Mathf.Max(1, data.currentLevel - 1);

        if (winText != null)
            winText.text = $"Уровень {completedLevel} пройден!";

        if (coinsAmountText != null)
            coinsAmountText.text = rewardCoins.ToString();

        if (addSignText != null)
            addSignText.text = "+";
    }

    private void OnContinue()
    {
        GameLauncher.LoadGameScene();
    }

    private void OnHome()
    {
        MenuManager.Instance.ShowMain();
    }
}