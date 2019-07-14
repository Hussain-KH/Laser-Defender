using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {

    [SerializeField] float delaySeconds = 2f;

    public void StratGame()
    {
        SceneManager.LoadScene(1);
        if(FindObjectsOfType<GameSession>().Length >= 1)
        {
            FindObjectOfType<GameSession>().ResetGame();
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void GameOver()
    {
        StartCoroutine(WaitAndLoad());
    }
    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(delaySeconds);
        SceneManager.LoadScene(2);
    }
}
