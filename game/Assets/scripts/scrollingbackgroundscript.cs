using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackgroundscript : MonoBehaviour
{
    public RawImage rawImage; // assign your RawImage here
    public Vector2 scrollSpeed = new Vector2(0.1f, 0.1f); // speed for x and y

    private Vector2 currentOffset = Vector2.zero;

    void Update()
    {
        if (rawImage == null) return;

        // Update the offset
        currentOffset += scrollSpeed * Time.deltaTime;

        // Keep the values between 0 and 1 for looping
        currentOffset.x = currentOffset.x % 1f;
        currentOffset.y = currentOffset.y % 1f;

        rawImage.uvRect = new Rect(currentOffset, rawImage.uvRect.size);
    }
}
