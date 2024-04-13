using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestEndGameScript : MonoBehaviour
{
    public TextMeshProUGUI enemiesKilledInGame;

    private void Start()
    {
        enemiesKilledInGame.text = Convert.ToString(GameManager.Instance.killedEnemiesCnt);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
