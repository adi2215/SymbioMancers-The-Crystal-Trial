using UnityEngine;

public class BubbleShield : MonoBehaviour
{
    [Header("Swim Mode Settings")]
    [SerializeField] private float swimSpeed = 3f;
    [SerializeField] private float swimGravityScale = 0.7f;
    
    private PlayerMovement playerMovement;
    private bool isActive = false;

    private void Start()
    {
        // Find the player and get their PlayerMovement component
        playerMovement = FindObjectOfType<PlayerMovement>();
        
        if (playerMovement != null)
        {
            // Enable swim mode
            playerMovement.EnableSwimMode(swimSpeed, swimGravityScale);
            isActive = true;
            Debug.Log("Bubble Shield activated - Swim mode enabled!");
        }
        else
        {
            Debug.LogError("BubbleShield: PlayerMovement component not found!");
        }
    }

    private void OnDestroy()
    {
        if (isActive && playerMovement != null)
        {
            // Disable swim mode when bubble shield is destroyed
            playerMovement.DisableSwimMode();
            Debug.Log("Bubble Shield destroyed - Swim mode disabled!");
        }
    }

    // Called when player exits water zone
    public void OnPlayerExitWater()
    {
        if (isActive)
        {
            // Destroy the bubble shield when player exits water
            Destroy(gameObject);
        }
    }
} 