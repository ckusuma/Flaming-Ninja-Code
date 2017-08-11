using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStatePattern : MonoBehaviour {

    #region VARIABLES

    public Animator anim;

    public EnemyStatePattern instance;

    public EnemyState currentState;

    //states
    public PatrolState patrolState;
    public AttackState attackState;
    public BlockShurikenState blockShurikenState;
    public BlockSwordState blockSwordState;
    public ChaseState chaseState;
    public InvestigateState investigateState;
    public EngardeState engardeState;

    Input_Listeners IPL;

    [SerializeField]
    float maxDistance = 10;
    [SerializeField]
    float maxDetectionDistance = 6;
    [SerializeField]
    float maxPOIDetectionDistance = 10;
    [SerializeField]
    float maxStrikingDistance = 1.5f;
    [SerializeField]
    float maxAngle = 45;
    [SerializeField]
    float attackDetectionThreshold = 2f;
    [SerializeField]
    float movementSpeed = 2;
    [SerializeField]
    float stepSize = 0.5f;
    [SerializeField]
    int chanceOfCounter = 50;
    [SerializeField]
    float backwardsImpulse = 10;
    float currTime = 0;
    bool lookedAt;
    bool playerPositionLogged;
    Vector3 playerFoundPosition;
    [SerializeField]
    float minKeptDistanceFromPlayer = 1;
    [SerializeField]
    float maxKeptDistanceFromPlayer = 2.2f;
    [SerializeField]
    float outOfBoundsEngardeDistance = 4;
    [SerializeField]
    float investigateWaitTime = 5;
    [SerializeField]
    float distanceUntilNextWaypoint = 0.5f;
    [SerializeField]
    Vector3 offset = new Vector3(0, .5f, 0);
    [SerializeField]
    float offsetMultiplier = 0.5f;
    [SerializeField]
    float attackTime = 3;
    [SerializeField]
    float impulseMultiplier = 100;
    float attackWaitTime = 6;

    //CONDITION TESTING
    bool attackNotFinished;
    bool priorityPatrolIsOn;
    bool startedEnemyAttackTimer;
    bool timerIsSetOff;
    [SerializeField]
    bool playerInFOV;
    [SerializeField]
    bool playerDetected;
    [SerializeField]
    bool playerIsAttacking;
    bool shurikenIncoming;
    bool willAttemptToCounter;
    [SerializeField]
    bool withinStrikingDistance;

    Rigidbody rb;
    GameObject player;
    public Transform playerPosition;
    [SerializeField]
    public NavMeshAgent agent;
    public Transform lookPosition;

    public GameObject normalHitMarker;
    public GameObject criticalHitMarker;



    //FOR TESTING
    public Transform spawner;
    public GameObject ball;


    public List<Transform> waypoints;
    public List<Transform> priorityWaypoints;
    int destPoint = 0;
    int priorityDestPoint = 0;

    //FOR SOUND
    public AudioSource enemyAudio;

    public AudioClip attackSound;
    public AudioClip investigateSound;
    public AudioClip patrolSound;
    public AudioClip chaseAndEngardeSound;
    public AudioClip damageSound;
    public AudioClip dieSound;
    public AudioClip normalHitSound;
    public AudioClip criticalHitSound;

    OffMeshLink offLink;
    #endregion

    #region UNITY FUNCTIONS
    private void Awake()
    {
        instance = this;
        //ADD NEW STATE PATTERNS HERE
        patrolState = this.GetComponent<PatrolState>();
        attackState = this.GetComponent<AttackState>();
        blockShurikenState = this.GetComponent<BlockShurikenState>();
        chaseState = this.GetComponent<ChaseState>();
        blockSwordState = this.GetComponent<BlockSwordState>();
        investigateState = this.GetComponent<InvestigateState>();
        engardeState = this.GetComponent<EngardeState>();
        enemyAudio = GetComponent<AudioSource>();

    }

    void Start()
    {
        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        if (IPL == null)
        {
            Debug.Assert(IPL != null, "No Input Listeners on Enemy State Pattern!");
        }
        rb = this.GetComponent<Rigidbody>();
        anim = this.GetComponentInChildren<Animator>();
        offLink = GetComponent<OffMeshLink>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = GameObject.FindGameObjectWithTag("Player Position").transform;
        agent = this.GetComponent<NavMeshAgent>();
        GoToNextPoint();
        currentState = patrolState;
        
    }

    void Update()
    {
        currentState.UpdateState();
        playerInFOV = PlayerInFieldOfView();
        withinStrikingDistance = PlayerIsWithinStrikingDistance();
        
    }

    void OnDisable()
    {
        startedEnemyAttackTimer = false;
    }

    #endregion

    #region BEHAVIOR FUNCTIONS

    public void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < distanceUntilNextWaypoint)
        {
            GoToNextPoint();
        }
        
    }

    public void PriorityPatrol()
    {
        if (!agent.pathPending && agent.remainingDistance < distanceUntilNextWaypoint)
        {
            GoToNextPriorityPoint();
        }
    }

    public void PriorityPatrolSetup()
    {
        DeletePriorityWaypoints();
        AddPriorityWaypoints();
        priorityDestPoint = 0;
    }

    public void ChasePlayer()
    {
        agent.destination = playerPosition.position;
        agent.speed = movementSpeed;
    }

    public void AvoidSword()
    {
        rb.AddForce(-transform.forward * impulseMultiplier, ForceMode.Impulse);
    }

    public void AvoidShuriken()
    {
        rb.AddForce(transform.right.normalized * backwardsImpulse, ForceMode.Impulse);
    }

    public void Investigate()
    {
        currTime += Time.deltaTime;
        if (!playerPositionLogged)
        {
            playerFoundPosition = playerPosition.position;
            playerPositionLogged = true;
        }
        
        lookedAt = true;
        agent.destination = this.transform.position;
        Quaternion rot = Quaternion.LookRotation(playerFoundPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, .02f);

        RaycastHit info;

        Debug.DrawRay(transform.position + Vector3.up * offsetMultiplier, transform.forward * maxDistance, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * offsetMultiplier, (transform.forward + transform.right).normalized * maxDistance, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * offsetMultiplier, (transform.forward - transform.right).normalized * maxDistance, Color.green);

        if (Physics.Raycast(lookPosition.position, transform.forward, out info, maxDistance))
        {
            if (info.collider.gameObject.tag == "Player" || info.collider.gameObject.tag == "Sheath" || 
                info.collider.gameObject.tag == "Left Hand" || info.collider.gameObject.tag == "Right Hand" || 
                info.collider.gameObject.tag == "Player Sword" || info.collider.gameObject.tag == "Shuriken Bag")
            {
                ResetAnimatorBools(anim);
                anim.SetBool("chase", true);
                currentState.ToChaseState();
            }
        }
        if (Physics.Raycast(lookPosition.position, (transform.forward + transform.right).normalized, out info, maxDistance))
        {
            if (info.collider.gameObject.tag == "Player" || info.collider.gameObject.tag == "Sheath" ||
                info.collider.gameObject.tag == "Left Hand" || info.collider.gameObject.tag == "Right Hand" ||
                info.collider.gameObject.tag == "Player Sword" || info.collider.gameObject.tag == "Shuriken Bag")
            {
                ResetAnimatorBools(anim);
                anim.SetBool("chase", true);
                currentState.ToChaseState();
            }
        }
        if (Physics.Raycast(lookPosition.position, (transform.forward - transform.right).normalized, out info, maxDistance))
        {
            if (info.collider.gameObject.tag == "Player" || info.collider.gameObject.tag == "Sheath" ||
                info.collider.gameObject.tag == "Left Hand" || info.collider.gameObject.tag == "Right Hand" ||
                info.collider.gameObject.tag == "Player Sword" || info.collider.gameObject.tag == "Shuriken Bag")
            {
                ResetAnimatorBools(anim);
                anim.SetBool("chase", true);
                currentState.ToChaseState();
            }
        }

        if (currTime >= investigateWaitTime)
        {
            currTime = 0;
            lookedAt = false;
            playerPositionLogged = false;
            ResetAnimatorBools(anim);
            anim.SetBool("patrol", true);
            currentState.ToPatrolState();           
        }
    }

    public void Engarde()
    {
        //agent.transform.Translate(transform.right * 0.005f);

        agent.destination = this.transform.position;
        agent.velocity = Vector3.zero;
        //transform.LookAt(playerPosition.position + offset);

        /*int leftOrRight = Random.Range(0, 2);
        //go left
        if (leftOrRight == 0)
        {
            agent.Move(Vector3.left * stepSize);
        }
        //go right
        else
        {
            agent.Move(Vector3.right * stepSize);
        }*/
    }

    public void Jump()
    {
        agent.enabled = false;
        rb.AddForce(Vector3.up * 5000, ForceMode.Impulse);       
        //agent.enabled = true;
    }

    //FOR TESTING
    public void ShootSpheres()
    {
        GameObject go = Instantiate(ball, spawner);
        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * 100, ForceMode.Impulse);
    }

    public void Attack()
    {
        transform.LookAt(playerPosition.position + offset);
        rb.AddForce(transform.forward * impulseMultiplier, ForceMode.Impulse);
    }
    #endregion

    #region BEHAVIOR HELPER FUNCTIONS

    public void TimeBeforeAttackStart()
    {
        StartCoroutine(TimeBeforeAttack());
    }

    public void AttackFinishedStart()
    {
        StartCoroutine(AttackFinishedTimer());
    }

    public IEnumerator TimeBeforeAttack()
    {
        SetAttackTimer(true);
        yield return new WaitForSeconds(attackWaitTime);
        SetAttackTimer(false);
        SetTimerSetOff(true);
    }

    public IEnumerator AttackFinishedTimer()
    {
        SetAttackNotFinished(true);
        yield return new WaitForSeconds(attackTime);
        SetAttackNotFinished(false);
    }

    void AddPriorityWaypoints()
    {
        Collider[] priorityPointsInRange = Physics.OverlapSphere(transform.position, maxPOIDetectionDistance);

        if (priorityPointsInRange.Length == 0)
        {
            return;
        }

        foreach (Collider c in priorityPointsInRange)
        {
            if (c.CompareTag("Replenisher"))
            {
                priorityWaypoints.Add(c.gameObject.transform);
            }
        }
    }

    void DeletePriorityWaypoints()
    {
        priorityWaypoints.Clear();
    }

    #endregion

    #region GETTERS AND SETTERS

    public void SetAttackNotFinished(bool b)
    {
        attackNotFinished = b;
    }

    public bool GetAttackNotFinished()
    {
        return attackNotFinished;
    }

    public void SetPriorityPatrol(bool b)
    {
        priorityPatrolIsOn = b;
    }

    public bool GetPriorityPatrol()
    {
        return priorityPatrolIsOn;
    }

    public void SetAttackTimer(bool b)
    {
        startedEnemyAttackTimer = b;
    }

    public bool GetAttackTimer()
    {
        return startedEnemyAttackTimer;
    }

    public void SetTimerSetOff(bool b)
    {
        timerIsSetOff = b;
    }

    public bool GetTimerSetOff()
    {
        return timerIsSetOff;
    }

    public Rigidbody GetRigidBody()
    {
        return rb;
    }

    #endregion

    #region STATE MACHINE CONDITIONS
    public void ResetAnimatorBools(Animator a)
    {
        a.SetBool("engarde", false);
        a.SetBool("investigate", false);
        a.SetBool("patrol", false);
        a.SetBool("chase", false);
        a.SetBool("attack", false);
        a.SetBool("block", false);
    }

    public bool EnemyIsOutsideEngardeDistance()
    {
        float dist = Vector3.Distance(this.transform.position, playerPosition.position);
        if (dist > outOfBoundsEngardeDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EnemyIsTooClose()
    {
        float dist = Vector3.Distance(this.transform.position, playerPosition.position);
        if (dist < minKeptDistanceFromPlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EnemyIsWithinAllowableBounds()
    {
        float dist = Vector3.Distance(this.transform.position, playerPosition.position);
        if (dist >= minKeptDistanceFromPlayer && dist <= maxKeptDistanceFromPlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PlayerInFieldOfView()
    {
        Collider[] objectsSeen = Physics.OverlapSphere(transform.position, maxDistance);

        if (objectsSeen.Length == 0)
        {
            return false;
        }

        foreach (Collider c in objectsSeen)
        {
            //found player
            if (c.CompareTag("Player"))
            {
                
                //now check that the enemy is ahead and in FOV
                //Debug.Log(Vector3.Angle(transform.forward, c.transform.position - transform.position);
                if (Vector3.Angle(transform.forward, (c.transform.position + offset) - lookPosition.position) < maxAngle)
                {
                    
                    RaycastHit info;

                    //now check that there is nothing in way of enemy and player
                    if (Physics.Raycast(lookPosition.position, (c.transform.position + offset) - lookPosition.position, out info))
                    {
                        //Debug.DrawRay(lookPosition.position, (c.transform.position + offset) - lookPosition.position, Color.blue);
                        //Debug.Log(info.collider.name);
                        if (info.collider.CompareTag("Player") || info.collider.CompareTag("Left Hand") || 
                            info.collider.CompareTag("Right Hand") || info.collider.CompareTag("Sheath") ||
                            info.collider.CompareTag("Shuriken Bag") || info.collider.CompareTag("Player Sword"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return false;
    }

    public bool PlayerDetected()
    {
        Collider[] objectsSeen = Physics.OverlapSphere(transform.position, maxDetectionDistance);

        if (objectsSeen.Length == 0)
        {
            return false;
        }

        foreach (Collider c in objectsSeen)
        {
            //found player
            if (c.CompareTag("Player"))
            {
                return true;
                /*RaycastHit info;

                //now check that there is nothing in way of enemy and player
                if (Physics.Raycast(transform.position, (c.transform.position + offset) - transform.position, out info))
                {
                    if (info.collider.CompareTag("Player") || info.collider.CompareTag("Left Hand") ||
                            info.collider.CompareTag("Right Hand") || info.collider.CompareTag("Sheath") ||
                            info.collider.CompareTag("Shuriken Bag") || info.collider.CompareTag("Player Sword"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }   */            
            }
        }

        return false;
    }

    public bool PlayerIsAttacking()
    {
        if (IPL.GetRightControllerVelocity().magnitude >= attackDetectionThreshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ShurikenIncoming()
    {
        Collider[] objectsSeen = Physics.OverlapSphere(transform.position, maxDistance);

        if (objectsSeen.Length == 0)
        {
            return false;
        }

        foreach (Collider c in objectsSeen)
        {
            //found shuriken
            if (c.CompareTag("Shuriken"))
            {
                //now check that the shuriken is in FOV
                if (Vector3.Angle(transform.forward, c.transform.position) < maxAngle)
                {
                    RaycastHit info;

                    //now check that there is nothing in way of enemy and shuriken
                    if (Physics.Raycast(transform.position, c.transform.position - transform.position, out info))
                    {
                        if (info.collider.CompareTag("Shuriken"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        return false;
    }

    public bool WillAttemptToCounter()
    {
        if (Random.Range(0, 100) < chanceOfCounter)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PlayerIsWithinStrikingDistance()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            return false;
        }
        else if (Vector3.Distance(this.transform.position, playerPosition.position) > maxStrikingDistance)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    #endregion

    #region NAVMESH AGENT DIRECTORIES

    void GoToNextPoint()
    {
        if (waypoints.Count == 0)
        {
            return;
        }

        agent.destination = waypoints[destPoint].position;
        agent.speed = movementSpeed;
        destPoint = (destPoint + 1) % waypoints.Count;
    }

    void GoToNextPriorityPoint()
    {
        if (priorityWaypoints.Count == 0)
        {
            SetPriorityPatrol(false);
            DeletePriorityWaypoints();
            return;
        }

        agent.destination = priorityWaypoints[priorityDestPoint].position;
        agent.speed = movementSpeed;
        priorityDestPoint++;
        //return to regular patrol
        if (priorityDestPoint == priorityWaypoints.Count)
        {
            SetPriorityPatrol(false);
            DeletePriorityWaypoints();
        }

    }

    #endregion

    #region SOUND FUNCTIONS

    public void PlaySound(AudioSource source, AudioClip clip)
    {
        if (source != null && clip != null)
        {
            source.clip = clip;
            source.pitch = Random.Range(0.9f, 1.1f);
            source.volume = Random.Range(0.9f, 1.1f);
            source.Play();
        }

    }

    #endregion

    #region UTILITIES

    

    #endregion
}
