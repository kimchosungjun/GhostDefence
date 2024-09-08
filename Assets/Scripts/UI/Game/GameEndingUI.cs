using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameEndingUI : MonoBehaviour
{
    [TextArea, SerializeField] string endingMent;
    [SerializeField] TextMeshProUGUI endingText;

    [SerializeField, Tooltip("0:이미지, 1:페이드")] Image[] endingImgaes;
    [SerializeField] Button exitBtn;
    public void Init()
    {
        exitBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.Data.SavePlayerData(GameManager.Instance.Data.CurrentPlayerData);
            LoadingManager.Instance.CallStartLoading(SceneName.Stage);
        });
        exitBtn.gameObject.SetActive(false);
        endingText.text = "";
        endingImgaes[0].gameObject.SetActive(false);
        endingImgaes[1].gameObject.SetActive(false);
    }

    public void EndingStart()
    {
        Time.timeScale = 1f;
        StartCoroutine(Fade());
        GameManager.Instance.Data.CurrentPlayerData.isShowEnding = true;
    }

    IEnumerator Fade()
    {
        // Out
        endingImgaes[1].gameObject.SetActive(true);
        float _timer = 0f;
        Color _color = endingImgaes[1].color;
        _color.a = 0f;
        while (_timer < 1f)
        {
            _timer += Time.deltaTime;
            _color.a = Mathf.Lerp(0, 1, _timer / 1);
            endingImgaes[1].color = _color;
            yield return null;
        }
        _color.a = 1f;
        endingImgaes[1].color = _color;

        yield return new WaitForSeconds(1f);

        endingImgaes[0].gameObject.SetActive(true);

        _timer = 0f;
        while (_timer < 1f)
        {
            _timer += Time.deltaTime;
            _color.a = Mathf.Lerp(1, 0, _timer / 1);
            endingImgaes[1].color = _color;
            yield return null;
        }
        _color.a = 0f;
        endingImgaes[1].color = _color;

        int _endingMentCnt = endingMent.Length;
        for(int i=0; i<_endingMentCnt; i++)
        {
            endingText.text += endingMent[i];
            yield return new WaitForSeconds(0.1f);
        }

        exitBtn.gameObject.SetActive(true);
    }
}
