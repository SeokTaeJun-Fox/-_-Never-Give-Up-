using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterInfoUser : MonoBehaviour
{
    public abstract void Setting(MonsterInfo _monsterInfo, IReadOnlyDictionary<PlayerStat, object> _monsterAbilities);
}
