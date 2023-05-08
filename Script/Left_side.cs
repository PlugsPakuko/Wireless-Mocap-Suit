using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Left_side : MonoBehaviour
{
    // Start is called before the first frame update
    private string UID = "8fWiuDCo7ROgsqC44nTKAWiRqO23";
    public string MPU_ID;
    private DatabaseReference DBreference;
    float yaw;
    float pitch;
    float roll;
    void Start()
    {
        Debug.Log("start");
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Update is called once per frame
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
        transform.rotation = Quaternion.Euler(-1*pitch,yaw,-1*roll);
        Debug.Log("YPR"+MPU_ID+": " + yaw + "," + pitch + "," + roll);
    }
}
