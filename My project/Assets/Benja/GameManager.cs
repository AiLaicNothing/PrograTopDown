using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ISubject
{
    public static GameManager Instance;

    private List<IObserver> observers = new List<IObserver>();

    public int progression = 1;

    private void Awake()
    {
        Instance = this;
    }

    public void IncreaseDifficulty()
    {
        progression++;

        Notify();
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