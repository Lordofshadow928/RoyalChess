using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageSelection 
{
    private const string SelectedStageKey = "SelectedStage";

    public static int SelectedStage
    {
        get => PlayerPrefs.GetInt(SelectedStageKey, 1);
        set
        {
            PlayerPrefs.SetInt(SelectedStageKey, value);
            PlayerPrefs.Save();
        }
    }
}
