using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class EndingPopup : MonoBehaviour
{
    #region SerializeField
    
    [Header("TMP Text")]
    [FormerlySerializedAs("totalScoreTMP")] [SerializeField] private TextMeshProUGUI currentScoreTMP;
    [SerializeField] private TextMeshProUGUI bestScoreTMP;
    [FormerlySerializedAs("currentComboTMP")] [SerializeField] private TextMeshProUGUI highComboTMP;
    [FormerlySerializedAs("highComboTMP")] [SerializeField] private TextMeshProUGUI bestComboTMP;
    
    [Header("Button")]
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject goTitleButton;
    [SerializeField] private GameObject exitButton;
    #endregion
    
    #region Variables

    private int _currentScore;
    private int _bestScore;
    private int _highCombo;
    private int _bestCombo;

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (!CheckSerializedField())
        {
            Debug.LogError($"{nameof(EndingPopup)} requires a {nameof(CheckSerializedField)}");
        }
    }
    
    #endregion
    
    #region Private Methods

    private bool CheckSerializedField()
    {
        if (currentScoreTMP is null || bestScoreTMP is null || highComboTMP is null || bestComboTMP is null)
        {
            Debug.LogError("TMP Text is null");
            return false;
        }
        
        if (restartButton is null || goTitleButton is null || exitButton is null)
        {
            Debug.LogError("Button is null");
            return false;
        }
        
        // Debug.Log("SerializedField Check Success");
        return true;
    }

    #endregion
    
    #region OnClick Methods
    
    public void OnClickRestartButton()
    {
        // 게임 매니저를 통해 게임을 재시작합니다.
        GameManager.instance.RestartGame();
        gameObject.SetActive(false);
    }
    
    public void OnClickGoTitleButton()
    {
        // 게임을 종료하고 타이틀 씬으로 이동합니다.
        SceneLoadManager.instance.LoadScene(SceneType.Init);
    }
    
    public void OnClickExitButton()
    {
        // // 기록을 저장하고 종료합니다.
        Application.Quit();
    }
    
    #endregion

    #region Public Methods

    public void SetUserData(UserData userData)
    {
        _bestScore = userData.bestScore;
        _bestCombo = userData.bestCombo;
        _highCombo = userData.highCombo;
        _currentScore = userData.currentScore;
        
        bestScoreTMP.text = _bestScore.ToString();
        bestComboTMP.text = _bestCombo.ToString();
        highComboTMP.text = _highCombo.ToString();
        currentScoreTMP.text = _currentScore.ToString();
    }

    #endregion
}
