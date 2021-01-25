using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class AiMovement : MonoBehaviour
{
    //Components needed to run this Script
    [Header("Needed Components")]
    public Camera cam;
    public NavMeshAgent agent;
    public GameObject player;
    public Transform[] wp;
    [Space]

    //Optional: These Variables should be hardcoded in the final Game
    [Header("Patrol Radius and wait Time")]
    [SerializeField]
    private float patrolRadius = 20f;
    [SerializeField]
    private float patrolTimer = 5f;
    [Space]

    //Bools used to choose Patrolmode, only one at a time usable
    [Header("Choose 1 Patrolmode")]
    [SerializeField]
    private bool chaseMode;
    [SerializeField]
    private bool randomWpPatrol;
    [SerializeField]
    private bool wpPatrol;
    [SerializeField]
    private bool randomPositionPatrol;

    private int wpIndex;
    private float timerCount;


    private void Start()
    {
        wpIndex = 0;
        timerCount = patrolTimer;
    }
    // Update is called once per frame
    // here we check which patrolemode is choosen
    void Update()
    {
        if (wpPatrol)
        {
            Patrol();
        }
        else if (chaseMode)
        {
            agent.SetDestination(player.transform.position);
        }
        else if (randomWpPatrol)
        {
            RandomPatrol();
        }
        else if (randomPositionPatrol)
        {
            RandomPositionPatrol();
        }
        else
        {
            MoveToMouseClick();
        }
    }


    // If no Patrolmode is choosen the left Mousebutton can be used
    // to set the agents destination
    void MoveToMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
        else { return; }
    }

    // used if bool randomWpPatrol = true
    // set 1 random wp as destination
    void RandomPatrol()
    {
        if (agent.remainingDistance < 1)
        {
            int d = Random.Range(0, wp.Length);
            agent.SetDestination(wp[d].position);
            Debug.Log(wp[d].position);
        }
    }

    // Used if bool wpPatrol = true
    // set destination to wp position
    // goes up one index after destination is reached
    void Patrol()
    {
        if (agent.remainingDistance < 1)
        {
            agent.SetDestination(wp[wpIndex].position);
            Debug.Log("current wp index: " + wpIndex);
            wpIndex++;

            // set index back to 0 if the last wp in the array is reached 
            if (wpIndex == wp.Length)
            {
                wpIndex = 0;
            }

        }
    }

    // Used if bool randomPatrol = true
    // set random Destination
    // use Timer to wait befor setting new random Destination
    void RandomPositionPatrol()
    {
        timerCount += Time.deltaTime;

        // if waittime is over set timerCount back to 0
        if (timerCount > patrolTimer)
        {
            SetRandomPosition();

            timerCount = 0;
        }
    }


    // Set agent destination to random position
    void SetRandomPosition()
    {
        int layerMask = 9;
        Vector3 randomPosition = NavigationSphere(transform.position, patrolRadius, layerMask);
        agent.SetDestination(randomPosition);
        Debug.Log(randomPosition);
    }

    // Creates sphere around character position
    // get randome position in that sphere
    // returns position as Vector3
    Vector3 NavigationSphere(Vector3 currentPosition, float patrolRadius, int layerMask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += currentPosition;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, patrolRadius, layerMask);

        return navHit.position;
    }

    // Used to draw the wireSphere in the sceneView
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
