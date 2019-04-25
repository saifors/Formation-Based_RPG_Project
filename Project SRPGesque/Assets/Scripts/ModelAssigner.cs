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

    public void Assign(CharControl_Battle character, int modelID)
	{
		GameObject model = Instantiate(modelsPrefabs[modelID]);
		Transform modelTrans = model.transform; 
		modelTrans.position = character.trans.position;
		modelTrans.SetParent(character.trans);
		//modelTrans.eulerAngles = Vector3.zero;
	}
}
