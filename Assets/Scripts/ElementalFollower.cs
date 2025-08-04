using UnityEngine;

public class ElementalFollower : MonoBehaviour
{
    private Transform target;
    private Vector3 baseOffset;

    private float wiggleAmplitude = 0.2f;
    private float wiggleFrequency = 3f;
    private Vector3 wiggle;

    void Start()
    {
        target = transform.parent;
    }

    public void SetOffset(Vector3 _offset)
    {
        baseOffset = _offset;
    }

    void Update()
    {
        if (target == null) return;

        float time = Time.time;
        wiggle = new Vector3(
            Mathf.Sin(time * wiggleFrequency + baseOffset.x) * wiggleAmplitude,
            Mathf.Cos(time * wiggleFrequency + baseOffset.y) * wiggleAmplitude,
            0f
        );

        Vector3 targetPos = target.position + baseOffset + wiggle;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);
    }
}
