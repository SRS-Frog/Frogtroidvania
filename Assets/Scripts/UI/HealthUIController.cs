using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour 
{
    
    [SerializeField] GameObject heartObject;
    public void DrawHearts(int hearts, int maxHearts) 
    {
        // Clear the health bar
        foreach (Transform child in transform) 
        {
            Destroy(child.gameObject);
        }

        for (int heartIndex = 0; heartIndex < maxHearts; ++heartIndex) 
        {
            if (heartIndex + 1 <= hearts)
            {
                GameObject newHeart = Instantiate(heartObject, transform.position, Quaternion.identity);
                newHeart.transform.parent = transform;
            } 
/*
            TODO: For now, we just delete the hearts that are lost. In the future, we may want to un-fill some
            hearts to indicate health loss
            else 
            {
                GameObject newHeart = Instantiate(heartObject, transform.position, Quaternion.identity);
                newHeart.transform.parent = transform;
            }
*/
        }
    }
}
