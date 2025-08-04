using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public float swingAngle = 70f;
    public float swingDuration = 0.2f;

    private float timeElapsed;
    private bool swinging = false;
    private Quaternion startRot;
    private Quaternion endRot;

    void Update()
    {
        if (!swinging && Input.GetMouseButtonDown(0)) // Left mouse click
        {
            SwingTowardMouse();
        }

        if (swinging)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / swingDuration;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            if (t >= 1f)
            {
                swinging = false;
                Destroy(gameObject); // remove sword after swing
            }
        }
    }

    void SwingTowardMouse()
    {
        Debug.Log("SwordSwing: SwingTowardMouse");
        swinging = true;
        timeElapsed = 0f;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 worldMouse = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 dir = worldMouse - transform.position;
        dir.z = 0; // keep in 2D

        Debug.Log("SwordSwing: SwingTowardMouse: dir: " + worldMouse.ToString() + " transform.position: " + transform.position.ToString());
        dir.z = 0;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        startRot = Quaternion.Euler(0, 0, angle - swingAngle / 2f);
        endRot = Quaternion.Euler(0, 0, angle + swingAngle / 2f);
        Debug.Log("SwordSwing: SwingTowardMouse: angle: " + angle.ToString());
        Debug.Log("SwordSwing: SwingTowardMouse: startRot: " + (angle - swingAngle / 2f).ToString() + " endRot: " + (angle + swingAngle / 2f).ToString());
        transform.rotation = startRot;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("SwordSwing: OnTriggerEnter2D");
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            Debug.Log("SwordSwing: OnTriggerEnter2D: Enemy or Obstacle detected");
            Destroy(other.gameObject);
        }
    }
}
