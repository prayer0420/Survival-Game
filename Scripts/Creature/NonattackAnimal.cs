using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NonattackAnimal : Creature
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
            if (isAttacked)
            {
                isAlerted = true;
                isEscaping = true;
                ChangeState(CreatureState.Escape);
            }
            else if (!isAttacked && IsPlayerInFieldOfView())
            {
                isAlerted = true;
                ChangeState(CreatureState.Alerted);
            }
        }
    }

    private void AlertedUpdate()
    {
        if (isAttacked)
        {
            isEscaping = true;
            ChangeState(CreatureState.Escape);
        }

        if (playerDistance > data.detectDistance)
        {
            isAlerted = false;
            ChangeState(CreatureState.Idle);
        }
    }
    private void EscapeUpdate()
    {
        if (playerDistance > data.detectDistance)
        {
            isAlerted = false;
            isEscaping = false;
            isAttacked = false;
            ChangeState(CreatureState.Idle);
        }
    }
}
