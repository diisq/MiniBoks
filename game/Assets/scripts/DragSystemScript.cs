using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DragThrow2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isDragging = false;
    private bool isFrozen = false;

    private Vector2 lastMousePos;
    private Vector2 velocity;

    [Header("Throw Settings")]
    public float throwForce = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        isDragging = true;
        rb.isKinematic = true; // stop physics while dragging
        lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseDrag()
    {
        if (isFrozen) return;

        Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = currentMousePos;

        // While time is stopped, do NOT compute throw velocity (keeps object frozen)
        if (TimeStop.IsStopped)
        {
            velocity = Vector2.zero;
        }
        else
        {
            float dt = Time.deltaTime;
            if (dt > 0f)
                velocity = (currentMousePos - lastMousePos) / dt;
            else
                velocity = Vector2.zero; // safety if dt == 0
        }

        lastMousePos = currentMousePos;
    }

    void OnMouseUp()
    {
        isDragging = false;
        rb.isKinematic = false;

        // If time is stopped or the object is frozen, don't throw
        if (!isFrozen && !TimeStop.IsStopped)
        {
            rb.linearVelocity = velocity * throwForce * 0.01f;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Update()
    {
        if (!isDragging) return;

        // Rotate object while dragging (works even when time is stopped)
        if (Input.GetKey(KeyCode.Q)) transform.Rotate(0, 0, 2f);
        if (Input.GetKey(KeyCode.E)) transform.Rotate(0, 0, -2f);

        // Freeze/unfreeze while holding
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFrozen = !isFrozen;
            rb.isKinematic = isFrozen || isDragging; // keep kinematic if frozen or dragging
        }
    }
}
