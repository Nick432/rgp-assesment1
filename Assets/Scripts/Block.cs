using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject blockPrefab;

    float blockSpeed;
    
    bool hasBeenPlaced = false;
    bool hasSpawned = false ;

    float width;
    float height;

    Game_Manager gameManager;
    Camera mainCamera;
    SpriteRenderer spriteRenderer;

    Vector3 cameraRelativePosition;

    float relativeX;
    
    float cameraWidth;
    float cameraheight;


    GameObject[] blockArray;

    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<Game_Manager>();
        mainCamera = Camera.main;

        blockSpeed = gameManager.GetBlockSpeed();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        width = gameManager.GetBlockWidth();
        spriteRenderer.transform.localScale = new Vector3 (width,
                                                           spriteRenderer.transform.localScale.y,
                                                           spriteRenderer.transform.localScale.z);
        height = spriteRenderer.transform.localScale.y;
        
        cameraWidth = mainCamera.orthographicSize * 2f * mainCamera.aspect;
        cameraheight = mainCamera.orthographicSize * 2f;
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraRelativePosition = transform.position - mainCamera.transform.position;
        relativeX = cameraRelativePosition.x;
        blockArray = gameManager.arrayofboxes;

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
        if (hasBeenPlaced == false)
        {
            if (relativeX >= cameraWidth / 2f)
            {
                blockSpeed *= -1f;
            }
            else if (relativeX <= -cameraWidth / 2f)
            {
                blockSpeed = Mathf.Abs(blockSpeed);
            }

            float newXPosition = transform.position.x + blockSpeed * Time.deltaTime;
            Vector3 newPosition = new Vector3(newXPosition, transform.position.y, 0);

            transform.position = newPosition;
        }
    }

    void HandleNewBlockInstantiation()
    {
        if (hasBeenPlaced == true && hasSpawned == false)
        {

            Instantiate(blockPrefab, new Vector3(transform.position.x, transform.position.y + height, 0), Quaternion.identity);

            hasSpawned = true;
        }
    }
}
