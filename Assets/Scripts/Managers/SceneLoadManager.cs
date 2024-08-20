using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public SceneName CurrentScene { get; set; } = SceneName.Lobby;

    static SceneLoadManager instance;
    public static SceneLoadManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject sceneManager = new GameObject("SceneManager");
                instance = sceneManager.AddComponent<SceneLoadManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public void LoadScene(SceneName _sceneName)
    {
        StartCoroutine(LoadSceneCor(_sceneName));
    }

    public IEnumerator LoadSceneCor(SceneName _sceneName)
    {
        CurrentScene = _sceneName;
        string _name = Enums.GetStringValue<SceneName>(_sceneName);
        AsyncOperation asyncOperation =  SceneManager.LoadSceneAsync(_name);
        while (!asyncOperation.isDone)
        {
            float progress = asyncOperation.progress;
            Debug.Log(progress);
            yield return null;
        }
    }

    public string GetSceneName()
    {
        return Enums.GetStringValue<SceneName>(CurrentScene);
    }

    public int GetSceneIndex()
    {
        return Enums.GetIntValue<SceneName>(CurrentScene);
    }
}
