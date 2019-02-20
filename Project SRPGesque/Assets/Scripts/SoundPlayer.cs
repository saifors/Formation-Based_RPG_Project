using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
	public AudioClip[] soundClips;
    public AudioClip[] musicClips;

	public AudioSource[] soundSource;
	public AudioSource[] musicSource;

	public float soundVol;
	public float musicVol;

	public int soundCounter;
	public int musicCounter;

	private void Start()
	{
		soundVol = PlayerPrefs.GetFloat("Sound_Volume", 1);
		musicVol = PlayerPrefs.GetFloat("Music_Volume", 1);

		soundSource = new AudioSource[10];
		musicSource = new AudioSource[3];

		soundCounter = 0;
		musicCounter = 0;
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

    //PlayMusicLoop?
    //UNFINISHED, TODO: Figure out how to loop.
    public void PlayMusic(int num)
    {
		
		GameObject obj = new GameObject();
        obj.transform.position = transform.position;
        obj.name = "MUSIC_" + musicClips[num].name;

        AudioSource source = obj.AddComponent<AudioSource>();
		musicSource[musicCounter] = source;
		AddToMusicCounter();
		source.clip = musicClips[num];
        source.volume = musicVol;
        source.spatialBlend = 1;
        source.loop = true;
        source.Play();

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
		musicCounter++;
		if (musicCounter >= musicSource.Length)
		{
			musicCounter = 0;
		}
	}
}
