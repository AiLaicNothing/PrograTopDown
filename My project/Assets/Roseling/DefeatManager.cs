using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatManager : MonoBehaviour
{
    public static DefeatManager instance;

    [SerializeField] private GameObject defeatPanel;

    [SerializeField] private float returnDelay = 3f;

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

    public void ShowDefeat()
    {
        StartCoroutine(DefeatCoroutine());
    }

    private IEnumerator DefeatCoroutine()
    {
        defeatPanel.SetActive(true);

        yield return new WaitForSeconds(returnDelay);

        Time.timeScale = 1f;

        SceneManager.LoadScene("MENU");
    }
}