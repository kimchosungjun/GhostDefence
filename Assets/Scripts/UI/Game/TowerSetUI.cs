using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSetUI : MonoBehaviour
{
    [SerializeField] TowerSlot[] towerSlots;
    public TowerSlot[] Slots { get { return towerSlots; } }
    public void Init()
    {
        int _id = GameManager.Instance.Data.CurrentStageData.StageID;
        int _slotCnt = towerSlots.Length;

        for(int i=0; i< _slotCnt; i++)
        {
            if (i <= _id / 2)
                towerSlots[i].Init(false);
            else
                towerSlots[i].Init(true);
        }
    }
}
