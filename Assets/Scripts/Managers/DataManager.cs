using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.IO;

public class DataManager : MonoBehaviour
{
    /*************URL************************/ /* Edit 전까지만 복사*/
    const string StageURL = "https://docs.google.com/spreadsheets/d/1v1GiLP9SS7OLewQLneGzZGmZh3hSmDzM9eipHIP1bis/export?format=tsv&range=A2:F";
    const string EnemyURL = "https://docs.google.com/spreadsheets/d/1sb_ePlJwgrWbd4G0mgp2Tw5Np7IMCc8_N2oATSjsXt0/export?format=tsv&range=A2:E";
    const string summonEnemyDataName = "summonEnemyData";
    string[] playerDataName = { "/Player0.json", "/Player1.json", "/Player2.json" };

    /************Data Dictionary************/
    Dictionary<int, StageData> stageDictionary = new Dictionary<int, StageData>();
    Dictionary<int, EnemyData> enemyDictionary = new Dictionary<int, EnemyData>();
    Dictionary<int, SummonData> stageSummonEnemyDictionary = new Dictionary<int, SummonData>();
    Dictionary<string, PlayerData> playerDataDictionary = new Dictionary<string, PlayerData>();

    public PlayerData CurrentPlayerData { get; set; } = null;
    public StageData CurrentStageData { get; set; } = null;

    #region Load Data Method
    public void Init()
    {
        // 코루틴으로 불러와야 함 (동기 방식)
        StartCoroutine(DownloadData(StageURL, SetStageIO));
        StartCoroutine(DownloadData(EnemyURL, SetEnemyIO));
        
        // 스테이지 소환될 몬스터들의 데이터를 불러옴
        SummonEnemyData _summonData = LoadJsonFromResources<SummonEnemyData>(summonEnemyDataName);
        int summonListCnt = _summonData.summonEnemyData.Count;
        for(int idx=0; idx<summonListCnt; idx++)
        {
            stageSummonEnemyDictionary.Add(_summonData.summonEnemyData[idx].stageID, _summonData.summonEnemyData[idx]);
        }
    }

    // 동기 방식으로 불러오기 (스테이지, 적 데이터)
    IEnumerator DownloadData(string _URL, UnityAction<string> _callBack = null)
    {
        UnityWebRequest _request = UnityWebRequest.Get(_URL);
        yield return _request.SendWebRequest();
        if (_request.result == UnityWebRequest.Result.ConnectionError || _request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(_request.error);
            yield break;
        }
        string _tsv = _request.downloadHandler.text;
        if (_callBack != null)
            _callBack(_tsv);
    }

    public T LoadJsonFromResources<T>(string _name) where T : class
    {
        TextAsset _dataText = Resources.Load<TextAsset>($"JsonData/{_name}");
        T _data = JsonUtility.FromJson<T>(_dataText.text);
        return _data;
    }

    public void ClearDataManager()
    {
        CurrentPlayerData = null;
        CurrentStageData = null;
    }
    #endregion

    #region Stage Data
    public StageData GetStageData(int _stageID)
    {
        if (stageDictionary.ContainsKey(_stageID))
            return stageDictionary[_stageID];
        return null;
    }

    void SetStageIO(string _tsv)
    {
        string[] _dataLines = _tsv.Split('\n');
        int lineSize = _dataLines.Length;
        int dataSize = _dataLines[0].Split('\t').Length;
        for(int idx=0; idx<lineSize; idx++)
        {
            // StageID / XSize / YSize / Time	/ HP	/ Money
            string[] _divideDatas = _dataLines[idx].Split('\t');
            for(int k=0; k<dataSize; k++)
            {
                int _id = int.Parse(_divideDatas[0]);
                // Check ID
                if (stageDictionary.ContainsKey(_id))
                    continue;

                int _xSize = int.Parse(_divideDatas[1]);
                int _ySize = int.Parse(_divideDatas[2]);
                float _time = float.Parse(_divideDatas[3]);
                int _hp = int.Parse(_divideDatas[4]);
                int _money = int.Parse(_divideDatas[5]);
                StageData _stageData = new StageData(_id, _xSize, _ySize, _time, _hp, _money);
                stageDictionary.Add(_id, _stageData);
            }
        }
    }
    #endregion

