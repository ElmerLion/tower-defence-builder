using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceGeneratorOverlay : MonoBehaviour {

    [SerializeField] private ResourceGenerator resourceGenerator;
    private Transform barTransform;

    private void Start() {
        ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData();

        barTransform = transform.GetChild(1);

        transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceTypeSO.sprite;

        transform.GetChild(2).GetComponent<TextMeshPro>().SetText(resourceGenerator.GetAmountGeneratedPerSecond().ToString("F1"));
    }

    private void Update() {
        barTransform.localScale = new Vector3(1-resourceGenerator.GetTimerNormalized(), 1, 1);
    }
}
    

