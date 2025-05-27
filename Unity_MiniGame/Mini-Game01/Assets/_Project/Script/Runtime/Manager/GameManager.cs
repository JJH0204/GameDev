using System;
using System.Collections;
using UnityEngine;
public class GameManager : ManagerBase
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance is null)
                {
                    GameObject obj = new GameObject("GameManager");
                    _instance = obj.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    #region Variables

    private float _time;
    private UserData _userData;
    private int _saveCombo; // fever 모드 콤보 저장
    
    private GameState gameState { get; set; } = GameState.None;
    private bool _saveHistoryDataJsonFile;
    
    #endregion

    #region Cache
    
    // private GameObject _objNoteGroup;
    private NoteGroupScript _noteGroupScript;
    private GameSceneUI _gameSceneUI;
    private float _pauseTime;

    #endregion

    #region Unity Methods
    private void Awake()
    {
        DontDestroy<GameManager>();
        _userData = new UserData();
    }

    private void Update()
    {
        if (SceneLoadManager.instance.IsSceneGame())
        {
            gameState = GameProcess(gameState);
        }
    }

    #endregion

    #region Custom Methods
    
    private GameState GameProcess(GameState state)
    {
        var newState = state;
        // 게임 매니저가 게임 씬에 도달했을 때 처리할 작업
        switch (state)
        {
            case GameState.None:
                newState = GameState.Init;
                break;
            case GameState.Init:
                if (Init())
                    newState =  GameState.Ready;
                break;
            case GameState.Ready:
                // 게임 시작 전 3초 카운트다운
                if (!_gameSceneUI.IsCountDownOn())
                    _gameSceneUI.SetCountDown(true);
                if (_gameSceneUI.IsFinished())
                {
                    _gameSceneUI.CountDownOff();
                    newState = GameState.Playing;
                }
                break;
            case GameState.Playing:
                if (IsGameOver())
                {
                    _time = 0;
                    newState = GameState.GameOver;  // 게임 오버 상태로 전환
                }
                
                // fever 모드 처리
                if (CanFever())
                {
                    _saveCombo = _userData.currentCombo;
                    if (!_gameSceneUI.IsFeverOn())
                        _gameSceneUI.StartFever(LocalDataManager.instance.gameData.FeverTimeLimit);
                }
                
                // fever 모드 Note 처리
                if (_gameSceneUI.IsFeverOn())
                {
                    _noteGroupScript.ChangeNoteType(NoteType.RainbowApple);
                }
                
                _time -= Time.deltaTime;    // 시간 경과 처리
                _gameSceneUI.SetTime(_time); // 게임 UI에 남은 시간 표시

                break;
            case GameState.Pause:
                /// 게임 일시 정지 기능 구현
                if (_pauseTime == 0)
                    _pauseTime = _time;
                _gameSceneUI.SetPausePopup(true);
                _time = _pauseTime;

                break;
            case GameState.GameOver:
                
                // 게임 종료 시 최고 점수, 최고 콤보 갱신
                UpdateHighestUserData();

                // 게임 오버 팝업의 버튼 클릭에 따라 씬 전환 처리
                // _gameSceneUI.SetEndingPopup(true, _userData, CleanUp(), () => { }, Application.Quit);
                _gameSceneUI.SetEndingPopup(true, _userData);
                
                // TODO: 게임 진행 결과 히스토리 저장
                if (!_saveHistoryDataJsonFile)
                    _saveHistoryDataJsonFile = LocalDataManager.instance.SaveHistoryDataJsonFile(_userData); 
                // TODO: 다시하기 버튼 클릭 시 _saveHistoryDataJsonFile = false; 실행
                
                break;
            default:
                // 예외 처리
                Debug.LogError("Invalid game state.");
                break;
        }
        return newState;
    }

    private void UpdateHighestUserData()
    {
        if (_userData.bestCombo < _userData.highCombo)
            _userData.bestCombo = _userData.highCombo;

        if (_userData.bestScore < _userData.currentScore)
            _userData.bestScore = _userData.currentScore;
    }

    private bool CanFever()
    {
        return (_userData.currentCombo > 0) &&
               (LocalDataManager.instance.gameData.FeverCombo != 0) &&
               (_userData.currentCombo % LocalDataManager.instance.gameData.FeverCombo == 0) &&
               (_userData.currentCombo != _saveCombo);
    }

    // 게임 종료 조건 검사
    private bool IsGameOver()
    {
        return _time <= 0;
    }
    
    // 게임이 진행중인지를 반환하는 메서드
    public bool IsGamePlaying()
    {
        return gameState == GameState.Playing;
    }
    
    // 게임 초기화 (점수, 콤보, 노트 그룹 오브젝트, 게임 UI 등)
    private bool Init()
    {
        if (!SceneLoadManager.instance.IsSceneGame())
        {
            // Debug.Log("게임 씬이 아닙니다.");
            return false;
        }

        // 게임 초기화
        _time = LocalDataManager.instance.gameData.TimeLimit;
        _userData = new UserData();
        _saveCombo = 0;
        
        // 노트 그룹 오브젝트 초기화
        StartCoroutine(WaitForNoteGroup());

        // 게임 UI 초기화
        StartCoroutine(WaitForGameSceneUI());

        // 모든 코루틴이 완료되면 true 반환
        return _noteGroupScript is not null && _gameSceneUI is not null;
    }
    
    // 게임 재시작 시 기존 인스턴스 삭제
    private void CleanUp()
    {
        // Debug.Log("CleanUp() 호출됨");
        if (gameState != GameState.GameOver) return;
        _gameSceneUI.ResetEndingPopup();
        _userData = new UserData();
        SetScoreComboUI(_userData);
        _time = LocalDataManager.instance.gameData.TimeLimit;

        // 노트 그룹 오브젝트 초기화
        if (_noteGroupScript is not null)
        {
            _noteGroupScript.ReLoad();
        }

        // Debug.Log("CleanUp() <UNK>");
    }

    private void SetScoreComboUI(UserData userData)
    {
        _gameSceneUI.SetScore(userData.currentScore);
        _gameSceneUI.SetCombo(userData.currentCombo);
    }

    public void InputProcess(InputType inputType)
    {
        bool isSuccess = false;
        // 게임 진행 중에만 입력 받도록 조건문 추가
        if (!IsGamePlaying())
        {
            // Debug.Log("게임이 진행 중이 아닙니다.");
            return;
        }
        
        try
        {
            if (_noteGroupScript is null)
            {
                Debug.LogError("NoteGroup_Script not found.");
                return;
            }

            switch (inputType)
            {
                // 입력 처리
                // Debug.Log("Have 버튼 클릭됨");
                case InputType.Catch when _noteGroupScript.GetNoteType(0) == NoteType.Apple:
                    _userData.currentScore += LocalDataManager.instance.gameData.PointApple;
                    _userData.currentCombo++;
                    isSuccess = true;
                    break;
                case InputType.Catch when _noteGroupScript.GetNoteType(0) == NoteType.GoldApple:
                    _userData.currentScore += LocalDataManager.instance.gameData.PointGoldApple;
                    _userData.currentCombo++;
                    isSuccess = true;
                    break;
                case InputType.Catch when _noteGroupScript.GetNoteType(0) == NoteType.RainbowApple:
                    _userData.currentScore += LocalDataManager.instance.gameData.PointRainbowApple;
                    isSuccess = true;
                    break;
                case InputType.Catch:
                    _userData.currentScore -= LocalDataManager.instance.gameData.PointRottenApple;
                    _userData.currentCombo = 0;
                    _saveCombo = 0;
                    isSuccess = false;
                    break;
                // Debug.Log("Throw 버튼 클릭됨");
                case InputType.Throw when _noteGroupScript.GetNoteType(0) == NoteType.RainbowApple:
                    _userData.currentScore += LocalDataManager.instance.gameData.PointRainbowApple;
                    isSuccess = true;
                    break;
                case InputType.Throw when _noteGroupScript.GetNoteType(0) != NoteType.RottenApple:
                    _userData.currentScore -= LocalDataManager.instance.gameData.PointRottenApple;
                    _userData.currentCombo = 0;
                    _saveCombo = 0;
                    isSuccess = false;
                    break;
                case InputType.Throw:
                    _userData.currentCombo++;
                    isSuccess = true;
                    break;
                
            }

            // _userData의 콤보를 갱신
            if (_userData.highCombo < _userData.currentCombo)
                _userData.highCombo = _userData.currentCombo;
            
            _noteGroupScript.NoteProcess(inputType, isSuccess);
            _gameSceneUI.SetScore(_userData.currentScore);
            _gameSceneUI.SetCombo(_userData.currentCombo);
        }
        catch (Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
    }
    
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        CleanUp();
        gameState = GameState.None;
    }

    public void KeepGoing()
    {
        gameState = GameState.Ready;
    }

    public void PauseGame()
    {
        gameState = GameState.Pause;
    }
    #endregion

    #region Coroutines
    private IEnumerator WaitForNoteGroup()
    {
        while (_noteGroupScript is null)
        {
            yield return new WaitForSeconds(0.5f);
            _noteGroupScript = GameSceneInit.instance.GetNoteGroupScript();
        }
        
    }
    private IEnumerator WaitForGameSceneUI()
    {
        while (_gameSceneUI is null)
        {
            yield return new WaitForSeconds(0.5f);
            _gameSceneUI = GameSceneInit.instance.GetGameSceneUI();
        }
    }
    #endregion
}
