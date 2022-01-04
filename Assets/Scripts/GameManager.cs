using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject playerPrefab;
    public GameObject panelPlay;
    public GameObject panelMenu;
    public GameObject panelPickLevel;
    public GameObject panelConfig;
    public GameObject panelLevelCompleted;
    public GameObject panelGameOver;
    public GameObject[] levels;
    public GameObject walls;
    public Text scoreText;
    public Text ballsText;
    public Text levelText;
    public Text highScoreText;
    public ToggleGroup toggleGr;
    private float _speed = 30;
    private int muted;

    bool _isSwitchingState;
    GameObject _curentBall;
    GameObject _curentLevel;
    GameObject _curentPlayer;
    State _state;

    public enum State { MENU, PICKLEVEL, CONFIG, INIT, LOADLEVEL, PLAY, LEVELCOMPLETED, GAMEOVER }
    public static GameManager Instance { get; private set; }

    private int _score;
    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            scoreText.text = "SCORE: " + _score;
        }
    }

    private int _level;
    public int Level
    {
        get { return _level; }
        set
        {
            _level = value;
            levelText.text = "LEVEL: " + _level;
        }
    }

    private int _balls;

    public int Balls
    {
        get { return _balls; }
        set
        {
            _balls = value;
            ballsText.text = "BALLS: " + _balls;
        }

    }



    // Start is called before the first frame update
    void Start()
    {
        // muted = PlayerPrefs.GetInt("Muted");
        if (!PlayerPrefs.HasKey("ToggleSelected"))
        {
            PlayerPrefs.SetInt("ToggleSelected", 1);
        }
        PlayerPrefs.DeleteKey("highScore");
        Instance = this;
        switchSate(State.MENU);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Balls = 0;
            Destroy(_curentBall);
            Destroy(_curentLevel);
            Destroy(_curentPlayer);
            switchSate(State.MENU);
        }
        switch (_state)
        {
            case State.MENU:
                highScoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("highScore");
                break;
            case State.INIT:
                break;
            case State.LOADLEVEL:
                break;
            case State.PLAY:
                if (_curentBall == null)
                {
                    if (Balls > 0)
                    {
                        _curentBall = Instantiate(ballPrefab);
                        _curentBall.GetComponent<Ball>().changeSpeed(_speed);
                    }
                    else
                    {
                        switchSate(State.GAMEOVER);
                    }
                }
                if (_curentLevel != null && _curentLevel.transform.childCount == 0 && !_isSwitchingState)
                //if (_curentLevel != null && _curentLevel.transform.childCount == 0)
                {
                    switchSate(State.LEVELCOMPLETED);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.GAMEOVER:
                if (Input.anyKey)
                {
                    switchSate(State.MENU);
                }
                break;
        }
    }

    public void playBtnClicked()
    {
        switchSate(State.INIT);
    }

    public void levelsBtnClicked()
    {
        switchSate(State.PICKLEVEL);
    }

    public void changeSpeed(float speed)
    {
        _speed = speed;
    }
    public void goToMenu()
    {
        switchSate(State.MENU);
    }
    public void configBtnClicked()
    {
        switchSate(State.CONFIG);
    }

    public void quitBtnClicked()
    {
        Application.Quit();
    }
    private void switchSate(State newState, float delay = 0)
    {
        StartCoroutine(SwitchDelay(newState, delay));
    }

    IEnumerator SwitchDelay(State newState, float delay)
    {
        _isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        endState();
        _state = newState;
        beginState(newState);
        _isSwitchingState = false;
    }

    private void beginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                walls.SetActive(false);
                Cursor.visible = true;
                panelMenu.SetActive(true);
                break;
            case State.PICKLEVEL:
                //Cursor.visible = true;
                panelPickLevel.SetActive(true);
                break;
            case State.CONFIG:
                //Cursor.visible = true;
                panelConfig.SetActive(true);
                break;
            case State.INIT:
                Cursor.visible = false;
                panelPlay.SetActive(true);
                Score = 0;
                Level = PlayerPrefs.GetInt("ToggleSelected");
                Balls = 3;
                if (_curentLevel != null)
                {
                    Destroy(_curentLevel);
                }
                _curentPlayer = Instantiate(playerPrefab);
                switchSate(State.LOADLEVEL);
                break;
            case State.LOADLEVEL:
                if (Level > levels.Length)
                {
                    switchSate(State.GAMEOVER);
                }
                else
                {
                    _curentLevel = Instantiate(levels[Level - 1]);
                    switchSate(State.PLAY);
                }
                break;
            case State.PLAY:
                walls.SetActive(true);
                panelPlay.SetActive(true);
                break;
            case State.LEVELCOMPLETED:
                walls.SetActive(false);
                Destroy(_curentBall);
                Destroy(_curentLevel);
                Level++;
                panelLevelCompleted.SetActive(true);
                switchSate(State.LOADLEVEL, 1.5f);
                break;
            case State.GAMEOVER:
                if (Score > PlayerPrefs.GetInt("highScore"))
                {
                    PlayerPrefs.SetInt("highScore", Score);
                }
                Destroy(_curentPlayer);
                panelGameOver.SetActive(true);
                break;
        }
    }

    private void endState()
    {
        switch (_state)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.PICKLEVEL:
                panelPickLevel.SetActive(false);
                break;
            case State.CONFIG:
                panelConfig.SetActive(false);
                break;
            case State.INIT:
                //panelPlay.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                panelLevelCompleted.SetActive(false);
                break;
            case State.GAMEOVER:
                panelPlay.SetActive(false);
                panelGameOver.SetActive(false);
                break;
        }
    }
}
