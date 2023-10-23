using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour {

    private static GameAssets instance;
    public static GameAssets Instance {
        get {
            if (instance == null) {
                instance = Resources.Load<GameAssets>("GameAssets");
            }
            return instance;
        }
    }
      

    public Transform enemy;
    public Transform enemyDieParticles;
    public Transform buildingDestroyedParticles;
    public Transform buildingPlacedParticles;
    public Transform buildingConstruction;
    public Transform arrowProjectile;

    
}
