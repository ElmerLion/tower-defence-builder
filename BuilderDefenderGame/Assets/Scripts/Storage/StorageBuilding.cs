using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBuilding : MonoBehaviour {
    private ResourceTypeListSO resourceTypeListSO;
    private StorageData[] storageData;

    private void Awake() {
        resourceTypeListSO = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        storageData = GetComponent<BuildingTypeHolder>().buildingTypeSO.storageData;
    }

    private void Start() {
        foreach (StorageData storageData in storageData) {
            StorageManager.Instance.AddStorage(storageData.storageType, storageData.storage);
        }
        
    }
}
