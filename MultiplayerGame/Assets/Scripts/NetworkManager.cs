using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.PunBehaviour
{
    private const string roomName = "RoomName";
    private RoomInfo[] roomsList;
    public List<PhotonPlayer> currentPlayersInRoom = new List<PhotonPlayer>();

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        //Ruudun vasemmassa ylänurkassa tarkempaa tietoa yhteyden tilasta
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

        //Jos ei olla missään huonessa, eli lobbyssa, näytetään nappuloita huoneista
        if (PhotonNetwork.room == null)
        {
            //Ei olla huoneessa, tehdään huoneenluomisnappula
            if(GUI.Button(new Rect(100, 100, 250, 100), "Start Server (Create Room)"))
            {
                //Luodaan jokaiselle huoneelle satunnainen nimi
                PhotonNetwork.CreateRoom(roomName + System.Guid.NewGuid().ToString("N"));
            }

            //Huoneisiin liittyminen
            if(roomsList != null)
            {
                for(int i = 0; i < roomsList.Length; i++)
                {
                    if (GUI.Button(new Rect(100, 250 +(110*i), 250, 100), "Join " + roomsList[i].Name + 
                        "\n\nCount: " + roomsList[i].PlayerCount))
                    {
                        PhotonNetwork.JoinRoom(roomsList[i].Name);
                    }
                }
            }
        }
    }

    public override void OnConnectedToPhoton()
    {
        Debug.Log("Yhteys Photoniin");
    }


    public override void OnJoinedLobby()
    {
        Debug.Log("Tultiin Lobbyyn");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Masteryhteys");
    }

    public override void OnReceivedRoomListUpdate()
    {
        //Jos huonelistaus päivittyy palvelimella, tämä metodi ajetaan.
        //Jos siis joku tekee huoneen tai joku huone menee tyhjäksi ja poistuu olemasta.
        roomsList = PhotonNetwork.GetRoomList();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Huone Tehty");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("PlayerBox", new Vector3(0, 0.5f, 0), Quaternion.identity, 0);
        Debug.Log("Tultiin Huoneeseen");
    }

}
