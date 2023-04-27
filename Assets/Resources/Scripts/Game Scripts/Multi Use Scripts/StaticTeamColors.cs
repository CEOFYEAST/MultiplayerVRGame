using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticTeamColors 
{
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
    

    /**
    public enum TeamColors : Color32
    {
        // blue
        1 = Color32(47, 86, 243, 255),
        // green
        2 = Color32(18, 238, 15, 255),
        // orange
        3 = Color32(255, 146, 0, 255),
        // purple
        4 = Color32(146, 0, 255, 255),
        // red
        5 = Color32(255, 0, 8, 255),
        // yellow
        6 = Color32(255, 254, 18, 255)

    }
    */
}
