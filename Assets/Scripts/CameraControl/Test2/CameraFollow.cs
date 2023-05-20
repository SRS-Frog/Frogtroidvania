using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private BoxCollider2D camBox;
    private GameObject[] boundaries;
    private Bounds[] allBounds;
    private Bounds targetBounds;

    public float speed;
    private float waitForSeconds = 0.5f;

    void Start()
    {
        player = GameObject.Find("Daehyun").GetComponent<Transform>();
        camBox = GetComponent<BoxCollider2D>();
        FindLimits();
    }

    void LateUpdate()
    {
        if(waitForSeconds > 0)
            waitForSeconds -= Time.deltaTime;
        else{
            SetOneLimit();
            FollowPlayer();
        }
    }

    //Finds all limits of the stage environment
    void FindLimits()
    {
        boundaries = GameObject.FindGameObjectsWithTag("Boundary");
        allBounds = new Bounds[boundaries.Length];
        for(int i = 0; i < allBounds.Length; i++)
            allBounds[i] = boundaries[i].gameObject.GetComponent<BoxCollider2D>().bounds;
    }

    //Sets limits on the camrea based on which boundary the player is located in
    void SetOneLimit()
    {
        for(int i = 0; i < allBounds.Length; i++)
        {
            if(player.position.x > allBounds[i].min.x && player.position.x < allBounds[i].max.x && player.position.y > allBounds[i].min.y && player.position.y < allBounds[i].max.y)
            {
                targetBounds = allBounds[i];
                return;
            }
        }
    }

    void FollowPlayer()
    {
        float xTarget = camBox.size.x < targetBounds.size.x ? Mathf.Clamp(player.position.x, targetBounds.min.x + camBox.size.x/2, targetBounds.max.x - camBox.size.x/2) : (targetBounds.min.x + targetBounds.max.x)/2;
        float yTarget = camBox.size.y < targetBounds.size.y ? Mathf.Clamp(player.position.y, targetBounds.min.y + camBox.size.y/2, targetBounds.max.y - camBox.size.y/2) : (targetBounds.min.y + targetBounds.max.y)/2;
        Vector3 target = new Vector3(xTarget, yTarget, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
    }
}
