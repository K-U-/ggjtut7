using UnityEngine;
using System.Collections;

public class SampleGUI : MonoBehaviour {

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical();
            {
                MoveCommandSample sampleCommand = new MoveCommandSample();


                if (GUILayout.Button("Right 1"))
                {
                    sampleCommand.target = "1";
                    sampleCommand.offset = 1f;
                    PhotonRPCModel model = new PhotonRPCModel();
                    model.senderId = "AAA";
                    model.command = PhotonRPCCommand.Move;
                    model.message = JsonUtility.ToJson(sampleCommand);
                    GetComponent<PhotonRPCHandler>().PostRPC(model);
                }

                if (GUILayout.Button("Right 2"))
                {
                    sampleCommand.target = "2";
                    sampleCommand.offset = 1f;
                    PhotonRPCModel model = new PhotonRPCModel();
                    model.senderId = "AAA";
                    model.command = PhotonRPCCommand.Move;
                    model.message = JsonUtility.ToJson(sampleCommand);
                    GetComponent<PhotonRPCHandler>().PostRPC(model);
                }

                if (GUILayout.Button("Right 3"))
                {
                    sampleCommand.target = "3";
                    sampleCommand.offset = 1f;
                    PhotonRPCModel model = new PhotonRPCModel();
                    model.senderId = "AAA";
                    model.command = PhotonRPCCommand.Move;
                    model.message = JsonUtility.ToJson(sampleCommand);
                    GetComponent<PhotonRPCHandler>().PostRPC(model);
                }

            } GUILayout.EndVertical();
            GUILayout.BeginVertical();
            {
                KillCommandSample sampleCommand = new KillCommandSample();
                if (GUILayout.Button("Kill 1"))
                {
                    sampleCommand.target = "1";
                    PhotonRPCModel model = new PhotonRPCModel();
                    model.senderId = "AAA";
                    model.command = PhotonRPCCommand.Kill;
                    model.message = JsonUtility.ToJson(sampleCommand);
                    GetComponent<PhotonRPCHandler>().PostRPC(model);
                }

                if (GUILayout.Button("Kill 2"))
                {
                    sampleCommand.target = "2";
                    PhotonRPCModel model = new PhotonRPCModel();
                    model.senderId = "AAA";
                    model.command = PhotonRPCCommand.Kill;
                    model.message = JsonUtility.ToJson(sampleCommand);
                    GetComponent<PhotonRPCHandler>().PostRPC(model);
                }

                if (GUILayout.Button("Kill 3"))
                {
                    sampleCommand.target = "3";
                    PhotonRPCModel model = new PhotonRPCModel();
                    model.senderId = "AAA";
                    model.command = PhotonRPCCommand.Kill;
                    model.message = JsonUtility.ToJson(sampleCommand);
                    GetComponent<PhotonRPCHandler>().PostRPC(model);
                }
            } GUILayout.EndVertical();
        } GUILayout.EndHorizontal();
    }
}
