using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject towerPref;

    [SerializeField] private Tile[] tiles;

    public bool isTowerSelected = false;

    public void TowerSelected()
    {
        isTowerSelected = !isTowerSelected;
    }

    private void Update()
    {
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

            if (!nearestTile.isOccupied)
            {
                Instantiate(towerPref, nearestTile.transform.position, Quaternion.identity);
                nearestTile.isOccupied = true;
                isTowerSelected = !isTowerSelected;
            }
        }
    }
}
