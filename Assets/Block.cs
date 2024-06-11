using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Start is called before the first frame update

    public bool hasBeenPlaced  = false;

    private bool has_spawned = false ;



    public Vector3 positionorigin;

    private  float xvel = 0.003f;

    public  float sprite_width;

    public float  sprite_height;
   

    public GameObject block;
    public GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        hasBeenPlaced = false;
    }

    // Update is called once per frame
    void Update()
    {



        var camera = Camera.main;
        Vector3 camerarelativeposition = transform.position - camera.transform.position;

        var relativex = camerarelativeposition.x;
        

        var camerawidth = camera.orthographicSize * 2f * camera.aspect;
        var cameraheight = camera.orthographicSize * 2f;


        var boxarray = gameManager.GetComponent<Game_Manager>().arrayofboxes;



        if (Input.GetKeyDown(KeyCode.Space))
        {

            for (var i = 0; i < boxarray.Length; i++)
            {


                var currentbox = boxarray[i].GetComponent<Block>();


                if (i == 0 && boxarray.Length - 1 == 0 && hasBeenPlaced == false)
                {
                    hasBeenPlaced = true;
                    Debug.Log("intiial block placed  ");

                }


                else if (boxarray.Length   >  1 && i  == boxarray.Length -1 )  {

                   // Debug.Log(boxarray);

                    if (currentbox.hasBeenPlaced == false && boxarray[i].transform.position.x >= boxarray[i - 1].transform.position.x - sprite_width && boxarray[i].transform.position.x <= boxarray[i - 1].transform.position.x + sprite_width)
                    {

                        
                      
                        currentbox.hasBeenPlaced = true;

                        Debug.Log("block succesfully stacked");


                    }

                    else if (currentbox.hasBeenPlaced == false && boxarray[i].transform.position.x >= boxarray[i - 1].transform.position.x - sprite_width == false || currentbox.hasBeenPlaced == false && boxarray[i].transform.position.x <= boxarray[i - 1].transform.position.x + sprite_width == false)
                    {


                        currentbox.hasBeenPlaced = true;
                        Debug.Log("failed to stack block correctly");

                       

                    }

                }

            }




        }













        if (hasBeenPlaced == false)
        {


            if (   relativex >= camerawidth / 2f )
            {

                xvel *= -1f ;


            }
            else if(relativex <= -camerawidth / 2f)
            {



                xvel = Mathf.Abs(xvel);
            }

            Vector3 newposition = new Vector3(transform.position.x + xvel, transform.position.y ,0);



            transform.position = newposition;



        }
      if(hasBeenPlaced == true && has_spawned == false)
        {

            Instantiate(block,new Vector3(transform.position.x ,transform.position.y + sprite_height,0),Quaternion.identity);

            has_spawned = true;
        }

    }
}
