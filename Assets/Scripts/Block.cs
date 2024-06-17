using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject blockPrefab;

    float blockSpeed;
    float blockDirection = 1f;
    
    bool hasBeenPlaced = false;
    bool hasSpawned = false;

    float width;
    float height;

    Game_Manager gameManager;
    Camera mainCamera;
    SpriteRenderer spriteRenderer;
    
    Transform playScreen;
    float leftGameBorder;
    float rightGameBorder;


    GameObject[] blockArray;

    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<Game_Manager>();
        mainCamera = Camera.main;

        BlockSetUp();
        GameBoundariesSetUp();
        RandomiseStart();
    }

    void BlockSetUp()
    {
        // These values are handled by the game manager.
        width = gameManager.blockWidth;
        blockSpeed = gameManager.blockSpeed;

        // Set height and width.
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.transform.localScale = new Vector3(width,
                                                           spriteRenderer.transform.localScale.y,
                                                           spriteRenderer.transform.localScale.z);
        height = gameManager.blockHeight;
    }

    void GameBoundariesSetUp()
    {
        // Get game boundaries.
        playScreen = gameManager.playScreen;

        // Set borders.
        float playScreenWidth = playScreen.localScale.x;

        leftGameBorder = playScreen.position.x - (playScreenWidth - width) / 2f;
        rightGameBorder = playScreen.position.x + (playScreenWidth - width) / 2f;
    }

    void RandomiseStart()
    {
        // Randomise starting direction.
        bool randomBool = Random.value > 0.5f;
        if (randomBool == false)
        {
            blockDirection = -1f;
        }

        // Randomise starting position.
        float randomX = Random.Range(leftGameBorder, rightGameBorder);
        transform.position = new Vector2 (randomX, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        blockArray = gameManager.blockArray;

        HandleInput();

        HandleBlockMovement();

        HandleNewBlockInstantiation();

    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceBlock();
        }
    }

    void PlaceBlock()
    {
        if (hasBeenPlaced) return;

        // If not placed yet, this must be the most recent block; i.e. the 
        // block with index equal to the array length - 1.
        int thisBlockIndex = blockArray.Length - 1;

        hasBeenPlaced = true;

        // Place the initial block without checking for a previous block.
        if (thisBlockIndex == 0)
        {
            Debug.Log("Intiial block placed.");
        }
        // Place subsequent blocks after comparing to the previous block placement.
        else
        {
            DetermineSuccessfulPlacement(thisBlockIndex);
        }
    }

    void DetermineSuccessfulPlacement(int thisBlockIndex)
    {
        Block previousBlock = blockArray[thisBlockIndex - 1].GetComponent<Block>();

        // Determine the position range that counts as a successful placement.
        float successfulPositionRange = previousBlock.width + width;
        float successfulPositionMin = previousBlock.transform.position.x -
                                        successfulPositionRange / 2f;
        float successfulPositionMax = previousBlock.transform.position.x +
                                        successfulPositionRange / 2f;
        float xPosition = transform.position.x;

        bool isWithinSuccessRange = false;

        if (xPosition >= successfulPositionMin && xPosition <= successfulPositionMax)
        {
            isWithinSuccessRange = true;
        }

        if (isWithinSuccessRange)
        {
            Debug.Log("Block succesfully stacked");
        }
        else
        {
            Debug.Log("Block failed to stack");
        }
    }

    void HandleBlockMovement()
    {
        if (hasBeenPlaced) return; //Only move if not placed.

        // Change direction when hitting game borders.
        if (transform.position.x <= leftGameBorder)
        {
            blockDirection = 1f;
        }
        if (transform.position.x >= rightGameBorder)
        {
            blockDirection = -1f;
        }

        // Update position.
        float newXPosition = transform.position.x + blockSpeed * blockDirection * Time.deltaTime;
        Vector3 newPosition = new Vector3(newXPosition, transform.position.y, 0);

        transform.position = newPosition;
    }

    void HandleNewBlockInstantiation()
    {
        if (hasBeenPlaced == true && hasSpawned == false)
        {
            // Check if the block stack has reached the limit.
            float instantiationYPosition = transform.position.y + height;
            float stackLimitPosition = gameManager.stackLimitMarker.position.y;

            if (instantiationYPosition >= stackLimitPosition)
            {
                // If it has reached the limit, keep the instance position at the current position
                // and move all the other blocks down.
                instantiationYPosition = transform.position.y;

                gameManager.MoveBlocksDown();
            }

            // Create new block and update game manager.
            Instantiate(blockPrefab, new Vector3(transform.position.x, instantiationYPosition, 0), Quaternion.identity);

            gameManager.UpdateBlocksPlaced();

            hasSpawned = true;
        }
    }
}
