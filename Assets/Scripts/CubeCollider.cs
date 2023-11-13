using UnityEngine;

public class CubeCollider : MonoBehaviour
{
    public bool hasCollided = false;
    private int parentID;

    void Start()
    {
        parentID = transform.parent.gameObject.GetInstanceID();
    }

    void Update()
    {
        if (!hasCollided)
        {
            CheckForCollisionBelow();
        }
    }

    private void CheckForCollisionBelow()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f))
        {
            if (hit.collider.CompareTag("Block") && hit.collider.transform.parent.gameObject.GetInstanceID() != parentID)
            {
                hasCollided = true;
            }
        }
    }

    public bool AllowMove(string direction)
    {
        Vector3 target;

        switch (direction)
        {
            case "back":
                target = new Vector3(-1,0,0);
                break;

            case "front":
                target = new Vector3(1,0,0);
                break;

            case "right":
                target = new Vector3(0,0,-1);
                break;

            case "left":
                target = new Vector3(0,0,1);
                break;
            
            default:
                target = Vector3.zero;
                break;
        }

        RaycastHit hit;

        if (Physics.Raycast(transform.position, target, out hit, 0.5f))
        {
            if (hit.collider.CompareTag("Block") && hit.collider.transform.parent.gameObject.GetInstanceID() != parentID)
            {
                return false;
            }
        }

        return true;
    }
}
