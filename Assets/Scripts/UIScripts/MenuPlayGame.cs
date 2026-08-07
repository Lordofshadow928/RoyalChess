using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPlayGame : MonoBehaviour
{
    [SerializeField] private MenuMapScroll mapScroll;
    public void PlayGame()
    {
        if (mapScroll.CurrentIslandLocked)
            return;
        
        StageSelection.SelectedStage = mapScroll.CurrentStageIndex;
        SceneManager.LoadScene("GamePlayScene");
    }
}
