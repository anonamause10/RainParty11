using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameSetupController : MonoBehaviourPun
{
    Dictionary<int, string> characterDict;
    // Start is called before the first frame update
    void Start()
    {
        characterDict = new Dictionary<int, string>();
        characterDict.Add(0,"TT_demo_female 2");
        characterDict.Add(1,"TT_demo_female");
        characterDict.Add(2,"TT_demo_male_A 2");
        characterDict.Add(3,"TT_demo_male_A");
        characterDict.Add(4,"TT_demo_male_B 2");
        characterDict.Add(5,"TT_demo_male_B");
        characterDict.Add(6,"TT_demo_police 2");
        characterDict.Add(7,"TT_demo_police");
        characterDict.Add(8,"TT_demo_zombie 2");
        characterDict.Add(9,"TT_demo_zombie");
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        string num = PhotonNetwork.NickName.Split('|')[1];
        if(num == ""){
            num = "0";
        }
        int randnum = int.Parse(num);
        PhotonNetwork.NickName = PhotonNetwork.NickName.Split('|')[0];
        print(randnum);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", characterDict[randnum]), Vector3.zero, Quaternion.Euler(0,180,0));
    }
}
