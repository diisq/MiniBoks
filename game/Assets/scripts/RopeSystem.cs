using UnityEngine;

public class RopeSystem : MonoBehaviour
{
    private static GameObject firstSelected = null; // first object selected
    public LineRenderer linePrefab; // optional line for visual rope

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (firstSelected == null)
                {
                    firstSelected = clickedObject; // select first object
                }
                else if (firstSelected != clickedObject)
                {
                    CreateRope(firstSelected, clickedObject); // connect to second
                    firstSelected = null;
                }
            }
        }
    }

    void CreateRope(GameObject a, GameObject b)
    {
        Rigidbody2D rbA = a.GetComponent<Rigidbody2D>();
        Rigidbody2D rbB = b.GetComponent<Rigidbody2D>();
        if (rbA == null || rbB == null) return;

        // Add DistanceJoint2D to A connecting to B
        DistanceJoint2D joint = a.AddComponent<DistanceJoint2D>();
        joint.connectedBody = rbB;
        joint.autoConfigureDistance = false;
        joint.distance = Vector2.Distance(a.transform.position, b.transform.position);
        joint.enableCollision = true; // optional: objects can collide

        // Optional: add visual rope
        if (linePrefab != null)
        {
            LineRenderer line = Instantiate(linePrefab);
            line.positionCount = 2;
            line.SetPosition(0, a.transform.position);
            line.SetPosition(1, b.transform.position);
            line.gameObject.AddComponent<RopeLineUpdater>().SetObjects(a.transform, b.transform);
        }
    }
}

public class RopeLineUpdater : MonoBehaviour
{
    private Transform a;
    private Transform b;
    private LineRenderer line;

    public void SetObjects(Transform a, Transform b)
    {
        this.a = a;
        this.b = b;
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (a != null && b != null)
        {
            line.SetPosition(0, a.position);
            line.SetPosition(1, b.position);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
