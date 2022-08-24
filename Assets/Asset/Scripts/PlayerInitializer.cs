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
    public Color teamAColor;
    public Color teamBColor;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
   
        //GetComponent<ThirdPersonController>().enabled = info.photonView.IsMine;
        //GetComponent<CharacterController>().enabled = info.photonView.IsMine;

        //GetComponent<PlayerController>().enabled = info.photonView.IsMine;

        if (!GetComponent<PhotonView>().IsMine)
        {
            if (info.photonView.Owner.CustomProperties["Team"].ToString().Equals(PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString()))
            {
                gameObject.layer = 10;
            }
            else
            {
                gameObject.layer = 8;
                gameObject.tag = "Enemy";
                GetComponent<EnemyView>().enabled = true;

                Destroy(GetComponent<ThirdPersonController>());
                Destroy(GetComponent<StarterAssetsInputs>());
                Destroy(GetComponent<CharacterController>());
                Destroy(GetComponent<BasicRigidBodyPush>());
                Destroy(GetComponent<PlayerController>());

                Destroy(GetComponent<PlayerInput>());
                Destroy(GetComponent<BallThrower>());
            }
            playerNameCanvas.GetComponentInChildren<TMP_Text>().color = info.photonView.Owner.CustomProperties["Team"].Equals("A") ? teamAColor : teamBColor;

            playerNameCanvas.SetActive(true);
            playerNameCanvas.GetComponentInChildren<TMP_Text>().text = info.photonView.Owner.NickName;
        }
        else
        {
            playerNameCanvas.SetActive(false);
            FindObjectOfType<CinemachineVirtualCamera>().Follow = playerCamera.transform;
            GetComponent<BallThrower>().cam = Camera.main.transform;
            GetComponent<EnemyView>().enabled = false;
        }

    }
}
