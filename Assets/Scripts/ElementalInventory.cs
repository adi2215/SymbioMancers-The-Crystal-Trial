using System.Collections.Generic;
using UnityEngine;

public class ElementalInventory : MonoBehaviour
{
    public GameObject fireFollowerPrefab;
    public GameObject earthFollowerPrefab;
    public GameObject airFollowerPrefab;
    public GameObject waterFollowerPrefab;
    public GameObject burningSwordPrefab;
    public GameObject bubbleShieldPrefab;
    
    private Dictionary<ElementalType, GameObject> activeFollowers = new();
    private ElementalType? firstSelection = null;
    private List<AbilityData> activeAbilities = new List<AbilityData>();

    [System.Serializable]
    private class AbilityData
    {
        public GameObject abilityObject;
        public ElementalType firstElemental;
        public ElementalType secondElemental;

        public AbilityData(GameObject ability, ElementalType first, ElementalType second)
        {
            abilityObject = ability;
            firstElemental = first;
            secondElemental = second;
        }
    }

    [System.Serializable]
    private class ElementalCombination
    {
        public ElementalType firstElemental;
        public ElementalType secondElemental;
        public GameObject abilityPrefab;
        public string combinationName;

        public ElementalCombination(ElementalType first, ElementalType second, GameObject prefab, string name)
        {
            firstElemental = first;
            secondElemental = second;
            abilityPrefab = prefab;
            combinationName = name;
        }
    }

    private List<ElementalCombination> possibleCombinations;

    void Awake()
    {
        InitializeCombinations();
    }

    void InitializeCombinations()
    {
        possibleCombinations = new List<ElementalCombination>
        {
            new ElementalCombination(ElementalType.Fire, ElementalType.Earth, burningSwordPrefab, "Burning Sword"),
            new ElementalCombination(ElementalType.Air, ElementalType.Water, bubbleShieldPrefab, "Bubble Shield")
            // Add new combinations here easily
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectElemental(ElementalType.Fire);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectElemental(ElementalType.Earth);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectElemental(ElementalType.Air);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectElemental(ElementalType.Water);
        
        // Q discharges all abilities
        if (Input.GetKeyDown(KeyCode.Q)) DischargeAllAbilities();
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
        ElementalCombination combination = FindCombination(a, b);
        
        if (combination != null)
        {
            GameObject ability = Instantiate(combination.abilityPrefab, transform.position, Quaternion.identity);
            ability.transform.SetParent(transform);
            
            AbilityData abilityData = new AbilityData(ability, combination.firstElemental, combination.secondElemental);
            activeAbilities.Add(abilityData);
            
            Destroy(activeFollowers[a]);
            Destroy(activeFollowers[b]);
            activeFollowers.Remove(a);
            activeFollowers.Remove(b);
            
            Debug.Log($"Created {combination.combinationName}!");
        }
        else
        {
            Debug.Log($"No ability for combination: {a} + {b}");
        }
    }

    ElementalCombination FindCombination(ElementalType a, ElementalType b)
    {
        foreach (var combination in possibleCombinations)
        {
            if ((combination.firstElemental == a && combination.secondElemental == b) ||
                (combination.firstElemental == b && combination.secondElemental == a))
            {
                return combination;
            }
        }
        return null;
    }

    void DischargeAllAbilities()
    {
        if (activeAbilities.Count == 0) return;

        // Restore all elementals from all abilities
        foreach (var abilityData in activeAbilities)
        {
            TryAddElemental(abilityData.firstElemental);
            TryAddElemental(abilityData.secondElemental);
            Destroy(abilityData.abilityObject);
        }
        
        activeAbilities.Clear();
        Debug.Log("All abilities discharged!");
    }

    GameObject GetPrefab(ElementalType type)
    {
        return type switch
        {
            ElementalType.Fire => fireFollowerPrefab,
            ElementalType.Earth => earthFollowerPrefab,
            ElementalType.Air => airFollowerPrefab,
            ElementalType.Water => waterFollowerPrefab,
            _ => null
        };
    }

    Vector3 GetOffset(ElementalType type)
    {
        return type switch
        {
            ElementalType.Fire => new Vector3(1, 1, 0),
            ElementalType.Earth => new Vector3(-1, 1, 0),
            ElementalType.Air => new Vector3(1, -1, 0),
            ElementalType.Water => new Vector3(-1, -1, 0),
            _ => Vector3.zero
        };
    }
}
