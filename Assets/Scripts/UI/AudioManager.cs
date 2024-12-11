using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] soundSource;
    public AudioSource musicSource;
    int soundIndex = 0;

    Dictionary<string, AudioClip> soundLibrary = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> musicLibrary = new Dictionary<string, AudioClip>();

    [SerializeField]
    AudioClip[] soundClips;
    [SerializeField]
    AudioClip[] musicClips;

    private void Start()
    {
        SetSoundVolume(PlayerPrefs.GetFloat("SoundVolume", 1));
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1);

        foreach (var clip in soundClips)
        {
            soundLibrary.Add(clip.name, clip);
        }
        foreach (var clip in musicClips)
        {
            musicLibrary.Add(clip.name, clip);
        }
    }

    public void SetSoundVolume(float volume)
    {
        for (int i = 0; i < soundSource.Length; i++)
        {
            soundSource[i].volume = volume;
        }
    }

    public static void PlaySound(string name)
    {
        var audio = GameManager.Instance.audioManager;

        if (audio.soundLibrary.ContainsKey(name))
        {
            audio.soundSource[audio.soundIndex].clip = audio.soundLibrary[name];
            audio.soundSource[audio.soundIndex].Play();
            audio.soundIndex = (audio.soundIndex + 1) % audio.soundSource.Length;
        }
        else
        {
            Debug.LogWarning("Sound not found in the library.");
        }
    }

    public static void PlayMusic(string name, bool loop = true)
    {
        var audio = GameManager.Instance.audioManager;
        if (audio.musicLibrary.ContainsKey(name))
        {
            audio.musicSource.clip = audio.musicLibrary[name];
            audio.musicSource.loop = loop;
            audio.musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music not found in the library.");
        }
    }
}
