using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSlot : MonoBehaviour
{
    [SerializeField , Tooltip("0:Not Lock, 1: Lock")] GameObject[] slots;
    [SerializeField] Button slotBtn;
    [SerializeField] Turret turret;
    public Turret SlotTurret { get { if (turret == null) { return null; }  return turret; } }

    GameSceneController controller = null;
    GameSystemManager system = null;
    public void Init(bool _isLock)
    {
        if (_isLock)
        {
            slots[0].SetActive(false);
            slots[1].SetActive(true);
        }
        else
        {
            slots[0].SetActive(true);
            slots[1].SetActive(false);
        }

        slotBtn.onClick.AddListener(() => PressTowerSlot());
        // 슬롯을 누르면 => 무슨 타워인지 알아야함 => 타워가 선택된 상황임 => 마우스를 타일 위에 가져다두면 건설 가능한지 확인함 => 취소하는 기능 필요
        // => 이거는 게임 컨트롤러에서

        // 슬롯을 눌렀을 때, 돈이 없으면 눌리질 않음

        // 슬롯을 누르면, 다른 슬롯에 눌린 버튼들이 누른 상태에서 벗어남 => 필요가 없을듯.. 취소기능 만들거니까
    }

    private void Start()
    {
        if (controller == null)
            controller = GameManager.Instance.GameSystem.GameController;

        if (system == null)
            system = GameManager.Instance.GameSystem;
    }

    public void PressTowerSlot()
    {
        if (system.CanUse(turret.TowerData.costMoney))
        {
            controller.BuildTurret = turret;
        }
        else
        {
            Debug.Log("돈이 부족합니다!");
            // 경고문 발생
        }
    }
}
