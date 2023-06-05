using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private GameObject lilypad;
    private List<Image> hearts;
    private Color startingColor;
    
    void Start()
    {
        // Get images making up health bar
        hearts = new List<Image>(GetComponentsInChildren<Image>());
        startingColor = hearts[0].color;
    }
    
    void Update()
    {
        
    }
    
    public void UpdateHealthBar(int currNumHearts, int maxHearts) {
        for (int i = hearts.Count; i < maxHearts; i++)
        {
            GameObject g = Instantiate(lilypad, transform);
            hearts.Add(g.GetComponent<Image>());
        }
        
        for (int heartIndex = 0; heartIndex < hearts.Count; ++heartIndex) {
            if (heartIndex < currNumHearts) {
                hearts[heartIndex].color = startingColor;
            } else {
                hearts[heartIndex].color = Color.grey;
            }
        }
    }
}
