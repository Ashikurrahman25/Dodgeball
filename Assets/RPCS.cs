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
    public void AddForce(Rigidbody projectileRb, GameObject projectile, float throwForce)
    {
        projectileRb.velocity = projectile.transform.forward * throwForce;

    }
}
