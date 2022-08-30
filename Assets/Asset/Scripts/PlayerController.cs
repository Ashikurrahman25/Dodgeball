using DG.Tweening;
using GG.Infrastructure.Utils.Swipe;
using Photon.Pun;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    public Animator anim;
    public bool disqualified = false;
    public bool freeze = false;
    public bool dizzy = false;




    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Movement();

        if(disqualified && !UIController.instance.showedDisInfo)
        {
            UIController.instance.ShowDisqualifey();
            transform.DOMove(UIController.instance.DisqualifyTransform().position,1f);
            GetComponent<PhotonView>().RPC("Remove", RpcTarget.All, PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString());

            if(GameManager.instance.AllDisqualified())
                GetComponent<PhotonView>().RPC("GameOver", RpcTarget.All, PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString());

            EnableOrDisablePlayer(false);

        }
        else if (freeze && !UIController.instance.startFreeze)
        {
            UIController.instance.FreezePlayer(()=> 
            {
                freeze = false;
                EnableOrDisablePlayer(true); 
            });
            EnableOrDisablePlayer(false);
        }
        else if (dizzy && !UIController.instance.startDizzy)
        {
            UIController.instance.MakePlayerDizzy(() =>
            {
                dizzy = false;
                EnableOrDisablePlayer(true);
            });
            EnableOrDisablePlayer(false);
        }


    }

    public void EnableOrDisablePlayer(bool status)
    {

        GetComponent<ThirdPersonController>().enabled = status;
        GetComponent<StarterAssetsInputs>().enabled = status;
        GetComponent<PlayerInput>().enabled = status;
        GetComponent<BallThrower>().enabled = status;
        //GetComponent<PlayerController>().enabled = status;
    }



    void Movement()
    {

        if (_controller.velocity != Vector3.zero)
        {
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.layer == 6)
        //{
        //    BallThrower ball = GetComponent<BallThrower>();
        //    BallView ballView = collision.gameObject.GetComponent<BallView>();

        //    if (ball.totalThrows > 0 || !ballView.canClaim || disqualified) return;

        //    GetComponent<BallThrower>().totalThrows = 1;
        //    GetComponent<BallThrower>().projectile = collision.gameObject;

        //    string team = PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString();
        //    collision.gameObject.GetComponent<BallView>().ClaimRPC(team);
        //    Debug.Log("Claiming Ball");
        //}
    }

}