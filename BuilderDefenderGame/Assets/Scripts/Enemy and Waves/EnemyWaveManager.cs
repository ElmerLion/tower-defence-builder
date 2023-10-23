using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour {

    public static EnemyWaveManager Instance { get; private set; }

    public event EventHandler OnWaveNumberChanged;

    private enum State {
        WaitingToSpawnNextWave,
        SpawningWave,
    }

    [SerializeField] private int firstWaveEnemies;
    [SerializeField] private int enemiesAddedPerWave;
    [SerializeField] private List<Transform> spawnPositionTransformList;
    [SerializeField] private Transform nextWaveSpawnPositionTransform;

    private State state;
    private int waveNumber;
    private Vector3 spawnPosition;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private int remainingEnemySpawnAmount;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        state = State.WaitingToSpawnNextWave;
        spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPositionTransform.position = spawnPosition;
        nextWaveSpawnTimer = 6f;
    }

    private void Update() {
        switch (state) {
            case State.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer < 0f) {
                    SpawnWave();
                }
                break;
            case State.SpawningWave:
                if (remainingEnemySpawnAmount > 0f) {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f) {
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0f, 0.2f);

                        Vector2 enemySpawnPoint = spawnPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(0f, 10f);

                        if (remainingEnemySpawnAmount > 60) {
                            Enemy.Create(enemySpawnPoint, 2);
                            remainingEnemySpawnAmount--;
                        }

                        Enemy.Create(spawnPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(0f, 10f), 0);
                        remainingEnemySpawnAmount--;

                        if (remainingEnemySpawnAmount > 20) {
                            Enemy.Create(enemySpawnPoint, 1);
                            remainingEnemySpawnAmount--;
                        }

                        

                        if (remainingEnemySpawnAmount <= 0f) {
                            state = State.WaitingToSpawnNextWave;
                            spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
                            nextWaveSpawnPositionTransform.position = spawnPosition;
                            nextWaveSpawnTimer = 15f;
                        }
                    }
                }
                break;
        }
        

        
        
        
    }

    private void SpawnWave() {

        
        remainingEnemySpawnAmount = firstWaveEnemies + enemiesAddedPerWave * waveNumber;
        state = State.SpawningWave;
        waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetWaveNumber() {
        return waveNumber;
    }

    public float GetNextWaveSpawnTimer() {
        return nextWaveSpawnTimer;
    }

    public Vector3 GetSpawnPosition() {
        return spawnPosition;
    }

}
