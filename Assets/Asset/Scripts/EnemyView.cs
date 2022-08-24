using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    public float health;
    public Animator animator;
    public bool disqualified = false;

    public void Damage(string team, int viewID, BallView.BallType ballType)
    {
        if (PhotonView.Find(viewID).Owner.CustomProperties["Team"].Equals(team))
        {
            switch (ballType)
            {
                case BallView.BallType.Normal:
                    PhotonView.Find(viewID).RPC("DoDisQualify", RpcTarget.All);
                    break;
                case BallView.BallType.Ice:
                    PhotonView.Find(viewID).RPC("DoFreeze", RpcTarget.All);
                    break;
                case BallView.BallType.Fire:
                    PhotonView.Find(viewID).RPC("DoDisQualify", RpcTarget.All);
                    break;
                case BallView.BallType.Rainbow:
                    PhotonView.Find(viewID).RPC("DoDizzy", RpcTarget.All);
                    break;
                case BallView.BallType.Green:
                    break;
                default:
                    break;
            }

            //GetComponent<EnemyView>().DoDisQualify();
        }
        else
            Debug.Log("Can't Disqualify");
    }

    [PunRPC]
    public void DoDisQualify()
    {

        if (GetComponent<PlayerController>())
            GetComponent<PlayerController>().disqualified = true;

        disqualified = true;
        Debug.Log("Disqualify Player ");
    }

    [PunRPC]
    public void DoFreeze()
    {
        if (GetComponent<PlayerController>())
            GetComponent<PlayerController>().freeze = true;

        Debug.Log("Freezing Player ");
    }

    [PunRPC]
    public void DoDizzy()
    {
        if (GetComponent<PlayerController>())
            GetComponent<PlayerController>().dizzy = true;

        Debug.Log("Freezing Player ");
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("AAA " + other.name + ", " + other.gameObject.layer);

        if(other.gameObject.layer == 6)
        {
            BallView ball = other.gameObject.GetComponent<BallView>();

            if (!ball.doDamage && GetComponent<PhotonView>().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                BallThrower ballView = GetComponent<BallThrower>();

                if (ballView.totalThrows > 0 || !ball.canClaim) return;

                ballView.totalThrows = 1;
                ballView.projectile = other.gameObject;

                string team = PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString();
                ball.ClaimRPC(team);
                Debug.Log("Claiming Ball");

            }
            else
            {
                string team =  ball.canDamageA ? "A" : "B";
                ball.DamagePlayer(this, team, GetComponent<PhotonView>().ViewID);

                Debug.Log("Damaging");
            }
        }
    }
}
