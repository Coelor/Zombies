using UnityEngine;
using UnityEngine.AI;

public class PetController : MonoBehaviour
{
    public float roamRadius = 20f;
    public float idleTime = 3f;
    
    NavMeshAgent agent;
    Animator animator;
    float idleTimer = 0f;
    Vector3 roamTarget;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetNewRoamTarget();
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if ((idleTimer += Time.deltaTime) >= idleTime)
            {
                SetNewRoamTarget();
                idleTimer = 0f;
            }
            animator.SetFloat("forward", 0.0f);
            animator.SetBool("isIdle", false);
        }
        else
        {
            animator.SetFloat("forward", 1.0f); 
            animator.SetBool("isIdle", true);
        }
    }

    void SetNewRoamTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, roamRadius, NavMesh.AllAreas))
        {
            roamTarget = hit.position;
            agent.SetDestination(roamTarget);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
    }

}