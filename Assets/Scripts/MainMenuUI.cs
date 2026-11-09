using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Кнопки")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button shopButton;

    private void Awake()
    {
        if (startButton)    startButton.onClick.AddListener(OnStart);
        if (continueButton) continueButton.onClick.AddListener(OnContinue);
        if (shopButton)     shopButton.onClick.AddListener(() => MenuManager.Instance.ShowShop());
    }

    private void OnEnable()
    {
        if (continueButton != null)
            continueButton.interactable = SaveManager.Instance != null
                                          && SaveManager.Instance.Data.hasSavedGame;
    }

    private void OnStart()    => MenuManager.Instance.ShowLevelSelect();
    private void OnContinue() => GameLauncher.LoadGameScene();
}