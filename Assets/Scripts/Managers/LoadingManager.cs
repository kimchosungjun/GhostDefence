using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    #region Singleton
    static LoadingManager instance;
    public static LoadingManager Instance { get { return instance; } }
    #endregion

    [Header("Fade")]
    [SerializeField] float fadeInTimer;
    [SerializeField] float fadeOutTimer;
    [SerializeField] Image fadeFilterImage;
    [Header("Loading")]
    [SerializeField] Slider loadingSlider;
    [SerializeField] TextMeshProUGUI loadingGauge;
    [SerializeField] TextMeshProUGUI loadingAnimText;

    UnityAction startLoadingAction;
    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        #endregion

        if (fadeFilterImage != null)
            fadeFilterImage.gameObject.SetActive(false);

        if (loadingSlider != null)
            loadingSlider.gameObject.SetActive(false);

        if (loadingAnimCor == null)
            loadingAnimCor = StartCoroutine(LoadingTextAnimCor());
        
        startLoadingAction = null;
        startLoadingAction += StartIndicator;
    }

    #region Call when you change scene
    // ���� ������ �� ȣ�� (�ܺο��� ȣ��)
    public void CallStartLoading(SceneName sceneName)
    {
        FadeOut(sceneName,startLoadingAction,0.5f);
    }

    // �� ����ȭ�� �Ϸ�Ǹ� ȣ�� (���ο��� ȣ��)
    public void EndLoading(UnityAction action =null, float delayTime=0f)
    {
        loadingSlider.gameObject.SetActive(false);
        FadeIn(action, delayTime);
        asyncOperation = null;
        EndIndicator();
    }
    #endregion

    #region Fade
    // ���̵� �� : �����

    /************ ���̵� �� ********************/

    // ���̵� �� ȿ���� �̿�
    public void FadeIn(UnityAction action = null, float delayTime = 0f) { StartCoroutine(FadeInCor(action,delayTime)); }
    public IEnumerator FadeInCor(UnityAction action = null, float delayTime = 0f)
    {
        float _timer = 0f;
        Color _fadeColor = fadeFilterImage.color;
        while (_timer < fadeInTimer)
        {
            _timer += Time.deltaTime;
            _fadeColor.a = Mathf.Lerp(1f, 0f, _timer / fadeInTimer);
            fadeFilterImage.color = _fadeColor;
            yield return null;
        }
        _fadeColor.a = 0f;
        fadeFilterImage.color = _fadeColor;
        fadeFilterImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(delayTime);
        if(action!=null)
            action();
    }

    // ���̵� �ƿ� : ��ο���

    /************ ���̵� �ƿ� ******************/

    // ���̵� �ƿ� ȿ���� �̿�
    public void FadeOut(UnityAction action = null, float delayTime = 0f) { StartCoroutine(FadeOutCor(action, delayTime)); }
    public IEnumerator FadeOutCor(UnityAction action = null, float delayTime = 0f)
    {
        fadeFilterImage.gameObject.SetActive(true);
        float _timer = 0f;
        Color _fadeColor = fadeFilterImage.color;
        while (_timer < fadeInTimer)
        {
            _timer += Time.deltaTime;
            _fadeColor.a = Mathf.Lerp(0f, 1f, _timer / fadeInTimer);
            fadeFilterImage.color = _fadeColor;
            yield return null;
        }
        _fadeColor.a = 1f;
        fadeFilterImage.color = _fadeColor;
        yield return new WaitForSeconds(delayTime);
        if (action != null)
            action();
    }

    // ���̵� �ƿ� + �� �ε�
    public void FadeOut(SceneName sceneName, UnityAction action = null, float delayTime = 0f) { StartCoroutine(FadeOutCor(sceneName, action, delayTime)); }
    public IEnumerator FadeOutCor(SceneName sceneName, UnityAction action = null, float delayTime = 0f)
    {
        fadeFilterImage.gameObject.SetActive(true);
        float _timer = 0f;
        Color _fadeColor = fadeFilterImage.color;
        while (_timer < fadeOutTimer)
        {
            _timer += Time.deltaTime;
            _fadeColor.a = Mathf.Lerp(0f, 1f, _timer / fadeOutTimer);
            fadeFilterImage.color = _fadeColor;
            yield return null;
        }
        _fadeColor.a = 1f;
        fadeFilterImage.color = _fadeColor;

        yield return new WaitForSeconds(delayTime);
        if (action != null)
            action();
        LoadScene(sceneName);
    }
    #endregion

    #region LoadScene
    AsyncOperation asyncOperation = null;
    WaitForSeconds halfSecond = new WaitForSeconds(0.5f);

    public SceneName CurrentScene { get; set; } = SceneName.Lobby;

    public void LoadScene(SceneName _sceneName)
    {
        StartCoroutine(LoadSceneCor(_sceneName));
    }

    public IEnumerator LoadSceneCor(SceneName _sceneName)
    {
        loadingSlider.value = 0f;
        loadingGauge.text = "0%";
        loadingSlider.gameObject.SetActive(true);
        CurrentScene = _sceneName;
        string _name = Enums.GetStringValue<SceneName>(_sceneName);
        asyncOperation = SceneManager.LoadSceneAsync(_name);
        asyncOperation.allowSceneActivation = false;

        float fakeProgress = 0f;
        while (fakeProgress < 0.9f)
        {
            float realProgress = asyncOperation.progress;
            fakeProgress = Mathf.MoveTowards(fakeProgress, realProgress, Time.deltaTime);
            loadingSlider.value = fakeProgress;
            loadingGauge.text = (fakeProgress * 100).ToString("0") + "%";
            yield return null;
        }

        while (fakeProgress < 1f)
        {
            fakeProgress = Mathf.MoveTowards(fakeProgress, 1f, Time.deltaTime);
            loadingSlider.value = fakeProgress;
            loadingGauge.text = (fakeProgress * 100).ToString("0") + "%";
            yield return null;
        }
        loadingSlider.value = 1f;
        loadingGauge.text = "100%";
        if(_sceneName == SceneName.Game)
            StartCoroutine(PreloadMap());
        else
        {
            asyncOperation.allowSceneActivation = true;
            yield return halfSecond;
            EndLoading();
        }
    }

    private IEnumerator PreloadMap()
    {
        StageData _stageData = GameManager.Instance.Data.CurrentStageData;
        if(_stageData==null)
        {
            Debug.LogError("�������� �����Ͱ� ����!! ����!!");
            yield break;
        }

        string _mapName = $"Ground{_stageData.StageID}";
        GameObject _loadMap = Resources.Load<GameObject>($"Map/{_mapName}");
        if (_loadMap == null)
        {
            Debug.LogError("�� �����Ͱ� ����!! ����!!");
            yield break;
        }

        GameObject _createMap = Instantiate(_loadMap);
        _createMap.SetActive(false); 
        DontDestroyOnLoad(_createMap);
        asyncOperation.allowSceneActivation = true;

        yield return null; // ���� ������ �ε�ǵ��� 0.5�� ��� (�� ���������ε� ����ҵ�??)
        
        Scene _newScene = SceneManager.GetActiveScene();
        SceneManager.MoveGameObjectToScene(_createMap, _newScene);
        _createMap.SetActive(true);
        EndLoading();
    }
    #endregion

    #region LoadingText Animation
    // �ε� ����
    int periodCnt = 0;
    string loadingText = "�ε����Դϴ�";
    Coroutine loadingAnimCor;
    WaitForSeconds textAnimTime = new WaitForSeconds(0.25f);
    // �ε� ���� �� ȣ��
    public void StartIndicator()
    {
        loadingAnimText.gameObject.SetActive(true);
        StartCoroutine(LoadingTextAnimCor());
    }

    // �ε� ������ �� ȣ��
    public void EndIndicator()
    {
        if (loadingAnimCor != null)
            StopCoroutine(loadingAnimCor);
        loadingAnimText.gameObject.SetActive(false);
    }

    // �ε� �ؽ�Ʈ �ִϸ��̼�
    public IEnumerator LoadingTextAnimCor()
    {
        while (true)
        {
            if (periodCnt < 4)
            {
                periodCnt += 1;
                loadingAnimText.text += ".";
            }
            else
            {
                loadingAnimText.text = loadingText;
                periodCnt = 0;
            }
            yield return textAnimTime;
        }
    }
    #endregion

    #region Scene Util
    public string GetSceneName()
    {
        return Enums.GetStringValue<SceneName>(CurrentScene);
    }

    public int GetSceneIndex()
    {
        return Enums.GetIntValue<SceneName>(CurrentScene);
    }
    #endregion
}
