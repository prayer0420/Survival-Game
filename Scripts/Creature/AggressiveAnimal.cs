using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AggressiveAnimal : Creature
{
    protected override void OnEnable()
    {
        base.OnEnable();        
    }

    protected override void Start()
    {
        stateContext.Transition(stateContext.wander);
    }

    protected override void Update()
    {
        base.Update();

        switch (creatureState)
        {
            case CreatureState.Idle:
            case CreatureState.Wander:
                IdleWanderUpdate();
                break;
            case CreatureState.Chase:
            case CreatureState.Attack:
                CombatUpdate();
                break;
            case CreatureState.Dead:
                return;
        }

        stateContext.currentState.OnStateUpdate();
    }

    private void IdleWanderUpdate()
    {
        if (creatureState == CreatureState.Wander && agent.remainingDistance < 0.5f)
        {
            ChangeState(CreatureState.Idle);
            wanderRate = Random.Range(data.minWanderWaitTime, data.maxWanderWaitTime);
        }
        else if (Time.time - lastWanderTime > wanderRate)
        {
            lastWanderTime = Time.time;
            ChangeState(CreatureState.Wander);
        }

        if (playerDistance <= data.detectDistance)
        {
            if (IsPlayerInFieldOfView() || isAttacked)
            {
                isChasing = true;
                isAlerted = true;
                ChangeState(CreatureState.Chase);
            }
        }
    }

    private void CombatUpdate()
    {
        if (playerDistance < data.attackDistance && IsPlayerInFieldOfView())
        {
            ChangeState(CreatureState.Attack);
        }
        else
        {
            if (playerDistance <= data.detectDistance)
            {
                if (CanChase())
                {
                    ChangeState(CreatureState.Chase);
                    return;
                }
                else
                {
                    isChasing = false;
                    isAlerted = false;
                    ChangeState(CreatureState.Wander);
                    return;
                }
            }
            else
            {
                isChasing = false;
                isAlerted = false;
                ChangeState(CreatureState.Idle);
            }
        }
    }
}
