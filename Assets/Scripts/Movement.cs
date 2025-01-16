using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float maxSpeed = 30f;
    public float midSpeed = 20f;
    public float lowSpeed = 10f;
    public float turnSpeed = 50f;
    public float jumpHeight = 5f;
    public KeyCode jumpKey = KeyCode.Space;
    public int clickCountLevel1 = 3;
    public int clickCountLevel2 = 5;
    public KeyCode moveKey = KeyCode.W;
    public KeyCode turnLeftKey = KeyCode.A;
    public KeyCode turnRightKey = KeyCode.D;
    private int clickCount = 0;
    private float currentSpeed = 0f;
    private float clickTimer = 0f;
    private bool isLocked = false;
    private bool isGrounded = true;
    private Rigidbody rb;
    private Vector3 jumpVelocity;
    private float gravity = -9.8f;
    private Queue<Vector3> positionHistory = new Queue<Vector3>();
    private float recordInterval = 0.1f;
    private float timeSinceLastRecord = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (Input.GetKeyDown(moveKey))
        {
            clickCount++;
        }

        clickTimer += Time.deltaTime;

        if (clickTimer >= 1f)
        {
            UpdateSpeedBasedOnClicks();
            clickCount = 0;
            clickTimer = 0f;
            Debug.Log("Speed: " + currentSpeed);
        }

        if (Input.GetKeyDown(jumpKey) && isGrounded && !isLocked)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        timeSinceLastRecord += Time.fixedDeltaTime;
        if (timeSinceLastRecord >= recordInterval)
        {
            RecordPosition();
            timeSinceLastRecord = 0f;
        }

        if (!isLocked)
        {
            Vector3 move = transform.forward * currentSpeed * Time.fixedDeltaTime;

            if (!isGrounded)
            {
                jumpVelocity.y += gravity * Time.fixedDeltaTime;
                move += jumpVelocity * Time.fixedDeltaTime;
            }

            rb.MovePosition(rb.position + move);

            float turn = 0f;
            if (Input.GetKey(turnLeftKey))
            {
                turn -= turnSpeed * Time.fixedDeltaTime;
            }
            if (Input.GetKey(turnRightKey))
            {
                turn += turnSpeed * Time.fixedDeltaTime;
            }

            Quaternion deltaRotation = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }

    void UpdateSpeedBasedOnClicks()
    {
        if (clickCount >= clickCountLevel2)
        {
            currentSpeed = maxSpeed;
        }
        else if (clickCount >= clickCountLevel1)
        {
            currentSpeed = midSpeed;
        }
        else if (clickCount > 0)
        {
            currentSpeed = lowSpeed;
        }
        else
        {
            currentSpeed = 0f;
        }
    }

    void Jump()
    {
        isGrounded = false;
        jumpVelocity = new Vector3(0, Mathf.Sqrt(-2 * gravity * jumpHeight), 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            isLocked = true;
            RestoreToPreviousPosition(2f);
            StartCoroutine(UnlockControlAfterDelay(2f));
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpVelocity = Vector3.zero;
            if (transform.position.y != 0.5f)
            {
                transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            }
        }
    }

    void RecordPosition()
    {
        positionHistory.Enqueue(transform.position);

        while (positionHistory.Count > Mathf.CeilToInt(2f / recordInterval))
        {
            positionHistory.Dequeue();
        }
    }

    void RestoreToPreviousPosition(float secondsAgo)
    {
        int recordsToSkip = Mathf.CeilToInt(secondsAgo / recordInterval);

        if (positionHistory.Count >= recordsToSkip)
        {
            Vector3[] positions = positionHistory.ToArray();
            transform.position = positions[positionHistory.Count - recordsToSkip];
            rb.position = transform.position;
            if (transform.position.y != 0.5f)
            {
                transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            }
        }
    }

    IEnumerator UnlockControlAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isLocked = false;
    }
}
