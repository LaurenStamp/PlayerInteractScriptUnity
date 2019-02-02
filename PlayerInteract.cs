// Used in Impaired by Lauren Stamp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{

    public GameObject currentInterObj = null;
    // References to scripts Inventory and InteractionObject
    public InteractionObject currentInterObjScript = null;
    public Inventory inventory;

    // Audio clips dedicated to the player
    public AudioClip dooropening;
    public AudioClip doorlocked;
    public AudioClip killerscream;
    public AudioClip winningsound;
    public AudioClip keys;

    // Random scares -- trigger points around the map that play a sound when the player enters the trigger point
    public AudioClip randomScare;
    public AudioClip randomScare1;
    public AudioClip randomScare2;
    public AudioClip randomScare3;
    public AudioClip randomScare4;
    public AudioClip randomScare5;

    // Checks if the player has entered the trigger point already
    private bool hasPlayed = false;
    private bool hasPlayed1 = false;
    private bool hasPlayed2 = false;
    private bool hasPlayed3 = false;
    private bool hasPlayed4 = false;
    private bool hasPlayed5 = false;

    public GameObject playerObject;

    // Count variable for counting how many keys the player has
    private int count;
    // Variable for the text that appears on the players screen for how many keys they have
    public Text countText;
    // Variable for the text that appears on the players screen if they collide with the note in the first room
    public Text instruction;

    public AudioSource audioSource;

    // Variables to load the winning and losing scenes
    public string loseScreen;
    public string sceneName;

    // Sound that plays when player interacts with the medkit
	public AudioClip heal;

    private bool flag = false;

    // Delays the amount of time it takes to change scene
    public float delayTimeLose = 0.5f;
    public float delayTimeWin = 3f;

    // Gets the audio source component, sets the key count to 0 at the start of the game, calls the function SetCountText and disables the instruction at start up
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        count = 0;
        SetCountText();
        instruction.enabled = false;
    }

    void Update()
    {
        // If player presses 'e' and that object is the current interactable object (in object trigger point)
        if (Input.GetButtonDown("Interact") && currentInterObj)
        {
            // If player has interacted with a medkit and is bleeding from the monster
            if (currentInterObjScript.IsMedkit() && GetComponent<Bleeding>().bleeding)
            {
                // Stops the player from bleeding, resets timer back to 120 and disables it, plays a sound and removes medkit from game
                GetComponent<Bleeding>().bleeding = false;
                GetComponent<Bleeding>().timer = 120;
                GetComponent<Bleeding>().Countdown.enabled = false;
				audioSource.PlayOneShot (heal);
                currentInterObj.SetActive(false);
            }
            else
            {
                //Alert player they can't use medkit
            }

            // If the item is an item that can be put in the inventory
            if (currentInterObjScript.inventory)
            {
               // Adds the item to the inventory, removes the item from game, plays sound, adds 1 to the counter and displays it to player. 
                inventory.AddItem(currentInterObj);
                currentInterObjScript.DoInteraction();
                audioSource.clip = keys;
                audioSource.pitch = 1.5f;
                audioSource.Play();
                count = count + 1;
                SetCountText();
                Debug.Log(currentInterObj.activeInHierarchy);
            }
            // uses tags to check which note in particular has been interacted with
            if (currentInterObj.CompareTag("Note"))
            {
                Debug.Log("inside");
                // shows that specific note the player interacts with
                currentInterObj.GetComponent<Note>().ShowNoteImage();
            }
            if (currentInterObj.CompareTag("otherNote"))
            {
                Debug.Log("inside");
                currentInterObj.GetComponent<Note1>().ShowNoteImage();
            }
            if (currentInterObj.CompareTag("otherNote1"))
            {
                Debug.Log("inside");
                currentInterObj.GetComponent<Note2>().ShowNoteImage();
            }
            if (currentInterObj.CompareTag("otherNote2"))
            {
                Debug.Log("inside");
                currentInterObj.GetComponent<Note3>().ShowNoteImage();
            }
            if (currentInterObj.CompareTag("otherNote3"))
            {
                Debug.Log("inside");
                currentInterObj.GetComponent<Note4>().ShowNoteImage();
            }

            // checks if the interactable object can be opened (eg. doors)
            if (currentInterObjScript.openable)
            {
                // checks if door is locked
                if (currentInterObjScript.locked)
                {
                    // checks if player has a key in their inventory
                    if (inventory.FindKey())
                    {
                        // key is found in inventory: door is unlocked, plays animation, removes 1 from the counter and shows player, plays sound.
                        currentInterObjScript.locked = false;
                        Debug.Log(currentInterObj.name + " was unlocked");
                        currentInterObjScript.Open();
                        count = count - 1;
                        SetCountText();
                        currentInterObjScript.DoInteraction();
                        audioSource.clip = dooropening;
                        audioSource.pitch = 1f;
						audioSource.PlayOneShot(dooropening);
                    }
                    else
                    {
                        // key was not found: door stays locked and plays sound.
                        Debug.Log("locked");
                        currentInterObjScript.locked = true;
                        audioSource.clip = doorlocked;
                        audioSource.pitch = 1f;
                        audioSource.Play();
                        Debug.Log(currentInterObj.name + " was not unlocked");
                    }
                }
                else
                {
                    Debug.Log(currentInterObj.name + " is open");
                }
            }

            currentInterObj = null;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        // using tags, checks if the player has collided/ triggered with a particular tag
        if (other.CompareTag("interObject"))
        {
            // if tag is interObject: sets it as the current interactable object
            Debug.Log("inside");
            currentInterObj = other.gameObject;
            currentInterObjScript = currentInterObj.GetComponent<InteractionObject>();
        }
        if (other.CompareTag("Note"))
        {
            // if tag is note: sets it as the current interactable object
            currentInterObj = other.gameObject;
            currentInterObjScript = currentInterObj.GetComponent<InteractionObject>();
            
        }
        if (other.CompareTag("otherNote"))
        {
            Debug.Log("inside the note");
            currentInterObj = other.gameObject;
            currentInterObjScript = currentInterObj.GetComponent<InteractionObject>();
        }
        if (other.CompareTag("otherNote1"))
        {
            Debug.Log("inside the note");
            currentInterObj = other.gameObject;
            currentInterObjScript = currentInterObj.GetComponent<InteractionObject>();
        }
        if (other.CompareTag("otherNote2"))
        {
            Debug.Log("inside the note");
            currentInterObj = other.gameObject;
            currentInterObjScript = currentInterObj.GetComponent<InteractionObject>();
        }
        if (other.CompareTag("otherNote3"))
        {
            Debug.Log("inside the note");
            currentInterObj = other.gameObject;
            currentInterObjScript = currentInterObj.GetComponent<InteractionObject>();
            // instruction text to only appear on first note in game and dissapear when out of trigger
			instruction.enabled = true;
        }

        // to activate the sound triggers located around the map by finding its tag
        if (other.CompareTag("soundTrigger"))
        {
            // if sound hasn't already played, play sound and set hasPlayed to true
            if (!hasPlayed)
            {
                audioSource.PlayOneShot(randomScare, 1);
                hasPlayed = true;
            }
        }

        if (other.CompareTag("soundTrigger1"))
        {
            //Debug.Log ("inside");
            if (!hasPlayed1)
            {
                audioSource.PlayOneShot(randomScare1, 1);
                hasPlayed1 = true;
            }
        }

        if (other.CompareTag("soundTrigger2"))
        {
            //Debug.Log ("inside");
            if (!hasPlayed2)
            {
				audioSource.pitch = 1f;
                audioSource.PlayOneShot(randomScare2, 1);
                hasPlayed2 = true;
            }
        }

        if (other.CompareTag("soundTrigger3"))
        {
            //Debug.Log ("inside");
            if (!hasPlayed3)
            {
				audioSource.pitch = 1f;
                audioSource.PlayOneShot(randomScare3, 1);
                hasPlayed3 = true;
            }
        }

        if (other.CompareTag("soundTrigger4"))
        {
            //Debug.Log ("inside");
            if (!hasPlayed4)
            {
				audioSource.pitch = 1f;
                audioSource.PlayOneShot(randomScare4, 1);
                hasPlayed4 = true;
            }
        }

        if (other.CompareTag("soundTrigger5"))
        {
            //Debug.Log ("inside");
            if (!hasPlayed5)
            {
				audioSource.pitch = 1f;
                audioSource.PlayOneShot(randomScare5, 1);
                hasPlayed5 = true;
            }
        }

    }
    // Methods that load a scene
    public void DelayedAction()
    {
        SceneManager.LoadScene(loseScreen);
    }
    public void DelayedAction2()
    {
        SceneManager.LoadScene(sceneName);
    }

    // On collision checks if player has collided with another object
    void OnCollisionEnter2D(Collision2D collision)
    {
        // if the player has collided with the enemy 
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // players controllers are disabled, sound plays and changes to losing scene after x seconds
            playerObject.GetComponent<Mobility>().enabled = false;
            audioSource.clip = killerscream;
            audioSource.Play();
            Invoke("DelayedAction", delayTimeLose);
        }
        // if the player has collided with the exit 
        if (collision.gameObject.CompareTag("Exit"))
        {
            // sound plays, players controllers are disabled and changes to winning scene after x seconds
            Debug.Log("got here");
            audioSource.clip = winningsound;
            GetComponent<Mobility>().enabled = false;
            audioSource.PlayOneShot(winningsound);
            Invoke("DelayedAction2", delayTimeWin);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // checks if the player is out of the trigger for objects and then sets the current interactable object to null
        if (other.CompareTag("interObject"))
        {
            Debug.Log(" it works");
            if (other.gameObject == currentInterObj)
            {
                currentInterObj = null;
            }
        }
        // disables the instruction text if player is out of the first notes collider.
		if (other.CompareTag("otherNote3"))
        {
            instruction.enabled = false;
        }
    }

    void SetCountText()
    {
        countText.text = count.ToString();
    }
}
