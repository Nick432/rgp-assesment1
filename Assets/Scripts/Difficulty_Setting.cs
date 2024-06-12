using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Difficulty_Setting
{
    [Tooltip("Applies the settings once this many blocks are placed")] 
    public int numberOfBlocksPlaced;
    public float blockWidth;
    public float blockSpeed;
}
