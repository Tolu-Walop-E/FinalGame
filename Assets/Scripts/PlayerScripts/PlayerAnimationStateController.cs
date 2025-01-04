using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationStateController : MonoBehaviour
{
    Animator animator;
    bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
        
    }
    void checkGrounded()
    {
        
        grounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool walkPressed = (Input.GetKey("d") || Input.GetKey("a"));
        
        bool isRunning = animator.GetBool("isRunning");
        bool runPressed = Input.GetKey("left shift");
        
        bool isJumping = animator.GetBool("isJumping");
        bool jumpPressed = Input.GetKey("w");
        bool isFalling = animator.GetBool("isFalling");

        bool isLanding = animator.GetBool("isLanding");
        
        //nest movement in grounded
        checkGrounded();
        if (grounded)
        {
            if (!isWalking && walkPressed)
            {
                animator.SetBool("isWalking", true);
                Debug.Log("isWalking True");
            }
            if (isWalking && !walkPressed)
            {
                animator.SetBool("isWalking", false);
                Debug.Log("isWalking False");
                
            }
        
            //walking and not running and presses run btn
            if (!isRunning && (walkPressed && runPressed))
            {
                animator.SetBool("isRunning", true);
                Debug.Log("isRunning True");
            }
        
            //walking and running and releases run btn or run+walk btn
            if (isRunning && (!walkPressed || !runPressed))
            {
                animator.SetBool("isRunning", false);
                Debug.Log("isRunning False");
            }
            
        }
        
        
        
        //if the player jumps, jump is true, grounded is false
        // if (jumpPressed)
        // {
        //     grounded = false;
        //     animator.SetBool("isJumping", true);
        // }
        
        //jump is true until the player is falling (effected by negative gravity)
        
        
        //whilst grounded is false and the player is landing, grounded becomes true
        
    }
    
}