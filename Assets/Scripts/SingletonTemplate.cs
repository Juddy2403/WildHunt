using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance || _applicationQuitting) return _instance;
            _instance = FindObjectOfType<T>();
            if (_instance) return _instance;
            GameObject newInstance = new GameObject($"Singleton_{typeof(T).Name}");
            _instance = newInstance.AddComponent<T>();
            return _instance;
        }
    }

    public static bool Exists => _instance != null;

    private static bool _applicationQuitting = false;
    protected virtual void OnApplicationQuit()
    {
        _applicationQuitting = true;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this as T)
        {
            _instance = null;
        }
    }

    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode) { }
}