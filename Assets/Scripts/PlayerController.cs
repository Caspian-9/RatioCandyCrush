using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;

    public Manager manager;

    public float moveSpeed;
    public float gravity = Physics.gravity.y;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(moveSpeed * Time.deltaTime * move);
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
        manager.LoadLevel();
    }
}
