using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject characterSprite;
    [SerializeField]
    public GameObject interactableObject, lastObject;
    
    public CharacterController characterController;
    public Animator animator;

    private Vector3 movementDirection;
    private  Vector3 lastMoveDirection;

    public bool controlsEnabled;

    private float moveX;
    private float moveZ;
    public float movementSpeed;

    private void Start()
    {
        lastMoveDirection.z = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlsEnabled == true && PauseMenu.gameIsPaused == false)
        {
            // Player Movement
            moveX = Input.GetAxisRaw("Horizontal");
            moveZ = Input.GetAxisRaw("Vertical");
            movementDirection = new Vector3(moveX, 0, moveZ).normalized;
            characterController.Move(movementDirection * movementSpeed * Time.deltaTime);

            // Check if Player presses A
            if (moveX >= -1 && moveX <= -0.1)
            {
                // Flipping sprite so it faces the side it is going to
                characterSprite.GetComponent<SpriteRenderer>().flipX = true;
            }
            // Check if Player presses D
            else if (moveX <= 1 && moveX >= 0.1)
            {
                characterSprite.GetComponent<SpriteRenderer>().flipX = false;
            }


            //Interaction with Objects
            if (interactableObject != null)
            {
                if (interactableObject.GetComponent<SphereCollider>().enabled == true)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        DisableWalkingAnimation();

                        if (interactableObject.tag == "Miri's Phone")
                        {
                            interactableObject.GetComponent<MirisPhone>().OnClicked();
                        }
                        else
                        {
                            interactableObject.GetComponent<InteractableObject>().OnClicked();
                        }
                    }
                }
                else
                {
                    interactableObject = null;
                }
            }
        }


        //Handling Player Animations
        Animate();
    }

    //Handling Player Animations
    private void Animate()
    {
        // Getting in which Z-Direction the player moved last -> To know which direction he is facing
        if (movementDirection.z != 0)
        {
            lastMoveDirection.z = movementDirection.z;
        }

        // Idle Animations
        if(moveX == 0 && moveZ == 0)
        {
            // Check if player moved to the back
            if (lastMoveDirection.z > 0)
            {
                animator.SetTrigger("Idle Back");
            }
            // Check if player moved to the front
            else if (lastMoveDirection.z < 0)
            {
                animator.SetTrigger("Idle Front");
            }

            return;
        }

        // Walk Animations
        // Check if player is currently moving to the back or moved to the back last
        if(lastMoveDirection.z > 0.1 || moveZ > 0.1)
        {
            animator.SetTrigger("Walk Back");
        }
        // Check if player is currently moving to the front or moved to the front last
        else if (lastMoveDirection.z < -0.1 || moveZ < -0.1)
        {
            animator.SetTrigger("Walk Front");
        }
    }

    // Stopps the walking animation by setting the input to 0
    public void DisableWalkingAnimation()
    {
        moveX = 0;
        moveZ = 0;
    }

    public void EnableControls()
    {
        controlsEnabled = true;
    }

    public void DisableControls()
    {
        controlsEnabled = false;
    }

    // Turns Miri to the Camera
    public void FaceCamera()
    {
        animator.SetTrigger("TurnToCam");
    }

    public void EnableCursor(bool isEnabled)
    {
        Cursor.visible = isEnabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interactable Object" || other.tag == "Miri's Phone")
        {
            // If there is an object stored in interactableObject, disabling icon and sets the object to lastObject
            if (interactableObject != null)
            {
                if (interactableObject.GetComponent<InteractableObject>().interactabilityIcon.GetComponent<SpriteRenderer>() != null)
                {
                    interactableObject.GetComponent<InteractableObject>().interactabilityIcon.GetComponent<SpriteRenderer>().enabled = false;
                }

                lastObject = interactableObject;
            }

            // Object of collider the player entered gets set to interactableObject
            interactableObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactable Object" || other.tag == "Miri's Phone")
        {
            if (interactableObject == other.gameObject)
            {
                if (interactableObject == lastObject)
                {
                    lastObject = null;
                    interactableObject = null;
                }
                else if (lastObject != null)
                {
                    interactableObject = lastObject;
                    interactableObject.GetComponent<InteractableObject>().interactabilityIcon.GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    interactableObject = null;
                }
            }
            else // If interactableObject and the object of the collider the player exits are not the same
            {
                lastObject = null;
            }
        }
    }
}
