using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISubject
{
    public static GameManager Instance;

    private List<IObserver> observers = new List<IObserver>();

    [Header("Progression")]
    public int progression = 1;

    [Header("Stats")]
    public int enemiesKilled;
    public int score;

    [Header("Boss")]
    public int enemiesPerBoss = 20;

    private float gameTime;

    public float GameTime => gameTime;

    public int EnemiesUntilBoss
    {
        get
        {
            return enemiesPerBoss - (enemiesKilled % enemiesPerBoss);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        gameTime += Time.deltaTime;
    }

    public void IncreaseDifficulty()
    {
        progression++;

        Debug.Log("Wave actual: " + progression);

        if (progression == 5)
        {
            SceneManager.LoadScene("LEVEL2");
        }
        else if (progression == 7)
        {
            SceneManager.LoadScene("LEVEL3");
        }

        Notify();
    }

    public void AddKill(int points)
    {
        enemiesKilled++;

        score += points;
    }

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (IObserver observer in observers)
        {
            observer.Execute(this);
        }
    }
}