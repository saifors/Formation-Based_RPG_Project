using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundPlayer : MonoBehaviour
{
	public AudioClip[] soundClips;
    public AudioClip[] musicClips;

	public AudioSource[] soundSource;
	public AudioSource[] musicSource;

	public float soundVol;
	public float musicVol;

	public int soundCounter;
	public int cutsceneMusicCounter;

	public int currentMusic;
	public int newMusic;

	public enum MusicType { Overworld, Battle, BossBattle, Cutscene1, Cutscene2, Victory};

	private GameManager gameManager;

	public void Init(GameManager gM, bool initMapMusic)
	{
		gameManager = gM;
		soundVol = PlayerPrefs.GetFloat("Sound_Volume", 1);
		musicVol = PlayerPrefs.GetFloat("Music_Volume", 1);

		soundSource = new AudioSource[10];
		musicSource = new AudioSource[6]; //0 is Overworld, 1 is Battle 2 is special Battle 3 & 4 are cutscene. 5 is Victory

		soundCounter = 0;
		cutsceneMusicCounter = 0;
		currentMusic = -1;

		if(initMapMusic) InitMapMusic();
	}

	public void PlaySound(int num, bool is2D)
    {
		
		GameObject obj = new GameObject();
        obj.transform.position = transform.position;
        obj.name = "AUDIO_" + soundClips[num].name;

        AudioSource source = obj.AddComponent<AudioSource>();
		soundSource[soundCounter] = source;
		AddToSoundCounter();
		source.clip = soundClips[num];
        source.volume = soundVol;
        //source.pitch = pit; //Not necessary but can be optional.
        if(is2D) source.spatialBlend = 0;
        else source.spatialBlend = 1;
        source.Play();

        Destroy(obj, soundClips[num].length); //Don't mistake length(length of clip) with "L"ength (Length of array)
    }
    public void PlaySound(int num, bool is2D, float pit)
    {
		
		GameObject obj = new GameObject();
        obj.transform.position = transform.position;
        obj.name = "AUDIO_" + soundClips[num].name;

        AudioSource source = obj.AddComponent<AudioSource>();
		soundSource[soundCounter] = source;
		AddToSoundCounter();
		source.clip = soundClips[num];
        source.volume = soundVol;
        source.pitch = pit; //Not necessary but can be optional.
        if (is2D) source.spatialBlend = 0;
        else source.spatialBlend = 1;
        source.Play();

        Destroy(obj, soundClips[num].length);
    }

    //Parented to an object?
    public void PlaySound(int num, bool is2D, float pit, Transform parent)
    {
		
		GameObject obj = new GameObject();        
        obj.transform.position = parent.transform.position;
        obj.name = "AUDIO_" + soundClips[num].name;
        obj.transform.SetParent(parent);


        AudioSource source = obj.AddComponent<AudioSource>();
		soundSource[soundCounter] = source;
		AddToSoundCounter();
		source.clip = soundClips[num];
        source.volume = soundVol;
        source.pitch = pit; //Not necessary but can be optional.
        if (is2D) source.spatialBlend = 0;
        else source.spatialBlend = 1;
        source.Play();

        Destroy(obj, soundClips[num].length); //Don't mistake length(length of clip) with "L"ength (Length of array)
    }

	public void InitMapMusic()
	{
		InitOWMusic();
		InitBattleMusic();
		InitBossBattleMusic();
		InitCutsceneMusic();
		InitVictoryMusic();
	}

	public void PlayMusic(int num)
	{
		GameObject obj = new GameObject();
		obj.transform.position = transform.position;
		obj.name = "Music_" + musicClips[num].name;

		AudioSource source = obj.AddComponent<AudioSource>();
		source.loop = true;
		source.clip = musicClips[num];
		source.volume = musicVol;
		source.spatialBlend = 0;
		
		source.Play();
	}

    //PlayMusicLoop?
	public void InitOWMusic()
	{
		GameObject obj = new GameObject();
		obj.transform.position = Vector3.zero;

		int musicID = gameManager.gameData.MapEncounterCollection[gameManager.currentEncounterMap].owMusicID;

		obj.name = "MUSIC_" + musicClips[musicID].name;

		AudioSource source = obj.AddComponent<AudioSource>();
		musicSource[0] = source;
		//AddToMusicCounter();
		source.clip = musicClips[musicID];
		source.volume = musicVol;
		source.spatialBlend = 0;
		source.loop = true;
	}
	public void InitBattleMusic()
	{
		GameObject obj = new GameObject();
		obj.transform.position = Vector3.zero;

		int musicID = gameManager.gameData.MapEncounterCollection[gameManager.currentEncounterMap].battleMusicID;

		obj.name = "MUSIC_" + musicClips[musicID].name;

		AudioSource source = obj.AddComponent<AudioSource>();
		musicSource[1] = source;
		//AddToMusicCounter();
		source.clip = musicClips[musicID];
		source.volume = musicVol;
		source.spatialBlend = 0;
		source.loop = true;
	}
	
	public void InitBossBattleMusic()
	{
		GameObject obj = new GameObject();
		obj.transform.position = Vector3.zero;
		
		obj.name = "MUSIC_Boss";

		AudioSource source = obj.AddComponent<AudioSource>();
		musicSource[2] = source;
		//AddToMusicCounter();
		//source.clip = musicClips[musicID];
		source.volume = musicVol;
		source.spatialBlend = 0;
		source.loop = true;
	}
	public void InitCutsceneMusic()
	{
		GameObject[] obj;
		obj = new GameObject[2];
		for (int i = 0; i < 2; i++)
		{
			obj[i] = new GameObject();
			obj[i].transform.position = Vector3.zero;
			

			obj[i].name = "MUSIC_Cutscene" + i;

			AudioSource source = obj[i].AddComponent<AudioSource>();
			musicSource[3+i] = source;
			//AddToMusicCounter();
			//source.clip = musicClips[musicID];
			source.volume = musicVol;
			source.spatialBlend = 0;
			source.loop = true;
		}
	}
	public void InitVictoryMusic()
	{
		GameObject obj = new GameObject();
		obj.transform.position = Vector3.zero;

		//int musicID = gameManager.gameData.MapEncounterCollection[gameManager.currentEncounterMap].battleMusicID;

		obj.name = "MUSIC_Victory";

		AudioSource source = obj.AddComponent<AudioSource>();
		musicSource[5] = source;
		//Victory Clip needed
		source.clip = musicClips[0];
		source.volume = musicVol;
		source.spatialBlend = 0;
		source.loop = true;
	}

	public void PlayMusic(MusicType mType)
    {
		int id;

		switch (mType)
		{
			case MusicType.Overworld:
				id = 0;
				break;
			case MusicType.Battle:
				id = 1;
				break;
			case MusicType.BossBattle:
				id = 2;
				break;
			case MusicType.Cutscene1:
				id = 3;
				break;
			case MusicType.Cutscene2:
				id = 4;
				break;
			case MusicType.Victory:
				id = 5;
				break;
			default:
				id = 1;
				break;
		}
		
		if (currentMusic >= 0)
		{
			CrossfadeMusic(id, 1.5f);
		}
		else
		{
			musicSource[id].Play();
			currentMusic = id;
		}
	}

	public void CrossfadeMusic(int nM, float duration)
	{
		newMusic = nM;

		musicSource[newMusic].Play();
		musicSource[newMusic].volume = musicVol;
		musicSource[newMusic].DOFade(0, duration).From();

		musicSource[currentMusic].DOFade(0, duration).OnComplete(FinishCrossFade);
	}

	public void NewCutsceneMusic(int musID)
	{

		int id = cutsceneMusicCounter + 3;

		musicSource[id].clip = musicClips[musID];

		if(id == 3) PlayMusic(MusicType.Cutscene1);
		else PlayMusic(MusicType.Cutscene2);
		
		
		AddToMusicCounter();
	}

	public void NewBossMusic(int musID)
	{
		musicSource[2].clip = musicClips[musID];
		PlayMusic(MusicType.BossBattle);
	}

	public void FinishCrossFade()
	{
		musicSource[currentMusic].Stop();

		currentMusic = newMusic;
	}

	public void StopMusic(int i)
	{
		Destroy(musicSource[i]);
	}

	public void UpdateSoundVol()
	{
		soundVol = PlayerPrefs.GetFloat("Sound_Volume");
		for (int i = 0; i < soundSource.Length; i++)
		{
			if(soundSource[i] != null) soundSource[i].volume = soundVol;
		}
	}
	public void UpdateMusicVol()
	{
		musicVol = PlayerPrefs.GetFloat("Music_Volume");
		for (int i = 0; i < musicSource.Length; i++)
		{
			if (musicSource[i] != null) musicSource[i].volume = musicVol;
		}
	}

	public void AddToSoundCounter()
	{
		soundCounter++;
		if(soundCounter >= soundSource.Length)
		{
			soundCounter = 0;
		}
	}
	public void AddToMusicCounter()
	{
		cutsceneMusicCounter++;
		if (cutsceneMusicCounter > 1)
		{
			cutsceneMusicCounter = 0;
		}
	}
}
