using Photon.Pun;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviourPunCallbacks
{
    public static UIController instance;
    public GameObject disqualifyPanel;
    public GameObject freezePanel;
    public GameObject dizzyPanel;
    public GameObject gameWonPanel;
    public GameObject gameLostPanel;
    public TMP_Text freezeText;
    public TMP_Text dizzyText;
    public float freezeTimer;
    public float dizzyTimer;
    public float minTimeToFreeze = 10f;
    public float minTimeToDizzy = 10f;
    public bool startFreeze;
    public bool showedDisInfo;
    public bool startDizzy;
    int transformIndex = 0;

    public TMP_Text[] ballCount;

    public List<Transform> disqualifiedTransformA = new List<Transform>();
    public List<Transform> disqualifiedTransformB = new List<Transform>();
    Action acFreeze;
    Action acDizzy;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        ResetBallCount();

    }

    private void Update()
    {
        if (startFreeze)
        {
            if(freezeTimer >= 0)
            {
                freezeTimer -= Time.deltaTime;
                freezeText.text = $"FROZEN FOR {freezeTimer.ToString("00:00")}";
            }
            else
            {
                startFreeze = false;
                freezePanel.SetActive(false);
                acFreeze.Invoke();
            }
        }
        else if (startDizzy)
        {
            if (dizzyTimer >= 0)
            {
                dizzyTimer -= Time.deltaTime;
                dizzyText.text = $"DIZZY FOR {dizzyTimer.ToString("00:00")}";
            }
            else
            {
                startDizzy = false;
                dizzyPanel.SetActive(false);
                acDizzy.Invoke();
            }
        }

    }

    public void ShowDisqualifey()
    {
        disqualifyPanel.SetActive(true);
        showedDisInfo = true;
    }

    public void FreezePlayer(Action action)
    {
        startFreeze = true;
        freezeTimer = minTimeToFreeze;
        freezePanel.SetActive(true);
        freezeText.text = $"FROZEN FOR {freezeTimer.ToString("00:00")}";
        acFreeze = action;
    }

    public void MakePlayerDizzy(Action action)
    {
        startDizzy = true;
        dizzyTimer = minTimeToDizzy;
        dizzyPanel.SetActive(true);
        dizzyText.text = $"DIZZY FOR {freezeTimer.ToString("00:00")}";
        acDizzy = action;
    }

    public Transform DisqualifyTransform()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["Team"].Equals("A"))
        {
            transformIndex = UnityEngine.Random.Range(0, disqualifiedTransformA.Count);
            return disqualifiedTransformA[transformIndex];
        }
        else
        {
            transformIndex = UnityEngine.Random.Range(0, disqualifiedTransformB.Count);
            return disqualifiedTransformB[transformIndex];
        }
      
    }

    public void RemoveTransform(string team)
    {
        if (team.Equals("A"))
        {
            disqualifiedTransformA.RemoveAt(transformIndex);
        }
        else
        {
            disqualifiedTransformB.RemoveAt(transformIndex);
        }
        
    }

    public void LoadMenu()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ShowGameOver(string team)
    {
        string playerTeam = PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString();

        if (playerTeam.Equals(team))
            gameLostPanel.SetActive(true);
        else
            gameWonPanel.SetActive(true);

        PhotonInstantiator.instance.localPlayer.GetComponent<StarterAssetsInputs>().cursorLocked = false;

    }

    public void ShowBallCount(BallView.BallType type)
    {
        ResetBallCount();
        switch (type)
        {
            case BallView.BallType.Normal:
                ballCount[0].text = "x1";
                break;
            case BallView.BallType.Ice:
                ballCount[1].text = "x1";
                break;
            case BallView.BallType.Fire:
                ballCount[2].text = "x1";
                break;
            case BallView.BallType.Rainbow:
                ballCount[3].text = "x1";
                break;
            case BallView.BallType.Green:
                ballCount[4].text = "x1";
                break;
            default:
                break;
        }
    }

    public void ResetBallCount()
    {
        for (int i = 0; i < ballCount.Length; i++)
        {
            ballCount[i].text = "x0";
        }
    }
}
