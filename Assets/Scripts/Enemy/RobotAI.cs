using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class RobotAI : MonoBehaviour
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

    public float attackDistance = 15.0f;
    public float chaseDistance = 20.0f;
    public float enemySpeed = 5.0f;
    public float shootRate = 1.0f;

    public GameObject player;

    GameObject[] wanderPoints;
    int currentDestinationIndex = 0;
    Vector3 nextDestination;

    float distanceToPlayer;
    bool playerInFOV;
    float elapsedTime = 0;

    private RobotVision visionScript;
    EnemyHealth enemyHealth;
    int health;

    // Animator anim;
    NavMeshAgent agent;

    public GameObject bulletPrefab;

    public Transform barrel1;
    public Transform barrel2;

    public AudioClip shootSFX;

    public float bulletSpeed = 25f;
    public int damage = 20;
    public float shootInterval = 0.5f;

    private int barrel;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        // anim = GetComponent<Animator>();
        // wandTip = GameObject.FindGameObjectWithTag("WandTip");
        agent = GetComponent<NavMeshAgent>();

        enemyHealth = GetComponentInChildren<EnemyHealth>();
        visionScript = GetComponentInChildren<RobotVision>();
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
            visionScript.ToggleIndicator(false);
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);

        // agent.SetDestination(nextDestination);
    }

    void UpdateChaseState()
    {
        print("Chasing!");

        // anim.SetInteger("animState", 2);

        agent.stoppingDistance = attackDistance;
        agent.speed = 5f;

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

        agent.SetDestination(nextDestination);
    }

    void UpdateAttackState()
    {
        print("Attacking!");

        agent.stoppingDistance = attackDistance;

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

        ShootProjectile();
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

    void ShootProjectile()
    {
        if (elapsedTime > shootRate)
        {
            Transform bulletSource = barrel % 2 == 0 ? barrel1 : barrel2;
            barrel = (barrel + 1) % 2;

            GameObject bullet = Instantiate
                (bulletPrefab, bulletSource.position + bulletSource.forward, bulletSource.rotation) as GameObject;

            bullet.GetComponent<EnemyBulletBehavior>().SetDamage(damage);
            bullet.transform.LookAt(player.transform);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(bullet.transform.forward * bulletSpeed, ForceMode.VelocityChange);

            bullet.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);

            // AudioSource.PlayClipAtPoint(shootSFX, bulletSource.position);

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
