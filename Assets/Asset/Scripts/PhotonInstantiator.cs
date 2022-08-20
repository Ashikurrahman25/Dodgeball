using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonInstantiator : MonoBehaviour
{

    public List<Transform> instantiatePlayerA = new List<Transform>();
    public List<Transform> instantiatePlayerB = new List<Transform>();

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
