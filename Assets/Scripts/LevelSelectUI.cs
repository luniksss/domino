using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    [Header("4 кнопки размеров поля (порядок = LevelConfig.FieldSizes)")]
    [Tooltip("0 = 5×4 (Easy), 1 = 6×5 (Medium), 2 = 7×6 (Hard), 3 = 8×7 (Expert)")]
    [SerializeField] private Button[] sizeButtons;

    [Header("Навигация")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button shopButton;

    private void Awake()
    {
        if (sizeButtons != null)
        {
            for (int i = 0; i < sizeButtons.Length; i++)
            {
                if (sizeButtons[i] == null) continue;
                int idx = i;
                sizeButtons[i].onClick.AddListener(() => OnSizeSelected(idx));
            }
        }

        if (backButton) backButton.onClick.AddListener(() => MenuManager.Instance.ShowMain());
        if (shopButton) shopButton.onClick.AddListener(() => MenuManager.Instance.ShowShop());
    }

    private void OnSizeSelected(int fieldSizeIndex)
    {
        if (fieldSizeIndex < 0 || fieldSizeIndex >= LevelConfig.FieldSizes.Length) return;
        SaveManager.Instance.StartNewRun(fieldSizeIndex);
        GameLauncher.LoadGameScene();
    }
}