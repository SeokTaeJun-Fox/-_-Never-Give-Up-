using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawnManager : MonoBehaviour
{
    [SerializeField] private SpawnAndPatrolPos[] spawnAndPatrolPos;

    [SerializeField] private PatrolInfo[] patrolInfos; //여러 순찰지점이 들어있는 클래스입니다.
    [SerializeField] private Transform[] spawnPositions;  //스폰위치 배열
    [SerializeField] private List<MonsterInfo> spawnPattern;    //스폰 패턴(스폰 함수 실행할때마다 인덱스 0번에 해당하는 몬스터를 스폰합니다.)
                                                            //스폰 함수 또 실행하면 1번 또 실행하면 2번 .. 끝번에 해당하는 몬스터를 스폰합니다.
                                                            //추후 하이드인스펙터로 변경예정 (디버그용 직렬화)
    //스크립터블 오브젝트
    [SerializeField] private MonsterSpawnInfoRegister monsterSpawnInfo;
    [SerializeField] private ObjectPoolEvent objectPoolEvent;
    int curSpawnPosIndex = 0;
    int curSpawnPatternIndex = 0;

    //네브메쉬 우선순위 할당
    private Dictionary<MonsterController, int> monsterToPriority = new Dictionary<MonsterController, int>();
    private HashSet<int> usedAvoidancePriorities = new HashSet<int>();
    private const int MIN_PRIORITY = 0;
    private const int MAX_PRIORITY = 99;

    private void Awake()
    {
        int id = GlobalData.curMapCode;
        spawnPattern = monsterSpawnInfo.Entry.Find(x => x.MapInfoId == id).SpawnPattern;

        if (spawnPattern == null)
            return;

        for(int i = 0; i < spawnAndPatrolPos.Length; i++)
            Spawn(i);
    }

    public void Spawn(int spawnAndPatrolPosIndex)
    {
        //순찰 지점 설정
        Transform[] patrols = spawnAndPatrolPos[spawnAndPatrolPosIndex].patrolInfo.Patrols;

        //스폰 위치 설정
        Transform spawnPos = spawnAndPatrolPos[spawnAndPatrolPosIndex].spawnPos;

        //몬스터 생성
        MonsterInfo curMonsterInfo = spawnPattern[curSpawnPatternIndex];
        GameObject monsterObj = objectPoolEvent.RaiseGet(curMonsterInfo.MonsterId, spawnPos.position, Quaternion.identity);
        if (monsterObj != null)
        {
            MonsterController controller = monsterObj.GetComponent<MonsterController>();
            controller.name = $"{spawnAndPatrolPosIndex} : {controller.MonsterName}";

            // 이전 이벤트 제거 후 새 이벤트 등록
            controller.OnDeadIncludeParam -= OnMonsterDead;
            controller.OnDeadIncludeParam += OnMonsterDead;

            //피하기 우선 순위 설정
            NavMeshAgent agent = controller.Agent;
            agent.avoidancePriority = GetNextUniqueAvoidancePriority();

            //몬스터 오브젝트 정보 세팅
            controller.ObjectCategory = spawnAndPatrolPosIndex; //오브젝트 카테고리에 현재 스폰위치/순찰위치 번호를 저장합니다. (스폰시 해당 번호에 다른 몬스터가 스폰하기위해서입니다.)
            controller.Setting(curMonsterInfo, patrols);

            //몬스터 활동 시작
            controller.ActiveStart();
        }
        else
        {
            Debug.LogWarning($"{curMonsterInfo.MonsterId}에 해당하는 오브젝트를 불러오지 못했습니다.");
        }

        curSpawnPatternIndex++;
        if (curSpawnPatternIndex >= spawnPattern.Count)
            curSpawnPatternIndex = 0;
    }

    private int GetNextUniqueAvoidancePriority()
    {
        for (int i = MIN_PRIORITY; i <= MAX_PRIORITY; i+=10)
        {
            if (!usedAvoidancePriorities.Contains(i))
            {
                usedAvoidancePriorities.Add(i);
                return i;
            }
        }

        Debug.LogWarning("모든 avoidancePriority 값이 사용 중입니다. 중복이 발생할 수 있습니다.");
        return Random.Range(MIN_PRIORITY, MAX_PRIORITY + 1); 
    }

    private void OnMonsterDead(MonsterController _deadMonster)
    {
        Debug.Log($"{_deadMonster.Agent.avoidancePriority} : 제거");
        usedAvoidancePriorities.Remove(_deadMonster.Agent.avoidancePriority);
        StartCoroutine(CoTermSpown(5, _deadMonster.ObjectCategory));
    }

    private IEnumerator CoTermSpown(int _time, int spawnAndPatrolPos)
    { 
        yield return new WaitForSeconds(_time);
        Spawn(spawnAndPatrolPos);
    }
}

[System.Serializable]
public struct SpawnAndPatrolPos
{
    public PatrolInfo patrolInfo;
    public Transform spawnPos;
}
