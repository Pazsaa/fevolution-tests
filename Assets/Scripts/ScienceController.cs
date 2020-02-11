using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScienceController : MonoBehaviour
{
    public NavMeshAgent Agent;
    private GameObject Bunny;
    
    // Update is called once per frame
    void Update()
    {
        Bunny = GameObject.Find("Bunny Holder");
        Agent.SetDestination(Bunny.transform.position);
    }
}
