using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public NavMeshAgent agent;

    public Transform player;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange; 
    public LayerMask whatIsPlayer;
    public LayerMask whatIsGround; 


    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake(){
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>(); 
    }

    private void Update(){
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) patrolling();
        if (playerInSightRange && !playerInAttackRange) chasePlayer();
        if (playerInSightRange && playerInAttackRange) attackPlayer();





    }

    private void patrolling (){
        if (!walkPointSet) searchWalkPoint();

        if (walkPointSet){
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f) {
            walkPointSet = false;
        }

    }

    private void searchWalkPoint(){
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)){
            walkPointSet = true;
        }




    }

    private void chasePlayer(){
        agent.SetDestination(player.position);

    }

    private void attackPlayer(){
        //make sure enemy doesn't move

        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked){
            //attack
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 34f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);



            alreadyAttacked = true;
            Invoke(nameof(resetAttack), timeBetweenAttacks);
        }

    }

    private void resetAttack(){
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}
