using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfoUIClient : MonsterInfoUser
{
    [SerializeField] private Transform owner;
    [SerializeField] private Transform uiPos;
    [SerializeField] private MonsterUIGetRetriveEvent monsterUIGetRetriveEvent;
    [SerializeField] private LayerMask senseLayer;
    [SerializeField] private float range;   //플레이어가 [range] 범위내에 있으면 ui가 보여주는지 확인합니다.
    [SerializeField] private bool isShowGizmo;

    private Collider[] playerFind = new Collider[1];
    private MonsterInfoUI myUi;
    private MonsterInfo myInfo;
    private IReadOnlyDictionary<PlayerStat, object> myAbility;
    private int prevHp;
    private int curHp;

    private IBuffController myBuffController;

    private void Update()
    {
        if (myUi == null)
            return;

        if (myAbility == null)
            return;

        //ui 보이기/안보이기
        int targetNum = Physics.OverlapSphereNonAlloc(owner.position, range, playerFind, senseLayer);
        if (targetNum > 0)
        {
            //카메라뒤에 몬스터가 가까이있을때 간혹 몬스터 정보ui가 뜨는 현상이 발생되므로,
            //이 현상을 막기위해 이 코드를 적었습니다. 카메라 정면벡터와, 몬스터-카메라벡터의 각도가 100도 이상일때
            //표시 안하도록 설정했습니다.
            Vector3 cameraFront = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1));
            Vector3 ownerPosition = Vector3.Scale(owner.position, new Vector3(1, 0, 1));
            Vector3 cameraPosition = Vector3.Scale(Camera.main.transform.position, new Vector3(1, 0, 1));
            float angle = Vector3.Angle(cameraFront, ownerPosition - cameraPosition);

            if (angle < 100)
                myUi.Hide(false);
            else
                myUi.Hide(true);
        }
        else
        {
            myUi.Hide(true);
        }

        //hp비교및 hpBar갱신
        curHp = (int)myAbility[PlayerStat.HP];
        if (curHp != prevHp)
        {
            myUi.UpdateHpBar();
        }
        prevHp = curHp;
    }

    public void SetBuffController(IBuffController _buffController)
    { 
        myBuffController = _buffController;
    }

    public override void Setting(MonsterInfo _monsterInfo, IReadOnlyDictionary<PlayerStat, object> _monsterAbilities)
    {
        myInfo = _monsterInfo;
        myAbility = _monsterAbilities;
        curHp = (int)_monsterAbilities[PlayerStat.HP];
        Active();
    }

    public void Active()
    {
        myUi = monsterUIGetRetriveEvent.RaiseGet();
        myUi.Setting(uiPos, myInfo, myAbility, myBuffController);
    }

    public void InActive()
    {
        monsterUIGetRetriveEvent.RaiseRetrieve(myUi);
        myUi.Hide(true);
        myUi = null;
        myInfo = null;
        myAbility = null;
    }

    private void OnDrawGizmos()
    {
        if (isShowGizmo && owner != null)
        {
            Gizmos.DrawWireSphere(owner.position, range);
        }
    }
}
