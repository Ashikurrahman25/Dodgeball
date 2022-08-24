using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonInstantiator : MonoBehaviour
{
    public int ballsToInstantiate;
    public List<string> balls = new List<string>();

    public List<Transform> instantiatePlayerA = new List<Transform>();
    public List<Transform> instantiatePlayerB = new List<Transform>();

    public List<Transform> instantiateBallA = new List<Transform>();
    public List<Transform> instantiateBallB = new List<Transform>();



    // Start is called before the first frame update
    void Start()
    {
        InstantiatePlayer();
    }

    public void InstantiatePlayer()
    {
        Vector3 position = instantiatePlayerA[0].position;
        Quaternion rotation = instantiatePlayerA[0].rotation;
        string player = "PlayerKoata";
            

        if (PhotonNetwork.LocalPlayer.CustomProperties["Team"].Equals("A"))
        {
             position = instantiatePlayerA[0].position;
             rotation = instantiatePlayerA[0].rotation;

            player = "PlayerKoata";
        }
        else
        {
            position = instantiatePlayerB[0].position;
            rotation = instantiatePlayerB[0].rotation;

            player = "PlayerGoat";
        }


        PhotonNetwork.Instantiate(player, position, rotation);

        if (PhotonNetwork.IsMasterClient)
        {
            InstantiateBall();
        }
    }

    public void InstantiateBall()
    {
        for (int i = 0; i < ballsToInstantiate; i++)
        {
            PhotonNetwork.Instantiate($"Balls/{balls[Random.Range(0, balls.Count)]}", instantiateBallA[i].position, Quaternion.identity);
            PhotonNetwork.Instantiate($"Balls/{balls[Random.Range(0, balls.Count)]}", instantiateBallB[i].position, Quaternion.identity);
        }
    }
}
