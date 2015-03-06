using UnityEngine;
using System.Collections;
using Assets.Engine.Other;
using System.Collections.Generic;
using Assets.Engine.AI.Flocking;
using Assets.Engine.SimpleArrow;
using Engine.ComputerGraphics.Component;

public class SwarmController : MonoBehaviour
{
    public SwarmController()
    {
        CurrentState = FlockingStates.Idle;
    }

    public GameObject CharacterPrefab;
    public int flockSize = 20; 

    internal Vector3 flockCenter = Vector3.zero;
    
    internal FlockingStates CurrentState { get; set; }

    public Path Path { get; set; }

    private List<BoidController> ListBoids = new List<BoidController>();

    public IslandBehaviour Island 
    {
        get {
            if (island == null)
            {
                island = FindObjectOfType<Terrain>().GetComponent<IslandBehaviour>();
                return island;
            }
            return island;
        }
    }
    private IslandBehaviour island;

    public float minVelocity = 5;
    public float maxVelocity = 20;
    public float randomness = 1;
    
    public Vector3 flockVelocity;

    void Start()
    {
    }

    internal void Load(LinkedList<BoidController> lstBoids)
    {
        foreach (BoidController bs in lstBoids)
        {
            ListBoids.Add(bs);
            bs.Run();
        }
        
    }

    public void CreateBoids()
    {
        Debug.Log("CreateBoids");

        ListBoids = new List<BoidController>(flockSize);

        for (var i = 0; i < flockSize; i++)
        {
            Vector3 position = RandomPosition();
            GameObject boid = Instantiate(CharacterPrefab, position, Quaternion.identity) as GameObject;
            boid.transform.parent = transform;

            BoidController bc = boid.GetComponent<BoidController>();
            bc.SetController(this);
            bc.Run();
            
            ListBoids.Add (bc);
        }

    }

    void Update()
    {
        bool IsIdle = false;
        Vector3 v_sum = Vector3.zero;
        foreach (BoidController boid in ListBoids)
        {
            v_sum = v_sum + boid.transform.position;
            
            if ( flockCenter != Vector3.zero) //from second update
            {
                if (Path != null && Path.FirstCheckPoint != Vector3.zero)
                {
                    if (Vector3.Distance(Path.FirstCheckPoint,
                                         boid.transform.position) < 1)
                    {
                        IsIdle = GroundQuiver.GoOnNextCheckPoint(Path);
                    }
                }
            }
        }

        flockCenter = v_sum / (flockSize);

        if ( IsIdle )
        {   
            //foreach (BoidController boid in ListBoids)
            //{
            //    boid.character.Speed = 0;
            //}
            CurrentState = FlockingStates.CompletePath;
        }
    }

    private Vector3 RandomPosition()
    {
        float Center = Island.TerrainSize / 2;
        float x = Randomize.NextFloat( Center - 150 , Center + 150 );
        float z = Randomize.NextFloat( Center - 150 , Center + 150 );
        return new Vector3(x, Island.GetRealHeight(z, x), z); 
                             
    }

    private void OnDrawGizmos()
    {
        Vector3 center = flockCenter;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(center - Vector3.up * 40, center + Vector3.up * 40);

        Gizmos.color = Color.yellow;
        //if (Path!=null)
        //    if (Path.CheckPoints != null)
        //        foreach (Vector3 p in Path.CheckPoints)
        //        {
        //            Gizmos.DrawLine( p - Vector3.up * 40, p + Vector3.up * 40);
        //        }

        if (Path == null) return;
        if (Path.LastCheckPoint == null) return;

        Gizmos.DrawLine(Path.LastCheckPoint - Vector3.up * 50, 
                        Path.LastCheckPoint + Vector3.up * 50);
    }
}
