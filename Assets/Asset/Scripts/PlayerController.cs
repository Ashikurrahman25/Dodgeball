using GG.Infrastructure.Utils.Swipe;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    public Animator anim;





    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Movement();

    }

    void Movement()
    {

        if (_controller.velocity != Vector3.zero)
        {
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
        }
    }

}