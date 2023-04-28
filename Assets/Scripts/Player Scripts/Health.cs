using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] private int maxHearts = STARTING_MAX_HEARTS;
    [SerializeField] private HealthUIController healthUIController;
    public bool isImmortalityOn; // Immortality flag, can be set from Unity editor

    public int hearts { get; private set; }
    public const int STARTING_MAX_HEARTS = 10;

    void Awake() {
        hearts = maxHearts;
        UpdateHeartsUI();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.U)) {
            if (!isImmortalityOn) {
                Damage(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.H)) {
            Heal(1);
        }
        
        if (Input.GetKeyDown(KeyCode.P)) {
            ExpandMaxHealth(1);
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

        this.hearts -= amount;
        StartCoroutine(VisualIndicator(Color.red));
        if(hearts <= 0) {
            Die();
        }
        UpdateHeartsUI();
    }

    public void Heal(int amount) {
        if (amount < 0) {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }

        bool wouldBeOverMaxHearts = hearts + amount > maxHearts;
        StartCoroutine(VisualIndicator(Color.green));
        if (wouldBeOverMaxHearts) {
            this.hearts = maxHearts;
        }
        else {
            this.hearts += amount;
        }
        UpdateHeartsUI();
    }

    public void ExpandMaxHealth(int amount) {
        maxHearts += amount;
    }

    public void UpdateHeartsUI() 
    {
        healthUIController.DrawHearts(hearts, maxHearts);
    }

    private void Die() {
        Debug.Log("I am Dead!");
        Destroy(gameObject);
    }
}