using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BuildingTypeSelectUI : MonoBehaviour {

    [SerializeField] private Transform buttonTemplate;
    [SerializeField] private Transform container;

    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private List<BuildingTypeSO> ignoreBuildingTypeList;

    private Dictionary<BuildingTypeSO, Transform> buttonTransformDictionary;

    private Transform arrowButton;

    private void Awake() {
        buttonTemplate.gameObject.SetActive(false);

        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        buttonTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();

        
       arrowButton = Instantiate(buttonTemplate, container);
       arrowButton.gameObject.SetActive(true);

       arrowButton.GetChild(1).GetComponent<Image>().sprite = arrowSprite;
        arrowButton.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(0, -30);

       arrowButton.GetComponent<Button>().onClick.AddListener(() => {
           BuildingManager.Instance.SetActiveBuildingType(null);
       });

        MouseEnterExitEvent mouseEnterExitEventsArrow = arrowButton.GetComponent<MouseEnterExitEvent>();
        mouseEnterExitEventsArrow.OnMouseEnter += (object sender, EventArgs e) => {
            TooltipUI.Instance.Show("Arrow");
        };

        mouseEnterExitEventsArrow.OnMouseExit += (object sender, EventArgs e) => {
            TooltipUI.Instance.Hide();
        };

        foreach (BuildingTypeSO buildingTypeSO in buildingTypeList.list) {
            if (ignoreBuildingTypeList.Contains(buildingTypeSO)) continue;

            
            
            Transform buttonTransform = Instantiate(buttonTemplate, container);
            buttonTransform.gameObject.SetActive(true);

            buttonTransform.GetChild(1).GetComponent<Image>().sprite = buildingTypeSO.sprite;

            buttonTransform.GetComponent<Button>().onClick.AddListener(() => {
                BuildingManager.Instance.SetActiveBuildingType(buildingTypeSO);
            });

            MouseEnterExitEvent mouseEnterExitEventsButtons = buttonTransform.GetComponent<MouseEnterExitEvent>();
            mouseEnterExitEventsButtons.OnMouseEnter += (object sender, EventArgs e) => {
                TooltipUI.Instance.Show(buildingTypeSO.nameString + "\n" + buildingTypeSO.GetConstructionResourceCostString());
            };

            mouseEnterExitEventsButtons.OnMouseExit += (object sender, EventArgs e) => {
                TooltipUI.Instance.Hide();
            };

            buttonTransformDictionary[buildingTypeSO] = buttonTransform;
        }
    }

    

    private void Start() {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
        UpdateActiveBuildingTypeButton();
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e) {
        UpdateActiveBuildingTypeButton();
    }

    

    private void UpdateActiveBuildingTypeButton() {
        arrowButton.GetChild(2).gameObject.SetActive(false);
        foreach (BuildingTypeSO buildingTypeSO in buttonTransformDictionary.Keys) {
            Transform buttonTransform = buttonTransformDictionary[buildingTypeSO];
            buttonTransform.GetChild(2).gameObject.SetActive(false);

        }

        BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
        if (activeBuildingType == null) {
            arrowButton.GetChild(2).gameObject.SetActive(true);
        } else {
            buttonTransformDictionary[activeBuildingType].GetChild(2).gameObject.SetActive(true);
        }

        
    }

}
