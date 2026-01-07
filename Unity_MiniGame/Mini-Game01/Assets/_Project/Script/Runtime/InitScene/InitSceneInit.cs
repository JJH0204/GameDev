using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitSceneInit : MonoBehaviour
{
    #region Variables
    
    private InitSceneUI _initSceneUI;        // cache
    private bool _isInstance;
    
    #endregion
    
    #region Inspector Variables
    
    [Header("Managers")]
    [SerializeField] private int initMaxCount;
    [SerializeField] private GameObject objInitSceneUI;
    
    #endregion


    #region Unity Methods
    private void Awake()
    {
        _initSceneUI = objInitSceneUI.GetComponent<InitSceneUI>();
        if (_initSceneUI is null)
        {
            Debug.LogError("InitSceneUI not found");
        }
        else
        {
            Debug.Log("InitSceneInit Awake");
        }
    }
    private void Start()
    {
        StartCoroutine(InstanceManager());
    }

    #endregion
    
    #region Custom Methods
    
    // 게임 진행에 필요한 매니저들을 인스턴스화 하는 코루틴
    private IEnumerator InstanceManager()
    {
        var initCount = 0;
        // 메니저 생성 대기
        yield return new WaitForSeconds(0.5f);
        // 메니저 생성
        for (var i = 0; i < 4; i++)
        {
            initCount++;
            yield return new WaitForSeconds(0.5f);

            _initSceneUI.SetLoadingText($"Loading... {initCount}/{initMaxCount}");
            _initSceneUI.SetLoadingSlider((float)initCount / initMaxCount);
        }
        yield return new WaitForSeconds(0.5f);
        // 생성 완료
        _initSceneUI.SetLoadingText("Loading Complete");
        yield return new WaitForSeconds(0.5f);
        
        // 씬 전환
        SceneLoadManager.instance.LoadScene(SceneType.Title);
    }
    
    #endregion
}
