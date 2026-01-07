using UnityEngine;

public abstract class ManagerBase<T> : MonoBehaviour where T : ManagerBase<T>
{
    #region Singleton

    private static T _instance;
    public static T instance
    {
        get
        {
            if (_instance is not null) return _instance;
            _instance = FindObjectOfType<T>();
            if (_instance is not null) return _instance;
            var obj = new GameObject(typeof(T).Name);
            _instance = obj.AddComponent<T>();
            DontDestroyOnLoad(obj);
            return _instance;
        }
    }
    
    #endregion
    
    #region Shared Methods

    public bool IsInstance()
    {
        return true;
    }
    
    #endregion
}
