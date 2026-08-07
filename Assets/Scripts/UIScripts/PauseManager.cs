using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private void Start()
    {
        pausePanel.SetActive(false);
    }
    public void ShowPause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ContinueGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void ReturnToMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MenuScene");
    }
}