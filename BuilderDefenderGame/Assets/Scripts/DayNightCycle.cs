using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour {

    [SerializeField] private Gradient gradient;
    [SerializeField] private Light light2D;
    [SerializeField] private float secondsPerDay = 10f;

    private float dayTime;
    private float dayTimeSpeed;

    private void Awake() {
        dayTimeSpeed = 1 / secondsPerDay;
    }

    private void Update() {
        dayTime += Time.deltaTime * dayTimeSpeed;
        light2D.color = gradient.Evaluate(dayTime % 1f);
    }

}
