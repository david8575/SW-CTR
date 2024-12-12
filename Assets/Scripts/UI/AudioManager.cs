using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] soundSource;
    public AudioSource musicSource;
    int soundIndex = 0;
    float soundVolume = 1;

    Dictionary<string, AudioClip> soundLibrary = new Dictionary<string, AudioClip>();

    public int[] stageBGM;

    [SerializeField]
    AudioClip[] soundClips;
    [SerializeField]
    AudioClip[] musicClips;

    private void Start()
    {
        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1);
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1);

        foreach (var clip in soundClips)
        {
            soundLibrary.Add(clip.name, clip);
        }
    }

    public void SetSoundVolume(float volume)
    {
        soundVolume = volume;
    }

    public static void PlaySound(string name, float volume = 1, float pitch = 1)
    {
        var audio = GameManager.Instance.audioManager;


        if (audio.soundLibrary.ContainsKey(name))
        {
            audio.soundSource[audio.soundIndex].clip = audio.soundLibrary[name];
            audio.soundSource[audio.soundIndex].volume = audio.soundVolume * volume;
            audio.soundSource[audio.soundIndex].pitch = pitch;
            audio.soundSource[audio.soundIndex].Play();
            audio.soundIndex = (audio.soundIndex + 1) % audio.soundSource.Length;
        }
        else
        {
            Debug.LogWarning("Sound not found in the library.");
        }
    }

    public void PlayMusic(int idx, bool loop = true)
    {
        Debug.Log("PlayMusic: " + idx);
        if (idx < 0 || idx >= musicClips.Length)
        {
            musicSource.Stop();
            return;
        }

        if (idx < musicClips.Length)
        {
            musicSource.clip = musicClips[idx];
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music not found in the library.");
        }
    }
}
