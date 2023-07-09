using UnityEngine;

///<summary>
///
///</summary>
public class ShootScript : MonoBehaviour
{
    #region Variables

    // Variables.
    private float holdTimer = 0f;

    private Rigidbody rb;
    private LineRenderer lr;
    private PlayerController pc;
    private CharacterController cc;
    private bool isLaunching = false;

    private int mask;

    [SerializeField] private float launchForce = 5000f;
    [SerializeField] private float maxHoldTime = 1.5f;

    [SerializeField] Mesh bulletMesh;
    [SerializeField] Mesh WizMesh;
    [SerializeField] MeshFilter mf;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject glasses;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip killClip;

    #endregion Variables

    #region Unity Methods

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        pc = PlayerController.Instance;
        cc = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        lr.SetPosition(1, transform.position);
        lr.SetPosition(0, transform.position);

        if (isLaunching)
        {
            // Detect if we are touching the floor in order to cancel and stand back up.
            // Reenabling the mvoement script and ability to launch again.

            // Rotate toward velocity.
            var lookRotation = Quaternion.LookRotation(rb.velocity);
            lookRotation *= Quaternion.Euler(90, 0, 0);
            rb.MoveRotation(lookRotation);

            // Cancel manually.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EndLaunch();
            }
            return;
        }

        if (Input.GetMouseButton(0))
        {
            CalculateTimeHeld();
            //Debug.Log(holdTimer);
            lr.SetPosition(1, transform.position + CalculateLaunchAngle() * 7 * holdTimer);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (holdTimer >= 0.15f)
            {
                // set rotation to the angle of launch
                pc.enabled = false;
                cc.enabled = false;
                rb.isKinematic = false;
                rb.constraints &= ~RigidbodyConstraints.FreezeRotationX;

                rb.AddForce(CalculateLaunchAngle() * CalculateLaunchForce() * Time.fixedDeltaTime, ForceMode.Impulse);
                // unset this when touching the ground.
                isLaunching = true;
                mf.mesh = bulletMesh;
                gun.SetActive(false);
                glasses.SetActive(false);
                audioSource.PlayOneShot(shootClip);
            }

            holdTimer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Hit enemy detection
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!isLaunching)
            {
                // Its so joever, respawn.
                return;
            }

            Destroy(other.gameObject);
            audioSource.PlayOneShot(killClip);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isLaunching)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            EndLaunch();
        }
    }

    #endregion Unity Methods

    #region Private Methods

    // Private Methods.
    private Vector3 CalculateLaunchAngle()
    {
        var screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var angle = transform.position - screenPoint;
        angle.z = 0;
        return angle.normalized;
    }

    private float CalculateLaunchForce()
    {
        // Calculate force based on base force and a velocity scaler based on how long the mouse is clicked before being released.
        return launchForce * holdTimer;
    }

    private void CalculateTimeHeld()
    {
        holdTimer += Time.deltaTime;
        holdTimer = Mathf.Clamp(holdTimer, 0f, maxHoldTime);
    }
    
    private void EndLaunch()
    {
        pc.enabled = true;
        cc.enabled = true;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        isLaunching = false;
        mf.mesh = WizMesh;

        // Make gun visable
        gun.SetActive(true);
        glasses.SetActive(true);
    }

    #endregion Private Methods

    #region Public Methods

    // Public Methods.

    #endregion Public Methods
}