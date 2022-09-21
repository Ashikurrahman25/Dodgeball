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

    public GameObject localPlayer;

    public static PhotonInstantiator instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        InstantiatePlayer();
    }

    public Transform GetRandom(string team)
    {
        Transform _transform = null;
        
        if(team.Equals("A"))
            _transform = instantiatePlayerA[UnityEngine.Random.Range(0, instantiatePlayerA.Count)];
        else if (team.Equals("B"))
            _transform = instantiatePlayerB[UnityEngine.Random.Range(0, instantiatePlayerB.Count)];

        return _transform;
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
             //rotation = new Quaternion(0f, 90f, 0f, 0f);

            player = "PlayerKoata";
        }
        else
        {
            position = instantiatePlayerB[0].position;
            rotation = new Quaternion(0f, -90f, 0f, 0f);

            player = "PlayerGoat";
        }


        localPlayer = PhotonNetwork.Instantiate(player, position, rotation);

        if (PhotonNetwork.IsMasterClient)
        {
            InstantiateBall();
        }
    }

    public void InstantiateBall()
    {
        for (int i = 0; i < ballsToInstantiate; i++)
        {
            GameObject ballA = PhotonNetwork.Instantiate($"Balls/{balls[Random.Range(0, balls.Count)]}", instantiateBallA[i].position, Quaternion.identity);
            GameObject ballB = PhotonNetwork.Instantiate($"Balls/{balls[Random.Range(0, balls.Count)]}", instantiateBallB[i].position, Quaternion.identity);
        }
    }
}
