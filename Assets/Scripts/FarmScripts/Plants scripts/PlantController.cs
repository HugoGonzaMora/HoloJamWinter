using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    public PlantType plantSO;

    private int _holopoints;

    private float _timeToGrow;

    private void Start()
    {
        _holopoints = plantSO.holopoints;
        _timeToGrow = plantSO.timeToGrow;
    }

    private void Update()
    {
        if (_timeToGrow > 0)
        {
            _timeToGrow -= Time.deltaTime;
        }
        else
        {
            GameManager.Instance.holoPointsCnt += _holopoints;
            GameManager.Instance.UpdateHoloPoints();
            Destroy(this.gameObject);
        }
    }
}
