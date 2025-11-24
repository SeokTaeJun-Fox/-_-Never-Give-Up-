using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

//3D 플레이어 캐릭터의 장비 착용을 제어하는 컨트롤러입니다.
//장비 아이템의 프리팹을 지정된 위치에 생성하고, 장착/해제 시 시각적 처리를 담당합니다.
public class Player3dEquipmentController : MonoBehaviour
{
    //장비별 부모 위치 정보
    [SerializeField] private List<EquipmentParentInfo> equipmentParentInfoRegistry;

    //장비 장착 시 비활성화할 오브젝트 정보
    [SerializeField] private List<EquipmentDisableObjInfo> equipmentDisableObjInfoRegistry;

    //무기 장착 변경 이벤트(스크립터블 오브젝트형 이벤트 채널)
    [SerializeField] private Weapon3DChangedEvent weapon3DChangedEvent;

    //이 클래스에 필요한 인터페이스
    private IEquipmentManager equipmentManager;

    //현재 장착된 3D 오브젝트 정보
    private Dictionary<EquipmentType, GameObject> equipInfoDic = new();

    private void Start()
    {
        equipmentManager = ServiceLocator.GetService<IEquipmentManager>();

        foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
        {
            equipInfoDic.Add(type, null);
        }

        if (equipmentManager != null)
        {
            IDictionary<EquipmentType, EquipmentItem> equipmentManagerDic = equipmentManager.EquippedItemDic;

            //장비 종류(EquipmentType) 열거형의 모든 값을 순회합니다.
            foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
            {
                if (equipmentManagerDic.ContainsKey(type) && equipmentManagerDic[type] != null)
                {
                    Equip3DItem(type, equipmentManagerDic[type]);
                }
            }

            //장비 변경 이벤트를 등록하여,
            //장비가 교체될 때 3D 장비 아이템을 장착하거나 해제합니다.
            equipmentManager.OnEquipChanged += OnEquipChanged;
        }
    }

    //3D 장비 아이템을 장착하거나 해제합니다.
    //기존 장비는 제거되고, 새로운 프리팹이 지정된 위치에 생성됩니다.
    public void Equip3DItem(EquipmentType _type, EquipmentItem _item)
    {
        //기존 장비 제거
        if (equipInfoDic.ContainsKey(_type) && equipInfoDic[_type] != null)
        {
            Destroy(equipInfoDic[_type]);

            List<GameObject> enableObjs = equipmentDisableObjInfoRegistry.Find((x) => x.type == _type)?.disableObjs;

            if (enableObjs != null && enableObjs.Count > 0)
            {
                foreach (var obj in enableObjs)
                    obj.SetActive(true);
            }
        }

        GameObject equipment3D = null;

        //새 장비 장착
        if (_item != null)
        {
            Transform parent = equipmentParentInfoRegistry.Find((x) => _item.ParentName == x.parentName)?.parentTransform;
            equipment3D = Instantiate(_item.Prefab, parent);

            if (equipment3D != null)
            {
                equipment3D.transform.localPosition = _item.LocalPos;
                equipment3D.transform.localRotation = Quaternion.Euler(_item.LocalRot);
                equipment3D.transform.localScale = _item.LocalScale;

                equipInfoDic[_type] = equipment3D;
            }

            List<GameObject> enableObjs = equipmentDisableObjInfoRegistry.Find((x) => x.type == _type)?.disableObjs;

            if (enableObjs != null && enableObjs.Count > 0)
            {
                foreach (var obj in enableObjs)
                    obj.SetActive(false);
            }
        }

        //무기 장착 시 이벤트 호출
        if (_type == EquipmentType.WEAPON)
            weapon3DChangedEvent.Raise(equipment3D);
    }

    private void OnDestroy()
    {
        if (equipmentManager != null)
            equipmentManager.OnEquipChanged -= OnEquipChanged;
    }

    //장비 변경 이벤트 처리
    private void OnEquipChanged(EquipmentType _type, EquipmentItem _item)
    {
        Equip3DItem(_type, _item);
    }
}

//장비 프리팹을 생성할 부모 위치 정보를 담는 클래스입니다.
[Serializable]
public class EquipmentParentInfo
{
    public string parentName;   //부모 이름
    public Transform parentTransform;   //부모 트랜스폼
}

//장비 장착 시 비활성화할 오브젝트 정보를 담는 클래스입니다.
[Serializable]
public class EquipmentDisableObjInfo
{
    public EquipmentType type;  //장비 타입
    public List<GameObject> disableObjs;    //비활성화할 오브젝트 리스트
}
