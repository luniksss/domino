public static class GameLauncher
{
    public const string GameSceneName = "Game";
    public const string MenuSceneName = "Menu";

    public static void LoadGameScene()  => UnityEngine.SceneManagement.SceneManager.LoadScene(GameSceneName);
    public static void LoadMenuScene()  => UnityEngine.SceneManagement.SceneManager.LoadScene(MenuSceneName);

    public static void OnLevelWon()
    {
        int reward = SaveManager.Instance.CompleteLevel();
        GameResults.ReportLevelCompleted(reward);
        LoadMenuScene();
    }
}