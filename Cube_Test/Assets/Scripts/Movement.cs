using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
     
    [SerializeField] private float moveSpeed,moveRotate;
    [SerializeField] private bool isMove,check;
    [SerializeField] private bool path;
    [SerializeField] private GameObject[] cube;

    private Vector3 cubePos;
    private Quaternion cubeRot;

    private Vector3 targetPos,tmpTarget,targetDir,dir;
    private Quaternion targetRot;
    
    private Ray ray;
    private RaycastHit hit;

    private void Start()
    {
        cubePos = transform.position;
        cubeRot = transform.rotation;
    }

    private void Update()
    {
        Move();
        
    }
    private void Move()
    {
        if(isMove)
        {
            if(tmpTarget!=targetPos&&check)
            {
                transform.position += (tmpTarget - transform.position).normalized * moveSpeed * Time.deltaTime;
                if (Vector3.Distance(tmpTarget, transform.position) < 0.01)
                    check = false;
            }
            else
                transform.position += (targetPos - transform.position).normalized * moveSpeed * Time.deltaTime;

            transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRot,moveRotate);
            if (Vector3.Distance(targetPos, transform.position) < 0.01 && transform.rotation==targetRot)
                isMove = false;
        }
        else if(!isMove)
        {
            if (Input.GetMouseButtonDown(0))
            { 
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name != "Cube3_1")
                {   
                    if(hit.collider.tag == "Cubes")
                    {
                        isMove = true;
                        targetPos = hit.collider.gameObject.transform.position;
                        targetRot = hit.collider.gameObject.transform.rotation;
                        if(path)
                            CheckDist();
                        
                    }         
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isMove = true;
                targetPos = cubePos;
                targetRot = cubeRot;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if(!path)
                    path = true;
                else if (path)
                    path = false;
            }
                
        }
        
    }
    void CheckDist()
    {
        for (int i = 0; i < cube.Length; i++)
        {
            targetDir = targetPos - transform.position;
            dir = cube[i].gameObject.transform.position - transform.position;
            if (dir.magnitude < targetDir.magnitude)
            {
                tmpTarget = cube[i].gameObject.transform.position;
                check = true;
            }
        }
        
    }
}
