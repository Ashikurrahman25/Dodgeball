using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Transform targetMarker;

    public void ThrowBall()
    {
        GetComponent<BallThrower>().Throw();
    }
    
}
