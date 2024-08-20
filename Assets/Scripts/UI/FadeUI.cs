using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeUI : UI
{
    string loadingText = "로딩중입니다";
    Coroutine loadingAnimCor;
    [SerializeField] TextMeshProUGUI loadingAnimText;

    public override void Init()
    {
        loadingAnimText.text = loadingText;
        if (loadingAnimCor == null)
            loadingAnimCor = StartCoroutine(LoadingTextAnimCor());
    }

    public void StartLoading()
    {
        StartCoroutine(LoadingTextAnimCor());
    }

    public void EndLoading()
    {
        if (loadingAnimCor != null)
            StopCoroutine(loadingAnimCor);
    }

    #region Loading Text
    int periodCnt = 0;
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
            yield return null;
        }
    }
    #endregion

    #region Loading Circle

    #endregion
}
