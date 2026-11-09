using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class CoinDisplay : MonoBehaviour
{
    private TMP_Text _label;
    private void Awake() => _label = GetComponent<TMP_Text>();

    private void OnEnable()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.OnCoinsChanged += Refresh;
            Refresh(SaveManager.Instance.Data.coins);
        }
    }

    private void OnDisable()
    {
        if (SaveManager.Instance != null)
            SaveManager.Instance.OnCoinsChanged -= Refresh;
    }

    private void Refresh(int coins)
    {
        _label.text = coins.ToString("N0").Replace(',', ' ').Replace('\u00A0', ' ');
    }
}