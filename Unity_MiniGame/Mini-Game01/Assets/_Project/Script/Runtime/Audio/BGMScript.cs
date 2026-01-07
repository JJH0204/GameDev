using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMScript : MonoBehaviour
{
    #region Unity Methods
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    #endregion
}
