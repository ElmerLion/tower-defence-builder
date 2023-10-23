using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour {
    private EnemyListSO enemyListSO;

    public static Enemy Create(Vector3 position, int enemyLevel) {
        Transform enemyTransform = Instantiate(GameAssets.Instance.enemy, position, Quaternion.identity);
        
        EnemyListSO enemyListSO = Resources.Load<EnemyListSO>(typeof(EnemyListSO).Name);
        EnemySO enemySO = enemyListSO.list[enemyLevel];
        enemyTransform.GetComponent<EnemyLevelHolder>().enemySO = enemySO;

        Transform enemyVisual = enemyTransform.GetChild(1);
        enemyVisual.GetComponent<SpriteRenderer>().color = enemySO.color;

        Transform enemyTrail = enemyTransform.GetChild(2);
        enemyTrail.GetComponent<TrailRenderer>().colorGradient = enemySO.trailGradient;

        

        

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }
    private HealthSystem healthSystem;

    private Transform targetTransform;
    private Object enemyDieParticles;
    private Rigidbody2D rigidbody2D;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = .2f;

    private EnemySO enemySO;



    private void Start() {
        enemyDieParticles = GameAssets.Instance.enemyDieParticles;

        healthSystem = GetComponent<HealthSystem>();
        enemySO = GetComponent<EnemyLevelHolder>().enemySO;
        healthSystem.SetHealthAmountMax(enemySO.maxHealth, true);


        rigidbody2D = GetComponent<Rigidbody2D>();
        if (BuildingManager.Instance.GetHQBuilding() != null ) {
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }
        
        
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
        CinemachineShake.Instance.ShakeCamera(3f, .1f);
        ChromaticAberrationEffect.Instance.SetWeight(.5f);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e) {
        Instantiate(enemyDieParticles, transform.position, Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(5f, .15f);
        ChromaticAberrationEffect.Instance.SetWeight(.5f);
        Destroy(gameObject);
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
    }

    private void Update() {

        HandleMovement();

        HandleTargeting();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Building building = collision.gameObject.GetComponent<Building>();

        if (building != null) {
            // There is a building
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(enemySO.damage);

            this.healthSystem.Damage(999);
        }
    }

    private void HandleMovement() {
        if (targetTransform != null) {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;

            float moveSpeed = enemySO.moveSpeed;
            rigidbody2D.velocity = moveDir * moveSpeed;
        } else {
            rigidbody2D.velocity = Vector2.zero;
        }
    }

    private void HandleTargeting() {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f) {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void LookForTargets() {
        float targetMaxRadius = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray) {
            Building building = collider2D.GetComponent<Building>();
            if (building != null) {
                // Its a building
                if (targetTransform == null) {
                    targetTransform = building.transform;
                } else {
                    if (Vector3.Distance(transform.position, building.transform.position) < Vector3.Distance(transform.position, targetTransform.position)){
                        // Closer building found
                        targetTransform = building.transform;
                    } 
                }
            }
        }

        if (targetTransform == null) {
            // Found no targets within range
            if (BuildingManager.Instance.GetHQBuilding() != null) {
                targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
            }
            
        }
    }

}
