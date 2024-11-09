using UnityEngine;

public class AlertedState : IState
{
    private Creature creature;

    public AlertedState(Creature _creature)
    {
        creature = _creature;
    }

    public void OnStateEnter()
    {
        creature.animator.SetBool("Walk", false);
        creature.animator.SetBool("Run", false);
        creature.agent.isStopped = true;
    }

    public void OnStateUpdate()
    {
        RotateToTarget(DirectionToTarget());
    }

    public void OnStateExit()
    {
        creature.animator.SetBool("Walk", false);
        creature.animator.SetBool("Run", false);
    }

    private void RotateToTarget(Vector3 direction)
    {
        float rotZ = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        creature.transform.rotation = Quaternion.Euler(0, rotZ, 0);
    }

    private Vector3 DirectionToTarget()
    {
        return (PlayerManager.Instance.Player.transform.position - creature.transform.position).normalized;
    }
}