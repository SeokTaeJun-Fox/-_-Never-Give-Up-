using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSpawnInfoEntry_", menuName = "Scriptable Object/MonsterSpawn/MonsterSpawnInfoEntry")]
public class MonsterSpawnInfoEntry : ScriptableObject
{
    [SerializeField] private int mapInfoId; //이 변수는 씬이 로드할때 mapInfoId를 몬스터스폰클래스에 전달하고
                                            //몬스터 스폰 클래스는 이 id를이용해서 스폰 패턴과 몹 스폰을 정합니다.

    [SerializeField] private List<MonsterInfo> spawnPattern;

    public int MapInfoId { get => mapInfoId; }
    public List<MonsterInfo> SpawnPattern { get => spawnPattern; }
}
