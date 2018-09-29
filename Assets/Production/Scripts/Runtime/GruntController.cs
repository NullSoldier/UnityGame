using System;
using System.Collections;
using MonsterLove.StateMachine;
using UnityEngine;
using UnityEngine.AI;

public class GruntController : MonoBehaviour, IAnimationEventReceiver
{
    public int MaxHealth = 3;
    public int Health = 0;
    public int Damage = 1;
    public bool IsDead = false;
    public float AttackCooldown = 2f;

    private Animator anim;
    private GameObject body;
    private CapsuleCollider collision;
    private Rigidbody rigidBody;
    private NavMeshAgent navAgent;
    private StateMachine<EnemyStates> states;
    private float attackTimer = 0f;
    private GameObject attackTarget;

    public enum EnemyStates
    {
        Init,
        Idle,
        Chase,
        TakeDamage,
        Die,
        Attack        
    }

    public void TakeDamage(int amount)
    {
        if (IsDead)
            return;

        Health -= amount;

        if (Health > 0)
            states.ChangeState(EnemyStates.TakeDamage);
        else
            states.ChangeState(EnemyStates.Die);
    }

    public void OnAnimationEvent(AnimationEvent ev)
    {
        if (ev.stringParameter == "OnAttack")
            attemptDamageTarget();
    }

    private void Start ()
    {
        navAgent = GetComponent<NavMeshAgent>();
        collision = GetComponent<CapsuleCollider>();
        rigidBody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        body = transform.Find("Grunt").gameObject;
        
        if (navAgent == null)
            throw new Exception("navAgent object required");
        if (rigidBody == null)
            throw new Exception("rigidBody object required");
        if (collision == null)
            throw new Exception("CapsuleCollider object required");
        if (body == null)
            throw new Exception("body object required");

	    navAgent.updatePosition = false;
        navAgent.autoBraking = true;

        states = StateMachine<EnemyStates>.Initialize(this);
        states.ChangeState(EnemyStates.Init);
    }

    private void Update()
    {
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
    }

    private void Init_Enter()
    {
        Health = MaxHealth;
        states.ChangeState(EnemyStates.Idle);
    }

    private void Idle_Enter()
    {
        anim.SetBool("IsMoving", false);
    }

    private void Idle_Update()
    {
        var player = GameObject.FindGameObjectWithTag("Player").transform;
        var controller = player.GetComponent<PlayerController>();

        if(!controller.IsDead)
            states.ChangeState(EnemyStates.Chase);

    }

    private void Chase_Enter()
    {
        navAgent.isStopped = false;
    }

    private bool IsInAttackRange(GameObject target)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);
        return distanceToPlayer <= navAgent.stoppingDistance;
    }

    private GameObject GetPlayerTarget()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    private void Chase_Update()
    {
        float distance = Vector3.Distance(navAgent.nextPosition, transform.position);
        anim.SetBool("IsMoving", distance > 0f);
        transform.position = navAgent.nextPosition;

        GameObject target = GetPlayerTarget();

        if (IsInAttackRange(target))
        {
            attackTarget = target;
            navAgent.SetDestination(transform.position);
            navAgent.isStopped = true;

            if (attackTimer <= 0)
                states.ChangeState(EnemyStates.Attack);
        }
        else
        {
            navAgent.isStopped = false;
            navAgent.SetDestination(target.transform.position);
        }
    }

    private void Chase_Exit()
    {
        navAgent.isStopped = true;
        navAgent.ResetPath();
        anim.SetBool("IsMoving", false);
    }

    private IEnumerator Attack_Enter()
    {
        anim.SetTrigger("Attack");
        attackTimer = AttackCooldown;

        yield return new WaitForSeconds(1);

        if (attackTarget.GetComponent<PlayerController>().IsDead)
            states.ChangeState(EnemyStates.Idle);
        else
            states.ChangeState(EnemyStates.Chase);
    }

    private IEnumerator TakeDamage_Enter()
    {
        anim.SetTrigger("TakeDamage");
        yield return new WaitForSeconds(1);
        states.ChangeState(EnemyStates.Idle);
    }

    private void Die_Enter()
    {
        IsDead = true;
        collision.isTrigger = true;
        anim.SetTrigger("Die");
        rigidBody.isKinematic = true;
        Destroy(gameObject, 1f);
    }

    private void attemptDamageTarget()
    {
        GameObject target = GetPlayerTarget();

        if (IsInAttackRange(target))
            target.GetComponent<PlayerController>().TakeDamage(Damage);
    }
}
