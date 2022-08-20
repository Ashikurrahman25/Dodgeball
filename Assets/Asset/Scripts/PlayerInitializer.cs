using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using TMPro;
using StarterAssets;
using UnityEngine.InputSystem;

public class PlayerInitializer : MonoBehaviour, IPunInstantiateMagicCallback
{
    public GameObject playerCamera;
    public GameObject playerNameCanvas;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
   
        //GetComponent<ThirdPersonController>().enabled = info.photonView.IsMine;
        //GetComponent<CharacterController>().enabled = info.photonView.IsMine;

        //GetComponent<PlayerController>().enabled = info.photonView.IsMine;

        if (!GetComponent<PhotonView>().IsMine)
        {
            playerNameCanvas.SetActive(true);
            playerNameCanvas.GetComponentInChildren<TMP_Text>().text = info.photonView.Owner.NickName;
            gameObject.layer = 8;
            gameObject.tag = "Enemy";

            Destroy(GetComponent<ThirdPersonController>());
            Destroy(GetComponent<StarterAssetsInputs>());
            Destroy(GetComponent<CharacterController>());
            Destroy(GetComponent<BasicRigidBodyPush>());
            Destroy(GetComponent<PlayerController>());

            Destroy(GetComponent<PlayerInput>());
            Destroy(GetComponent<BallThrower>());
            //    GetComponent<StarterAssetsInputs>().enabled = info.photonView.IsMine;
            //    GetComponent<PlayerInput>().enabled = info.photonView.IsMine;
            //    GetComponent<BallThrower>().enabled = info.photonView.IsMine;
        }
        else
        {
            playerNameCanvas.SetActive(false);
            FindObjectOfType<CinemachineVirtualCamera>().Follow = playerCamera.transform;
            GetComponent<BallThrower>().cam = Camera.main.transform;
        }

    }
}
