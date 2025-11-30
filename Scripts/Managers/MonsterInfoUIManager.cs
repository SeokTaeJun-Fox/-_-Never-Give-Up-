using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 상단에 보이는 정보UI를 관리하는 클래스입니다.
public class MonsterInfoUIManager : MonoBehaviour
{
    [SerializeField] private string objectName; //몬스터정보유아이 오브젝트 이름 (오브젝트 풀 불러오기, 회수하기에 사용됩니다.)
    [SerializeField] private ObjectPoolEvent poolEvent;
    [SerializeField] private MonsterUIGetRetriveEvent monsterUIGetRetriveEvent; //몬스터 ui 불러오기/회수하기 이벤트
    [SerializeField] private GetCanvasRequestEvent canvasRequestEvent;  //ui매니저에 캔버스 불러오기 이벤트
    [SerializeField] private List<MonsterInfoUI> curActiveMonsterInfoUi;    //디버그용 인스펙터에 수정x

    private void Awake()
    {
        monsterUIGetRetriveEvent.OnGet += GetMonsterInfoUi;
        monsterUIGetRetriveEvent.OnRetrieve += RetriveMonsterInfoUi;
    }

    //불러오기
    public MonsterInfoUI GetMonsterInfoUi()
    {
        MonsterInfoUI monsterInfoUI = poolEvent.RaiseGet(objectName).GetComponent<MonsterInfoUI>();
        monsterInfoUI.transform.SetParent(canvasRequestEvent.RaiseGet(CanvasType.WorldLayer).transform);
        curActiveMonsterInfoUi.Add(monsterInfoUI);
        //FOV사이즈 조절
        return monsterInfoUI;
    }

    //회수
    public void RetriveMonsterInfoUi(MonsterInfoUI monsterInfoUI)
    {
        monsterInfoUI.Initial();
        poolEvent.RaiseRelease(objectName, monsterInfoUI.gameObject);
    }

    //FOV사이즈 조절

    private void OnDisable()
    {
        monsterUIGetRetriveEvent.OnGet -= GetMonsterInfoUi;
        monsterUIGetRetriveEvent.OnRetrieve -= RetriveMonsterInfoUi;
    }
}
