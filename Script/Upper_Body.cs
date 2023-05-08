using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.CompilerServices;

public class Upper_Body : MonoBehaviour

{
    private string UID = "8fWiuDCo7ROgsqC44nTKAWiRqO23";
    public string MPU_ID;
    private DatabaseReference DBreference;
    float yaw;
    float pitch;
    float roll;
    float first_yaw;
    float first_pitch;
    float first_roll;
    int i = 0;

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

    void OptimizeRotation()
    {
        //yaw
        if (first_yaw < 0)
        {

        }
        //pitch
        //roll
    }

    // Update is called once per frame
    void Update()
    {
        while (i < 3)
        {
            if (i == 2)
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


                        first_pitch = float.Parse(STpitch);
                        first_roll = float.Parse(STroll);
                        first_yaw = float.Parse(STyaw);
                    

                    }
                });
                i += 1;
                Debug.Log(i);
                break;
            }
            else
            {
                ReadIMU();
                i+=1;
            }

        }
        OptimizeRotation();
        ReadIMU();
        transform.rotation = Quaternion.Euler(-1*pitch,-1*yaw,roll);
        Debug.Log("YPR"+ MPU_ID + ": " + yaw + "," + pitch + "," + roll);
    }




}
