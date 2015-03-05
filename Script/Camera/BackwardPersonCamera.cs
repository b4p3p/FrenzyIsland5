using UnityEngine;
using System.Collections;
using Engine.Characters.Entity.Characters;

public class BackwardPersonCamera : MonoBehaviour
{
	public float smooth = 3f;		// a public variable to adjust smoothing of camera motion
    public bool IsActive = false;
    
    //Transform standardPos;			// the usual position for the camera, specified by a transform in the game
    private Vector3 startPos;
    private Vector3 startFrw;
    private Transform lookAtPos;			// the position to move the camera to when using head look
    private NativeController nativeController = null;
    
    public float Z_rotation;
    private float Z_start;

    public bool IsInFlip { 
        get  { 
            return Z_rotation > 0 || Vector3.Distance ( transform.position , lookAtPos.position ) == 0; 
        } 
    }
    
	void Start()
	{
        startPos = transform.position;
        startFrw = transform.forward;
        
        //if (GameObject.Find("LookAtPos"))
        //    lookAtPos = GameObject.Find("LookAtPos").transform;

        //Z_rotation = lookAtPos.localPosition.z;
	}

    public void GrabCamera ( NativeController nativeController )
    {
        this.IsActive = true;

        if ( nativeController != null )
        {
            startPos = transform.position;  // Grab camera position
            startFrw = transform.forward;                        
            IsActive = true;
        }
        this.nativeController = nativeController;
    }

    public void ReleaseCamera()
    {
        IsActive = false;
        lookAtPos.localPosition = new Vector3(0, 2.5f, Z_start);
        lookAtPos.LookAt(nativeController.transform.position + Vector3.up * 1.2f);
    }
	
	void FixedUpdate ()
	{
        if (IsActive == false) return;

		// Grab camera
        if(nativeController != null)
        {
            // lerp the camera position to the look at position, and lerp its forward direction to match 
            transform.position = Vector3.Lerp(transform.position, lookAtPos.position, Time.deltaTime * smooth);
            transform.forward =  Vector3.Lerp(transform.forward, lookAtPos.forward, Time.deltaTime * smooth);
        }
        else
        {	
            // return the camera to standard position and direction
            transform.position = Vector3.Lerp( transform.position, startPos, Time.deltaTime * smooth);	
            transform.forward = Vector3.Lerp( transform.forward,   startFrw, Time.deltaTime * smooth);
        }


        //Flip
        if (nativeController != null && Z_rotation > 0)
        {
            lookAtPos.Translate( Vector3.right * Time.deltaTime * smooth , Space.Self);
            Z_rotation = Z_rotation - Time.deltaTime * smooth;
            if ( Z_rotation < 0)
            {
                Z_rotation = 0;
                lookAtPos.localPosition = new Vector3( 0, 2.5f , Z_start * (-1) );
            }
            lookAtPos.LookAt( nativeController.transform.position + Vector3.up * 1.2f );
        }
		
	}

    internal void Flip()
    {
        Z_start = lookAtPos.localPosition.z;
        Z_rotation = Mathf.Abs(Z_start * 2);
    }
}
