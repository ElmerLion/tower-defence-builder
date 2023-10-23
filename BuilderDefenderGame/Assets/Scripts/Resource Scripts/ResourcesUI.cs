using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesUI : MonoBehaviour {

    [SerializeField] private Transform resourceTemplate;
    [SerializeField] private Transform container;

    private ResourceTypeListSO resourceTypeList;
    private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformDictionary;

    private void Awake() {
        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        resourceTypeTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();

        resourceTemplate.gameObject.SetActive(false);

        foreach (ResourceTypeSO resourceTypeSO in resourceTypeList.list) {
            Transform resourceTransform = Instantiate(resourceTemplate, container);
            resourceTransform.gameObject.SetActive(true);

            resourceTransform.GetChild(0).GetComponent<Image>().sprite = resourceTypeSO.sprite;

            resourceTypeTransformDictionary[resourceTypeSO] = resourceTransform;
        }
    }

    private void Start() {
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
        StorageManager.Instance.OnStorageChanged += StorageManager_OnStorageChanged;

        UpdateResourceAmount();
    }

    private void StorageManager_OnStorageChanged(object sender, System.EventArgs e) {
        UpdateResourceAmount();
    }

    private void ResourceManager_OnResourceAmountChanged(object sender, System.EventArgs e) {
        UpdateResourceAmount();
    }

    private void UpdateResourceAmount() {
        foreach (ResourceTypeSO resourceTypeSO in resourceTypeList.list) {
            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceTypeSO);
            float maxStorage = StorageManager.Instance.GetMaxCurrentStorage(resourceTypeSO);

            Transform resourceTransform = resourceTypeTransformDictionary[resourceTypeSO];
            resourceTransform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString() + "/" + maxStorage.ToString());
        }
    }

}
