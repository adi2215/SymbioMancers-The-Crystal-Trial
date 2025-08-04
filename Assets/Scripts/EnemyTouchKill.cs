using UnityEngine;

public class EnemyTouchKill : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("EnemyTouchKill: OnCollisionEnter2D");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("EnemyTouchKill: Player detected");
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}
