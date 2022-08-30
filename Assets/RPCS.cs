using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCS : MonoBehaviour
{
    [PunRPC]
    public void Remove(string team)
    {
        UIController.instance.RemoveTransform(team);
    }

    [PunRPC]
    public void GameOver(string team)
    {
        UIController.instance.ShowGameOver(team);
    }

    [PunRPC]
    public void UnDisqualify()
    {
        GetComponent<PlayerController>().disqualified = false;
        GetComponent<PlayerController>().EnableOrDisablePlayer(true);
        string team = PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString();
        transform.position =  PhotonInstantiator.instance.GetRandom(team).position;
    }

    [PunRPC]
    public void AddForce(Rigidbody projectileRb, GameObject projectile, float throwForce)
    {
        projectileRb.velocity = projectile.transform.forward * throwForce;

    }

    
}
