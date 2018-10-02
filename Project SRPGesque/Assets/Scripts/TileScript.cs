using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    public GameObject tilePrefab;
    public TileData[] tiles;
    public int tileAmount;
    public int xTiles;
    public int yTiles;
    private int id = 0;

    // Use this for initialization
    void Start ()
    {

        tileAmount = xTiles * yTiles; //automatically calculate the amount of tiles.
        

        for(int iX = 0; iX < xTiles;  iX++)
        {
            for(int iY = 0; iY < yTiles; iY++)
            {
                //double for one for columns and one for rows
                
                GameObject obj = Instantiate(tilePrefab); 
                obj.GetComponent<TileData>().Init(iX,iY, id, tileAmount);
                id++;

            }
        }

        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
