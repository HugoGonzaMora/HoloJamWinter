using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestEndGameScript : MonoBehaviour
{
    public TextMeshProUGUI enemiesKilledInGame;
    public TextMeshProUGUI survivedTimeInGame;
    
    private int _enemiesKilledCnt;
    private int _survivedTime;

    private void Start()
    {
        _enemiesKilledCnt = GameManager.Instance.enemiesKilledCnt;
        _survivedTime = Convert.ToInt32(GameManager.Instance.survivedTime);

        enemiesKilledInGame.text = $"{_enemiesKilledCnt}";
        survivedTimeInGame.text = ConvertSurvivalTime(_survivedTime);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    private string ConvertSurvivalTime(int _survivedTime)
    {
        int h = _survivedTime / 3600;
        int s = _survivedTime % 3600;
        int m = s / 60;
        s = s % 60;

        return string.Format($"{h}:{m:D2}:{s:D2}");
    }
}
