using UnityEngine;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public GameData Data { get; private set; }

    public event Action<int> OnCoinsChanged;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (Instance != null)
        {
            return;
        }
        var go = new GameObject("[SaveManager]");
        go.AddComponent<SaveManager>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject);
            return; 
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetString("game_data", JsonUtility.ToJson(Data));
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("game_data"))
        {
            Data = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString("game_data"));
        }
        else
        {
            Data = new GameData();
        }
        OnCoinsChanged?.Invoke(Data.coins);
    }

    public void StartNewRun(int fieldSizeIndex)
    {
        Data.currentFieldSizeIndex = fieldSizeIndex;
        Data.currentLevel = 1;
        Data.hasSavedGame = true;
        Data.levelStateJson = "";
        Save();
    }

    public void AddCoins(int amount)
    {
        Data.coins += amount;
        Save();
        OnCoinsChanged?.Invoke(Data.coins);
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0 || Data.coins < amount)
        {
            return false;
        }

        Data.coins -= amount;
        Save();
        OnCoinsChanged?.Invoke(Data.coins);
        return true;
    }
}