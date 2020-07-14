using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Main { get; private set; }

    [SerializeField] private BallController ball;

    [SerializeField] private Text countDownText;

    [SerializeField] private Text timerText;

    [SerializeField] private Text scoreTextLeft;

    [SerializeField] private Text scoreTextRight;

    [SerializeField] private Text winText;

    [SerializeField] private int timeLimit = 60;

    private int _scoreA = 0;

    private int _scoreB = 0;

    private float _timeRemaining;

    private PlayerController[] _players;

    public BallController Ball => ball;

    public bool enabledCatching = true;

    private bool _isTimeRunning;

    private WaitForSeconds _waitForOneSecond;

    // Start is called before the first frame update
    private void Start()
    {
        Main = this;
        _waitForOneSecond = new WaitForSeconds(1);
        _timeRemaining = timeLimit;
        _players = GameObject.FindGameObjectsWithTag("Player").Select(x => x.GetComponent<PlayerController>()).ToArray();
        UpdateTimerText();
        StartCoroutine(StartGame());
    }

    public void DisableCatching()
    {
        enabledCatching = false;
    }
    
    public void EnableCatching()
    {
        enabledCatching = true;
    }

    public void EnableAllPlayerMovement()
    {
        foreach (var player in _players)
        {
            player.EnableMovement();
        }
    }

    public void DisableAllPlayerMovement()
    {
        foreach (var player in _players)
        {
            player.DisableMovement(false);
        }
    }

    public void StopTimer()
    {
        _isTimeRunning = false;
    }

    public void ResetGame()
    {
        _isTimeRunning = false;
        StartCoroutine(StartGame());
    }

    public void GiveScore(GoalZone.Side side)
    {
        if (side == GoalZone.Side.A)
        {
            _scoreA++;
        }
        else
        {
            _scoreB++;
        }
        UpdateScoreText();
    }

    private IEnumerator StartGame()
    {
        countDownText.gameObject.SetActive(true);
        Ball.ResetBall();
        DisableAllPlayerMovement();
        
        countDownText.text = "3";
        yield return _waitForOneSecond;

        countDownText.text = "2";
        yield return _waitForOneSecond;

        countDownText.text = "1";
        yield return _waitForOneSecond;

        countDownText.gameObject.SetActive(false);
        Ball.ShootBallRandom();
        _isTimeRunning = true;
        EnableAllPlayerMovement();
    }

    private void Update()
    {
        if (!_isTimeRunning) return;
        if (_timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;
        }
        else
        {
            _timeRemaining = 0.0f;
            var winSide = GoalZone.Side.Both;
            if (_scoreA > _scoreB)
            {
                winSide = GoalZone.Side.A;
            }
            else if (_scoreB > _scoreA)
            {
                winSide = GoalZone.Side.B;
            }
            WinGame(winSide);
            StopTimer();
        }
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        timerText.text = Mathf.FloorToInt(_timeRemaining).ToString();
    }

    private void UpdateScoreText()
    {
        scoreTextLeft.text = _scoreA.ToString();
        scoreTextRight.text = _scoreB.ToString();
    }

    private void WinGame(GoalZone.Side side)
    {
        DisableAllPlayerMovement();
        Ball.ResetBall();
        winText.gameObject.SetActive(true);
        if (side == GoalZone.Side.Both)
        {
            winText.text = "DRAW!!!";
        }
        else
        {
            winText.text = $"{side.ToString()} WIN!!!";
        }
    }
}
