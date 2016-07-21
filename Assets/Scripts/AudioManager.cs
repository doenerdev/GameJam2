using UnityEngine;
using System;
using System.Collections;

public class AudioManager : SingletonClass<AudioManager>
{

    private AudioClip _dotSelectedSound1;
    private AudioClip _dotSelectedSound2;
    private AudioClip _dotSelectedSound3;
    private AudioClip _dotSelectedSound4;
    private AudioClip[] _doteSelectedSounds;
    private int _dotSelectedIndexbefore = -1;
    private AudioSource _audioSource = new AudioSource();

    private void Awake()
    {
        _dotSelectedSound1 = Resources.Load("DotConnectedSound1") as AudioClip;
        _dotSelectedSound2 = Resources.Load("DotConnectedSound2") as AudioClip;
        _dotSelectedSound3 = Resources.Load("DotConnectedSound3") as AudioClip;
        _dotSelectedSound4 = Resources.Load("DotConnectedSound4") as AudioClip;
        _doteSelectedSounds = new AudioClip[] {_dotSelectedSound1, _dotSelectedSound2, _dotSelectedSound3, _dotSelectedSound4};
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayDotConnectedSound()
    {

        System.Random rnd = new System.Random();
        var rand = rnd.Next(0, 4);
        while (rand == _dotSelectedIndexbefore)
        {
            rand = rnd.Next(0, 4);
        }
        _dotSelectedIndexbefore = rand;

        Debug.Log(rand);
        _audioSource.clip = _doteSelectedSounds[rand];
        _audioSource.Play();
    }
}
