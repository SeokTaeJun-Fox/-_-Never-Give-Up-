using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemDropController : MonsterInfoUser
{
    [SerializeField] private ItemDropTable itemDropTable;
    [SerializeField] private Transform dropPos;
    [SerializeField] private float dropPower;
    [SerializeField] private float minX, minZ, maxX, maxZ;

    [SerializeField] private ObjectPoolEvent objectPoolEvent;
    [SerializeField] private UnityEvent OnItemDropped;

    public override void Setting(MonsterInfo _monsterInfo, IReadOnlyDictionary<PlayerStat, object> _monsterAbilities)
    {
        itemDropTable = _monsterInfo.ItemDropTable;
    }

    //아이템을 드롭합니다.
    public void DropItem()
    {
        //아이템 드롭 리스트를 처음부터 끝까지 읽습니다.
        foreach (itemDropInfo info in itemDropTable.ItemDropInfos)
        {
            float tryNum = Random.Range(0f, 1f);    //0~1까지 랜덤한 수를 불러옵니다.
            if (tryNum < info.probability && info.IsCondition)  //랜덤한 수가 제시한 확률보다 낮다면 아이템을 드롭합니다.
            {
                GameObject go = objectPoolEvent.RaiseGet(info.item.ObjectPoolKey, dropPos.position, dropPos.rotation);
                    //Instantiate(info.item.Prefab, dropPos.position, dropPos.rotation);  //오브젝트 생성
                Rigidbody rb = go.GetComponent<Rigidbody>();
                rb.useGravity = true;

                //아이템을 드롭하는 순간에 아이템이 위로 던져지게되는 효과를 얻습니다.
                if (rb != null)
                {
                    float x = Random.Range(minX, maxX);
                    float z = Random.Range(minZ, maxZ);
                    rb.AddForce(new Vector3(x, 1, z).normalized * dropPower);
                }
                else
                    Debug.LogWarning($"아이템 {go.name}에 리지드바디가 부착되지 않았습니다.");

                Item3D item = go.GetComponent<Item3D>();

                //item 3d인스턴스를 세팅합니다.
                if (item != null)
                {
                    int amount = Random.Range(info.amountMin, info.amountMax + 1);
                    item.Setting(info.item, amount);
                }
                else
                {
                    Debug.LogError($"아이템 {go.name}에 item3d컴포넌트가 부착되지 않았습니다.");
                }

                OnItemDropped?.Invoke();
            }
        }
    }
}
