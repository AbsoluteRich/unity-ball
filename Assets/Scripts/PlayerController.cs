using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private AudioSource explode;
    private string sceneName;
    public GameObject loseTextObject;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        explode = GetComponent<AudioSource>();
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        loseTextObject.SetActive(false);
    }

    // Called when a move event occurs
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Called to change the count text in the top left of the screen
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 12) // When the player has won
        {
            Debug.Log(sceneName);
            switch (sceneName)
            {
                case "MiniGame":
                    SceneManager.LoadScene(sceneName: "Level2");
                    break;

                case "Level2":
                    winTextObject.SetActive(true);
                    break;
            }
            
        }
    }

    // Called every frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    // Called when player collides with an object
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
        if (other.gameObject.CompareTag("PickUp")) // When the player collides with a pick up
        {
            explode.Play();
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Walls")) // When the player collides with a wall
        {
            // Todo: Hide the player, lock inputs, and play a sound effect
            //  Pickups should not be collidable when the player has lost
            gameObject.SetActive(false);  // THIS JUST WORKS!!
            loseTextObject.SetActive(true);
        }
    }
}
