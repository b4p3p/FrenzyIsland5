using UnityEngine;
using System.Collections;
using Assets.Engine.AI.Flocking;
using Engine.Characters.Entity.Characters;

public class BoidController : MonoBehaviour
{
    private SwarmController swarmController;
    internal NativeController character;

    private bool inited = false;
    private float minVelocity;
    private float maxVelocity;
    private float randomness;
    private GameObject chasee;

    void Start()
    {
        character = GetComponent<NativeController>();
    }

    public void Run()
    {
        StartCoroutine("BoidSteering");
    }


    IEnumerator BoidSteering()
    {
        while (true)
        {
            float waitTime = Random.Range(0.3f, 0.5f);

            if (character == null) yield return new WaitForSeconds(waitTime);

            if ( swarmController.CurrentState == FlockingStates.FollowCenter)
            {
                CollisionDetect();
                character.Speed = Mathf.Lerp ( character.Speed ,1f , Time.deltaTime );   //Speed and animation
                character.SetDirection( character.GetDirection( swarmController.flockCenter ));
            }

            if (swarmController.CurrentState == FlockingStates.FollowPath)
            {
                CollisionDetect();
                character.Speed = Mathf.Lerp(character.Speed, 1f, Time.deltaTime);
                character.SetDirection(swarmController.Path.FirstCheckPoint);
            }

            if (swarmController.CurrentState == FlockingStates.CompletePath)
            {
                CollisionDetect();
                float dist = Vector3.Distance(swarmController.Path.LastCheckPoint,
                                              transform.position);

                if ( dist > 5)
                {
                    character.Speed = Mathf.Lerp(character.Speed, 1f, Time.deltaTime);
                    character.SetDirection( swarmController.Path.LastCheckPoint);
                }
                else
                {
                    character.Speed = 0;
                }
            }

            yield return new WaitForSeconds(waitTime);
        }
    }

    private void CollisionDetect()
    {
        Vector3 fwd = transform.position + transform.forward + Vector3.up;
        Vector3 right = transform.position + transform.right + Vector3.up + Vector3.forward;
        Vector3 left = transform.position - transform.right + Vector3.up + Vector3.forward;

        //Vector3 back = transform.TransformDirection(Vector3.back);
        RaycastHit hit;
        Ray ray_fwd = new Ray(transform.position + Vector3.up, transform.forward);
        Ray ray_left = new Ray(transform.position + Vector3.up, transform.right);
        Ray ray_right = new Ray(transform.position + Vector3.up, transform.right * (-1));
        
        int layers = 0;                                 //Character is 9

        Debug.DrawLine(transform.position + Vector3.up, fwd);
        Debug.DrawLine(transform.position + Vector3.up, right);
        Debug.DrawLine(transform.position + Vector3.up, left);

        float step = Time.deltaTime;
        float limSup = 2;

        if (Physics.Raycast(ray_fwd, out hit, 10))
        {
            if (hit.collider.gameObject.layer != 9) return;
            character.Speed = Mathf.Lerp(character.Speed, 0, step * ( limSup - hit.distance ) );
        }

        if (Physics.Raycast(ray_left, out hit, 10))
        {
            if (hit.collider.gameObject.layer != 9) return;
            character.Speed = Mathf.Lerp(character.Speed, 0, step * (limSup - hit.distance));
        }

        if (Physics.Raycast(ray_right, out hit, 10))
        {
            if (hit.collider.gameObject.layer != 9) return;
            character.Speed = Mathf.Lerp(character.Speed, 0, step * (limSup - hit.distance));
        }
    }

    public void SetController(SwarmController theController)
    {
        swarmController = theController;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position + Vector3.up * 1.5f,
        //                 collider.transform.TransformPoint(
        //                    collider.transform.forward + Vector3.up * 1.5f));
        //Gizmos.DrawLine(transform.position + Vector3.up * 1.5f,
        //                transform.TransformPoint(Vector3.forward) + Vector3.up * 1.5f);

        

        //CapsuleCollider capsule = collider as CapsuleCollider;
        //RaycastHit hit;
        //Vector3 p1 = transform.position;
        //Vector3 p2 = p1 + Vector3.up * capsule.height;

        //Gizmos.DrawSphere((p1 + p2) / 2, capsule.height);

        //CapsuleCollider capsule = collider as CapsuleCollider;
        //RaycastHit hit;
        //Vector3 p1 = transform.position;
        //Vector3 p2 = p1 + Vector3.up * capsule.height;
        //if (Physics.CapsuleCast(p1 + Vector3.up * 0.2f, p2, capsule.radius, transform.forward, out hit, 2))
        //{
        //    Debug.Log("hit! " + hit.transform.position + " dist: " + hit.distance);
        //    Gizmos.DrawSphere(hit.transform.position, 2);
        //}
    }

    public void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.layer == 9)
        //{
        //    character.Speed = 0f;
        //}
        
        //foreach (ContactPoint contact in collision.contacts)
        //{
        //    contact

            //contact

            //if (contact.GetType() != typeof(Terrain))
            //{
            //    Debug.Log(contact.GetType());
            //    character.SetSpeed(0f);
            //    return;
            //}
            //else
            //{
            //    Debug.Log("Terrain");
            //}
        //}
    }

    //public void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.layer == 9)
    //    {
    //        character.SetSpeed(0.2f);
    //    }
    //}
}
