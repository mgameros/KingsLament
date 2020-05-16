using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// I tried to avoid using global variables, but some stuff had to be passed between player iterations
public static class Globals
{
    public static bool ghost = false;
    public static bool door;
    public static bool princess;
    public static bool king;
    public static bool throneRoom;
}

// This class controls every playable iteration of the player. It handles movement, scene navigation, and player interaction.
public class Player : MonoBehaviour
{
    // playerAnimation is the player's animations
    Animator playerAnimation;
    
    // dialogue is all the dialogue in the scene
    public GameObject dialogue;

    // camera is the scene's camera
    public Camera camera;
    
    // x, y, and z are used when a position needs to be updated
    float x;
    float y;
    float z;

    // move, jump, and interact hold the key recognition for movement and player interaction
    float move;
    float jump;
    float interact;

    // notJumping is to make sure that the player can only jump once, which prevents the player from flying off into space
    bool notJumping = true;

    // interval is the set interval to move the player by
    float interval = 0.05F;

    // tablet, up, down, crypt, princessCrown, and kingCrown are all boolean triggers for interactions
    bool tablet;
    bool up;
    bool down;
    bool crypt;
    bool princessCrown;
    bool kingCrown;

    // Start is called before the first frame update
    void Start()
    {
        // get the player's animation so it can be manipulated later
        playerAnimation = GetComponent<Animator>();
        
        // set the dialogue to be hidden until a tablet is activated
        dialogue.SetActive(false);

        // check the global variables

        // if the door is active, then that means that the player has moved from the upstairs to the downstairs and needs to be repositioned
        if(Globals.door)
        {
            transform.position = new Vector3(42.3F, -1.6F, 0F);
            camera.transform.position = new Vector3(42.3F, 0F, -10F);
            Globals.door = false;
            Debug.Log("Door");
        }

        // if the princess is active, then that means that the player has moved from the princess' room to the upper level hallways and needs to be repositioned
        if(Globals.princess)
        {
            transform.position = new Vector3(20.45F, -1.68F, 0F);
            camera.transform.position = new Vector3(14.1F, 0F, -10F);
            playerAnimation.SetBool("Left", true);
            playerAnimation.SetBool("Right", false);
            Globals.princess = false;
            Debug.Log("Princess");
        }

        // if the king is active, then that means that the player has moved from the king's room to the upper level hallways and needs to be repositioned
        if(Globals.king)
        {
            transform.position = new Vector3(-20.53F, -1.68F, 0F);
            camera.transform.position = new Vector3(-14.18F, 0F, -10F);
            Globals.king = false;
            Debug.Log("King");
        }

        // if the throne room is active, then that means that the player has moved from the throne room to the lower level hallways and needs to be repositioned
        if(Globals.throneRoom)
        {
            transform.position = new Vector3(62.96F, -1.6F, 0F);
            camera.transform.position = new Vector3(56.59F, 0F, -10F);
            playerAnimation.SetBool("Left", true);
            playerAnimation.SetBool("Right", false);
            Globals.throneRoom = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        // check when the player has hit a movement key
        move = Input.GetAxis("Horizontal");

        // check when the player has hit a jump key
        if(notJumping)
            jump = Input.GetAxis("Vertical");

        // check and handle when the interact key is pressed (F key)
        interact = Input.GetAxis("Fire1");


        if(interact != 0)
        {
            // if a tablet has been activated, make the dialogue visible
            if (tablet)
            {
                dialogue.SetActive(true);
            }

            // if the player is going up, load the upper level hallway
            if (up)
            {
                // check to see if the ghosts have been summoned yet
                if (Globals.ghost)
                    SceneManager.LoadScene(sceneName: "Ghosts2");
                else
                    SceneManager.LoadScene(sceneName: "Explore2");
            }
            
            // if the player is going down, load the lower level hallway
            if (down)
            {
                // set the global variable door to be true so the next iteration of the player knows that it just went downstairs
                Globals.door = true;

                // check to see if the ghosts have been summoned yet
                if (Globals.ghost)
                    SceneManager.LoadScene(sceneName: "Ghosts");
                else
                    SceneManager.LoadScene(sceneName: "Explore");
            }
            if(kingCrown)
            {
                // the ghosts have just been summoned, so set the ghost levels to activate
                Globals.ghost = true;

                // load the ghost throne room
                SceneManager.LoadScene(sceneName: "ThroneRoomGhost");
            }
            if(princessCrown)
            {
                // load the ending cutscene
                SceneManager.LoadScene(sceneName: "Cutscene");
            }
            if(crypt)
            {
                // load the crypt
                SceneManager.LoadScene(sceneName: "Crypt");
            }
        }
        
    }

    // This function updates periodically at a fixed time. It's best for physics because it won't lead to sporadic movement and animation
    private void FixedUpdate()
    {
        // handle movement
        // If move is 1, that means the player has pressed one of the keys to move right
        // If move is -1, that means the player has pressed one of the keys to move left
        // If move is 0, that means the player is stopped

        if (move == 1)
        {
            // set the animation to move right
            playerAnimation.SetBool("Right", true);
            playerAnimation.SetBool("Left", false);

            // get the player's position
            x = transform.position.x;
            y = transform.position.y;
            z = transform.position.z;

            // move the player right by a set interval
            transform.position = new Vector3(x+interval,y,z);
        }
        else if (move == -1)
        {
            // set the animation to move left
            playerAnimation.SetBool("Left", true);
            playerAnimation.SetBool("Right", false);

            // get the player's position
            x = transform.position.x;
            y = transform.position.y;
            z = transform.position.z;

            // move the player left by a set interval
            transform.position = new Vector3(x-interval,y,z);
        }
        else
        {
            // set the animation to stop moving
            playerAnimation.SetBool("Right", false);
            playerAnimation.SetBool("Left", false);
        }

        // handle jumping
        // If jump is equal to 1, that means the jump key has been pressed
        // Also check to make sure the player isn't moving when jumping so the player doesn't go flying off the screen

        if (jump==1 && GetComponent<Rigidbody2D>().velocity.magnitude.Equals(0))
        {
            // set notJumping to false to stop the player from being able to jump twice
            notJumping = false;

            // set the jumping animation to start
            playerAnimation.SetBool("Jump", true);

            // add a force to the player to start the jump
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 9), ForceMode2D.Impulse);

            // reset the jump variable
            jump = 0;
        }
    }

    // This calls when the player collides with something
    // The only collision that "A King's Lament" handles is when the player lands on the floor to reset the jump key
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if the player hits a floor sprite, reactivate jumping
        if (collision.gameObject.tag == "floor")
        {
            notJumping = true;
            playerAnimation.SetBool("Jump", false);
        }
    }

    // This is the bulk of the code. It activates whenever the player intersects a trigger.
    // There are two kinds of triggers in "A King's Lament": boolean triggers and movement triggers.
    // Boolean triggers set a boolean to be true so the player can interact with it.
    // Movement triggers control the movement between rooms.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // These are the boolean triggers

        // if the player intersects a lore tablet, set tablet to true so the player can view the lore
        if (collision.gameObject.tag == "lore")
        {
            tablet = true;
        }

        // if the player intersects an upstairs trigger, set up to true so the player can go upstairs
        if (collision.gameObject.tag == "upstairs")
        {
            up = true;
        }

        // if the player intersects a downstairs trigger, set down to true so the player can go downstairs
        if (collision.gameObject.tag == "downstairs")
        {
            down = true;
        }

        // if the player intersects the crypt trigger, set crypt to true so the player can go to the crypt
        if (collision.gameObject.tag == "crypt")
        {
            crypt = true;
        }

        // if the player intersects the king's crown trigger, set kingCrown to true so the player can activate it
        if (collision.gameObject.tag == "king crown")
        {
            kingCrown = true;
        }

        // if the player intersects the princess' crown trigger, set princessCrown to true so the player can activate it
        if (collision.gameObject.tag == "princess crown")
        {
            princessCrown = true;
        }

        // These are the movement triggers

        // if the player intersects the throne room trigger, navigate to the throne room
        if (collision.gameObject.tag == "throne room")
        {
            // check to see if the ghosts have been summoned yet
            if (Globals.ghost)
                SceneManager.LoadScene(sceneName: "ThroneRoomGhosts");
            else
                SceneManager.LoadScene(sceneName: "ThroneRoom");
        }

        // if the player intersects the princess trigger, navigate to the princess' room
        if (collision.gameObject.tag == "princess")
        {
            // check to see if the ghosts have been summoned yet
            if (Globals.ghost)
                SceneManager.LoadScene(sceneName: "PrincessGhost");
            else
                SceneManager.LoadScene(sceneName: "Princess");
        }

        // if the player intersects the king trigger, navigate to the king's room
        if (collision.gameObject.tag == "king")
        {
            // check to see if the ghosts have been summoned yet
            if (Globals.ghost)
                SceneManager.LoadScene(sceneName: "KingGhost");
            else
                SceneManager.LoadScene(sceneName: "King");
        }

        // if the player intersects the main trigger, navigate to the downstairs hallway
        if(collision.gameObject.tag == "main")
        {
            // set the throne room variable to true so the next iteration of the player knows they came from the throne room
            Globals.throneRoom = true;

            // check to see if the ghosts have been summoned yet
            if (Globals.ghost)
                SceneManager.LoadScene(sceneName: "Ghosts");
            else
                SceneManager.LoadScene(sceneName: "Explore");
        }

        // if the player intersects the princess upstairs trigger, navigate to the upstairs hallway
        if(collision.gameObject.tag == "princess upstairs")
        {
            // set the princess variable to true so the next iteration of the player knows they came from the princess' room
            Globals.princess = true;

            // check to see if the ghosts have been summoned yet
            if (Globals.ghost)
                SceneManager.LoadScene(sceneName: "Ghosts2");
            else
                SceneManager.LoadScene(sceneName: "Explore2");
        }

        // if the player intersects the king upstairs trigger, navigate to the upstairs hallway
        if(collision.gameObject.tag == "king upstairs")
        {
            // set the king variable to true so the next iteration of the player knows they came from the king's room
            Globals.king = true;

            // check to see if the ghosts have been summoned yet
            if (Globals.ghost)
                SceneManager.LoadScene(sceneName: "Ghosts2");
            else
                SceneManager.LoadScene(sceneName: "Explore2");
        }
    }

    // This function calls whenever the player leaves a trigger.
    // "A King's Lament" deals with exiting two kinds of triggers: boolean and camera pans.
    // Boolean triggers will reset to false when the player leaves it so it won't activate when the player isn't near it.
    // Camera pans tell the camera to move to a different part of a level.
    private void OnTriggerExit2D(Collider2D collision)
    {
        // These are the Boolean triggers

        // when the player leaves a lore tablet, turn off any active dialogue and reset the trigger
        if (collision.gameObject.tag == "lore")
        {
            // reset the trigger
            tablet = false;

            // hide the dialogue
            dialogue.SetActive(false);
        }

        // when the player leaves the lower level door
        if (collision.gameObject.tag == "upstairs")
        {
            up = false;
        }

        // when the player leaves the upper level door
        if (collision.gameObject.tag == "downstairs")
        {
            down = false;
        }
        
        // when the player leaves the king's crown
        if (collision.gameObject.tag == "king crown")
        {
            kingCrown = false;
        }

        // when the player leaves the princess' crown
        if (collision.gameObject.tag == "princess crown")
        {
            princessCrown = false;
        }
        
        // when the player leaves the crypt door
        if (collision.gameObject.tag == "crypt")
        {
            crypt = false;
        }

        // This is the camera pan trigger

        // when the player leaves a camera pan trigger, move the camera a set distance
        if (collision.gameObject.tag == "pan camera")
        {
            // get the camera's current position
            x = camera.transform.position.x;
            y = camera.transform.position.y;
            z = camera.transform.position.z;

            // get the trigger's x value
            float xCollision = collision.gameObject.transform.position.x;

            // if the player has moved to the left of the trigger, move the camera left by a set size
            if(transform.position.x < xCollision)
            {
                camera.transform.position = new Vector3(xCollision - 7.01F, y, z);
            }
            // otherwise, if the player has moved to the right of the trigger, move the camera right by a set size
            else if(transform.position.x > xCollision)
            {
                camera.transform.position = new Vector3(xCollision + 7.01F, y, z);
            }
        }
    }
}
