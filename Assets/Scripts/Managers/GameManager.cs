using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public TextMeshProUGUI holoPoints;
    public TextMeshProUGUI seedsTextBattleField;
    public TextMeshProUGUI seedsTextFarm;
    public TextMeshProUGUI SurvivalTimeText;

    public int seedsCnt;
    public int holoPointsCnt;
    private int _towerCost;
    
    [SerializeField] private GameObject[] _plantPrefs;
    private GameObject _towerPref;

    [SerializeField] private Tile[] tiles;
    [SerializeField] private Tile[] farmTiles;

    public bool isTowerSelected = false;

    private Button cardButton;

    [SerializeField] private Color selectedButtonColor;
    [SerializeField] private Color defaultButtonColor;

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _farmCamera;

    [HideInInspector] public  bool isGameEnd;
    
    [HideInInspector] public int enemiesKilledCnt = 0;
    [HideInInspector] public float survivedTime = 0f;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isGameEnd = false;
        
        UpdateHoloPoints();
        UpdateSeeds();
    }

    #region CardButtonMethods

    public void TowerSelected(GameObject towerPref)
    {
        isTowerSelected = !isTowerSelected;
        this._towerPref = towerPref;

        foreach (Tile tile in tiles)
        {
            if (tile.transform.childCount == 0)
            {
                tile.isOccupied = false;
            }
        }
    }

    public void ButtonSelected(Button button)
    {
        cardButton = button;
        cardButton.image.color = selectedButtonColor;
    }

    #endregion
    
    private void Update()
    {
        #region TowerPlacing

        if (holoPointsCnt < _towerCost)
        {
            isTowerSelected = false;
            cardButton.image.color = defaultButtonColor;
        }

        if (Input.GetMouseButtonDown(0) && isTowerSelected)
        {
            Tile nearestTile = null;
            float nearestDistance = float.MaxValue;
            foreach (Tile tile in tiles)
            {
                float distance = Vector2.Distance(tile.transform.position, _mainCamera.ScreenToWorldPoint(Input.mousePosition));
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTile = tile;
                }
            }

            if (!nearestTile.isOccupied && nearestDistance < 0.9f)
            {
                GameObject towerInstance = Instantiate(_towerPref, nearestTile.transform.position, Quaternion.identity);
                towerInstance.transform.parent = nearestTile.transform;
                nearestTile.isOccupied = true;
                isTowerSelected = !isTowerSelected;
                cardButton.image.color = defaultButtonColor;
                holoPointsCnt -= _towerCost;
                UpdateHoloPoints();
            }
            else
            {
                isTowerSelected = !isTowerSelected;
                cardButton.image.color = defaultButtonColor;
            }
        }

        #endregion

        #region Seeds Planting

        if (Input.GetMouseButtonDown(0) && seedsCnt > 0)
        {
            foreach (Tile tile in farmTiles)
            {
                if (tile.transform.childCount == 0)
                {
                    tile.isOccupied = false;
                }
            }/////////////////////////////////////////////////////////////////////
            
            Tile nearestTile = null;
            float nearestDistance = float.MaxValue;
            foreach (Tile tile in farmTiles)
            {
                float distance = Vector2.Distance(tile.transform.position, _farmCamera.ScreenToWorldPoint(Input.mousePosition));
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTile = tile;
                }
            }

            if (!nearestTile.isOccupied && nearestDistance < 0.9f)
            {
                GameObject _plantPref = _plantPrefs[Random.Range(0, _plantPrefs.Length)];
                GameObject seedInstance = Instantiate(_plantPref, nearestTile.transform.position, Quaternion.identity);
                seedInstance.transform.parent = nearestTile.transform;
                nearestTile.isOccupied = true;
                seedsCnt -= 1;
                UpdateSeeds();
            }
        }

        #endregion

        InvokeRepeating(nameof(DisplaySurvivingTime), 1f, 1f);
        
        if (isGameEnd == true)
        {
            EndGame();
        }

        if (isGameEnd == false)
        {
            survivedTime += Time.deltaTime;
        }
    }

    public void UpdateSeeds()
    {
        seedsTextBattleField.text = Convert.ToString(seedsCnt);
        seedsTextFarm.text = Convert.ToString(seedsCnt);
    }

    public void UpdateHoloPoints()
    {
        holoPoints.text = Convert.ToString(holoPointsCnt);
    }

    public void CheckTowerCost(TextMeshProUGUI towerCost)
    {
        _towerCost = Convert.ToInt32(towerCost.text);
    }

    private void EndGame()
    {
        SceneManager.LoadScene("EndGameScene");
    }

    public string ConvertSurvivalTime(int survivedTime)
    {
        int h = survivedTime / 3600;
        int s = survivedTime % 3600;
        int m = s / 60;
        s = s % 60;

        
        return string.Format($"{h}:{m:D2}:{s:D2}");
    }

    private void DisplaySurvivingTime()
    {
        SurvivalTimeText.text = ConvertSurvivalTime(Convert.ToInt32(survivedTime));
    }
}
