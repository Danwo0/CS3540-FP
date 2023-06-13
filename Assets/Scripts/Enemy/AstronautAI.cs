using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AstronautAI : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public FSMStates currentState;

    public float attackDistance = 5.0f;
    public float chaseDistance = 10.0f;
    public float enemySpeed = 5.0f;
    public float shootRate = 1.0f;

    public GameObject player;
    public GameObject meleePrefab;

    GameObject[] wanderPoints;
    int currentDestinationIndex = 0;
    Vector3 nextDestination;

    float distanceToPlayer;
    bool playerInFOV;
    float elapsedTime = 0;

    EnemyHealth enemyHealth;
    int health;

    // Animator anim;
    // NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        // anim = GetComponent<Animator>();
        // wandTip = GameObject.FindGameObjectWithTag("WandTip");
        // agent = GetComponent<NavMeshAgent>();

        enemyHealth = GetComponentInChildren<EnemyHealth>();
        health = enemyHealth.currentHealth;

        currentState = FSMStates.Patrol;
        FindNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        health = enemyHealth.currentHealth;

        switch (currentState)
        {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
            case FSMStates.Dead:
                UpdateDeadState();
                break;
        }

        elapsedTime += Time.deltaTime;

        if (health <= 0)
        {
            currentState = FSMStates.Dead;
        }
    }

    public void playerSeen()
    {
        this.playerInFOV = true;
    }

    public void playerLost()
    {
        this.playerInFOV = false;
    }

    void UpdatePatrolState()
    {
        print("Patrolling!");
        print(nextDestination);

        // anim.SetInteger("animState", 1);

        // agent.stoppingDistance = 0f;
        // agent.speed = 3.5f;

        if (Vector3.Distance(transform.position, nextDestination) < 2)
        {
            FindNextPoint();
        }
        else if (playerInFOV)
        {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);

        // agent.SetDestination(nextDestination);
    }

    void UpdateChaseState()
    {
        print("Chasing!");

        // anim.SetInteger("animState", 2);

        // agent.stoppingDistance = attackDistance;
        // agent.speed = 5f;

        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
            FindNextPoint();
        }

        FaceTarget(nextDestination);

        // transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);

        // agent.SetDestination(nextDestination);
    }

    void UpdateAttackState()
    {
        print("Attacking!");

        // agent.stoppingDistance = attackDistance;

        nextDestination = player.transform.position;

        if (distanceToPlayer < attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
            FindNextPoint();
        }

        FaceTarget(nextDestination);

        // anim.SetInteger("animState", 3);

        MeleeAttack();
    }

    void UpdateDeadState()
    {
        // anim.SetInteger("animState", 4);

        // AudioSource.PlayClipAtPoint( , transform.position);

        Destroy(gameObject, 3);
    }

    void FindNextPoint()
    {
        // nextDestination = wanderPoints[currentDestinationIndex].transform.position;
        // currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;

        // agent.SetDestination(nextDestination);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    void MeleeAttack()
    {
        if (elapsedTime > shootRate)
        {
            Vector3 pos = transform.position + transform.forward;
            pos.y += 2;

            GameObject projectile = Instantiate(meleePrefab, pos, transform.rotation) as GameObject;
            projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);
            // AudioSource.PlayClipAtPoint( , transform.position);

            elapsedTime = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
