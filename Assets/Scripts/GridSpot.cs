using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridSpot : MonoBehaviour
{
    void Start()
    {
               
    }

    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Block"))
        {
            DrawGizmo();
        }
    }

    public void DrawGizmo()
    {
        Debug.DrawRay(transform.position, Vector3.up * 0.25f, Color.red);
        Debug.DrawRay(transform.position, Vector3.down * 0.25f, Color.red);
        Debug.DrawRay(transform.position, Vector3.left * 0.25f, Color.red);
        Debug.DrawRay(transform.position, Vector3.right * 0.25f, Color.red);
        Debug.DrawRay(transform.position, Vector3.forward * 0.25f, Color.red);
        Debug.DrawRay(transform.position, Vector3.back * 0.25f, Color.red);
    }
}
