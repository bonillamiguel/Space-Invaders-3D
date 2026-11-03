using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [Header("Inscribed")]
    public Button replayButton;
    public Button quitButton;
    public GameObject startScreen;
    public Button startButton;

    static bool hasStarted = false;

    // Domain reload is off in this project, so statics must be reset for each Play
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        hasStarted = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        replayButton.onClick.AddListener(Replay);
        quitButton.onClick.AddListener(Quit);
        startButton.onClick.AddListener(StartGame);
        SetButtonsVisible(false);

        startScreen.SetActive(!hasStarted);
        if (!hasStarted)
        {
            Time.timeScale = 0;
        }
    }

    void StartGame()
    {
        hasStarted = true;
        startScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShowGameOver()
    {
        SetButtonsVisible(true);
    }

    void SetButtonsVisible(bool visible)
    {
        replayButton.gameObject.SetActive(visible);
        quitButton.gameObject.SetActive(visible);
    }

    void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
