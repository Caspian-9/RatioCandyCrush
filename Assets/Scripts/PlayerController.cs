using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    private Rigidbody RB;

    public Manager manager;

    public float moveSpeed;
    public float gravity = Physics.gravity.y * 5;
    public float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        RB = GetComponent<Rigidbody>();
        moveSpeed = 10f;
        jumpForce = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        RB.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, RB.velocity.y, Input.GetAxis("Vertical") * moveSpeed);

        if (Input.GetButtonDown("Jump"))
        {
            RB.velocity = new Vector3(RB.velocity.x, jumpForce, RB.velocity.z);
        }

        if (gameObject.transform.position.y < -20)
        {
            gameObject.transform.position = new Vector3(0, 1, 0);
        }


        // this uses CharacterController

        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //controller.Move(moveSpeed * Time.deltaTime * move);

        //if (gameObject.transform.position.y < -10)
        //{
        //    gameObject.transform.position = new Vector3(0, 1, 0);
        //}
    }

    private void OnTriggerEnter(Collider other) {

        // other is the collider of the gameobject the bean is hitting
        if (other.gameObject.tag == "Level0") {

            Debug.Log("Entering tutorial level");
            AllLevelsData.level = 0;
        }

        if (other.gameObject.tag == "Level1")
        {
            Debug.Log("Entering level 1");
            AllLevelsData.level = 1;
        }

        if (other.gameObject.tag == "Level2")
        {
            Debug.Log("Entering level 2");
            AllLevelsData.level = 2;
        }

        if (other.gameObject.tag == "Level3")
        {
            Debug.Log("Entering level 3");
            AllLevelsData.level = 3;
        }

        manager.LoadLevel();
    }
}
