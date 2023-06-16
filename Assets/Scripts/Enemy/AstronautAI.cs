using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AstronautAI : MonoBehaviour
{
    public static int keyEnemyCount = 0;
    private LevelManager lm;
    
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

    public AudioClip meleeSFX;
    public AudioClip deadSFX;
    
    private GameObject[] wanderPoints;
    private int currentDestinationIndex = 0;
    private Vector3 nextDestination;

    private float distanceToPlayer;
    private bool playerInFOV;
    private float elapsedTime = 0;

    private AstronautVision visionScript;
    private EnemyHealth enemyHealth;
    private int health;

    private Collider[] nearbyColliders;

    // Animator anim;
    // NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        keyEnemyCount++;
        
        player = GameObject.FindGameObjectWithTag("Player");
        // anim = GetComponent<Animator>();
        // agent = GetComponent<NavMeshAgent>();

        enemyHealth = GetComponent<EnemyHealth>();
        visionScript = GetComponentInChildren<AstronautVision>();
        lm = FindObjectOfType<LevelManager>();
        health = enemyHealth.currentHealth;
        
        currentState = FSMStates.Patrol;
        //FindNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.isGameOver)
        {
            keyEnemyCount = 0;
            return;
        }
        
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        health = enemyHealth.currentHealth;

        switch (currentState)
        {
            case FSMStates.Patrol:
                Debug.Log("State: Patrol");
                UpdatePatrolState();
                break;
            case FSMStates.Alert:
                Debug.Log("State: Alert");
                UpdateAlertState();
                break;
            case FSMStates.Chase:
                Debug.Log("State: Chase");
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                Debug.Log("State: Attack");
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

    public void PlayerSeen(bool newPlayerSeen)
    {
        this.playerInFOV = newPlayerSeen;
        AlertNearby();
    }
    
    public void Alert()
    {
        this.currentState = FSMStates.Alert;
        elapsedTime = 0;
        visionScript.ToggleIndicator(false);
    }

    void AlertNearby()
    {
        Collider[] others = Physics.OverlapSphere(transform.position, alertRadius);

        foreach(Collider other in others)
        {
            if (other.gameObject.CompareTag("Astronaut"))
            {
                AstronautAI astronaut = other.gameObject.GetComponent<AstronautAI>();
                astronaut.Alert();
            }
            if (other.gameObject.CompareTag("Robot"))
            {
                RobotAI robot = other.gameObject.GetComponent<RobotAI>();
                robot.Alert();
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

        // FaceTarget(nextDestination);

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
            visionScript.ToggleIndicator(true);
            currentState = FSMStates.Patrol;
        }

        FaceTarget(player.transform.position);
    }

    void UpdateChaseState()
    {
        // print("Chasing!");

        // anim.SetInteger("animState", 2);

        // agent.stoppingDistance = attackDistance;
        // agent.speed = enemySpeed;

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

        AudioSource.PlayClipAtPoint(deadSFX, transform.position, 2f);
        
        keyEnemyCount--;
        if (keyEnemyCount <= 0) lm.LevelBeat();
        
        Destroy(gameObject);
    }

    void FindNextPoint()
    {
        // nextDestination = wanderPoints[currentDestinationIndex].transform.position;
        // currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;

        // agent.SetDestination(nextDestination);
    }

    public void FaceTarget(Vector3 target)
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
            AudioSource.PlayClipAtPoint(meleeSFX, transform.position);

            elapsedTime = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
