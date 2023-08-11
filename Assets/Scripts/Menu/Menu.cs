using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        // ������Ϸ����
    }
    public void GoBackToMenu()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        TransitionManager.Instance.Transition(currentScene, "Menu");

        // ������Ϸ����
    }

    public void StartGameWeek(int gameWeek)
    {
        EventHandler.CallStartNewGameEvent(gameWeek);
    }


}
