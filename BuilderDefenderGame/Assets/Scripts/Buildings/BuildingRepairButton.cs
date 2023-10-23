using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairButton : MonoBehaviour {

    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ResourceTypeSO goldResourceTypeSO;

    private void Awake() {
        transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => {
            int missingHealth = healthSystem.GetMaxHealthAmount() - healthSystem.GetCurrentHealthAmount();
            int repairCost = missingHealth / 3;

            ResourceAmount[] resourceAmountCost = new ResourceAmount[] {
                new ResourceAmount {resourceTypeSO = goldResourceTypeSO, amount = repairCost } };

            if (ResourceManager.Instance.CanAfford(new ResourceAmount[] {new ResourceAmount { resourceTypeSO = goldResourceTypeSO, amount = repairCost} })) {
                // Can affod to repair
                ResourceManager.Instance.SpendResources(resourceAmountCost);
                healthSystem.HealFull();
            } else {
                // Can't afford
                TooltipUI.Instance.Show("Cannot afford repair cost! G" + repairCost, new TooltipUI.TooltipTimer { timer = 2f });
            }

            
        });
    }

}
