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
    // 씬을 변경할 때 호출 (외부에서 호출)
    public void CallStartLoading(SceneName sceneName)
    {
        FadeOut(sceneName,startLoadingAction,0.5f);
    }

    // 씬 동기화가 완료되면 호출 (내부에서 호출)
    public void EndLoading(UnityAction action =null, float delayTime=0f)
    {
        loadingSlider.gameObject.SetActive(false);
        FadeIn(action, delayTime);
        asyncOperation = null;
        EndIndicator();
    }
    #endregion

    #region Fade
    // 페이드 인 : 밝아짐

    /************ 페이드 인 ********************/

    // 페이드 인 효과만 이용
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

    // 페이드 아웃 : 어두워짐

    /************ 페이드 아웃 ******************/

    // 페이드 아웃 효과만 이용
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

    // 페이드 아웃 + 씬 로딩
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
            Debug.LogError("스테이지 데이터가 없음!! 에러!!");
            yield break;
        }

        string _mapName = $"Ground{_stageData.StageID}";
        GameObject _loadMap = Resources.Load<GameObject>($"Map/{_mapName}");
        if (_loadMap == null)
        {
            Debug.LogError("맵 데이터가 없음!! 에러!!");
            yield break;
        }

        GameObject _createMap = Instantiate(_loadMap);
        _createMap.SetActive(false); 
        DontDestroyOnLoad(_createMap);
        asyncOperation.allowSceneActivation = true;

        yield return null; // 씬이 완전히 로드되도록 0.5초 대기 (한 프레임으로도 충분할듯??)
        
        Scene _newScene = SceneManager.GetActiveScene();
        SceneManager.MoveGameObjectToScene(_createMap, _newScene);
        _createMap.SetActive(true);
        EndLoading();
    }
    #endregion

    #region LoadingText Animation
    // 로딩 변수
    int periodCnt = 0;
    string loadingText = "로딩중입니다";
    Coroutine loadingAnimCor;
    WaitForSeconds textAnimTime = new WaitForSeconds(0.25f);
    // 로딩 시작 시 호출
    public void StartIndicator()
    {
        loadingAnimText.gameObject.SetActive(true);
        StartCoroutine(LoadingTextAnimCor());
    }

    // 로딩 끝났을 때 호출
    public void EndIndicator()
    {
        if (loadingAnimCor != null)
            StopCoroutine(loadingAnimCor);
        loadingAnimText.gameObject.SetActive(false);
    }

    // 로딩 텍스트 애니메이션
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
