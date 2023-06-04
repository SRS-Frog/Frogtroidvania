using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour {
    
    private PlayerAttributes playerAttributes;
    private Image[] hearts;
    private Color startingColor;
    public LayerMask enemyLayers;
    private Collider2D playerHitBox;

    // Start is called before the first frame update
    void Start() {
        playerAttributes = GetComponent<PlayerAttributes>();
        playerHitBox = GetComponent<Collider2D>();
        GameObject healthBar = GameObject.FindGameObjectWithTag("HealthBarRoot");
        hearts = healthBar.GetComponentsInChildren<Image>();
        startingColor = hearts[0].color;
        UpdateHealthBar();
    }

    void FixedUpdate() {
        
        // Collider2D[] hitEnemies = Physics2D.OverlapCircle(transform.position, 10, enemyLayers);
    }

    public void UpdateHealthBar() {
        int currNumHearts = playerAttributes.playerHealth;
        
        for (int heartIndex = 0; heartIndex < hearts.Length; ++heartIndex) {
            if (heartIndex < currNumHearts) {
                hearts[heartIndex].color = startingColor;
            } else {
                hearts[heartIndex].color = Color.grey;
            }
        }
    }

    public void Damage(int amt) {
        playerAttributes.playerHealth -= amt;
        UpdateHealthBar();
        if (playerAttributes.playerHealth <= 0) {
            Die();
        }
    }

    private void Die() {

    }
}
