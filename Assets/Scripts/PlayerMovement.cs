using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public GameObject mapController;
    private MapGen mapGen;
    private int xloc;
    private int yloc;

    // Start is called before the first frame update
    void Start()
    {
        mapGen = mapController.GetComponent<MapGen>();
        xloc = (int)transform.position.x;
        yloc = (int)transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            if (mapGen.checkBridge(xloc, yloc, 0))
            {
                // Debug.Log("Move N");
                yloc += 1;
                transform.position = new Vector3(xloc, yloc, 0);
            }
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            if (mapGen.checkBridge(xloc, yloc, 1))
            {
                // Debug.Log("Move E");
                xloc += 1;
                transform.position = new Vector3(xloc, yloc, 0);
            }
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            if (mapGen.checkBridge(xloc, yloc, 2))
            {
                // Debug.Log("Move S");
                yloc -= 1;
                transform.position = new Vector3(xloc, yloc, 0);
            }
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            if (mapGen.checkBridge(xloc, yloc, 3))
            {
                // Debug.Log("Move W");
                xloc -= 1;
                transform.position = new Vector3(xloc, yloc, 0);
            }
        }
    }   
}
