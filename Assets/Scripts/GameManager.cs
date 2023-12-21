using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject grid;
    private GameObject towerPref;

    [SerializeField] private Tile[] tiles;

    public bool isTowerSelected = false;

    private Button cardButton;

    [SerializeField] private Color selectedButtonColor;
    [SerializeField] private Color defaultButtonColor;

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
            }
            else
            {
                isTowerSelected = !isTowerSelected;
                cardButton.image.color = defaultButtonColor;
            }
        }

        #endregion
    }
}
