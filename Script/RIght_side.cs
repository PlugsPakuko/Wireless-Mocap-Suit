using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.CompilerServices;

public class Right_side: MonoBehaviour

{
    private string UID = "8fWiuDCo7ROgsqC44nTKAWiRqO23";
    public string MPU_ID;
    private DatabaseReference DBreference;
    float yaw;
    float pitch;
    float roll;

    // Start is called before the first frame update
    void Start()

    {
        Debug.Log("start");
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void ReadIMU()
    {
        DBreference.Child("MPU_READING").Child(UID).Child("MPU" + MPU_ID).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error getting from databse");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string STyaw = snapshot.Child("yaw").Value.ToString();
                string STpitch = snapshot.Child("pitch").Value.ToString();
                string STroll = snapshot.Child("roll").Value.ToString();

                pitch = float.Parse(STpitch);
                roll = float.Parse(STroll);
                yaw = float.Parse(STyaw);

               

              
            }
        });
    }

    // Update is called once per frame
    void Update()
    { 
        ReadIMU();
        transform.rotation= Quaternion.Euler(pitch, yaw, roll);
        Debug.Log("YPR" + MPU_ID + ": " + yaw + "," + pitch + "," + roll);
    }




}
