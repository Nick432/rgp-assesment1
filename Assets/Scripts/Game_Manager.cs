using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public GameObject[] arrayofboxes;
    [SerializeField] float defaultBlockWidth = 2f;
    [SerializeField] float defaultBlockSpeed = 2f;
    [SerializeField] List<Difficulty_Setting> difficultySettings = new List<Difficulty_Setting>();
    [SerializeField] bool orderList;

    float blockWidth;
    float blockSpeed;

    int currentSettingIndex = -1;

    // Ensures the list is in order of number of placed blocks from smallest to largest.
    void OnValidate() 
    {
        if (orderList)
        {
            orderList = false;
            difficultySettings = difficultySettings.OrderByDescending(
                                 x => int.MaxValue - x.numberOfBlocksPlaced).ToList();
        }
    }

    int blocksPlaced;

    void Awake()
    {
        // Add starting block to array.
        arrayofboxes = GameObject.FindGameObjectsWithTag("Block");
        blockWidth = defaultBlockWidth;
        blockSpeed = defaultBlockSpeed;
        CheckForUpdatedSettings();

    }

    // Update is called once per frame
    void Update()
    {

        // Add new blocks to array.
        
    }

    public float GetBlockWidth()
    {
        CheckForUpdatedSettings();

        return blockWidth;
    }

    public float GetBlockSpeed()
    {
        CheckForUpdatedSettings();

        return blockSpeed;
    }

    void CheckForUpdatedSettings()
    {
        arrayofboxes = GameObject.FindGameObjectsWithTag("Block");
        blocksPlaced = arrayofboxes.Length - 1;
        
        int nextSettingIndex = currentSettingIndex + 1;

        // Return if there aren't any higher settings to go to.
        if (nextSettingIndex >= difficultySettings.Count) return;

        // Check if # of blocks placed matches a higher requirement, and update settings.
        if (blocksPlaced == difficultySettings[nextSettingIndex].numberOfBlocksPlaced)
        {
            currentSettingIndex = nextSettingIndex;
            blockWidth = difficultySettings[currentSettingIndex].blockWidth;
            blockSpeed = difficultySettings[currentSettingIndex].blockSpeed;
        }

    }

}
