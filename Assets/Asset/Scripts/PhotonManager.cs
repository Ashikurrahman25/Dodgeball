using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtables = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : MonoBehaviourPunCallbacks
{

    public GameObject roomJoinPopup;
    public GameObject playerListPopup;
    public GameObject loadingPanel;
    public TMP_Text playerCount;
    public TMP_Text roomNameText;
    public TMP_Text countDownText;
    public TMP_InputField roomName;

    public GameObject playerListPrefab;
    public GameObject startButton;
    public GameObject countdown;
    public List<GameObject> playerList;
    public RectTransform playerListParent;

    public int maxPlayer = 6;

    public double startTime;
    double timerIncrementValue;
    public double timer = 60;
    public bool startTimer;
    TimeSpan time;

    void Start()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            loadingPanel.SetActive(true);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.LeaveLobby();
        }
    }
    public override void OnConnectedToMaster()
    {
        loadingPanel.SetActive(false);
        Debug.Log("Connected to master");
    }

    public void JoinLobby()
    {
        if (PhotonNetwork.IsConnected)
        {
            loadingPanel.SetActive(true);
            if (string.IsNullOrEmpty(PhotonNetwork.NickName))
                PhotonNetwork.NickName = "GUEST" + UnityEngine.Random.Range(999, 18000).ToString();

            PhotonNetwork.JoinLobby();
        }

    }

    public override void OnJoinedLobby()
    {
        loadingPanel.SetActive(false);
        roomJoinPopup.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        roomJoinPopup.SetActive(false);
        playerListPopup.SetActive(true);
        loadingPanel.SetActive(false);

        playerCount.text = $"{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        roomNameText.text = $"Room: {PhotonNetwork.CurrentRoom.Name}";

        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            UpdatePlayerList();
            StartCoroutine(ShowCountdown());
        }
        else
        {
            GameObject g = Instantiate(playerListPrefab, playerListParent);
            playerList.Add(g);
            g.GetComponentInChildren<TMP_Text>().text = PhotonNetwork.NickName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!startTimer) return;

        timerIncrementValue = PhotonNetwork.Time - startTime;
        time = TimeSpan.FromSeconds(timer - timerIncrementValue);
        countDownText.text = $"{time.Minutes}: {time.Seconds}";

        if (timerIncrementValue >= timer)
        {
            startTimer = false;

            if (PhotonNetwork.IsMasterClient)
                StartGame();

            Debug.Log(timerIncrementValue);
        }
    }
    public void UpdatePlayerList()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Destroy(playerList[i]);
        }
        playerList.Clear();
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            GameObject g = Instantiate(playerListPrefab, playerListParent);
            playerList.Add(g);
            g.GetComponentInChildren<TMP_Text>().text = PhotonNetwork.PlayerList[i].NickName;
        }
        //playerCount.text = PhotonNetwork.PlayerList[0].NickName;
    }


    public void LoadScene()
    {
        SceneManager.LoadScene("Game");
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (newMasterClient.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
            startTimer = true;
            startButton.SetActive(true);
            countdown.SetActive(true);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
        playerCount.text = $"{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            startButton.SetActive(true);
            countdown.SetActive(true);
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                ExitGames.Client.Photon.Hashtable customeValue = new ExitGames.Client.Photon.Hashtable();
                startTime = PhotonNetwork.Time;
                startTimer = true;
                customeValue.Add("StartTime", startTime);
                PhotonNetwork.CurrentRoom.SetCustomProperties(customeValue);
            }
            else
            {
                startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
                startTimer = true;
            }
        }

    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.IsOpen = false;

        PhotonNetwork.LoadLevel(PlayerPrefs.GetString("Level", PlayerPrefs.GetString("Level", "Level 1")));
    }
    
    public void CreateRoom()
    {
        loadingPanel.SetActive(true);
        if (!string.IsNullOrEmpty(roomName.text))
        {
            RoomOptions newRoom = new RoomOptions()
            {
                MaxPlayers = (byte)maxPlayer

            };
            Hashtables newProps = new Hashtables();
            newProps.Add("SpawnIndex", 0);
            newRoom.CustomRoomProperties = newProps;
            PhotonNetwork.CreateRoom(roomName.text, newRoom);
        }

    }
    public void JoinRoom()
    {
        loadingPanel.SetActive(true);
        if (!string.IsNullOrEmpty(roomName.text))
        {
            RoomOptions room = new RoomOptions();
            room.MaxPlayers = (byte)maxPlayer;
            PhotonNetwork.JoinOrCreateRoom(roomName.text, room,TypedLobby.Default);
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }

    }


    IEnumerator ShowCountdown()
    {
        yield return new WaitForSeconds(1f);
        startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
        startTimer = true;
        countdown.SetActive(true);

    }
}
