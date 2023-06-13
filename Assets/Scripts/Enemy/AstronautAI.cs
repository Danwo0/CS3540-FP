using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class AstronautAI : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Patrol,
        Alert,
        Chase,
        Attack,
        Dead
    }

    public FSMStates currentState;

    public float attackDistance = 5.0f;
    public float chaseDistance = 10.0f;
    public float enemySpeed = 5.0f;
    public float shootRate = 1.0f;
    public float alertTimer = 5.0f;
    public float alertRadius = 20f;

    public GameObject player;
    public GameObject meleePrefab;
    public GameObject alertSphere;

    public AudioClip deadSFX;
    
    GameObject[] wanderPoints;
    int currentDestinationIndex = 0;
    Vector3 nextDestination;

    float distanceToPlayer;
    bool playerInFOV;
    float elapsedTime = 0;

    private AstronautVision visionScript;
    EnemyHealth enemyHealth;
    int health;
    private bool isDead;

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
        visionScript = GetComponentInChildren<AstronautVision>();
        health = enemyHealth.currentHealth;

        currentState = FSMStates.Patrol;
        isDead = false;
        FindNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        health = enemyHealth.currentHealth;

        switch (currentState)
        {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Alert:
                UpdateAlertState();
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

        AlertNearby();
    }

    public void playerLost()
    {
        this.playerInFOV = false;
    }
    public void alert()
    {
        this.currentState = FSMStates.Alert;
        elapsedTime = 0;
    }

    void AlertNearby()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, alertRadius);
        foreach (var other in hitColliders)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                AstronautAI astronaut = other.gameObject.GetComponent<AstronautAI>();
                RobotAI robot = other.gameObject.GetComponent<RobotAI>();

                if (astronaut != null)
                {
                    astronaut.alert();
                    Debug.Log("Alerted " + other.name);
                }
                else if (robot != null)
                {
                    robot.alert();
                    Debug.Log("Alerted " + other.name);
                }
            }
        }
    }

    void UpdatePatrolState()
    {
        // print("Patrolling!");
        // print(nextDestination);

        // anim.SetInteger("animState", 1);

        // agent.stoppingDistance = 0f;
        // agent.speed = 3.5f;

        if (Vector3.Distance(transform.position, nextDestination) < 2)
        {
            FindNextPoint();
        }
        else if (playerInFOV)
        {
            visionScript.ToggleIndicator(false);
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);

        // agent.SetDestination(nextDestination);
    }

    void UpdateAlertState()
    {
        print("Alert!");

        if (distanceToPlayer < chaseDistance)
        {
            currentState = FSMStates.Chase;
        }

        if (elapsedTime > alertTimer)
        {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(player.transform.position);
    }

    void UpdateChaseState()
    {
        // print("Chasing!");

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
            visionScript.ToggleIndicator(true);
            currentState = FSMStates.Patrol;
            FindNextPoint();
        }

        FaceTarget(nextDestination);

        // transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);

        // agent.SetDestination(nextDestination);
    }

    void UpdateAttackState()
    {
        // print("Attacking!");

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
            visionScript.ToggleIndicator(true);
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

        isDead = true;
        AudioSource.PlayClipAtPoint(deadSFX, transform.position, 2f);

        Destroy(gameObject);
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
