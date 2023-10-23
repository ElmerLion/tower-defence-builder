using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceNearbyOverlay : MonoBehaviour {

    private ResourceGeneratorData resourceGeneratorData;

    private void Awake() {
        Hide();
    }

    private void Update() {
        

        int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, transform.position - transform.localPosition);
        float percent = Mathf.RoundToInt((float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount * 100f);
        transform.GetChild(1).GetComponent<TextMeshPro>().SetText(percent + "%");
    }
    public void Show(ResourceGeneratorData resourceGeneratorData) {
        this.resourceGeneratorData = resourceGeneratorData;
        gameObject.SetActive(true);

        if (transform.Find("Icon") != null ) {
            transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceTypeSO.sprite;
        }
        
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

}
