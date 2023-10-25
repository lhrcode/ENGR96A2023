using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    public float speed = 0;

    public float strength = 0;

    private int count;

    private bool onGround;

    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        Vector3 movement = new(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            count++;
            Vector3 randomPos = new(Random.Range(-9.5f, 9.5f), 0.5f, Random.Range(-9.5f, 9.5f));
            Instantiate(other.gameObject, randomPos, other.gameObject.transform.rotation);
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
            SetCountText();
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump()
    {
        if (onGround == true)
        {
            Vector3 jump = new(0.0f, 10f, 0.0f);
            rb.AddForce(jump * strength, ForceMode.Impulse);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        float angle = Vector3.Angle(collision.contacts[0].normal, Vector3.up);

        if (angle <= 45)
        {
            onGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        onGround = false;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 11)
        {
            winTextObject.SetActive(true);
        }
    }
}