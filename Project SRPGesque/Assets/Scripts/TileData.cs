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

	private Color allyColor;
	private Color enemyColor;
	private Color allyEmission;
	private Color enemyEmission;


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

		allyColor = Color.cyan;
		allyColor.a = 0.75f;
		enemyColor = Color.red;
		enemyColor.a = 0.75f;
		allyEmission = new Color(0.705f, 0.945f, 0.945f, 1);
		enemyEmission = new Color(0.937f, 0.419f, 0.419f, 1);

		tileID = ID;
        this.name = "Tile_" + (tileID);

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
            block.SetColor(Shader.PropertyToID("_Color"), Color.blue );
            rend.SetPropertyBlock(block);
            ally = Alliance.Neutral;
        }
        else if (alliance == 1)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            rend.GetPropertyBlock(block);
            block.SetColor(Shader.PropertyToID("_Color"), allyColor);
            block.SetColor(Shader.PropertyToID("_EmissionColor"), allyEmission * 1);
			rend.SetPropertyBlock(block);
            ally = Alliance.Player;
        }
        else if (alliance == 2)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            rend.GetPropertyBlock(block);
            block.SetColor(Shader.PropertyToID("_Color"), enemyColor);
            block.SetColor(Shader.PropertyToID("_EmissionColor"), enemyEmission * 1);
			rend.SetPropertyBlock(block);
            ally = Alliance.Enemy;
        }
    }
}
