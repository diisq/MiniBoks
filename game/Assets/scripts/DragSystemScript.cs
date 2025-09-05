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
    public float dragForce = 10f;

    // Store previous positions for better velocity calculation
    private Vector2[] mousePositions = new Vector2[5];
    private int positionIndex = 0;
    private float velocityUpdateTimer = 0f;
    private const float velocityUpdateInterval = 0.02f; // 50 FPS for velocity calculation

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        isDragging = true;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastMousePos = mouseWorldPos;

        // Initialize position array
        for (int i = 0; i < mousePositions.Length; i++)
        {
            mousePositions[i] = mouseWorldPos;
        }
        positionIndex = 0;
        velocity = Vector2.zero;
    }

    void OnMouseDrag()
    {
        // Empty - handling in Update/FixedUpdate
    }

    void OnMouseUp()
    {
        isDragging = false;

        // If time is stopped or the object is frozen, don't throw
        if (!isFrozen && !TimeStop.IsStopped)
        {
            // Use the calculated velocity for throwing
            rb.linearVelocity = velocity * throwForce;
            Debug.Log("ATTEMPTED THROW... velocity: " + velocity.ToString() + " final velocity: " + (velocity * throwForce).ToString());
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Update()
    {
        rb.gravityScale = (isFrozen) ? 0f : 1f;

        if (isFrozen)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0;
        }

        if (!isDragging) return;

        // Rotate object while dragging (works even when time is stopped)
        if (Input.GetKey(KeyCode.Q)) transform.Rotate(0, 0, 2f);
        if (Input.GetKey(KeyCode.E)) transform.Rotate(0, 0, -2f);

        // Freeze/unfreeze while holding
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFrozen = !isFrozen;
        }

        if (isFrozen) return;

        Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Update velocity calculation timer
        velocityUpdateTimer += Time.deltaTime;

        // Calculate velocity for throwing (not while time is stopped)
        if (!TimeStop.IsStopped && velocityUpdateTimer >= velocityUpdateInterval)
        {
            // Store current position
            mousePositions[positionIndex] = currentMousePos;

            // Calculate velocity using positions from a few frames ago for stability
            int oldIndex = (positionIndex + 1) % mousePositions.Length;
            Vector2 oldPos = mousePositions[oldIndex];
            float deltaTime = velocityUpdateInterval * mousePositions.Length;

            if (deltaTime > 0f)
            {
                velocity = (currentMousePos - oldPos) / deltaTime;
            }

            positionIndex = (positionIndex + 1) % mousePositions.Length;
            velocityUpdateTimer = 0f;

            Debug.Log("Velocity updated: " + velocity.ToString());
        }
        else if (TimeStop.IsStopped)
        {
            velocity = Vector2.zero;
        }

        
    }

    private void FixedUpdate()
    {
        if (isDragging && !isFrozen && !TimeStop.IsStopped)
        {
            Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 targetDir = currentMousePos - (Vector2)transform.position;

            // Apply drag force to follow mouse
            rb.linearVelocity = targetDir * dragForce;
        }
    }
}