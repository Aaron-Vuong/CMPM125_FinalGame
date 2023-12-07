using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    // only one ControllerManager per scene
    private static ControllerManager _instance;

    public static ControllerManager Instance { get { return _instance; }}

    private void Awake(){
        if(_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public enum ControllerStates{
        _3DFPGame,
        _2DGame
    }

    public ControllerStates controllerState;

    // enables 2D game dev/debug
    public bool _2DDev;

    public RenderTexture screen;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        if(_2DDev){
            controllerState = ControllerStates._2DGame;
            return;
        }
        controllerState = ControllerStates._3DFPGame;

    }

    // Update is called once per frame
    void Update()
    {
        switch (controllerState)
        {
            case ControllerStates._3DFPGame:
                //Debug.Log("3D control");
                if(FirstPersonPlayerController.Instance)
                    FirstPersonPlayerController.Instance.ReceiveMovementInput();
                break;

            case ControllerStates._2DGame:
                //Debug.Log("2D control");
                if(MiniGamePlayerController.Instance)
                    MiniGamePlayerController.Instance.ReceiveMovementInput();
                break;
        }
    }

    public void setControllerState(ControllerStates controllerState){
        this.controllerState = controllerState;
    }

}
