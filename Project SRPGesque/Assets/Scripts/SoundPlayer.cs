using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
public AudioClip[] soundClips;
    public AudioClip[] musicClips;

    public void PlaySound(int num, float vol, bool is2D)
    {
        GameObject obj = new GameObject();
        obj.transform.position = transform.position;
        obj.name = "AUDIO_" + soundClips[num].name;

        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = soundClips[num];
        source.volume = vol;
        //source.pitch = pit; //Not necessary but can be optional.
        if(is2D) source.spatialBlend = 0;
        else source.spatialBlend = 1;
        source.Play();

        Destroy(obj, soundClips[num].length); //Don't mistake length(length of clip) with "L"ength (Length of array)
    }
    public void PlaySound(int num, float vol, bool is2D, float pit)
    {
        GameObject obj = new GameObject();
        obj.transform.position = transform.position;
        obj.name = "AUDIO_" + soundClips[num].name;

        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = soundClips[num];
        source.volume = vol;
        source.pitch = pit; //Not necessary but can be optional.
        if (is2D) source.spatialBlend = 0;
        else source.spatialBlend = 1;
        source.Play();

        Destroy(obj, soundClips[num].length);
    }

    //Parented to an object?
    public void PlaySound(int num, float vol, bool is2D, float pit, Transform parent)
    {
        GameObject obj = new GameObject();        
        obj.transform.position = parent.transform.position;
        obj.name = "AUDIO_" + soundClips[num].name;
        obj.transform.SetParent(parent);


        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = soundClips[num];
        source.volume = vol;
        source.pitch = pit; //Not necessary but can be optional.
        if (is2D) source.spatialBlend = 0;
        else source.spatialBlend = 1;
        source.Play();

        Destroy(obj, soundClips[num].length); //Don't mistake length(length of clip) with "L"ength (Length of array)
    }

    //PlayMusicLoop?
    //UNFINISHED, TODO: Figure out how to loop.
    public void PlayMusic(int num, float vol)
    {
        GameObject obj = new GameObject();
        obj.transform.position = transform.position;
        obj.name = "MUSIC_" + musicClips[num].name;

        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = musicClips[num];
        source.volume = vol;
        source.spatialBlend = 1;
        source.loop = true;
        source.Play();

    }
}
