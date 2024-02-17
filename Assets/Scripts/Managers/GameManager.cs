using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public TextMeshProUGUI holoPoints;
    public TextMeshProUGUI seedsTextBattleField;
    public TextMeshProUGUI seedsTextFarm;

    public int seedsCnt = 0;
    public int holoPointsCnt;
    private int towerCost;
    
    [SerializeField] private GameObject grid;
    private GameObject towerPref;

    [SerializeField] private Tile[] tiles;

    public bool isTowerSelected = false;

    private Button cardButton;

    [SerializeField] private Color selectedButtonColor;
    [SerializeField] private Color defaultButtonColor;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateHoloPoints();
        UpdateSeeds();
    }

    #region CardButtonMethods

    public void TowerSelected(GameObject towerPref)
    {
        isTowerSelected = !isTowerSelected;
        this.towerPref = towerPref;

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

        if (holoPointsCnt < towerCost)
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
                float distance = Vector2.Distance(tile.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTile = tile;
                }
            }

            if (!nearestTile.isOccupied && nearestDistance < 0.9f)
            {
                GameObject towerInstance = Instantiate(towerPref, nearestTile.transform.position, Quaternion.identity);
                towerInstance.transform.parent = nearestTile.transform;
                nearestTile.isOccupied = true;
                isTowerSelected = !isTowerSelected;
                cardButton.image.color = defaultButtonColor;
                holoPointsCnt -= towerCost;
                UpdateHoloPoints();
            }
            else
            {
                isTowerSelected = !isTowerSelected;
                cardButton.image.color = defaultButtonColor;
            }
        }

        #endregion
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
        this.towerCost = Convert.ToInt32(towerCost.text);
    }
}
