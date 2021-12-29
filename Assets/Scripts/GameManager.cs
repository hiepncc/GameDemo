using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  public GameObject ballPrefab;
  public GameObject playerPrefab;
  public GameObject panelPlay;
  public GameObject panelMenu;
  public GameObject panelLevelCompleted;
  public GameObject panelGameOver;
  public GameObject[] levels;
  public Text scoreText;
  public Text ballsText;
  public Text levelText;
  public Text highScoreText;

  //   bool _isSwitchingState;
  GameObject _curentBall;
  GameObject _curentLevel;
  GameObject _curentPlayer;
  State _state;

  public enum State { MENU, INIT, LOADLEVEL, PLAY, LEVELCOMPLETED, GAMEOVER }
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
    PlayerPrefs.DeleteKey("highScore");
    Instance = this;
    switchSate(State.MENU);

  }

  // Update is called once per frame
  void Update()
  {
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
          }
          else
          {
            switchSate(State.GAMEOVER);
          }
        }
        // if (_curentLevel != null && _curentLevel.transform.childCount == 0 && !_isSwitchingState)
        if (_curentLevel != null && _curentLevel.transform.childCount == 0)
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

  public void playerClicked()
  {
    switchSate(State.INIT);
  }
  private void switchSate(State newState, float delay = 0)
  {
    StartCoroutine(SwitchDelay(newState, delay));
  }

  IEnumerator SwitchDelay(State newState, float delay)
  {
    // _isSwitchingState = true;
    yield return new WaitForSeconds(delay);
    endState();
    _state = newState;
    beginState(newState);
    // _isSwitchingState = false;
  }

  private void beginState(State newState)
  {
    switch (newState)
    {
      case State.MENU:
        Cursor.visible = true;
        panelMenu.SetActive(true);
        break;
      case State.INIT:
        Cursor.visible = false;
        panelPlay.SetActive(true);
        Score = 0;
        Level = 1;
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
        panelPlay.SetActive(true);
        break;
      case State.LEVELCOMPLETED:
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
