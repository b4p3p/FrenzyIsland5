using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Engine.AI.Flocking;
using Assets.Engine.SimpleArrow;

public class PartyController : MonoBehaviour {

    public GameObject PartyPrefab;

    private int IDParty = 0;
    private List<SwarmController> ListParty = new List<SwarmController>();

	// Use this for initialization
	void Start () 
    {
        /* Start with prefab*/
        foreach (Transform party in transform)
        {
            
            if (  party.gameObject.activeInHierarchy == false) continue;

            SwarmController swarm = party.GetComponent<SwarmController>();
            ListParty.Add( swarm );

            LinkedList<BoidController> lstBoids = new LinkedList<BoidController>();
            foreach (Transform Boid in party.transform)
            {
                BoidController bc = Boid.GetComponentInChildren<BoidController>();
                bc.SetController(swarm);
                lstBoids.AddLast(bc);
            }
            swarm.Load(lstBoids);

        }
	}
	
	// Update is called once per frame
	void Update () {
        GetKeyDown();
	}

    private void GetKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwarmController controller = ListParty[0];
            controller.CurrentState = FlockingStates.FollowCenter;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwarmController controller = ListParty[0];
            controller.Path = GroundQuiver.CreateBezierPath(
                controller.Island, controller.flockCenter, 
                "pathFlocking" , 4 , 50 , 50, 0.2f );
            controller.CurrentState = FlockingStates.FollowPath;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            CreateParty();
        }
    }

    private void CreateParty()
    {
        IDParty++;
        GameObject newParty = Instantiate(PartyPrefab) as GameObject;
        newParty.name = "party_" + IDParty;
        newParty.transform.parent = transform;

        SwarmController swarm = newParty.GetComponent<SwarmController>();
        swarm.CreateBoids();
        ListParty.Add(swarm);

    }
}
