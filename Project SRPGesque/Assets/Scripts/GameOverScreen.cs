using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
	public TextMeshProUGUI gameOverText;
	public float separationValue;

	//815
	// Start is called before the first frame update
	void Start()
    {
		DOTween.To(() => separationValue, x => separationValue = x, 815, 8).From();
    }

    // Update is called once per frame
    void Update()
    {
		gameOverText.wordSpacing = separationValue;
    }
}
