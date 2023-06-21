using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AstronautAI : MonoBehaviour
{
    public static int keyEnemyCount = 0;

    public enum FSMStates
    {
        Idle,
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

    private Vector3 nextDestination;

    private float distanceToPlayer;
    private float elapsedTime = 0;
    private float attackElapsedTime = 0;
    private Vector3 alertPosition;

    private EnemyHealth enemyHealth;
    private int health;
    private LevelManager lm;

    public Transform enemyEyes;
    public float fieldOfView = 45f;

    private Collider[] nearbyColliders;

    // Animator anim;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        keyEnemyCount++;
        player = GameObject.FindGameObjectWithTag("Player");
        // anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        lm = FindObjectOfType<LevelManager>();

        enemyHealth = GetComponent<EnemyHealth>();
        health = enemyHealth.currentHealth;
        
        currentState = FSMStates.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.isGameOver)
        {
            return;
        }

        UpdateValues();

        switch (currentState)
        {
            case FSMStates.Idle:
                UpdateIdleState();
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
        attackElapsedTime += Time.deltaTime;

        if (health <= 0 && currentState != FSMStates.Dead)
        {
            currentState = FSMStates.Dead;
        }
    }

    void UpdateValues()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        int prevHealth = health;
        health = enemyHealth.currentHealth;

        if (health < prevHealth)
        {
            currentState = FSMStates.Alert;
            elapsedTime = 0;
            alertPosition = player.transform.position;
        }
    }

    public void Alert()
    {
        if (currentState == FSMStates.Idle)
        {
            this.currentState = FSMStates.Alert;
            elapsedTime = 0;
            alertPosition = player.transform.position;
        }
    }

    void AlertNearby()
    {
        Collider[] others = Physics.OverlapSphere(transform.position, alertRadius);

        foreach (Collider other in others)
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

    void UpdateIdleState()
    {
        if (IsPlayerInClearFOV())
        {
            currentState = FSMStates.Chase;
            AlertNearby();
        }
    }

    void UpdateAlertState()
    {
        if (IsPlayerInClearFOV())
        {
            currentState = FSMStates.Chase;
            AlertNearby();
        }
        else if (elapsedTime > alertTimer)
        {
            currentState = FSMStates.Idle;
        }

        FaceTarget(alertPosition);
    }

    void UpdateChaseState()
    {
        // anim.SetInteger("animState", 2);

        agent.speed = enemySpeed;

        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance + .5f)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Idle;
        }

        FaceTarget(nextDestination);
        agent.SetDestination(nextDestination);
    }

    void UpdateAttackState()
    {
        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > attackDistance + .5f && distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Idle;
        }

        FaceTarget(nextDestination);

        // anim.SetInteger("animState", 3);

        MeleeAttack();
    }

    void UpdateDeadState()
    {
        // anim.SetInteger("animState", 4);
        AudioSource.PlayClipAtPoint(deadSFX, transform.position);
        keyEnemyCount--;
        
        Destroy(gameObject);
        
        if (keyEnemyCount <= 0)
        {
            lm.LevelBeat();
        }
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
        if (attackElapsedTime > shootRate)
        {
            Vector3 pos = transform.position + transform.forward;
            pos.y += 2;

            GameObject projectile = Instantiate(meleePrefab, pos, transform.rotation) as GameObject;
            projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);
            AudioSource.PlayClipAtPoint(meleeSFX, transform.position);

            attackElapsedTime = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        // attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Gizmos.color = Color.blue;
        Vector3 frontRayPoint = enemyEyes.position + (enemyEyes.forward * chaseDistance);
        Vector3 leftRayPoint = Quaternion.Euler(0, fieldOfView * .5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -fieldOfView * .5f, 0) * frontRayPoint;

        Debug.DrawLine(enemyEyes.position, frontRayPoint, Color.cyan);
        Debug.DrawLine(enemyEyes.position, leftRayPoint, Color.yellow);
        Debug.DrawLine(enemyEyes.position, rightRayPoint, Color.yellow);
    }

    // courtesy of Calgar Yildrim
    bool IsPlayerInClearFOV()
    {
        RaycastHit hit;

        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;

        if (Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fieldOfView)
        {
            if (Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance))
            {
                if (hit.collider.CompareTag("PlayerDetector"))
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        return false;
    }
}