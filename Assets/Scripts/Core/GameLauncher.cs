using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameLauncher
{
    public const string GameSceneName = "Game";
    public const string MenuSceneName = "Menu";

    public static void StartNewGameScene()
    {
        PlayerPrefs.SetInt("IsContinueLoad", 0);
        SceneManager.LoadScene(GameSceneName);
    }

    public static void LoadGameScene()
    {
        PlayerPrefs.SetInt("IsContinueLoad", 1);
        SceneManager.LoadScene(GameSceneName);
    }

    public static void LoadMenuScene()
    {
        SceneManager.LoadScene(MenuSceneName);
    }

    public static void LoadMenuWithVictory(int reward)
    {
        PlayerPrefs.SetInt("LastReward", reward);
        PlayerPrefs.SetInt("ShowLevelComplete", 1);

        SceneManager.LoadScene(MenuSceneName);
    }

    public static void OnLevelWon()
    {
       
    }
}
