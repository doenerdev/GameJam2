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

    private AudioClip _dotReverseSound;

    private AudioClip _badgeSound;
    private AudioClip _ratingStarSound;
    private AudioClip _levelJoy;

    private int _dotSelectedIndexbefore = -1;
    private AudioSource _audioSource = new AudioSource();
    private AudioSource _audioSourceSecond = new AudioSource();

    private void Awake()
    {
        _dotSelectedSound1 = Resources.Load("DotConnectedSound1") as AudioClip;
        _dotSelectedSound2 = Resources.Load("DotConnectedSound2") as AudioClip;
        _dotSelectedSound3 = Resources.Load("DotConnectedSound3") as AudioClip;
        _dotSelectedSound4 = Resources.Load("DotConnectedSound4") as AudioClip;

        _dotReverseSound = Resources.Load("DotRevert") as AudioClip;

        _badgeSound = Resources.Load("BadgeSound") as AudioClip;
        _ratingStarSound = Resources.Load("Bing") as AudioClip;
        _levelJoy = Resources.Load("Level Completed") as AudioClip;

        _doteSelectedSounds = new AudioClip[] {_dotSelectedSound1, _dotSelectedSound2, _dotSelectedSound3, _dotSelectedSound4};
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSourceSecond = gameObject.AddComponent<AudioSource>();
    }

    private void ReloadSources()
    {
        _audioSource = Camera.main.gameObject.AddComponent<AudioSource>();
        _audioSourceSecond = Camera.main.gameObject.AddComponent<AudioSource>();
    }

    public void PlayDotConnectedSound()
    {
        if(_audioSource == null)
            ReloadSources();

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

    public void PlayBadgeSound()
    {
        if (_audioSource == null)
            ReloadSources();

        Debug.Log("Sound2");
        if(_audioSourceSecond == null) Debug.Log("Waaaaaaaaaaaaaah");
        _audioSourceSecond.clip = _badgeSound;
        _audioSourceSecond.Play();
    }

    public void PlayRatingStarSound()
    {
        if (_audioSource == null)
            ReloadSources();

        Debug.Log("Sound");
        _audioSourceSecond.clip = _ratingStarSound;
        _audioSourceSecond.Play();
    }

    public void PlayLevelJoySound()
    {
        if (_audioSource == null)
            ReloadSources();

        Debug.Log("Sound");
        _audioSource.clip = _levelJoy;
        _audioSource.Play();
    }

    public void PlayRevertSound()
    {
        if (_audioSource == null)
            ReloadSources();

        _audioSource.clip = _dotReverseSound;
        _audioSource.Play();
    }
}
