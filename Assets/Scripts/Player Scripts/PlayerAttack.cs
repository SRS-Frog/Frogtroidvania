using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    private GameObject attackArea = default;

    private bool attacking = false;

    private float timeToAttack = 0.15f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start() {
        attackArea = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.P)) {
            Attack();
        }
        if(attacking) {
            timer += Time.deltaTime;
            if(timer >= timeToAttack) {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
            }
        }
    }

    private void Attack() {
        // Debug.Log("Attack function called");
        attacking = true;
        attackArea.SetActive(attacking);
    }
}