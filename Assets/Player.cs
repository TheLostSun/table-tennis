using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform aimTarget; // the target where we aim to land the ball
    public Transform net,backWall,leftWall,rightWall;
    float speed = 3f; // move speed
    float force = 13; // ball impact force

    bool hitting; // boolean to know if we are hitting the ball or not 

    public Transform ball; // the ball 
    Animator animator;

    Vector3 aimTargetInitialPosition; // initial position of the aiming gameObject which is the center of the opposite court

    ShotManager shotManager; // reference to the shotmanager component
    Shot currentShot; // the current shot we are playing to acces it's attributes

    private void Start()
    {
        animator = GetComponent<Animator>(); // referennce out animator
        aimTargetInitialPosition = aimTarget.position; // initialise the aim position to the center( where we placed it in the editor )
        shotManager = GetComponent<ShotManager>(); // accesing our shot manager component 
        currentShot = shotManager.topSpin; // defaulting our current shot as topspin
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); // get the horizontal axis of the keyboard
        float v = Input.GetAxisRaw("Vertical"); // get the vertical axis of the keyboard



    if (transform.position.x < net.position.x) {
    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -90f, transform.rotation.eulerAngles.z);
} else {
    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90f, transform.rotation.eulerAngles.z);
}



        // if (Input.GetKeyDown(KeyCode.F)) 
        // {
        //     hitting = true; // we are trying to hit the ball and aim where to make it land
        //     currentShot = shotManager.topSpin; // set our current shot to top spin
        // }
        // else if (Input.GetKeyUp(KeyCode.F))
        // {
        //     hitting = false; // we let go of the key so we are not hitting anymore and this 
        // }                    // is used to alternate between moving the aim target and ourself

        if (Input.GetKeyDown(KeyCode.E))
        {
            hitting = true; // we are trying to hit the ball and aim where to make it land
            currentShot = shotManager.flat; // set our current shot to top spin
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            hitting = false;
        }



        if (hitting)  // if we are trying to hit the ball
        {
            aimTarget.Translate(new Vector3(h, 0, 0) * speed * 2 * Time.deltaTime); //translate the aiming gameObject on the court horizontallly
        }


           if ((h != 0 || v != 0) &&!hitting) 
            {
                // Convert input direction from world space to local space
                Vector3 localDirection = transform.InverseTransformDirection(new Vector3(h, 0, v));

                // Perform movement in local space
                transform.Translate(localDirection * speed * Time.deltaTime);

                transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftWall.position.x, rightWall.position.x),
                                              transform.position.y,
                                              Mathf.Clamp(transform.position.z, backWall.position.z, (net.position.z+backWall.position.z)/2));
            }



    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) // if we collide with the ball 
        {
            Vector3 dir = aimTarget.position - transform.position; // get the direction to where we want to send the ball
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
            //add force to the ball plus some upward force according to the shot being played

            Vector3 ballDir = ball.position - transform.position; // get the direction of the ball compared to us to know if it is
             animator.Play("forehand");                        // play a forhand animation if the ball is on our right
            
           

            aimTarget.position = aimTargetInitialPosition; // reset the position of the aiming gameObject to it's original position ( center)

        }
    }


}