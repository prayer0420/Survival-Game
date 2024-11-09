using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PassiveAnimal : Creature
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
            case CreatureState.Alerted:
                AlertedUpdate();
                break;
            case CreatureState.Chase:
            case CreatureState.Attack:
                CombatUpdate();
                break;
            case CreatureState.Escape:
                EscapeUpdate();
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
            if (!isAttacked && IsPlayerInFieldOfView())
            {
                isAlerted = true;
                ChangeState(CreatureState.Alerted);
                return;
            }
            else if (isAttacked)
            {
                isAlerted = true;
                if (currentHealth < data.health * 0.1f)
                {
                    isEscaping = true;
                    isChasing = false;
                    ChangeState(CreatureState.Escape);
                    return;
                }
                else
                {
                    isChasing = true;
                    ChangeState(CreatureState.Chase);
                }
            }
        }
    }

    private void AlertedUpdate()
    {
        if (currentHealth < data.health * 0.1f)
        {
            isEscaping = true;
            ChangeState(CreatureState.Escape);
            return;
        }
        else
        {
            if (isAttacked)
            {
                isChasing = true;
                ChangeState(CreatureState.Chase);
                return;
            }
        }

        if (playerDistance > data.detectDistance)
        {
            isAlerted = false;
            isChasing = false;
            isEscaping = false;
            isAttacked = false;
            ChangeState(CreatureState.Idle);
        }
    }

    private void CombatUpdate()
    {
        if (currentHealth < data.health * 0.1f)
        {
            isEscaping = true;
            isChasing = false;
            ChangeState(CreatureState.Escape);
            return;
        }
        else
        {
            if (playerDistance < data.attackDistance && IsPlayerInFieldOfView())
            {
                ChangeState(CreatureState.Attack);
                return;
            }
            else
            {
                if (playerDistance <= data.detectDistance)
                {
                    if (CanChase())
                    {
                        isChasing = true;
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
                    isAlerted = false;
                    isChasing = false;
                    isEscaping = false;
                    isAttacked = false;
                    ChangeState(CreatureState.Idle);
                }
            }
        }
    }

    private void EscapeUpdate()
    {
        if (playerDistance > data.detectDistance)
        {
            isAlerted = false;
            isChasing = false;
            isEscaping = false;
            isAttacked = false;
            ChangeState(CreatureState.Idle);
        }
    }
}
