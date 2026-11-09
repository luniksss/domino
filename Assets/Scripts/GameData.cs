using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int coins = 0;

    public int currentFieldSizeIndex = 0;
    public int currentLevel = 1;
    public bool hasSavedGame = false;

    public List<string> unlockedBackgroundSkins = new List<string> { "bg_light" };
    public string selectedBackgroundSkin = "bg_light";

    public List<string> unlockedTileSkins = new List<string> { "tile_light" };
    public string selectedTileSkin = "tile_light";
}