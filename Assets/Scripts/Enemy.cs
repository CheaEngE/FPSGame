using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, Health.IHealthListener
{
    enum State
    {
        Idle,
        Walk,
        Attack,
        Dying
    };

    public GameObject player;
    NavMeshAgent agent;
    Animator animator;

    State state;
    float timeForNextState = 2;

    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case State.Idle:
                float distance = (player.transform.position - 
                    (transform.position + GetComponent<CapsuleCollider>().center)).magnitude;
                if(distance < 1.0f)
                {
                    Attack();
                }
                else
                {
                    timeForNextState -= Time.deltaTime;
                    if(timeForNextState < 0)
                    {
                        StartWalk();
                    }
                }
                break;
            case State.Walk:
                if (agent.remainingDistance < 1.0f || !agent.hasPath)
                {
                    StartIdle();
                }
                break;
            case State.Attack:
                timeForNextState -= Time .deltaTime;
                if(timeForNextState < 0)
                {
                    StartIdle();
                }
                break;
        }
    }

    void Attack()
    {
        state = State.Attack;
        timeForNextState = 1.5f;
        animator.SetTrigger("Attack");
    }

    void StartIdle()
    {
        audio.Stop();
        state = State.Idle;
        agent.isStopped = true;
        animator.SetTrigger("Idle");
    }

    void StartWalk()
    {
        audio.Play();
        state = State.Walk;
        agent.destination = player.transform.position;
        timeForNextState = Random.Range(5f, 7f);
        agent.isStopped = false;
        animator.SetTrigger("Walk");
    }

    public void Die()
    {
        state = State.Dying;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        Invoke("DestroyThis", 2.5f);
    }

    void DestroyThis()
    {
        GameManager.instance.EnemyDied();
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Health>().Damage(5);
        }
    }
}
