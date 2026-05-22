using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerGameplay : MonoBehaviour
{
    public static UIManagerGameplay Instance;

    [Header("Texts")]
    [SerializeField] private TMP_Text mapText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text bossText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text scoreText;

    private Player player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        FindPlayer();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayer();
    }

    private void FindPlayer()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (SceneManager.GetActiveScene().name == "MENU")
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

        if (GameManager.Instance == null)
            return;

        GameManager gm = GameManager.Instance;

        mapText.text = "Mapa: " + SceneManager.GetActiveScene().name;
        waveText.text = "Wave: " + gm.progression;
        timerText.text = "Tiempo: " + FormatTime(gm.GameTime);
        killsText.text = "Kills: " + gm.enemiesKilled;
        bossText.text = "Boss en: " + gm.EnemiesUntilBoss;
        scoreText.text = "Puntaje: " + gm.score;

        if (player != null)
        {
            healthText.text = "Vida: " + Mathf.Ceil(player.CurrentHealth);
        }
        else
        {
            healthText.text = "Vida: --";
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return $"{minutes:00}:{seconds:00}";
    }
}