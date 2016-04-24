using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SOneMovement : MonoBehaviour {
	public GameObject player;
    public float speed = 0.1f;

    //private CardboardHead head = null;
    private GameObject head;
    private GameObject friendObject;
    private Animator friendAnimator;
    private Animator victimAnimator;
    private Animator bullyAnimator;
    private CanvasGroup panelGroup;
    private Vector3 dest = new Vector3(0, 0, 0);
    private bool started = false;
    private bool startFlag = false;
    private bool destReached = false;
    private bool showMenu = false;
    private bool fadeMenu = false;
    private bool friendRotate = false;
    private bool question2 = false;
    private bool friendAgree = false;
    private bool victimDowned = false;
    private float delayVictimFall = 0f;

    private Text textQuestion;
    private Button opt1;
    private Button opt2;
    float smooth = 2.0F;
    float tiltAngle = 30.0F;
    private bool friendRotateBack;
    private bool question1;

    // Use this for initialization
    void Start () {
		//head = Camera.main.GetComponent<StereoController>().Head;
        head = GameObject.Find("CardboardMain/Head");
        panelGroup = GameObject.Find("QuestionMenu").GetComponent<CanvasGroup>();
        friendObject = GameObject.Find("Friend");
        dest = new Vector3(player.transform.position.x, player.transform.position.y, -3.0f);
        textQuestion = GameObject.Find("Question").GetComponent<Text>();
        opt1 = GameObject.Find("Button1").GetComponent<Button>();
        opt2 = GameObject.Find("Button2").GetComponent<Button>();
        disableMenu();

        friendAnimator = GameObject.Find("Friend").GetComponent<Animator>();
        victimAnimator = GameObject.Find("Bullied").GetComponent<Animator>();
        bullyAnimator = GameObject.Find("Bully").GetComponent<Animator>();
    }
    
    void Update()
    {
        float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
        float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
        Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
        if (Cardboard.SDK.Triggered && !started)
        {            
            startFlag = true;
            started = true;
        }
        if (startFlag)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, dest, speed * Time.deltaTime);
        }
        if (friendRotate)
        {
            friendObject.transform.Rotate(Vector3.down * Time.deltaTime * 100.0f);
            friendAnimator.SetInteger("Current State", 1);
            if (friendObject.transform.eulerAngles.y <= 220)
            {
                friendRotate = false;
                question2 = true;
                friendAnimator.SetInteger("Current State", 2);
                changeQuestion("Do you want to interfere?", "Yes", "No");
                // Make char glow to click on trigger
            }
        }
        if (friendRotateBack)
        {
            friendObject.transform.Rotate(Vector3.up * Time.deltaTime * 100.0f);
            friendAnimator.SetInteger("Current State", 1);
            if (friendObject.transform.eulerAngles.y >= 350)
            {
                friendRotateBack = false;
            }
        }
        if (friendAgree)
        {
            if (friendAnimator.GetCurrentAnimatorStateInfo(0).IsName("agreeing"))
            {
                if (friendAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                {
                    
                    friendRotateBack = true;
                    friendAgree = false;
                }
            }
        }
        if (destReached)
        {
            if (victimAnimator.GetCurrentAnimatorStateInfo(0).IsName("being_strangled"))
            {
                if (victimAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                {
                    victimAnimator.SetInteger("Current State", 2);
                }
            }
        }
        if (showMenu && !fadeMenu)
        {
            enableMenu();
        }
        if (fadeMenu && !showMenu)
        {
            disableMenu();
        }

        if (victimDowned)
        {
            victimAnimator.SetInteger("Current State", 2);
            victimDowned = false;
            /*
            if (victimAnimator.GetCurrentAnimatorStateInfo(0).IsName("stumble_backwards"))
            {
                if (victimAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                {
                    victimAnimator.SetInteger("Current State", 3);
                    victimDowned = false;
                }
            }
            */
        }
        
           
    }

    void enableMenu()
    {
        panelGroup.alpha += 1f * Time.deltaTime;
        opt1.interactable = true;
        opt2.interactable = true;
        if (panelGroup.alpha == 1f)
        {
            showMenu = false;
        }
    }

    void disableMenu()
    {
        panelGroup.alpha -= 1f * Time.deltaTime;
        opt1.interactable = false;
        opt2.interactable = false;
        if (panelGroup.alpha == 0f)
        {
            fadeMenu = false;
        }
    }

    
    // For delayed script execution
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        victimDowned = true;
        
    }
    

    void OnTriggerEnter(Collider other)
    {
        // Player reached trigger dest, can do animation here
        startFlag = false;
        destReached = true;
        question1 = true;

        bullyAnimator.SetInteger("Current State", 1);
        victimAnimator.SetInteger("Current State", 1);
        // StartCoroutine(ExecuteAfterTime(5));

    }

    public void changeQuestion(string question, string btn1, string btn2)
    {
        textQuestion.text = question;
        opt1.GetComponentInChildren<Text>().text = btn1;
        opt2.GetComponentInChildren<Text>().text = btn2;
    }

    public void OnGazeClick()
    {
        if (destReached && !fadeMenu)
        {
            print("Perform actions");
            showMenu = true;
        }
    }


    // Question X Choice 1
    public void onPositive()
    {        
        if (question1 && !showMenu)
        {
            print("Show friend stopping you");
            
            fadeMenu = true;
            friendRotate = true;
            question1 = false;
            
            // StartCoroutine(ExecuteAfterTime(5));
        }
        if (question2 && !showMenu)
        {
            print("Show friend agreeing with you");
            bullyAnimator.SetInteger("Current State", 2);

            StartCoroutine(ExecuteAfterTime(0.5f));
            
            friendAnimator.SetInteger("Current State", 3);
            fadeMenu = true;
            friendAgree = true;        
            // StartCoroutine(ExecuteAfterTime(5));
        }
    }
    // Question X Choice 2
    public void onNegative()
    {        
        if (question1 && !showMenu)
        {
            print("Continue bully scene");
            fadeMenu = true;
            question1 = false;
        }
    }
}
