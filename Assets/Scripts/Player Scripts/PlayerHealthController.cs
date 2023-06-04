using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerHealthController : MonoBehaviour {
    
    public LayerMask enemyLayers;
    public float enemyTouchKnockback = 30f;

    private PlayerAttributes playerAttributes;
    private PlayerInput playerInput;
    private Image[] hearts;
    private Color startingColor;
    // For detecting if player is overlapping with an enemy
    private ContactFilter2D overlapEnemiesFilter;

    // Use to calculate when player can take damage again (after invincibility frames)
    private float nextTimeCanTakeDamage = 0f; 

    void Start() {
        playerAttributes = GetComponent<PlayerAttributes>();
        playerInput = GetComponent<PlayerInput>();

        // Get images making up health bar
        GameObject healthBar = GameObject.FindGameObjectWithTag("HealthBarRoot");
        hearts = healthBar.GetComponentsInChildren<Image>();
        startingColor = hearts[0].color;
        UpdateHealthBar();

        overlapEnemiesFilter = new ContactFilter2D();
        overlapEnemiesFilter.SetLayerMask(enemyLayers);
    }

    void FixedUpdate() {
        // If player overlaps with enemies, take damage
        Collider2D[] overlapEnemies = new Collider2D[1];    // only using one enemy to calculate knockback TODO: avg enemies?
        if ((playerAttributes.humanCollider.enabled && playerAttributes.humanCollider.OverlapCollider(overlapEnemiesFilter, overlapEnemies) > 0) ||
            (playerAttributes.frogCollider.enabled && playerAttributes.frogCollider.OverlapCollider(overlapEnemiesFilter, overlapEnemies) > 0)) {
            if (Time.time >= nextTimeCanTakeDamage) {
                nextTimeCanTakeDamage = Time.time + playerAttributes.invincibilityTime;
                Damage(1);
            }
            playerAttributes.rb.velocity = ((playerAttributes.rb.transform.position - overlapEnemies[0].transform.position).normalized * enemyTouchKnockback);
        }
    }

    // Update grayed out hearts UI depending on player health value
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

    // Flash color twice and disable player input temporarily
    private IEnumerator StunPlayer(Color color){
        playerInput.DeactivateInput();
        float delay = (playerAttributes.invincibilityTime-0.1f)/3f; // -0.1f to give player a grace window on invincibility
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(delay);
        playerInput.ActivateInput();
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(delay);
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(delay);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Always use this to damage player!
    public void Damage(int amt) {
        if (playerAttributes.playerHealth <= 0) {
            Die();
        } else {
            playerAttributes.playerHealth -= amt;
            UpdateHealthBar();
            StartCoroutine(StunPlayer(Color.red));
        }
    }

    private void Die() {
        playerAttributes.isDead = true;
    }
}
