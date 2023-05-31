using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticConstants
{
    /// <summary>
    /// contains the colors associated with every team
    /// <summary>
    public static IDictionary<int, Color32> teamColors = new Dictionary<int, Color32>()
    {
        // blue
        {0, new Color32(47, 86, 243, 255)},
        // green
        {1, new Color32(18, 238, 15, 255)},
        // orange
        {2, new Color32(255, 146, 0, 255)},
        // purple
        {3, new Color32(146, 0, 255, 255)},
        // red
        {4, new Color32(255, 0, 8, 255)},
        // yellow
        {5, new Color32(255, 254, 18, 255)}
    };

    /// <summary>
    /// hashtable keys stored to avoid typos/mis-references
    /// <summary>
        // game settings
    public const string teamNumberHashmapKey = "TeamNumber";
    public const string warmupLengthKey = "WarmupLength";
    public const string gameLengthKey = "GameLength";
    public const string pointsPerScoreKey = "PointsPerScore";
        // preferences
    public const string regionPrefKey = "RegionPreference";
    public const string playerNamePrefKey = "PlayerName";
}
