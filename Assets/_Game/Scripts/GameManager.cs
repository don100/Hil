using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public enum GameStatusType
{
    Menu,
    Play
}

public class GameManager : MonoBehaviour {

    public static GameStatusType CurrentGameStatus { get; set; }

    private static GameManager me;

    private static int triggermove = Animator.StringToHash("move"); //amento delle prestazioni, maggiore ottimizzazione

    [SerializeField]
    private GameObject playerprefab;

    [SerializeField]
    private Animator shopPanelAnimator;

    private Transform playerstart;
    private GameObject playerGO;
    private WheelJoint2D frontWheelJoint;
    private WheelJoint2D rearWheelJoint;
    private Rigidbody2D frontWheelRB;
    private Rigidbody2D rearWheelRB;
    private Rigidbody2D bodyRB;
    private JointMotor2D frontMotor;
    private JointMotor2D rearMotor;

    private void Awake()
    {
        if(me==null)
        {
            me = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        shopPanelAnimator.SetTrigger(triggermove);
        

        CurrentGameStatus = GameStatusType.Menu;
    }

    public void Play()
    {
        shopPanelAnimator.SetTrigger(triggermove);
        playerstart = GameObject.FindGameObjectWithTag("Respawn").transform;
        playerGO = Instantiate<GameObject>(playerprefab, playerstart.position, playerstart.rotation);
        WheelJoint2D[] tmpWJ = GameObject.FindObjectsOfType<WheelJoint2D>();
        if (tmpWJ[0].name.Contains("(0)"))
        {
            frontWheelJoint = tmpWJ[1];
            rearWheelJoint = tmpWJ[0];
        }
        else
        {
            frontWheelJoint = tmpWJ[0];
            rearWheelJoint = tmpWJ[1];
        }
        frontWheelRB = frontWheelJoint.gameObject.GetComponent<Rigidbody2D>();
        rearWheelRB = rearWheelJoint.gameObject.GetComponent<Rigidbody2D>();
        //bodyRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        SetRearMotorSpeed(0f);
        SetFrontMotorSpeed(0f);
        CurrentGameStatus = GameStatusType.Play;

    }

    private void SetFrontMotorSpeed(float speed)
    {
        frontMotor = frontWheelJoint.motor;
        frontMotor.motorSpeed = speed;
        frontWheelJoint.motor = frontMotor;
    }

    private void SetRearMotorSpeed(float speed)
    {
        rearMotor = rearWheelJoint.motor;
        rearMotor.motorSpeed = speed;
        rearWheelJoint.motor = rearMotor;
    }

    // Update is called once per frame
    void Update () {
        if(CurrentGameStatus==GameStatusType.Play)
        {
            if(CrossPlatformInputManager.GetButton("Up"))
            {
                SetFrontMotorSpeed(frontWheelJoint.motor.motorSpeed + 10f);
                SetRearMotorSpeed(rearWheelJoint.motor.motorSpeed + 10f);
            }
            else if(CrossPlatformInputManager.GetButton("Down"))
            {
                SetFrontMotorSpeed(frontWheelJoint.motor.motorSpeed - 10f);
                SetRearMotorSpeed(rearWheelJoint.motor.motorSpeed - 10f);
            }
            else if (CrossPlatformInputManager.GetButton("Left"))
            {
                bodyRB.AddTorque(1f);
            }
            else if (CrossPlatformInputManager.GetButton("Right"))
            {
                bodyRB.AddTorque(1f);
            }
        }
		
	}
}
