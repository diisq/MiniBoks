using UnityEngine;

public class Balance : MonoBehaviour
{
    public float targetRotation;
    private Rigidbody2D rb;
    public float force;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation, force * Time.fixedDeltaTime));
    }
}
