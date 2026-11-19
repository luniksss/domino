using System.Collections.Generic;
using System;

[Serializable]
public class GameData
{
    public int coins = 0;
    public int currentLevel = 0;
    public int currentFieldSizeIndex = 0;

    public string currentBackgroundSkin = "bg_light";
    public string currentDominoSkin = "domino_light";

    public string levelStateJson = "";
    public bool hasSavedGame = false;
}
