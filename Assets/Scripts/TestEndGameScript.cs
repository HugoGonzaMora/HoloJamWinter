using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestEndGameScript : MonoBehaviour
{
    public TextMeshProUGUI enemiesKilledInGame;
    public TextMeshProUGUI survivedTimeInGame;
    public TextMeshProUGUI wavesSurvived;

    private int _wavesSurvived;
    private int _enemiesKilledCnt;
    private int _survivedTime;

    private void Start()
    {
        _wavesSurvived = WaveManager.Instance.waveIndex - 1;
        _enemiesKilledCnt = GameManager.Instance.enemiesKilledCnt;
        _survivedTime = Convert.ToInt32(GameManager.Instance.survivedTime);

        wavesSurvived.text = $"{_wavesSurvived}";
        enemiesKilledInGame.text = $"{_enemiesKilledCnt}";
        survivedTimeInGame.text = GameManager.Instance.ConvertSurvivalTime(_survivedTime);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
