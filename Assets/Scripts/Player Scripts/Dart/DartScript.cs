using UnityEngine;

public class DartScript : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 mousePos;
    public GameObject projectile;
    public Transform projectileTransform;
    // public float timeBetweenFiring;
    public bool canFire; 
    public bool enableCrosshairFiring;

    //Daehyun variables
    private GameObject daehyun;
    private Rigidbody2D daehyunRB;
    private StateManager player;
    private PlayerAttributes playerAttributes;

    private void Awake() 
    {
        // daehyun references
        daehyun = transform.parent.GetChild(0).gameObject; // get a reference to daehyun's game object
        daehyunRB = daehyun.GetComponent<Rigidbody2D>(); // daehyun's rigidbody
        player = daehyun.GetComponent<StateManager>();
        playerAttributes = daehyun.GetComponent<PlayerAttributes>();
    }
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set the aimer pivot position to daehyun's position
        transform.position = daehyun.transform.position;

        if (enableCrosshairFiring) {

            // Crosshair rotation logic, messy geometry stuff
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 rotation = mousePos - transform.position;
            float rotationInZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            
            transform.rotation = Quaternion.Euler(0, 0, rotationInZ);
        } else {
            // TODO: Don't use magic numbers, also get consensus on how far the dart origination point should be
            float xPosition = player.playerController.GetDir() == 1 ? 3f : -3f;
            transform.GetChild(0).localPosition = new Vector3(xPosition, 1f, 0f);
        }
    }

    public void Shoot() 
    {
        bool isStateValidForFiring = true;
        if(playerAttributes.state == PlayerAttributes.PlayerStates.Plunging || playerAttributes.state == PlayerAttributes.PlayerStates.Dashing) {
            isStateValidForFiring = false;
        }

        if (isStateValidForFiring && canFire) {
            GameObject dart = Instantiate(projectile, projectileTransform.position, Quaternion.identity);
            dart.GetComponent<ProjectileScript>().enableCrosshairFiring = enableCrosshairFiring;
            
            // Set direction of the dart using the player's direction
            if (!enableCrosshairFiring) {
                dart.GetComponent<ProjectileScript>().force *= player.playerController.GetDir() == 1 ? 1 : -1;
            }
        }
    }
}
