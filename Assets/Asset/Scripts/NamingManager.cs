using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using Photon.Pun;

public class NamingManager : MonoBehaviour
{
    public TMP_InputField userNameInput;
    public TMP_Text userName;
    public GameObject namePopup;
    void Start()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("UserName","")))
        {
            userName.text = PlayerPrefs.GetString("UserName", "");
            userNameInput.text = PlayerPrefs.GetString("UserName", "");
            //PhotonNetwork.NickName = userName.text;
            namePopup.SetActive(false);

        }
        else
        {
            namePopup.SetActive(true);
        }
    }

    public void DONE()
    {
        if (!string.IsNullOrEmpty(userNameInput.text))
        {
            userName.text = userNameInput.text;
            //PhotonNetwork.NickName = userNameInput.text;
            PlayerPrefs.SetString("UserName", userNameInput.text);
        }
        else
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("UserName", "")))
            {
                userName.text = "GUEST" + Random.Range(999, 18000).ToString();
                //PhotonNetwork.NickName = userName.text;
                PlayerPrefs.SetString("UserName", userName.text);
            }
            else
            {
                userName.text = PlayerPrefs.GetString("UserName", "");
                //PhotonNetwork.NickName = PlayerPrefs.GetString("UserName", "");
            }
        }

        namePopup.GetComponent<PanelAnim>().DestroySelf();
    
    }
}
