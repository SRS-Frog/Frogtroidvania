using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerHealthController : MonoBehaviour {
    
    public LayerMask enemyLayers;
    public float enemyTouchKnockback = 30f;

    private PlayerAttributes playerAttributes;
    private PlayerHealthUI playerHealthUI;
    private PlayerInput playerInput;
    // For detecting if player is overlapping with an enemy
    private ContactFilter2D overlapEnemiesFilter;

    // Use to calculate when player can take damage again (after invincibility frames)
    private float nextTimeCanTakeDamage = 0f; 
    public Vector2 currentRespawnPoint;

    void Start() {
        playerAttributes = GetComponent<PlayerAttributes>();
        playerInput = GetComponent<PlayerInput>();

        overlapEnemiesFilter = new ContactFilter2D();
        overlapEnemiesFilter.SetLayerMask(enemyLayers);
        overlapEnemiesFilter.useTriggers = true;

        playerHealthUI = GameObject.FindObjectOfType<PlayerHealthUI>();
        playerHealthUI.UpdateHealthBar(playerAttributes.playerHealth, playerAttributes.playerMaxHealth);

        currentRespawnPoint = this.transform.position;
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
        if (playerAttributes.playerHealth - amt <= 0) {
            playerAttributes.playerHealth = 0;
            Die();
        } else {
            playerAttributes.playerHealth -= amt;
            playerHealthUI.UpdateHealthBar(playerAttributes.playerHealth, playerAttributes.playerMaxHealth);
            StartCoroutine(StunPlayer(Color.red));
        }
    }
    
    public void Heal(int amt) {
        if (playerAttributes.playerHealth >= playerAttributes.playerMaxHealth) {
            playerAttributes.playerHealth += amt;
            playerHealthUI.UpdateHealthBar(playerAttributes.playerHealth, playerAttributes.playerMaxHealth);
        }
        else
        {
            playerAttributes.playerHealth += amt;
            playerHealthUI.UpdateHealthBar(playerAttributes.playerHealth, playerAttributes.playerMaxHealth);
        }
    }

    public void IncreaseMaxHealth()
    {
        playerAttributes.playerMaxHealth += 1;
        playerAttributes.playerHealth = playerAttributes.playerMaxHealth;
        
        playerHealthUI.UpdateHealthBar(playerAttributes.playerHealth, playerAttributes.playerMaxHealth);
    }

    private void Die() {
        // playerAttributes.isDead = true;
        this.transform.position = currentRespawnPoint;
        Heal(playerAttributes.playerMaxHealth);
        playerAttributes.rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Checkpoint") {
            currentRespawnPoint = other.gameObject.transform.position;
        }
    }
}
