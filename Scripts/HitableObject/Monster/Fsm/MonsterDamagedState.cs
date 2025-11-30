using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamagedState : FSMSingleton<MonsterDamagedState>, IFSMState<MonsterController>
{
    public void Enter(MonsterController e)
    {
        e.Agent.isStopped = true;

        e.Agent.enabled = false;
        e.MyRigid.isKinematic = false;

        e.CancelAllCancelableParticle();
    }

    public void Execute(MonsterController e)
    {
        if (e.RemainDamageActionTime < 0)
        {
            e.Agent.enabled = true;
            Transform target = null;

            if (!e.ChaseAttackController.CheckFindTarget(out target) && e.Agent.remainingDistance <= e.Agent.stoppingDistance)
            {
                e.ChangeMonsterState(MonsterNormalState.Instance);
                return;
            }
            else if (e.ChaseAttackController.CheckFindTarget(out target) &&
                Vector3.Distance(e.transform.position, target.position) <= e.Agent.stoppingDistance)
            {
                e.ChangeMonsterState(MonsterAttackState.Instance);
                return;
            }

            if (e.ChaseAttackController.CheckFindTarget(out target))
            {
                e.ChangeMonsterState(MonsterChaseState.Instance);
            }
        }

        e.RemainDamageActionTime -= Time.deltaTime;
    }

    public void Exit(MonsterController e)
    {
        e.PlayDamageAnimation(false);
        e.Agent.enabled = true;
        e.MyRigid.isKinematic = true;
    }
}
