using System.Collections.Generic;
using UnityEngine;

public class ElementalInventory : MonoBehaviour
{
    public GameObject fireFollowerPrefab;
    public GameObject earthFollowerPrefab;
    public GameObject burningSwordPrefab;

    private Dictionary<ElementalType, GameObject> activeFollowers = new();
    private ElementalType? firstSelection = null;
    private GameObject currentAbility;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectElemental(ElementalType.Fire);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectElemental(ElementalType.Earth);
        if (Input.GetKeyDown(KeyCode.Q)) DischargeAbility();
    }

    public bool TryAddElemental(ElementalType type)
    {
        if (activeFollowers.ContainsKey(type)) return false;

        GameObject prefab = GetPrefab(type);
        if (prefab == null) return false;

        GameObject follower = Instantiate(prefab, transform.position, Quaternion.identity);
        follower.transform.SetParent(transform);
        follower.GetComponent<ElementalFollower>().SetOffset(GetOffset(type));
        activeFollowers.Add(type, follower);
        return true;
    }

    void SelectElemental(ElementalType type)
    {
        if (!activeFollowers.ContainsKey(type)) return;

        if (firstSelection == null)
        {
            firstSelection = type;
            // You could add UI feedback here.
        }
        else if (firstSelection != type)
        {
            CombineElementals(firstSelection.Value, type);
            firstSelection = null;
        }
    }

    void CombineElementals(ElementalType a, ElementalType b)
    {
        bool ok = false;
        if ((a == ElementalType.Fire && b == ElementalType.Earth) || (a == ElementalType.Earth && b == ElementalType.Fire))
        {
            currentAbility = Instantiate(burningSwordPrefab, transform.position, Quaternion.identity);
            currentAbility.transform.SetParent(transform);
            ok = true;
        }
        else
        {
            Debug.Log($"No ability for combination: {a} + {b}");
            ok = false;
        }

        if (ok)
        {
            Destroy(activeFollowers[a]);
            Destroy(activeFollowers[b]);
            activeFollowers.Remove(a);
            activeFollowers.Remove(b);
        }
    }

    void DischargeAbility()
    {
        if (currentAbility != null)
        {
            Destroy(currentAbility);
            currentAbility = null;

            // Restore elementals
            TryAddElemental(ElementalType.Fire);
            TryAddElemental(ElementalType.Earth);
        }
    }

    GameObject GetPrefab(ElementalType type)
    {
        return type switch
        {
            ElementalType.Fire => fireFollowerPrefab,
            ElementalType.Earth => earthFollowerPrefab,
            _ => null
        };
    }

    Vector3 GetOffset(ElementalType type)
    {
        return type switch
        {
            ElementalType.Fire => new Vector3(1, 1, 0),
            ElementalType.Earth => new Vector3(-1, 1, 0),
            _ => Vector3.zero
        };
    }
}
