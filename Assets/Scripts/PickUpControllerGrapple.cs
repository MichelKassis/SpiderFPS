using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpControllerGrapple : MonoBehaviour
{

    public GrapplingGun gunScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;


    private void Start()
    {
        //Setup
        if (!equipped) {
            gunScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped) {
            gunScript.enabled = true; 
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check if player in range of a pickup weapon
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude < pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull) { Pickup(); }

        if (equipped && Input.GetKeyDown(KeyCode.Q)) { Drop(); }


    }

    private void Pickup() {
        equipped = true;
        slotFull = true;

        //make weapon child of container
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        rb.isKinematic = true;
        coll.isTrigger = true;

        //Enable Script
        gunScript.enabled = true; 
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        //SetParent to null
        transform.SetParent(null);

        rb.isKinematic = false;
        coll.isTrigger = false;

        //Gun Carries momentum of player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        //Add throwing gun Force
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);

        //Add Random Rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);




        //Enable Script
        gunScript.enabled = false; 

    }

}
