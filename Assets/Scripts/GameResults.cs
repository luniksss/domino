public static class GameResults
{
    private static bool _levelCompleted;
    private static int _reward;

    public static void ReportLevelCompleted(int reward)
    {
        _levelCompleted = true;
        _reward = reward;
    }

    public static bool TryConsumeLevelCompleted(out int reward)
    {
        reward = _reward;
        if (!_levelCompleted) return false;
        _levelCompleted = false;
        _reward = 0;
        return true;
    }
}