using UnityEngine;

public sealed class CompassNeedle : MonoBehaviour
{
    /// <summary>
    /// In seconds
    /// </summary>
    private float time = 0;
    private const float IntervalSeconds = 10;
    private const float IntervalRadians = (2 * Mathf.PI) / IntervalSeconds;
    private const float OscillationDegrees = 45;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.localEulerAngles = new Vector3(0, 0, GetRotatingAngleFromTime(time));
    }

    private static float GetOscillatingAngleFromTime(float time)
	{
        return OscillationDegrees * Mathf.Sin(time / IntervalRadians);
    }

    private static float GetRotatingAngleFromTime(float time)
    {
        return 360 / IntervalSeconds * time;
    }
}
