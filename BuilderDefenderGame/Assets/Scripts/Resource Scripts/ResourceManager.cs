using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    public static ResourceManager Instance { get; private set; }

    public event EventHandler OnResourceAmountChanged;

    [SerializeField] private List<ResourceAmount> startingResourceAmountList;

    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;

    private void Awake() {
        Instance = this;

        resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        foreach (ResourceTypeSO resourceTypeSO in resourceTypeList.list) {
            resourceAmountDictionary[resourceTypeSO] = 0;
        }

        foreach (ResourceAmount resourceAmount in startingResourceAmountList) {
            AddResource(resourceAmount.resourceTypeSO, resourceAmount.amount);
        }

        
    }

    

    public void AddResource(ResourceTypeSO resourceTypeSO, int amount) {
        if (resourceAmountDictionary[resourceTypeSO] < StorageManager.Instance.GetMaxCurrentStorage(resourceTypeSO)) {
            resourceAmountDictionary[resourceTypeSO] += amount;

            OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
        }
        

        
    }

    public int GetResourceAmount(ResourceTypeSO resourceTypeSO) {
        return resourceAmountDictionary[resourceTypeSO];
    }

    public bool CanAfford(ResourceAmount[] resourceAmountArray) {
        foreach (ResourceAmount resourceAmount in resourceAmountArray) {
            if (GetResourceAmount(resourceAmount.resourceTypeSO) < resourceAmount.amount) {
                // Can afford
                return false;
            }
        }

        return true;
    }

    public void SpendResources(ResourceAmount[] resourceAmountArray) {
        foreach (ResourceAmount resourceAmount in resourceAmountArray) {
            resourceAmountDictionary[resourceAmount.resourceTypeSO] -= resourceAmount.amount;
        }
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

}
