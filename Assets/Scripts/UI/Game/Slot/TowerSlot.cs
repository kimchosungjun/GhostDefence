using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TowerSlot : MonoBehaviour
{
    [SerializeField , Tooltip("0:Not Lock, 1: Lock")] GameObject[] slots;
    [SerializeField] Button slotBtn;
    [SerializeField] Turret turret;
    [SerializeField] TextMeshProUGUI costText;
    public Turret SlotTurret { get { if (turret == null) { return null; }  return turret; } }

    GameSceneController controller = null;
    GameSystemManager system = null;
    public void Init(bool _isLock)
    {
        if (_isLock)
        {
            slotBtn.interactable = false;
            slots[0].SetActive(false);
            slots[1].SetActive(true);
        }
        else
        {
            slotBtn.interactable = true;
            slots[0].SetActive(true);
            slots[1].SetActive(false);
        }

        costText.text = turret.ScriptableTowerData.costMoney.ToString();
        slotBtn.onClick.AddListener(() => PressTowerSlot());
        // ������ ������ => ���� Ÿ������ �˾ƾ��� => Ÿ���� ���õ� ��Ȳ�� => ���콺�� Ÿ�� ���� �����ٵθ� �Ǽ� �������� Ȯ���� => ����ϴ� ��� �ʿ�
        // => �̰Ŵ� ���� ��Ʈ�ѷ�����

        // ������ ������ ��, ���� ������ ������ ����

        // ������ ������, �ٸ� ���Կ� ���� ��ư���� ���� ���¿��� ��� => �ʿ䰡 ������.. ��ұ�� ����Ŵϱ�
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
        if (system.CanUse(turret.ScriptableTowerData.costMoney))
        {
            controller.BuildTurret = turret;
        }
        else
        {
            Debug.Log("���� �����մϴ�!");
            // ��� �߻�
        }
    }
}
