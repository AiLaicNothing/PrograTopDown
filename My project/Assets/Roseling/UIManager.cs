using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Screens")]
    public GameObject initialScreen;
    public GameObject creditsScreen;
    public GameObject levelSelectionScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(string levelName)
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(levelName);
    }

    private void Start()
    {
        ShowInitialScreen();
    }

    public void ShowInitialScreen()
    {
        HideAll();

        initialScreen.SetActive(true);
    }

    public void ShowCreditsScreen()
    {
        HideAll();

        creditsScreen.SetActive(true);
    }

    public void ShowLevelSelectionScreen()
    {
        HideAll();

        levelSelectionScreen.SetActive(true);
    }

    private void HideAll()
    {
        initialScreen.SetActive(false);
        creditsScreen.SetActive(false);
        levelSelectionScreen.SetActive(false);
    }
    public void BackToMainMenu() // para los niveles, para que puedan volver al mnuprincipal
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MENU");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
