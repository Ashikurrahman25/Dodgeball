using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualSync : MonoBehaviour, IPunObservable
{
    Rigidbody player;
    PhotonView photonView;

    Vector3 networkedPosition;
    Quaternion networkedRotation;

    private float distance;
    private float angle;

    public bool synchronizeVelocity = true;
    public bool synchronizeAngularVelocity = true;
    public bool isTeleportEnabled = true;
    public float teleportIfDistanceGreaterThan = 1.0f;

    private void Awake()
    {
        player = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        networkedPosition = new Vector3();
        networkedRotation = new Quaternion();
    }



    private void Update()
    {

        if (!photonView.IsMine)
        {
            player.position = Vector3.MoveTowards(player.position, networkedPosition, Time.fixedDeltaTime);
            player.rotation = Quaternion.RotateTowards(player.rotation, networkedRotation, angle * Time.fixedDeltaTime);
        }

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            //Then, photonView is mine and I am the one who controls this player.
            //should send position, velocity etc. data to the other players
            stream.SendNext(player.position);
            stream.SendNext(player.rotation);
            stream.SendNext(player.useGravity);

            if (synchronizeVelocity)
            {
                stream.SendNext(player.velocity);
            }

            if (synchronizeAngularVelocity)
            {
                stream.SendNext(player.angularVelocity);
            }
        }
        else
        {
            //Called on my player gameobject that exists in remote player's game
            networkedPosition = (Vector3)stream.ReceiveNext();
            networkedRotation = (Quaternion)stream.ReceiveNext();
            player.useGravity = (bool)stream.ReceiveNext();

            if (isTeleportEnabled)
            {
                if (Vector3.Distance(player.position, networkedPosition) > teleportIfDistanceGreaterThan)
                {
                    player.position = networkedPosition;
                }
            }

            if (synchronizeVelocity || synchronizeAngularVelocity)
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if (synchronizeVelocity)
                {
                    player.velocity = (Vector3)stream.ReceiveNext();
                    networkedPosition += player.velocity * lag;
                    distance = Vector3.Distance(player.position, networkedPosition);
                }

                if (synchronizeAngularVelocity)
                {
                    player.angularVelocity = (Vector3)stream.ReceiveNext();

                    networkedRotation = Quaternion.Euler(player.angularVelocity * lag) * networkedRotation;

                    angle = Quaternion.Angle(player.rotation, networkedRotation);
                }
            }
        }
    }
}
