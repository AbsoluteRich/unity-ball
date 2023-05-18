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
    private AudioSource[] sfx;
    private AudioSource powerup;
    private bool hasWon;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        sfx = GetComponents<AudioSource>();
        explode = sfx[0];
        powerup = sfx[1];
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        loseTextObject.SetActive(false);
        hasWon = false;
    }

    // Called when a move event occurs
    void OnMove(InputValue movementValue)
    {
        // Todo: Make the player's movement slow down to 0 after releasing the key
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
                    powerup.Play();
                    hasWon = true;
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

    // Called when the player dies after 2 seconds
    private void Respawn()
    {
        loseTextObject.SetActive(false);
        gameObject.transform.position = new Vector3(0f, 0.5f, 0f);
        gameObject.SetActive(true);
    }

    // Called when player collides with an object
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.gameObject.name);

        // Todo: Make the win sound effect play instead of this when the player, well, wins
        //  The winning effect plays *as* the player gets a pickup, add logic where if the count is above a certain point then play the sound effect
        explode.Play();

        if (other.gameObject.CompareTag("PickUp")) // When the player collides with a pick up
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Walls")) // When the player collides with a red wall
        {
            if (!hasWon)
            {
                gameObject.SetActive(false);
                loseTextObject.SetActive(true);
                Invoke("Respawn", 2.0f);
            }
            else
            {
                gameObject.transform.position = new Vector3(0f, 0.5f, 0f);
            }
        }
    }
}
