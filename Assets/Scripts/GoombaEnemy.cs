using UnityEngine;

public class EnemySwing : MonoBehaviour
{
    public float moveDistance = 3f;
    public float moveSpeed = 2f;

    private Vector3 startPos;
    private bool movingRight = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = moveDistance * Mathf.Sin(Time.time * moveSpeed);
        transform.position = startPos + new Vector3(offset, 0f, 0f);
    }
}
