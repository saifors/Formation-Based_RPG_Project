using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimation : MonoBehaviour
{
	public GameManager gameManager;

	public GameObject[] particleAnim;

	private List<GameObject> parts;

	public int step;

	Coroutine routine;
	//bool wait;

    void Start()
    {
		gameManager = GetComponent<GameManager>();
		parts = new List<GameObject>();

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
		List<SpellAnim> spellAnims = new List<SpellAnim>();

		float finishTime = 0;
		//Particle animation I guess???
		for (int target = 0; target < gameManager.selectedTargets.Length; target++)
		{
			//Instantiate & play particle animation On targeted tiles. 
			GameObject particle = Instantiate(particleAnim[0]);
			parts.Add(particle);
			spellAnims.Add(particle.GetComponent<SpellAnim>());
			spellAnims[target].Init();
			if (gameManager.charControl[gameManager.activeCharacter].alliance == CharacterStats.Alliance.Player)
			{
				Debug.Log("Player Spell");
				spellAnims[target].trans.position = gameManager.selectedTargetsTransform[target].position;
			}

			else
			{
				Debug.Log("Enemy Spell");
				spellAnims[target].trans.position = gameManager.tileScript.tileTransform[gameManager.selectedTargets[target]].position;
			}
			if (target == 0) finishTime = spellAnims[target].particles[0].startLifetime + spellAnims[target].particles[0].duration;
			Destroy(particle, finishTime + 0.2f);
		}
		//Debug.Log("hey" + (finishTime + 0.25f));
		Debug.Log("yo");
		StartCoroutine(WaitForParticleDestruction(finishTime + 0.1f));
		
		//FinishedAnim();
	}

	/*public void HitAnim(Animator anim)
	{
		Debug.Log("HitAnim");
		StartCoroutine(WaitForAnimation(anim));
	}*/

	public void FinishedAnim()
	{

		if (step == 0)
		{
			//Debug.Log("step0");
			//Particle Spell animation
			SpellAnim();
		}
		else if (step == 1)
		{
			//Debug.Log("step1");
			gameManager.HitAttack();

		}

		if (step < 2)
		{
			Debug.Log("step fulfilled " + step);
			step++;
			
		}
		else
		{
			Debug.Log("Final step fulfilled");
			step = 0;
			gameManager.EndTurn();
			
		}
		
	}
	
	IEnumerator WaitForAnimation(Animator anim)
	{
		//wait = true;
		//Debug.Log("Waiting...");
		if(anim != null) yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + 1);

		//Debug.Log("Animation ended");
		//wait = false;
		FinishedAnim();
	}
	IEnumerator WaitForParticleDestruction(float time)
	{
		//Debug.Log("Particle start");
		//Debug.Log("wait time " + time);
		yield return new WaitForSeconds(time);
		Debug.Log("Particle ended");
		FinishedAnim();
	}

	
}
