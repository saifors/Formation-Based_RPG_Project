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
		//Particle animation I guess???
		for (int target = 0; target < gameManager.selectedTargets.Length; target++)
		{
			//Instantiate & play particle animation On targeted tiles. 
			GameObject particle = Instantiate(particleAnim[0]);
			parts.Add(particle);
			spellAnims.Add(particle.GetComponent<SpellAnim>());
			spellAnims[target].Init();
			spellAnims[target].trans.position = gameManager.selectedTargetsTransform[target].position;			
		}

		//WaitForParticleDestruction(spellAnims[0].particles[0].startLifetime);

		FinishedAnim();
	}

	public void HitAnim(Animator anim)
	{
		Debug.Log("HitAnim");
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
		if(anim != null) yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + 1);

		Debug.Log("Animation ended");
		//wait = false;
		FinishedAnim();
	}
	IEnumerator WaitForParticleDestruction(float time)
	{
		yield return new WaitForSeconds(time);
		for (int i = 0; i < parts.Count; i++)
		{
			Destroy(parts[i]);
		}
		//Destroy();
		FinishedAnim();
	}
}
