using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    [Header("Block Parameters")]
    [SerializeField] float defaultBlockWidth = 2f;
    [SerializeField] float defaultBlockSpeed = 2f;
    public float blockHeight = 0.5f;
    [SerializeField] List<Difficulty_Setting> difficultySettings = new List<Difficulty_Setting>();
    [SerializeField] bool orderList; //Button to sort list in inspector;

    [Header("Game Boundaries")]
    public Transform playScreen;
    public Transform stackLimitMarker;
    

    [HideInInspector] public GameObject[] blockArray;
    [HideInInspector] public float blockWidth;
    [HideInInspector] public float blockSpeed;

    int blocksPlaced = 0;
    int currentSettingIndex = -1; // Initialised at -1 since default values are used first.


    // Ensures the list is in order of number of placed blocks from smallest to largest.
    // OnValidate is called when a value changes in the inspector.
    void OnValidate() 
    {
        if (orderList)
        {
            orderList = false;
            difficultySettings = difficultySettings.OrderByDescending(
                                 x => -x.numberOfBlocksPlaced).ToList();
        }
    }

    void Awake()
    {
        // Add starting block to array.
        blockArray = GameObject.FindGameObjectsWithTag("Block");
        // Set initial block settings.
        blockWidth = defaultBlockWidth;
        blockSpeed = defaultBlockSpeed;
        // Check settings list in case it overrides the default settings.
        CheckForUpdatedSettings();
    }

    public void UpdateBlocksPlaced()
    {
        blocksPlaced++;
        Debug.Log("Blocks placed: " + blocksPlaced);

        CheckForUpdatedSettings();
    }

    void CheckForUpdatedSettings()
    {
        blockArray = GameObject.FindGameObjectsWithTag("Block");
        
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

    public void MoveBlocksDown()
    {
        // Move each block down by a distance of one block's height.
        foreach (GameObject block in blockArray)
        {
            block.transform.position = new Vector2 (block.transform.position.x,
                                                    block.transform.position.y - blockHeight);
        }

        // Deactivate and destroy the first block in the stack.
        blockArray[0].SetActive(false);
        Destroy(blockArray[0]);
    }
}
