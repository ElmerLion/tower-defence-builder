using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    private BuildingTypeSO buildingTypeSO;
    private HealthSystem healthSystem;
    private Transform buildingDemolishButton;
    private StorageData[] storageData;
    private Transform buildingRepairButton;

    private void Awake() {
        buildingDemolishButton = transform.Find("BuildingDemolishButton");
        buildingRepairButton = transform.Find("BuildingRepairButton");

        HideBuildingDemolishButton();
        HideBuildingRepairButton();
        
    }

    private void Start() {
        buildingTypeSO = GetComponent<BuildingTypeHolder>().buildingTypeSO;
        healthSystem = GetComponent<HealthSystem>();
        storageData = buildingTypeSO.storageData;

        healthSystem.SetHealthAmountMax(buildingTypeSO.maxHealthAmount, true);

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;

        healthSystem.OnDied += HealthSystem_OnDied;
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e) {
        if (healthSystem.IsFullHealth()) {
            HideBuildingRepairButton();
        }
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
        ShowBuildingRepairButton();
        CinemachineShake.Instance.ShakeCamera(5f, .1f);
        ChromaticAberrationEffect.Instance.SetWeight(1f);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e) {
        Instantiate(GameAssets.Instance.buildingDestroyedParticles, transform.position, Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(10f, .2f);
        ChromaticAberrationEffect.Instance.SetWeight(1f);
        Destroy(gameObject);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);

        if (buildingTypeSO.storageData.Length > 0) {
            foreach (StorageData storageData in storageData) {
                StorageManager.Instance.AddStorage(storageData.storageType, -storageData.storage);
            }
            
        }
    }

    private void OnMouseEnter() {
        ShowBuildingDemolishButton();
    }

    private void OnMouseExit() {
        HideBuildingDemolishButton();
    }

    private void ShowBuildingDemolishButton() {
        if (buildingDemolishButton != null) {
            buildingDemolishButton.gameObject.SetActive(true);
        }
    }

    private void HideBuildingDemolishButton() {
        if (buildingDemolishButton != null) {
            buildingDemolishButton.gameObject.SetActive(false);
        }
    }

    private void ShowBuildingRepairButton() {
        if (buildingRepairButton != null) {
            buildingRepairButton.gameObject.SetActive(true);
        }
    }

    private void HideBuildingRepairButton() {
        if (buildingRepairButton != null) {
            buildingRepairButton.gameObject.SetActive(false);
        }
    }
}
