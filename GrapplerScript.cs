using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplerScript : MonoBehaviour
{
    [SerializeField]
    Transform Hook;

    DistanceJoint2D DistJoint;

    LineRenderer LR;


    // raycast 
    RaycastHit2D Hit;

    bool GrapplerStatus;

    Vector3 GrapplerPoint;

    bool PlayerMove;


    void Start()
    {
        DistJoint = GetComponent<DistanceJoint2D>();

        LR = GetComponent<LineRenderer>();

        DistJoint.enabled = false;

    }

    
    void Update()
    {

        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * 3f, 0, 0);

        LR.SetPosition(0, transform.position);
        LR.SetPosition(1, Hook.position);

        Hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);

        if(Input.GetMouseButtonDown(0) )
        {
            // grapple
            if (GrapplerStatus == false && Hit.transform != null)
            {
                GrapplerStatus = true;

                GrapplerPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

            }
            else if(GrapplerStatus == true)
            {
                GrapplerStatus = false;
                
                DistJoint.enabled = false;
                Hook.position = transform.position;
                Hook.parent = transform;
                PlayerMove = false;
            }
                

        }

        if(GrapplerStatus)
        {

            if (Hook.parent != null)
            {
                Hook.parent = null;
            }

            if ((Hook.position-GrapplerPoint).sqrMagnitude < 0.1f )
            {
                //player start moving

                if (DistJoint.enabled == false)
                {
                    DistJoint.enabled = true;
                    PlayerMove = true;
                }
            }
            else
            {
                Hook.position = Vector3.Lerp(Hook.position, GrapplerPoint, 0.1f);
            }
        }


        if(PlayerMove)
        {
            transform.position = Vector3.Lerp(transform.position, Hook.position, 0.01f);

            if ((transform.position - Hook.position).sqrMagnitude < 0.5f)
            {
              

                PlayerMove = false;
            }

        }

    }
}
