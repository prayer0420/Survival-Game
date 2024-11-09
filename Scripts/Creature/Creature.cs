using SurvivalEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Creature : MonoBehaviour, IDamagable
{
    public CreatureData data;
    protected float currentHealth;

    protected StateContext stateContext;
    protected float playerDistance;

    public Transform spawnPoint;
    public CreatureState creatureState;
    public NavMeshAgent agent;
    public Animator animator;
    protected SkinnedMeshRenderer meshRenderer;

    public GameObject creatureUI;
    public Image hpBar;

    protected float wanderRate;
    protected float lastWanderTime;

    public bool isAlerted;
    public bool isChasing;
    public bool isEscaping;
    public bool isAttacked;

    protected virtual void OnEnable()
    {
        spawnPoint = CreatureSpawnManager.Instance.pool.Pools[data.id].spawnPoint;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        stateContext = new StateContext(this);
        isAlerted = false;
        isChasing = false;
        isEscaping = false;
        isAttacked = false;
        currentHealth = data.health;
        creatureState = CreatureState.Idle;
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        playerDistance = Vector3.Distance(transform.position, PlayerManager.Instance.Player.transform.position);

        if (creatureState != CreatureState.Dead && currentHealth < data.health)
        {
            currentHealth += data.passiveHealthRecovery * Time.deltaTime;
        }

        if (isAlerted)
        {
            creatureUI.SetActive(true);
            creatureUI.transform.rotation = Camera.main.transform.rotation;
            hpBar.fillAmount = currentHealth / data.health;
        }
        else
        {
            creatureUI.SetActive(false);
        }
    }

    protected void ChangeState(CreatureState _creatureState)
    {
        creatureState = _creatureState;
        switch (creatureState)
        {
            case CreatureState.Idle:
                stateContext.Transition(stateContext.idle);
                break;
            case CreatureState.Wander:
                stateContext.Transition(stateContext.wander);
                break;
            case CreatureState.Alerted:
                stateContext.Transition(stateContext.alerted);
                break;
            case CreatureState.Chase:
                stateContext.Transition(stateContext.chase);
                break;
            case CreatureState.Attack:
                stateContext.Transition(stateContext.attack);
                break;
            case CreatureState.Escape:
                stateContext.Transition(stateContext.escape);
                break;
            case CreatureState.Dead:
                stateContext.Transition(stateContext.dead);
                break;
        }
    }

    protected bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = PlayerManager.Instance.Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < data.fieldOfView * 0.5;
    }

    protected bool CanChase()
    {
        NavMeshPath path = new NavMeshPath();
        return agent.CalculatePath(PlayerManager.Instance.Player.transform.position, path);
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            //creatureState = CreatureState.Dead;
            //stateContext.Transition(stateContext.dead);
            ChangeState(CreatureState.Dead);
            Invoke("Die", 2f);
        }

        StartCoroutine(DamageFlash());
    }

    protected IEnumerator DamageFlash()
    {
        Color[] colors = new Color[meshRenderer.materials.Length];

        for (int i = 0; i < meshRenderer.materials.Length; i++)
        {
            colors[i] = meshRenderer.materials[i].color;
            meshRenderer.materials[i].color = new Color(1.0f, 0.6f, 0.6f);
        }
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < meshRenderer.materials.Length; i++)
        {
            meshRenderer.materials[i].color = colors[i];
        }
    }

    protected void Die()
    {
        for (int i = 0; i < data.dropOnDeath.Length; i++)
        {
            Instantiate(data.dropOnDeath[i].dropPrefab, transform.position, Quaternion.identity);
        }

        CreatureSpawnManager.Instance.pool.Release(gameObject);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetCurrentHealth(float health)
    {
        currentHealth = health;
    }

}