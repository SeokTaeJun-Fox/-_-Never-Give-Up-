using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 정보입니다.
[CreateAssetMenu(fileName = "MonsterInfo_", menuName = "Scriptable Object/Monster/MonsterInfo")]
public class MonsterInfo : ScriptableObject
{
    [SerializeField] private string monsterId;      //몬스터 id    (몬스터 종류마다 다릅니다. 오브젝트 풀 로드하는데 사용됩니다.)
    [SerializeField] private string monsterName;    //몬스터 이름
    [SerializeField] private GameObject preb;       //프리팹
    [SerializeField] private ItemDropTable itemDropTable;   //몬스터 처치시 아이템 드랍 정보
    [SerializeField] private MonsterAbility monsterAbility; //몬스터 능력치
    [SerializeField] private int exp;               //몬스터 처치시 획득 경험치

    public string MonsterName { get => monsterName; }
    public GameObject Preb { get => preb; }
    public ItemDropTable ItemDropTable { get => itemDropTable; }
    public MonsterAbility MonsterAbility { get => monsterAbility; }
    public int Exp { get => exp; }
    public string MonsterId { get => monsterId; }
}
