using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAssigner : MonoBehaviour
{
	public GameObject[] modelsPrefabs;
	
	// Start is called before the first frame update
    void Start()
    {
        
    }

    public void Assign(GameObject obj, int modelID)
	{
		GameObject model = Instantiate(modelsPrefabs[modelID]);

	}
}
