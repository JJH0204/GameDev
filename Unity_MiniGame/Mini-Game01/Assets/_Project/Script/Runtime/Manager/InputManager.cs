using UnityEngine;

public class InputManager : ManagerBase<InputManager>
{
    #region Cache
    
    private GameSceneUI _gameSceneUI;
    
    #endregion

    #region Unity Methods

    void Update()
    { 
        if (SceneLoadManager.instance.IsSceneGame() && GameManager.instance.IsGamePlaying())
        {
            if (_gameSceneUI is null)
                _gameSceneUI = FindAnyObjectByType<GameSceneUI>();
            
            // 버리기 단축기 (A)
            if (Input.GetKeyDown(KeyCode.A))
                _gameSceneUI.OnClick_ThrowButton();

            // 담기 단축기 (S)
            if (Input.GetKeyDown(KeyCode.S))
                _gameSceneUI.OnClick_CatchButton();
        }
        
    }
#endregion
}
