using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody myRB;

    [SerializeField] Color myColor;
    [SerializeField] bool isPlaying;

    [SerializeField] float forwardSpeed;
    [SerializeField] float sideLerpSpeed;

    [SerializeField] Transform stackPosition;
    Transform parentPickup;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            MoveForward();
        }
        if (Input.GetMouseButton(0))
        {
            if(isPlaying == false)
            {
                isPlaying = true;
            }
            MoveSideways();
        }
    }

    void MoveSideways()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(hit.point.x, transform.position.y, transform.position.z), sideLerpSpeed * Time.deltaTime);
        }
    }
    void MoveForward()
    {
        myRB.velocity = Vector3.forward * forwardSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pickup")
        {
            Transform otherTransform = other.transform;

            Rigidbody otherRB = otherTransform.GetComponent<Rigidbody>();
            otherRB.isKinematic = true;
            other.enabled = false;
            if (parentPickup == null)
            {
                parentPickup = otherTransform;
                parentPickup.position = stackPosition.position;
                parentPickup.parent = stackPosition;
            }
            else
            {
                parentPickup.position += Vector3.up * (otherTransform.localScale.y);
                otherTransform.position = stackPosition.position;
                otherTransform.parent = parentPickup;
            }
        }
    }
}
