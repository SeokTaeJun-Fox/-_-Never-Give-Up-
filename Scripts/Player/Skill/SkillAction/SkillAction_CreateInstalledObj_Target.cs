using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

//대상에게 공격 장판을 설치하는 스킬행동입니다.
[CreateAssetMenu(fileName = "SA_CreateInstalledObject_Target_", menuName = "Scriptable Object/PlayerSkill/SkillAction/SA_CreateInstalledObject_Target")]
public class SkillAction_CreateInstalledObj_Target : SkillAction
{
    [SerializeField] private string objectName; //오브젝트 풀 객체를 불러올때 사용됩니다. (불러올 객체명)
    [SerializeField] private ObjectPoolEvent poolEvent;
    [SerializeField] private float damageMultiple;  //데미지 배수
    [SerializeField] private PlayerStat dependance; //데미지 측정 기준
    [SerializeField] private float addDistance;   //플레이어 중앙기준 공격 추가 범위 (공격범위 = 무기 공격범위 + addDistance)
    [SerializeField] private LayerMask targetLayerMask;   //범위안에 어떤 레이어형 오브젝트위치에 장판을 소환하는지 확인합니다.
    [SerializeField] private bool isMultiple;   //다수 적용 스킬인지 확인합니다.

    public override void Action(ISkillUser _user)
    {
        Transform owner = _user.Owner();
        int power = (int)((int)_user.Ability()[dependance] * damageMultiple);

        float attackDistance = (float)_user.Ability()[PlayerStat.ATTACKRANGE] + addDistance;
        Collider[] colliders = Physics.OverlapSphere(owner.position, attackDistance, targetLayerMask);
        foreach (Collider col in colliders)
        {
            GameObject go = poolEvent.RaiseGet(objectName, col.transform.position, col.transform.rotation);
            DO_InstalledObject installedObject = go.GetComponent<DO_InstalledObject>();
            if (installedObject != null)
            {
                installedObject.Setting(power, targetLayerMask);
            }
            else
            {
                Debug.LogWarning($"{_user.Owner().name}가 사용한 스킬 {name}에 {objectName}오브젝트에 DO_InstalledObject컴포넌트가 존재하지 않습니다.");
            }

            if (!isMultiple)
                break;
        }
    }
}
