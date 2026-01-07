using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePopup : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private GameObject keepGoingButton;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject goTitleButton;
    [SerializeField] private GameObject exitButton;
    
    #region Unity Methods

    private void Start()
    {
        if (!CheckSerializedField())
        {
            Debug.LogError($"{nameof(EndingPopup)} requires a {nameof(CheckSerializedField)}");
        }
    }

    private bool CheckSerializedField()
    {
        if (restartButton is not null && goTitleButton is not null && exitButton is not null) return true;
        Debug.LogError("Button is null");
        return false;
    }
    
    public void OnClickKeepGoingButton()
    {
        // 게임 매니저를 통해 게임을 재개합니다.
        // GameManager.instance.RunTime();
        GameManager.instance.KeepGoing();
        gameObject.SetActive(false);
    }
    
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
}
