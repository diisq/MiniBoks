using UnityEngine;

public class ConveyerBeltScript : MonoBehaviour
{
    public float speed = 2f;                 // conveyor speed
    public Vector2 direction = Vector2.right; // movement direction
    public bool isActive = true;             // starts ON

    private void Update()
    {
        // Right Click anywhere toggles conveyor ON/OFF
        if (Input.GetMouseButtonDown(1))
        {
            isActive = !isActive;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isActive) return;

        Rigidbody2D rb = collision.rigidbody;
        if (rb != null)
        {
            // Move object along conveyor
            rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
        }
    }
}
