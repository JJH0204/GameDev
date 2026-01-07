using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private GameObject countOwn;
    [SerializeField] private GameObject countTwo;
    [SerializeField] private GameObject countThree;
    [SerializeField] private GameObject countGameStart;

    #endregion

    #region Boolean Fields

    private bool _isFinished;

    #endregion
    
    #region Unity Methods
    
    private void Awake()
    {
        countOwn.SetActive(false);
        countTwo.SetActive(false);
        countThree.SetActive(false);
        countGameStart.SetActive(false);
    }
    
    #endregion
    
    #region Methods
    
    private IEnumerator CountDownCoroutine()
    {
        _isFinished = false;
        
        countThree.SetActive(true);
        yield return new WaitForSeconds(1f);
        countThree.SetActive(false);
        
        countTwo.SetActive(true);
        yield return new WaitForSeconds(1f);
        countTwo.SetActive(false);
        
        countOwn.SetActive(true);
        yield return new WaitForSeconds(1f);
        countOwn.SetActive(false);
        
        countGameStart.SetActive(true);
        yield return new WaitForSeconds(1f);
        countGameStart.SetActive(false);

        _isFinished = true;
    }

    public bool IsFinished()
    {
        return _isFinished;
    }
    
    #endregion
}
