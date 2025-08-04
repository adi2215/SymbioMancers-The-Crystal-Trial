using System.Collections.Generic;
using UnityEngine;

public class ElementalManager : MonoBehaviour
{
    public GameObject fireFollowerPrefab;
    public GameObject earthFollowerPrefab;
    public Transform followerParent;

    private Dictionary<ElementalType, GameObject> ownedElementals = new();
    private List<ElementalType> selected = new();

    public GameObject swordPrefab;
    private GameObject currentPowerUp;

    void Update()
    {
        HandleSelectionInput();
        HandlePowerupRelease();
    }

    void HandleSelectionInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) TrySelect(ElementalType.Fire);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TrySelect(ElementalType.Earth);
        // add other keys for Water, Air if needed
    }

    void TrySelect(ElementalType type)
    {
        if (!ownedElementals.ContainsKey(type)) return;
        if (selected.Contains(type)) return;

        selected.Add(type);

        if (selected.Count == 2)
            CombineSelected();
    }

    void CombineSelected()
    {
        // Hide or destroy followers
        foreach (ElementalType type in selected)
        {
            if (ownedElementals[type] != null)
                ownedElementals[type].SetActive(false); // deactivate for revival
        }

        // Spawn powerup
        currentPowerUp = Instantiate(swordPrefab, transform.position + Vector3.right, Quaternion.identity);
    }

    void HandlePowerupRelease()
    {
        if (Input.GetKeyDown(KeyCode.Q) && currentPowerUp != null)
        {
            Destroy(currentPowerUp);
            currentPowerUp = null;

            // Restore elementals
            foreach (ElementalType type in selected)
            {
                if (ownedElementals.ContainsKey(type))
                    ownedElementals[type].SetActive(true);
            }

            selected.Clear();
        }
    }

    public void AddElemental(ElementalType type)
    {
        if (ownedElementals.ContainsKey(type)) return;

        GameObject prefab = GetFollowerPrefab(type);
        GameObject follower = Instantiate(prefab, transform.position, Quaternion.identity, followerParent);
        follower.GetComponent<ElementalFollower>().SetOffset(GetOffsetFor(type));
        ownedElementals[type] = follower;
    }

    GameObject GetFollowerPrefab(ElementalType type)
    {
        return type switch
        {
            ElementalType.Fire => fireFollowerPrefab,
            ElementalType.Earth => earthFollowerPrefab,
            _ => null,
        };
    }

    Vector3 GetOffsetFor(ElementalType type)
    {
        return type switch
        {
            ElementalType.Fire => new Vector3(-1, 1, 0),
            ElementalType.Earth => new Vector3(1, 1, 0),
            _ => Vector3.zero,
        };
    }
}
