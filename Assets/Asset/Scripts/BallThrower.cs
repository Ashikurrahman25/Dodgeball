using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class BallThrower : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public Transform debugTransform;

    [Header("Settings")]
    //public int totalThrows;
    public float throwCooldown;


    [Header("Throwing")]
    //public Key throwKey = Key.;
    public float throwForce;
    public float throwUpwardForce;
    public LayerMask IgnoreMe;

    //bool readyToThrow;
    private InputAction leftMouseClick;

    private void Awake()
    {
        leftMouseClick = new InputAction(binding: "<Mouse>/leftButton");
        leftMouseClick.performed += ctx => LeftMouseClicked();
        leftMouseClick.Enable();

    }
    
    private void LeftMouseClicked()
    {
        print("LeftMouseClicked");
        Throw();
    }

    public void Throw()
    {
        // instantiate object to throw

        Vector3 forceDirection = Vector3.zero;
       



        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));


        // calculate direction

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMe))
        {
            debugTransform.position = hit.point;
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, Quaternion.LookRotation(forceDirection, Vector3.up));

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.velocity = projectile.transform.forward * throwForce;
    }

    private void ResetThrow()
    {
        //readyToThrow = true;
    }
}
