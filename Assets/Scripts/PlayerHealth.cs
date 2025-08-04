using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public void Die()
    {
        Debug.Log("Player died.");
        // Replace this with your death logic:
        // - Reload scene
        // - Show game over screen
        // - Play animation/sound, etc.
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}