    #region Enemy Data
    public EnemyData GetEnemyData(int _enemyID)
    {
        if (enemyDictionary.ContainsKey(_enemyID))
            return enemyDictionary[_enemyID];
        return null;
    }

    void SetEnemyIO(string _tsv)
    {
        string[] _dataLines = _tsv.Split('\n');
        int lineSize = _dataLines.Length;
        int dataSize = _dataLines[0].Split('\t').Length;
        for (int idx = 0; idx < lineSize; idx++)
        {
            // EnemyID / Speed / HP / EnemyName / Damage
            string[] _divideDatas = _dataLines[idx].Split('\t');
            for (int k = 0; k < dataSize; k++)
            {
                int _id = int.Parse(_divideDatas[0]);
                // Check ID
                if (enemyDictionary.ContainsKey(_id))
                    continue;

                float _speed = float.Parse(_divideDatas[1]);
                float _hp = float.Parse(_divideDatas[2]);
                string _name = _divideDatas[3];
                int _damage = int.Parse(_divideDatas[4]);
                EnemyData _enemyData = new EnemyData(_id, _speed, _hp, _name,_damage);
                enemyDictionary.Add(_id, _enemyData);
            }
        }
    }
    #endregion

    #region Summon EnemyData

    public SummonData GetSummonData(int _stageID)
    {
        if (!stageSummonEnemyDictionary.ContainsKey(_stageID))
            return null;
        return stageSummonEnemyDictionary[_stageID];
     }
    #endregion

    #region Player Data
    // UI에서 호출해서 사용
    public PlayerData[] LoadPlayerData()
    {
        PlayerData[] playerDataGroup = new PlayerData[3];

        string playerDataPath;
        for(int idx=0; idx<3; idx++)
        {
            playerDataPath = Application.persistentDataPath + playerDataName[idx];
            if (File.Exists(playerDataPath))
            {
                string _playerData = File.ReadAllText(playerDataPath);
                playerDataGroup[idx] = JsonUtility.FromJson<PlayerData>(_playerData);

                if (!playerDataDictionary.ContainsKey(playerDataGroup[idx].playerName))
                    playerDataDictionary.Add(playerDataGroup[idx].playerName, playerDataGroup[idx]);
            }
            else
            {
                playerDataGroup[idx] = null;
            }
        }
        return playerDataGroup;
    }

    // 생성할 데이터의 경로 출력
    public string GetPlayerDataPath(int _index)
    {
        return Application.persistentDataPath + playerDataName[_index];
    }

    // UI에서 눌렀을 때, 데이터를 생성할지 결정
    public bool CanCreatePlayerData(string _name)
    {
        if (playerDataDictionary.ContainsKey(_name))
            return false;
        return true;
    }

    public void DeletePlayerData(string _name)
    {
        if (!playerDataDictionary.ContainsKey(_name))
            return;
        playerDataDictionary.Remove(_name);
    }

    // 데이터를 생성
    public PlayerData CreatePlayerData(string _name, int _index)
    {
        PlayerData _playerData = new PlayerData(_name, _index);
        playerDataDictionary.Add(_playerData.playerName, _playerData);
        string _playerDataJsonText = JsonUtility.ToJson(_playerData);
        File.WriteAllText(GetPlayerDataPath(_index), _playerDataJsonText);
        return _playerData;
    }

    // 데이터를 저장
    public void SavePlayerData(PlayerData _playerData)
    {
        string _saveDataText = JsonUtility.ToJson(_playerData);
        File.WriteAllText(GetPlayerDataPath(_playerData.nameIndex), _saveDataText);
    }

    // 데이터를 삭제
    public void DeletePlayerData(int _index)
    {
        string _dataPathName = GetPlayerDataPath(_index);
        if (File.Exists(_dataPathName))
            File.Delete(_dataPathName);
    }
    #endregion
}
