using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle;
    [SerializeField] private float viewDistance;
    [SerializeField] private LayerMask targetMask;

    private Pig pig;

    private void Awake()
    {
        pig = GetComponent<Pig>();
    }

    // Update is called once per frame
    void Update()
    {
        View();
    }

    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }
    
    private void View()
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);
        
        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        Collider[] target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < target.Length; i++)
        {
            var targetTf = target[i].transform;
            if (targetTf.CompareTag("Player"))
            {
                var dir = (targetTf.position - transform.position).normalized;
                var angle = Vector3.Angle(dir, transform.forward);

                if (angle < viewAngle * 0.5f)
                {
                    //시야 내에 있다
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + transform.up, dir, out hit, viewDistance))
                    {
                        if (hit.transform.CompareTag("Player"))
                        {
                            Debug.Log("플레이어가 돼지 시야 내에 있다.");
                            Debug.DrawRay(transform.position + transform.up, dir, Color.blue);
                            pig.Run(hit.transform.position);
                        }
                    }
                }
            }
        }
    }
}
