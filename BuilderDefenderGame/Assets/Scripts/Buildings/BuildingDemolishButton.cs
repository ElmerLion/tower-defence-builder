using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDemolishButton : MonoBehaviour {

    [SerializeField] private Building building;

    private void Awake() {
        transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => {
            BuildingTypeSO buildingTypeSO = building.GetComponent<BuildingTypeHolder>().buildingTypeSO;
            foreach (ResourceAmount resourceAmount in buildingTypeSO.constructionResourceCostArray) {
                ResourceManager.Instance.AddResource(resourceAmount.resourceTypeSO, Mathf.FloorToInt(resourceAmount.amount * .6f));
            }
        Destroy(building.gameObject);
        });
    }

}
