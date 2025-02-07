using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float dichuyenX = Input.GetAxis("Horizontal");
        float dichuyenY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(dichuyenX, dichuyenY);
        transform.position += movement * speed * Time.deltaTime;
    }
}
