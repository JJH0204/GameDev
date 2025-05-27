using UnityEngine;
using UnityEngine.Serialization;

// using UnityEngine.Playables;

public class GameSceneUI : MonoBehaviour
{
    #region SerializeField
    
    [Header("GameObjects")]
    [FormerlySerializedAs("Timer")] [SerializeField] private GameObject timer; // 타이머 UI 오브젝트
    [FormerlySerializedAs("Score")] [SerializeField] private GameObject score; // 점수 UI 오브젝트
    [FormerlySerializedAs("Combo")] [SerializeField] private GameObject combo; // 콤보 UI 오브젝트
    [SerializeField] private GameObject fever; // 피버 UI 오브젝트
    
    [Header("Popup Prefabs")]
    [SerializeField] private GameObject pausePopupPrefab; // 일시정지 팝업
    [FormerlySerializedAs("EndingPopupPrefab")] [SerializeField] private GameObject endingPopupPrefab; // 게임 종료 팝업
    
    [Header("CountDown Prefab")]
    [SerializeField] private GameObject countDownPrefab; // 카운트다운 UI 오브젝트
    #endregion

    #region Cache
    // 캐시된 컴포넌트들
    private TimeView _timeView; // 타이머 UI 컴포넌트
    private ScoreView _scoreView; // 점수 UI 컴포넌트
    private ComboView _comboView; // 콤보 UI 컴포넌트
    
    private GameObject _endingPopup;
    private GameObject _pausePopup;
    
    private GameObject _countDown;
    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }
    
    #endregion

    #region Boolean Methods

    private bool _isCountDownOn = false;

    #endregion
    
    #region Custom Methods
    // 게임 UI 초기화 메서드
    private void Init()
    {
        // Debug.Log("GameSceneUI Init() 호출됨");
        _timeView = timer.GetComponent<TimeView>();
        _scoreView = score.GetComponent<ScoreView>();
        _comboView = combo.GetComponent<ComboView>();

        _timeView.Init();
        _scoreView.Init();
        _comboView.Init();
    }
    // Have 버튼
    public void OnClick_CatchButton()
    {
        // Debug.Log("Have 버튼 클릭됨");
        GameManager.instance.InputProcess(InputType.Catch);
    }
    // Throw 버튼
    public void OnClick_ThrowButton()
    {
        // Debug.Log("Throw 버튼 클릭됨");
        GameManager.instance.InputProcess(InputType.Throw);
    }
    
    public void OnClick_PauseButton()
    {
        // Debug.Log("Pause 버튼 클릭됨");
        GameManager.instance.PauseGame();
    }

    public void SetScore(int nScore)
    {
        // Debug.Log("SetScore() 호출됨");
        _scoreView.Score = nScore;
    }
    public void SetCombo(int nCombo)
    {
        // Debug.Log("SetCombo() 호출됨");
        _comboView.Combo = nCombo;
    }

    public void SetTime(float time)
    {
        // Debug.Log("SetTime() 호출됨");
        _timeView.StartTimer(time);
    }
    
    public void SetEndingPopup(bool isActive, UserData userData)
    {
        // Debug.Log("SetEndingPopup() 호출됨");
        if (_endingPopup is null)
        {
            if (endingPopupPrefab is not null)
                _endingPopup = Instantiate(endingPopupPrefab, transform);
            else
                Debug.LogError($"{nameof(endingPopupPrefab)} is null");
        }
        _endingPopup.SetActive(isActive);
        _endingPopup.GetComponent<EndingPopup>().SetUserData(userData);
    }

    public void ResetEndingPopup()
    {
        if (_endingPopup is null) return;
        Destroy(_endingPopup.gameObject);
        _endingPopup = null;
    }

    public void SetCountDown(bool isActive)
    {
        if (_countDown is null)
        {
            if (countDownPrefab is not null)
                _countDown = Instantiate(countDownPrefab, transform);
            else
                Debug.LogError($"{nameof(countDownPrefab)} is null");
        }
        _isCountDownOn = isActive;
        _countDown.SetActive(isActive);
        if (isActive)
        {
            _countDown.GetComponent<CountDown>().StartCoroutine("CountDownCoroutine");
        }
    }
    
    public bool IsFinished()
    {
        return _countDown.GetComponent<CountDown>().IsFinished();
    }

    public bool IsCountDownOn()
    {
        return _isCountDownOn;
    }
    
    public bool CountDownOff()
    {
        return _isCountDownOn = false;
    }

    public void SetPausePopup(bool b)
    {
        if (_pausePopup is null)
        {
            if (pausePopupPrefab is not null)
                _pausePopup = Instantiate(pausePopupPrefab, transform);
            else
                Debug.LogError($"{nameof(pausePopupPrefab)} is null");
        }
        _pausePopup.SetActive(b);
    }

    public bool IsFeverOn()
    {
        return fever.GetComponent<FeverSystem>().IsFever();
    }

    public void StartFever(float feverTime)
    {
        fever.GetComponent<FeverSystem>().StartFever(feverTime);
    }
    
    #endregion
}
