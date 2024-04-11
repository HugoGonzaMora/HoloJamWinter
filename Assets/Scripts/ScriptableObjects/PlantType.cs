using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlantType", menuName = "PlantType")]

public class PlantType : ScriptableObject
{
    [Header("Plant Type")] 
    public int holopoints;

    public float timeToGrow;
}
