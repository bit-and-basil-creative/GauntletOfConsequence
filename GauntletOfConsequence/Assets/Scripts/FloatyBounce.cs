using UnityEngine;

public class FloatyBounce : MonoBehaviour
{
    public float bounceSpeed = 2f;
    public float bounceHeight = 0.1f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.localPosition = startPos + new Vector3(0, newY, 0);
    }
}
