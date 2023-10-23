using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour {



    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingTypeSO) {
        Transform buildingConstructionPrefab = GameAssets.Instance.buildingConstruction;
        Transform buildingConstructionTransform = Instantiate(buildingConstructionPrefab, position, Quaternion.identity);

        BuildingConstruction buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingType(buildingTypeSO);
        return buildingConstruction;
    }

    private BuildingTypeSO buildingTypeSO;
    private float constructionTimer; 
    private float constructionTimerMax;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    private BuildingTypeHolder buildingTypeHolder;
    private Material constructionMaterial;
    private Transform buildingPlacedParticles;

    private void Awake() {
        buildingPlacedParticles = GameAssets.Instance.buildingPlacedParticles;

        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        constructionMaterial = spriteRenderer.material;

        Instantiate(buildingPlacedParticles, transform.position, Quaternion.identity);
    }

    private void Update() {
        constructionTimer -= Time.deltaTime;

        constructionMaterial.SetFloat("_Progress", GetConstructionTimerNormalized());
        if (constructionTimer <= 0f ) {
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
            Instantiate(buildingPlacedParticles, transform.position, Quaternion.identity);
            Instantiate(buildingTypeSO.prefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void SetBuildingType(BuildingTypeSO buildingTypeSO) {
        this.buildingTypeSO = buildingTypeSO;

        constructionTimerMax = buildingTypeSO.constructionTimerMax;
        constructionTimer = constructionTimerMax;

        spriteRenderer.sprite = buildingTypeSO.sprite;

        boxCollider2D.offset = buildingTypeSO.prefab.GetComponent<BoxCollider2D>().offset;
        boxCollider2D.size = buildingTypeSO.prefab.GetComponent<BoxCollider2D>().size;

        buildingTypeHolder.buildingTypeSO = buildingTypeSO;
    }

    public float GetConstructionTimerNormalized() {
        return 1 - constructionTimer / constructionTimerMax;
    }

}
