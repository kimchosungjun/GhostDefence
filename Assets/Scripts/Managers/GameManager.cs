using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /************* �̱��� *******************/
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject _go = new GameObject("GameManager");
                instance = _go.AddComponent<GameManager>();
                DontDestroyOnLoad(_go);
            }
            return instance;
        }
    }
    #endregion

    /************** �Ŵ��� ******************/
    #region Refer Managers
    [SerializeField] DataManager data;
    public DataManager Data { get { if (data == null) data = gameObject.GetComponentInChildren<DataManager>(); return data; } }
    #endregion

    #region Not Refer Managers
    private GridManager grid = new GridManager();
    public static GridManager Grid { get { return Instance.grid; } }

    private GameSystemManager gameSystem = new GameSystemManager();
    public static GameSystemManager GameSystem { get { return Instance.gameSystem; } }
    #endregion
    /*********** ������ ����Ŭ *************/
    void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        #endregion

        #region Init Manager
        Data.Init();
        #endregion
    }
}
