using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour {

    public Transform tileTrans;

    public TileScript tileScript;

    private float x;
    private float y;

    public int tileID;

    public int xSquare;
    public int ySquare;
    public enum Alliance { Neutral, Player, Enemy };
    public Alliance ally;
    public bool isOccupied;
    public enum Terrain { Normal, Tree, Rock};
    public Terrain terrain;
    //public int terrain;
    public Renderer rend;
    private Transform gridCenter;


    // Update is called once per frame
    void Update()
    {

    }

    public void Init(int xS, int yS, int ID, int tileSum)
    {
        rend = GetComponent<Renderer>();
        tileTrans = transform;
        tileScript = GameObject.FindGameObjectWithTag("TileManager").GetComponent<TileScript>();
        gridCenter = tileScript.transform;
        xSquare = xS;
        ySquare = yS;

        tileID = ID;
        this.name = "Tile_" + (tileID+1);

        if (tileID <= tileSum/2 - 1) SetAlliance(2);
        else SetAlliance(1);

        x = 0.5f * (tileScript.xTiles - 1) - xSquare;
        y = 0.5f * (tileScript.yTiles - 1) - ySquare;

        tileTrans.position = new Vector3(x, 0, y);
    }

    public void SetAlliance(int alliance) 
    {
        if (alliance == 0)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            rend.GetPropertyBlock(block);
            block.SetColor(Shader.PropertyToID("_Color"), Color.blue);
            rend.SetPropertyBlock(block);
            ally = Alliance.Neutral;
        }
        else if (alliance == 1)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            rend.GetPropertyBlock(block);
            block.SetColor(Shader.PropertyToID("_Color"), Color.cyan);
            rend.SetPropertyBlock(block);
            ally = Alliance.Player;
        }
        else if (alliance == 2)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            rend.GetPropertyBlock(block);
            block.SetColor(Shader.PropertyToID("_Color"), Color.red);
            rend.SetPropertyBlock(block);
            ally = Alliance.Enemy;
        }
    }
}
