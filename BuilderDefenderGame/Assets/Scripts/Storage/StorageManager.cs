using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour {

    public static StorageManager Instance { get; private set; }

    public event EventHandler OnStorageChanged;

    private Dictionary<ResourceTypeSO, float> currentResourceStorage;
    private ResourceTypeListSO resourceTypeListSO;

    private void Awake() {
        Instance = this;

        resourceTypeListSO = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        currentResourceStorage = new Dictionary<ResourceTypeSO, float>();

        float startingStorageAmount = 100;
        foreach (ResourceTypeSO resourceTypeSO in resourceTypeListSO.list) {
            if (!currentResourceStorage.ContainsKey(resourceTypeSO)) {
                currentResourceStorage.Add(resourceTypeSO, startingStorageAmount);
                
            }
        }
    }

    public void AddStorage(ResourceTypeSO resourceTypeSO, float amount) {
        currentResourceStorage[resourceTypeSO] += amount;

        OnStorageChanged?.Invoke(this, EventArgs.Empty);
    }

    public float GetMaxCurrentStorage(ResourceTypeSO resourceTypeSO) {
        return currentResourceStorage[resourceTypeSO];
    }

}
