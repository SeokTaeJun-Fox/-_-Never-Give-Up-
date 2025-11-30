using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//장비 아이템 정보를 담고 있는 클래스입니다.
//장착 시 능력치를 적용하며, 강화 아이템을 통해 능력치를 추가할 수 있습니다.
[CreateAssetMenu(fileName = "EquipmentItem_", menuName = "Scriptable Object/Item/EquipmentItem")]
public class EquipmentItem : Item
{
    [Header("Equip")]
    [SerializeField] private EquipmentType equipmentType;   //장비 타입 (NONE, WEAPON, HELMET, ARMOR, RING)
    [SerializeField] private List<ItemFunctionElement> abilities;   //장비 능력치
    [SerializeField] private int enchantPossibleAmount;  //강화 가능횟수

    //장착위치
    [Header("WearInfo")]
    [SerializeField] private string parentName; //장착시 부모 오브젝트 이름
    [SerializeField] private Vector3 localPos;   //장착시 로컬 위치값
    [SerializeField] private Vector3 localRot;   //장착시 로컬 회전값
    [SerializeField] private Vector3 localScale; //장착시 로컬 스케일값

    private int curEnchantedCount;   //현재 강화 횟수
    private bool isEquipped;    //장착 여부
    private List<EnchantItem> enchantList = new List<EnchantItem>();    //적용된 강화 아이템 리스트
                                                                        //세이브,로드시 사용됩니다.

    public override ItemType ItemType
    {
        get => ItemType.EQUIPMENT;
    }
    public EquipmentType EquipmentType { get => equipmentType; }
    public List<ItemFunctionElement> Abilities { get => abilities; }
    public int EnchantPossibleAmount { get => enchantPossibleAmount; }
    public int CurEnchantedCount { get => curEnchantedCount; }
    public bool IsEquipped { get => isEquipped; }
    public string ParentName { get => parentName; }
    public Vector3 LocalPos { get => localPos; }
    public Vector3 LocalRot { get => localRot; }
    public Vector3 LocalScale { get => localScale; }

    public IReadOnlyList<EnchantItem> EnchantList => enchantList;

    //장비에 강화 아이템을 적용합니다.
    //중첩 불가능한 능력치는 더 높은 값으로 교체되며, 중첩 가능한 능력치는 추가됩니다.
    public void Enchant(EnchantItem _enchantItem, IItemUser _user)
    {
        if (EnchantPossibleAmount <= 0)
            return;

        //중첩이 가능한 능력치는 그대로 추가되고, 중첩 불가능한 능력치는 더 높은 값으로 교체됩니다.
        foreach (var ability in _enchantItem.Abilities)
        {
            //기존 능력치가 같은 카테고리이고, 둘 다 중첩이 불가능하고, 강화할 능력치가 더 높다면
            ItemFunctionElement removeElement = abilities.Find((x) => (x.Category == ability.Category) && 
                                                               (!x.IsOverlap && !ability.IsOverlap) &&
                                                               (x.Amount < ability.Amount));

            if (removeElement != null)
            {
                if (isEquipped) removeElement.UnUse(_user);
                abilities.Remove(removeElement);
            }

            if(isEquipped) ability.Use(_user);
            abilities.Add(ability);
        }

        curEnchantedCount++;
        enchantPossibleAmount--;

        Debug.Log($"{ItemKey} : {ItemName}아이템이 {_enchantItem.ItemName}강화를 했습니다.");

        enchantList.Add(_enchantItem);
    }

    //장비 능력치를 딕셔너리 형태로 반환합니다. UI 출력용으로 사용됩니다.
    public Dictionary<string, int> GetAbilityDictionary()
    { 
        Dictionary<string, int> abilityDic = new Dictionary<string, int>();

        foreach (var ability in abilities)
        {
            //카테고리 설정
            //중첩 불가능한 카테고리,중첩 가능한 카테고리의 키값은 다르게 입력됩니다.
            string category;
            if (ability.IsOverlap)
            { 
                category = ability.Category;
            }
            else
            {
                category = $"<color=green>{ability.Category}</color>";
            }

            //중첩 불가 카테고리는 기존 값보다 크면 갱신됩니다. (최대값만 노출됩니다)
            //중첩 가능 카테고리는 기존 값에 합산됩니다.
            if (abilityDic.ContainsKey(category))
            {
                if (!ability.IsOverlap && ability.Amount > abilityDic[category])
                {
                    abilityDic[category] = ability.Amount;
                }

                if (ability.IsOverlap)
                {
                    abilityDic[category] += ability.Amount;
                }
            }
            else
            { 
                abilityDic.Add(category, ability.Amount);
            }
        }

        return abilityDic;
    }

    //장비를 장착하여 능력치를 적용합니다.
    public void Equip(IItemUser _user)
    {
        foreach (var ability in abilities)
            ability.Use(_user);

        isEquipped = true;
    }

    //장비를 해제하여 능력치를 제거합니다.
    public void UnEquip(IItemUser _user)
    {
        foreach (var ability in abilities)
            ability.UnUse(_user);

        isEquipped = false;
    }
}
