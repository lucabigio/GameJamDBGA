using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoSingleton<AudioController>
{
    public AudioClip[] AudioClips;
    public List<AudioSource> AudioSources;
    public float MasterVolume;

    private void Start()
    {
        GetComponent<AudioLowPassFilter>().enabled = false;
        AudioSources[1].ignoreListenerPause = true;
        AudioSources[2].ignoreListenerPause = true;
    }

    public void Play(int audioIndex)
    {
        AudioSources[0].PlayOneShot(AudioClips[audioIndex]);
    }

    public void PlayBtnEffect(int audioIndex)
    {
        AudioSources[2].PlayOneShot(AudioClips[audioIndex]);
    }

    public void Play(string audioName)
    {
        foreach (AudioClip clips in AudioClips)
        {
            if (clips.name.Equals(audioName))
            {
                AudioSources[0].PlayOneShot(clips);
            }
        }
    }

    public void PlayLoop(int audioIndex)
    {
        AudioSources[0].clip = AudioClips[audioIndex];
        AudioSources[0].loop = true;
        AudioSources[0].Play();
    }

    public void StopLoop(int audioIndex)
    {
        AudioSources[0].clip = AudioClips[audioIndex];
        AudioSources[0].loop = false;
        AudioSources[0].Stop();
    }

    public void PlayMusic(int audioIndex, bool IsLooped)
    {
        AudioSources[1].clip = AudioClips[audioIndex];
        AudioSources[1].loop = IsLooped;
        AudioSources[1].Play();
    }
    public void StopMusic(int audioIndex)
    {
        AudioSources[1].clip = AudioClips[audioIndex];
        AudioSources[1].loop = false;
        AudioSources[1].Stop();
    }
    public void PlayMusic(string audioName)
    {
        foreach (AudioClip clips in AudioClips)
        {
            if (clips.name.Equals(audioName))
            {
                AudioSources[1].PlayOneShot(clips);
            }
        }
    }
    public void Stop(int audioIndex)
    {
        AudioSources[audioIndex].Stop();
    }

    public void Stop(string audioName)
    {
        foreach (AudioSource source in AudioSources)
        {
            if (source.name.Equals(audioName))
            {
                source.Stop();
            }
        }
    }

    public void ChangeSFXVolume(float volume)
    {
        AudioSources[0].volume = volume * MasterVolume;
        AudioSources[2].volume = volume * MasterVolume;
    }

    public void ChangeMusicVolume(float volume)
    {
        AudioSources[1].volume = volume * MasterVolume;
    }

    public void ChangeMaster(float volumePerc)
    {
        MasterVolume = volumePerc;
    }
}
