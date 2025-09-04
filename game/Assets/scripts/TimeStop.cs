using UnityEngine;

public class TimeStop : MonoBehaviour
{
    public static bool IsStopped { get; private set; }
    public KeyCode toggleKey = KeyCode.T;

    float previousScale = 1f;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (IsStopped) ResumeTime();
            else StopTime();
        }
    }

    public void StopTime()
    {
        if (IsStopped) return;
        previousScale = Time.timeScale <= 0f ? 1f : Time.timeScale;
        Time.timeScale = 0f;
        IsStopped = true;
    }

    public void ResumeTime()
    {
        if (!IsStopped) return;
        Time.timeScale = previousScale <= 0f ? 1f : previousScale;
        IsStopped = false;
    }
}
