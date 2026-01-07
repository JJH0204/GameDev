using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FeverSystem : MonoBehaviour
{
    private bool _isFever;
    private float _feverTime;

    #region Serialized Fields

    [SerializeField] private Slider feverTimer;
    [SerializeField] private GameObject feverLogo;
    
    #endregion
    private void Awake()
    {
        _isFever = false;
        // this.GetComponent<GameObject>().SetActive(false);
    }

    public void StartFever(float feverTime)
    {
        if (!_isFever)
        {
            _isFever = !_isFever;
            _feverTime = feverTime;
            feverLogo.gameObject.SetActive(_isFever);
            feverTimer.gameObject.SetActive(_isFever);
            StartCoroutine(FeverRuntime());
        }
    }

    private IEnumerator FeverRuntime()
    {
        while (_feverTime > 0)
        {
            _feverTime -= Time.deltaTime;
            feverTimer.value = _feverTime;
            yield return null;
        }
        _isFever = false;
        feverTimer.value = 0;
        feverLogo.gameObject.SetActive(_isFever);
        feverTimer.gameObject.SetActive(_isFever);
    }
    
    public bool IsFever()
    {
        return _isFever;
    }
}
