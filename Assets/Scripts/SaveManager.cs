using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string SaveKey = "domino_save_v1";

    public static SaveManager Instance { get; private set; }
    public GameData Data { get; private set; }
    public event Action<int> OnCoinsChanged;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (Instance != null) return;
        var go = new GameObject("[SaveManager]");
        go.AddComponent<SaveManager>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        Data.coins += amount;
        Save();
        OnCoinsChanged?.Invoke(Data.coins);
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0 || Data.coins < amount) return false;
        Data.coins -= amount;
        Save();
        OnCoinsChanged?.Invoke(Data.coins);
        return true;
    }

    public void StartNewRun(int fieldSizeIndex)
    {
        Data.currentFieldSizeIndex = fieldSizeIndex;
        Data.currentLevel = 1;
        Data.hasSavedGame = true;
        Save();
    }

    public int CompleteLevel()
    {
        var cfg = LevelConfig.FieldSizes[Data.currentFieldSizeIndex];
        int reward = cfg.rewardCoins;

        Data.coins += reward;
        Data.currentLevel++;
        Data.hasSavedGame = true;
        Save();
        OnCoinsChanged?.Invoke(Data.coins);
        return reward;
    }

    public bool IsBackgroundOwned(string id) => Data.unlockedBackgroundSkins.Contains(id);
    public bool IsTileOwned(string id)       => Data.unlockedTileSkins.Contains(id);

    public void UnlockBackground(string id)
    {
        if (!IsBackgroundOwned(id)) Data.unlockedBackgroundSkins.Add(id);
        Save();
    }

    public void UnlockTile(string id)
    {
        if (!IsTileOwned(id)) Data.unlockedTileSkins.Add(id);
        Save();
    }

    public void EquipBackground(string id)
    {
        if (!IsBackgroundOwned(id)) return;
        Data.selectedBackgroundSkin = id;
        Save();
    }

    public void EquipTile(string id)
    {
        if (!IsTileOwned(id)) return;
        Data.selectedTileSkin = id;
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(Data));
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            var json = PlayerPrefs.GetString(SaveKey);
            Data = JsonUtility.FromJson<GameData>(json) ?? new GameData();
        }
        else
        {
            Data = new GameData();
        }
    }
}