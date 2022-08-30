using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<BallView> balls = new List<BallView>();
    public List<PlayerController> players = new List<PlayerController>();
    public List<EnemyView> enemies = new List<EnemyView>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }


    public bool AllDisqualified()
    {
        int count = 0;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].disqualified) count++;
        }

        if (count == players.Count)
            return true;
        else
            return false;
    }

    public void ReGeneratePlayer()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].disqualified)
                players[i].GetComponent<PhotonView>().RPC("UnDisqualify", RpcTarget.All);
        }
    }


    public void CalculateDistance(string team, Transform playerTransform, BallThrower ballThrower)
    {
        Debug.Log(team);

        if (team.Equals("A"))
        {
            for (int i = 0; i < balls.Count; i++)
            {
                if (balls[i].doDamage && balls[i].canDamageA)
                {
                    float dist = Vector3.Distance(balls[i].transform.position, playerTransform.position);
                    Debug.Log(dist);
                    if ( dist >= 0 && dist <= 1.6f)
                    {
                        Debug.Log("Caught the ball");
                        ballThrower.ClaimBall(balls[i]);
                        ReGeneratePlayer();
                    }
                }
            }
        }
        else if (team.Equals("B"))
        {
            for (int i = 0; i < balls.Count; i++)
            {
                if (balls[i].doDamage && balls[i].canDamageB)
                {
                    float dist = Vector3.Distance(balls[i].transform.position, playerTransform.position);
                    Debug.Log(dist);
                    if (dist >= 0 && dist <= 1.6f)
                    {
                        Debug.Log("Caught the ball");

                        ballThrower.ClaimBall(balls[i]);
                        ReGeneratePlayer();
                    }
                }
            }
        }
    }
}
