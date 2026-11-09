using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DebugCheats : MonoBehaviour
{
    [SerializeField] private int coinsToAdd = 500;
    [SerializeField] private KeyCode addCoinsKey = KeyCode.F1;
    [SerializeField] private KeyCode resetSaveKey = KeyCode.F2;

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(addCoinsKey))
        {
            SaveManager.Instance.AddCoins(coinsToAdd);
            Debug.Log($"[Debug] +{coinsToAdd} монет. Баланс: {SaveManager.Instance.Data.coins}");
        }

        if (Input.GetKeyDown(resetSaveKey))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("[Debug] Сохранение сброшено. Перезапустите Play.");
        }
    }

    [MenuItem("Tools/Debug/Add 1000 Coins")]
    private static void MenuAddCoins()
    {
        if (Application.isPlaying && SaveManager.Instance != null)
            SaveManager.Instance.AddCoins(1000);
        else
            Debug.LogWarning("[Debug] Запустите Play — меню работает только в режиме игры.");
    }
#endif
}