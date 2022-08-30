using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallView : MonoBehaviour
{

    public BallType ballType;
    public bool canDamageA = false;
    public bool canDamageB = false;
    public bool canClaim = false;
    public bool doDamage = false;

    private void OnEnable()
    {
        GameManager.instance.balls.Add(this);
    }

    private void OnCollisionEnter(Collision collision)
    {

        GetComponent<PhotonView>().RPC("MakeClaimable", RpcTarget.All);
        if(collision.gameObject.layer != 9 && collision.gameObject.layer != 8)
        {
            GetComponent<Rigidbody>().useGravity = true;
        }
    }
    //private void (Collider other)
    //{
        
    //}

    public void DamagePlayer(EnemyView enemy, string team, int id)
    {
        enemy.Damage(team, id, ballType);
    }

    public void ClaimRPC(string team)
    {
        if (!canClaim) return;

        GetComponent<PhotonView>().RPC("ClaimBall", RpcTarget.All, team);
    } 
    
    public void ActivateRPC()
    {
        GetComponent<PhotonView>().RPC("ActivateBall", RpcTarget.All);
    }

    [PunRPC]
    public void MakeClaimable()
    {
        canClaim = true;
        doDamage = false;
        canDamageA = false;
        canDamageB = false;
    }

    [PunRPC]
    public void ClaimBall(string team)
    {
        if(team.Equals("A"))
        {
            canDamageA = false;
            canDamageB = true;
        }
        else
        {
            canDamageA = true;
            canDamageB = false;
        }
        canClaim = false;
        transform.GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
    }

    [PunRPC]
    public void ActivateBall()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        doDamage = true;

    }

    public enum BallType
    {
        Normal = 0,
        Ice = 1,
        Fire = 2,
        Rainbow = 3,
        Green = 4
    }

}
