using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameMenu : SingletonClass<InGameMenu>
{
    [SerializeField] private Image _revertBttnBg;
    [SerializeField] private Image _revertBttnIcon;
    [SerializeField] private Image[] _revertSteps;
    [SerializeField] private Animator _revertStepAnimator;
    [SerializeField] private Color _revertStepsColor;

    [SerializeField] private GameObject _levelComplete;
    [SerializeField] private Image[] _ratingStars;
    [SerializeField] private Color _ratingColor;
    [SerializeField] private GameObject _blackBG;

    [SerializeField] private Text _timerText;

    private bool _ratingReady = false;
    private int _rating;


    public void DisableRevertStep(int step)
    {
        _revertSteps[step].color = _revertStepsColor;
    }

    public void DisableRevertButton()
    {
        //_revertBttnBg.color = Color.red;
        _revertStepAnimator.enabled = false;
    }

    public void ShowLevelComplete(int rating)
    {
        AudioManager.instance.PlayLevelJoySound();
        _blackBG.SetActive(true);
        _levelComplete.SetActive(true);
        _rating = rating;
    }

    public void SetRatingReady(bool ready)
    {
        _ratingReady = ready;
        if(_ratingReady == true)
            StartCoroutine("EnableRatingStars", _rating);
    }

    private IEnumerator EnableRatingStars(int rating)
    {
        yield return new WaitForSeconds(1.2f);
        for (int i = 0; i < rating; i++)
        {
            _ratingStars[i].color = _ratingColor;
            AudioManager.instance.PlayRatingStarSound();
            yield return new WaitForSeconds(0.65f);
        }
    }

    private void Update()
    {
        float minutesF = Mathf.Floor(DotManager.instance.Timer / 60);
        float secondsF = Mathf.RoundToInt(DotManager.instance.Timer % 60);
        string minutes = "";
        string seconds = "";

        if (minutesF < 10)
            minutes = "0" + minutesF.ToString();
        else
            minutes = minutesF.ToString();
        if (secondsF < 10)
            seconds = "0" + Mathf.RoundToInt(secondsF).ToString();
        else
            seconds = Mathf.RoundToInt(secondsF).ToString();

        if(_timerText != null)
            _timerText.text = minutes + ":" + seconds;
    }
}