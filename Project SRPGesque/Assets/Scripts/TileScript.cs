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

        tiles = new TileData[tileAmount];
        //hacer un for dentro de un for uno para vertical otro horizontal y recorrer el array Instanciar la prefab y y llamar la Init

        for(int iX = 0; iX < xTiles;  iX++)
        {
            for(int iY = 0; iY < yTiles; iY++)
            {
                
                
                GameObject obj = Instantiate(tilePrefab);
                obj.GetComponent<TileData>().Init(iX,iY, id);
                id++;

            }
        }

        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
