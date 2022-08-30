using DG.Tweening;
using Photon.Pun;
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

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;


    [Header("Throwing")]
    //public Key throwKey = Key.;
    public float throwForce;
    public float throwUpwardForce;
    public LayerMask IgnoreMe;

    //bool readyToThrow;
    private InputAction leftMouseClick;
    public GameObject projectile;

    private void Awake()
    {
        //leftMouseClick = new InputAction(binding: "<Mouse>/leftButton");
        //leftMouseClick.performed += ctx => LeftMouseClicked();
        //leftMouseClick.Enable();

    }
    
    private void LeftMouseClicked()
    {
        print("LeftMouseClicked");
        //Throw();
    }

    public void Throw()
    {
        // instantiate object to throw
        if(totalThrows <= 0)
        {
            Debug.Log("No throws");
            return;
        }
        Vector3 forceDirection = Vector3.zero;
      
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));


        // calculate direction

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMe))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;

                Debug.Log(hit.transform.name);
            if (hit.transform.GetComponent<EnemyView>())
            {
                Debug.Log("Enemyyyy");
                string team = PhotonNetwork.LocalPlayer.CustomProperties["Team"].Equals("A") ? "B" : "A";
                //projectile.GetComponent<BallView>().DamagePlayer(hit.transform.GetComponent<EnemyView>(), team, hit.transform.GetComponent<PhotonView>().ViewID);
            }
        }

        //GameObject projectile = Instantiate(objectToThrow, attackPoint.position, Quaternion.LookRotation(forceDirection, Vector3.up));

        projectile.transform.DOMove(attackPoint.position, .1f).OnComplete(() => {

            float doubleForce = throwForce * 2;
            projectile.transform.rotation = Quaternion.LookRotation(forceDirection, Vector3.up);
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            projectile.GetComponent<BallView>().ActivateRPC();

            if(projectile.GetComponent<BallView>().ballType == BallView.BallType.Green)
                projectileRb.velocity = projectile.transform.forward * throwForce*2;
            else
                projectileRb.velocity = projectile.transform.forward * throwForce;
            
            totalThrows--;
            projectile = null;
            UIController.instance.ResetBallCount();

        }) ;
       
    }

    public void ClaimBall(BallView ball)
    {
        if (totalThrows > 0 || !ball.canClaim) return;

        totalThrows = 1;
        projectile = ball.gameObject;

        string team = PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString();
        ball.ClaimRPC(team);
    }
}
