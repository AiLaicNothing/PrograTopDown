using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatManager : MonoBehaviour
{
    public static DefeatManager Instance;

    [Header("Panel")]
    [SerializeField] private GameObject defeatPanel;

    [Header("Final Stats")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text waveText;

    [Header("Settings")]
    [SerializeField] private float returnDelay = 5f;

    private bool defeatShown;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        defeatPanel.SetActive(false);
    }

    public void ShowDefeat()
    {
        if (defeatShown)
            return;

        defeatShown = true;

        StartCoroutine(DefeatCoroutine());
    }

    private IEnumerator DefeatCoroutine()
    {
        GameManager gm = GameManager.Instance;

        defeatPanel.SetActive(true);

        timeText.text =
            "Tiempo sobrevivido: " +
            FormatTime(gm.GameTime);

        killsText.text =
            "Kills: " +
            gm.enemiesKilled;

        scoreText.text =
            "Puntaje: " +
            gm.score;

        waveText.text =
            "Wave alcanzada: " +
            gm.progression;

        yield return new WaitForSeconds(returnDelay);

        Time.timeScale = 1f;

        defeatShown = false;

        defeatPanel.SetActive(false);

        SceneManager.LoadScene("MENU");
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return $"{minutes:00}:{seconds:00}";
    }
}