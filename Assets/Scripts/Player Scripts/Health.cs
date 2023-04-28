using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] private int maxHealth = MAX_HEALTH;
    [SerializeField] private int maxHearts = 10;
    [SerializeField] private HealthUIController healthUIController;

    public int health { get; private set; }
    public const int MAX_HEALTH = 100; 

    void Awake() {
        health = maxHealth;
        UpdateHeartsUI();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.U)) {
            Damage(10);
        }

        if (Input.GetKeyDown(KeyCode.H)) {
            Heal(10);
        }
    }

    private IEnumerator VisualIndicator(Color color){
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = Color.white;

    }

    public void Damage(int amount) {
        if(amount < 0) {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }

        this.health -= amount;
        StartCoroutine(VisualIndicator(Color.red));
        if(health <= 0) {
            Die();
        }
        UpdateHeartsUI();
    }

    public void Heal(int amount) {
        if (amount < 0) {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;
        StartCoroutine(VisualIndicator(Color.green));
        if (wouldBeOverMaxHealth) {
            this.health = MAX_HEALTH;
        }
        else {
            this.health += amount;
        }
        UpdateHeartsUI();
    }

    public void UpdateHeartsUI() 
    {
        int currNumHearts = (health * maxHearts)/maxHealth;
        Debug.Log("NUM HEARTS:" + currNumHearts.ToString());
        healthUIController.DrawHearts(currNumHearts, maxHearts);
    }

    private void Die() {
        Debug.Log("I am Dead!");
        Destroy(gameObject);
    }
}