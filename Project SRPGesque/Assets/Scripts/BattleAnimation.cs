using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimation : MonoBehaviour
{
	public GameManager gameManager;

	public int step;

	Coroutine routine;
	//bool wait;

    void Start()
    {
		gameManager = GetComponent<GameManager>();

		/*routine = StartCoroutine(WaitForAnimation(anim));

		if(routine != null) StopCoroutine(routine);
		routine = null;*/
    }

    public void LaunchAttackAnim()
	{
		string nameKey = gameManager.gameData.AttackList[gameManager.currentAttack].nameKey;

		gameManager.battleUI.ChangeNotifText(LanguageManager.langData.attackName[nameKey]);
		step = 0;
		Animator anim = gameManager.charControl[gameManager.activeCharacter].CastAnim();
		StartCoroutine(WaitForAnimation(anim));
		
	}

	public void SpellAnim()
	{
		//Particle animation I guess???
	}

	public void HitAnim(Animator anim)
	{
		
		StartCoroutine(WaitForAnimation(anim));
	}

	public void FinishedAnim()
	{
		if(step == 0)
		{
			//Particle Spell animation
			SpellAnim();
		}
		else if (step == 1)
		{
			gameManager.HitAttack();

		}
		else if (step == 2)
		{
			//Final anim
			gameManager.EndTurn();
		}

		step++;
	}
	
	IEnumerator WaitForAnimation(Animator anim)
	{
		//wait = true;
		Debug.Log("Waiting...");
		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + 1);
		Debug.Log("Animation ended");
		//wait = false;
		FinishedAnim();
	}
}
