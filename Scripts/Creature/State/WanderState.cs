using UnityEngine;
using UnityEngine.AI;

public interface IState
{
    public void OnStateEnter();
    public void OnStateUpdate();
    public void OnStateExit();
}

public class WanderState : IState
{
    private Creature creature;

    public WanderState(Creature _creature)
    {
        creature = _creature;
    }

    public void OnStateEnter()
    {
        creature.agent.isStopped = false;

        if (IsOutArea())
        {
            creature.animator.SetBool("Walk", false);
            creature.animator.SetBool("Run", true);
            creature.agent.speed = creature.data.runSpeed;
            Vector3 randomPos = Vector3.forward * Random.Range(-creature.data.minWanderDistance, creature.data.minWanderDistance)
                                + Vector3.right * Random.Range(-creature.data.minWanderDistance, creature.data.minWanderDistance);
            creature.agent.SetDestination(creature.spawnPoint.position + randomPos);
        }
        else
        {
            creature.animator.SetBool("Walk", true);
            creature.animator.SetBool("Run", false);
            creature.agent.speed = creature.data.walkSpeed;
            creature.agent.SetDestination(GetWanderLocation());
        }

        //creature.animator.speed = creature.agent.speed / creature.data.walkSpeed;
    }

    public void OnStateUpdate()
    {
        
    }

    public void OnStateExit()
    {
        creature.animator.SetBool("Walk", false);
        creature.animator.SetBool("Run", false);
    }

    private bool IsOutArea()
    {
        return Vector3.Distance(creature.transform.position, creature.spawnPoint.position) > creature.data.activityAreaRadius;
    }

    private Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(creature.transform.position + (Random.onUnitSphere * Random.Range(creature.data.minWanderDistance, creature.data.maxWanderDistance)),
                                out hit, creature.data.maxWanderDistance, NavMesh.AllAreas);

        int i = 0;

        while (Vector3.Distance(creature.transform.position, hit.position) < creature.data.detectDistance)
        {
            NavMesh.SamplePosition(creature.transform.position + (Random.onUnitSphere * Random.Range(creature.data.minWanderDistance, creature.data.maxWanderDistance)),
                                out hit, creature.data.maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }

        return hit.position;
    }
}
