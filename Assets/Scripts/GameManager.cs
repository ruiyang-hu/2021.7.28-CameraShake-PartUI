using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    float gameTime;
    public static int levelTime = 30;
    public static int currentLevel;

    public GameObject gameOverUI;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Start()
    {
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        currentLevel = (int)gameTime / levelTime + 1;
        //UIManager
        UIManager.UpdateTimeUI(gameTime);
        UIManager.UpdateLevelUI(currentLevel);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public static void GameOver(bool isDead)
    {
        if (isDead)
        {
            instance.gameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

}
