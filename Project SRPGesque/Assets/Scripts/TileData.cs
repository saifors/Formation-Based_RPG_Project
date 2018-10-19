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
    public Material neutralMat;
    public Material playerMat;
    public Material enemyMat;
    public Renderer rend;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        tileScript = GameObject.FindGameObjectWithTag("TileManager").GetComponent<TileScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(int xS, int yS, int ID, int tileSum)
    {
        xSquare = xS;
        ySquare = yS;

        tileID = ID;
        this.name = "Tile_" + (tileID+1);

        if (tileID <= tileSum/2 - 1) SetAlliance(2);
        else SetAlliance(1);

        x = 0.5f - xSquare;
        y = 0.5f - ySquare;

        tileTrans.position = new Vector3(x, 0, y);
    }

    public void SetAlliance(int alliance) //Change this to use only one Material and have the Color changed with SetColor on the MaterialPropertyBlock
    {
        if (alliance == 0)
        {
            rend.material = neutralMat;
            ally = Alliance.Neutral;
        }
        else if (alliance == 1)
        {
            rend.material = playerMat;
            ally = Alliance.Player;
        }
        else if (alliance == 2)
        {
            rend.material = enemyMat;
            ally = Alliance.Enemy;
        }
    }
}
