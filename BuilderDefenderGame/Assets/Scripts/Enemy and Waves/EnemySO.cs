using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemySO")]
public class EnemySO : ScriptableObject {

    public int maxHealth;
    public int damage;
    public float moveSpeed;
    public Color color;
    public Gradient trailGradient;

}
