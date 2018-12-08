using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    public GameObject tilePrefab;

    public TileData[] tiles;
    public Transform[] tileTransform;
    public int tileAmount;
    public int xTiles;
    public int yTiles;
    private int id = 0;
    private Transform tileCenter;
    public Transform battlefield;

    // Use this for initialization
    public void Init ()
    {
        battlefield = GameObject.FindGameObjectWithTag("Battlefield").GetComponent<Transform>();
        tileCenter = this.transform;
        tileAmount = xTiles * yTiles; //automatically calculate the amount of tiles.
        tiles = new TileData[tileAmount];
        tileTransform = new Transform[tileAmount];

        for(int iX = 0; iX < xTiles;  iX++)
        {
            for(int iY = 0; iY < yTiles; iY++)
            {
                //double for one for columns and one for rows
                
                GameObject obj = Instantiate(tilePrefab);
                
                tiles[id] = obj.GetComponent<TileData>();
                tiles[id].Init(iX,iY, id, tileAmount);
                tileTransform[id] = tiles[id].transform;
                //here is a problem
                tileTransform[id].position = obj.transform.position;
                id++;
                obj.transform.parent = tileCenter;//Only works becasue it's inside tilemanager, if that were to change things will have to change.

            }
        }

        tileCenter.position = new Vector3(battlefield.position.x, battlefield.position.y + 0.001f, battlefield.position.z);
	}
	

}

