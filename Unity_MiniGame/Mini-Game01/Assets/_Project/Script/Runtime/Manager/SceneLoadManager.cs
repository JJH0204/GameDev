using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : ManagerBase<SceneLoadManager>
{
    #region Variables
    private SceneType _currentSceneType;
    #endregion

    #region Unity Methods
    
    private void Start()
    {
        _currentSceneType = SceneType.None;
    }

    #endregion

    #region Custom Methods
    
    public void LoadScene(SceneType sceneType)
    {
        // 씬 중복 전환 방지
        if (_currentSceneType == sceneType)
        {
            Debug.Log($"Already in {sceneType} scene.");
            return;
        }
        // 씬 전환 처리
        _currentSceneType = sceneType;
        SceneManager.LoadSceneAsync(_currentSceneType.ToString());
        Debug.Log($"Loading {sceneType} scene...");
    }
    
    // 씬이 게임 씬인지 확인하는 메서드
    public bool IsSceneGame()
    {
        return _currentSceneType == SceneType.Game;
    }
    
    public bool IsSceneTitle()
    {
        return _currentSceneType == SceneType.Title;
    }
    
    #endregion
}
