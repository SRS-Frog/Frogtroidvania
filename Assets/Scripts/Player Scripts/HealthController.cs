using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour {
    
    [SerializeField] private Health playerHealth;

    [SerializeField] private Image[] hearts;

    private Color startingColor;

    // Start is called before the first frame update
    void Start() {
        startingColor = hearts[0].color;
        UpdateHealth();
    }

    // Update is called once per frame
    void Update() {
        UpdateHealth();
    }

    public void UpdateHealth() {
        int currNumHearts = (playerHealth.health * hearts.Length)/Health.MAX_HEALTH;
        
        for (int heartIndex = 0; heartIndex < hearts.Length; ++heartIndex) {
            if (heartIndex < currNumHearts) {
                hearts[heartIndex].color = startingColor;
            } else {
                hearts[heartIndex].color = Color.grey;
            }
        }
    }
}
