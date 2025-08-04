using UnityEngine;

public class BubbleShield : MonoBehaviour
{
    [Header("Swim Mode Settings")]
    [SerializeField] private float swimSpeed = 3f;
    [SerializeField] private float swimGravityScale = 0.7f;
    
    private PlayerMovement playerMovement;
    private bool isActive = false;
    private bool swimModeEnabled = false;

    private void Start()
    {
        // Find the player and get their PlayerMovement component
        playerMovement = FindObjectOfType<PlayerMovement>();
        
        if (playerMovement != null)
        {
            isActive = true;
            
            // Only enable swim mode if player is already in water
            playerMovement._activeBubbleShield = this;
            if (playerMovement.IsInWater())
            {
                EnableSwimMode();
            }
            else
            {
                Debug.Log("Bubble Shield created - Swim mode will activate when entering water!");
            }
        }
        else
        {
            Debug.LogError("BubbleShield: PlayerMovement component not found!");
        }
    }

    private void EnableSwimMode()
    {
        if (playerMovement != null && !swimModeEnabled)
        {
            playerMovement.EnableSwimMode(swimSpeed, swimGravityScale);
            swimModeEnabled = true;
            Debug.Log("Bubble Shield activated - Swim mode enabled!");
        }
    }

    private void DisableSwimMode()
    {
        if (playerMovement != null && swimModeEnabled)
        {
            playerMovement.DisableSwimMode();
            swimModeEnabled = false;
            Debug.Log("Bubble Shield deactivated - Swim mode disabled!");
        }
    }

    private void OnDestroy()
    {
        if (isActive && swimModeEnabled)
        {
            DisableSwimMode();
        }
    }

    // Called when player enters water zone
    public void OnPlayerEnterWater()
    {
        if (isActive && !swimModeEnabled)
        {
            EnableSwimMode();
        }
    }

    // Called when player exits water zone
    public void OnPlayerExitWater()
    {
        if (isActive)
        {
            DisableSwimMode();
            // Destroy the bubble shield when player exits water
            Destroy(gameObject);
        }
    }
} 