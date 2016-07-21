using UnityEngine;
using System.Collections;

public class FinishedLevelAnim : MonoBehaviour {

    public void LevelAnimComplete()
    {
        InGameMenu.instance.SetRatingReady(true);
    }

    public void PlayBadgeSound()
    {
        AudioManager.instance.PlayBadgeSound();
    }
}
