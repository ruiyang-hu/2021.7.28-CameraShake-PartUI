using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    public Text timeText;
    public Text levelText;
    public static Animator levelUpAnim;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        levelUpAnim = transform.Find("StageUI").Find("LevelUp").gameObject.GetComponent<Animator>();
        levelUpAnim.gameObject.SetActive(false);
    }

    private void Update()
    {
        timeText.text = "Time: " + Time.deltaTime.ToString("mm:ss");
    }

    public static void UpdateTimeUI(float time)
    {
        int minutes = (int)time / 60;
        float seconds = time % 60;

        instance.timeText.text = "Time:      " + minutes.ToString("00") + ":" + seconds.ToString("00.00");
    }

    public static void UpdateLevelUI(int currentLevel)
    {
        instance.levelText.text = "Level:        " + currentLevel;
        if(currentLevel == 2)
        {
            levelUpAnim.gameObject.SetActive(true);
        }
    }
}
