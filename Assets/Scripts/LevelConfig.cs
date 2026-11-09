using System;

[Serializable]
public class FieldSizeConfig
{
    public string displayName;
    public int width;
    public int height;
    public int rewardCoins;
}

public static class LevelConfig
{
    public static readonly FieldSizeConfig[] FieldSizes = new[]
    {
        new FieldSizeConfig { displayName = "5 × 4", width = 5, height = 4, rewardCoins = 10 },
        new FieldSizeConfig { displayName = "6 × 5", width = 6, height = 5, rewardCoins = 20 },
        new FieldSizeConfig { displayName = "7 × 6", width = 7, height = 6, rewardCoins = 35 },
        new FieldSizeConfig { displayName = "8 × 7", width = 8, height = 7, rewardCoins = 50 },
    };
}