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
    private GameObject bullyObject;
    private MeshRenderer dialogFriend;
    private MeshRenderer dialogBully;
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
    public bool reachedBully = false;
    private bool startEngageBully = false;
    private float delayVictimFall = 0f;
    private bool talkedToBully = false;

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
        bullyObject = GameObject.Find("Bully");
        dest = new Vector3(player.transform.position.x, player.transform.position.y, -3.0f);
        textQuestion = GameObject.Find("Question").GetComponent<Text>();
        opt1 = GameObject.Find("Button1").GetComponent<Button>();
        opt2 = GameObject.Find("Button2").GetComponent<Button>();
        disableMenu();

        friendAnimator = GameObject.Find("Friend").GetComponent<Animator>();
        victimAnimator = GameObject.Find("Bullied").GetComponent<Animator>();
        bullyAnimator = GameObject.Find("Bully").GetComponent<Animator>();

        dialogFriend = GameObject.Find("DialogFriend").GetComponent<MeshRenderer>();
        dialogBully = GameObject.Find("DialogBully").GetComponent<MeshRenderer>();
    }
    
    void Update()
    {
        //dialogFriend.transform.rotation.Set(dialogFriend.transform.rotation.x, Camera.main.transform.rotation.y, dialogFriend.transform.rotation.z, dialogFriend.transform.rotation.w);

        Vector3 rot = Camera.main.transform.rotation.eulerAngles;
        rot.x = 0;
        rot.z = 270;
        rot.y += 95;
        dialogFriend.transform.rotation = Quaternion.Euler(rot);
        dialogBully.transform.rotation = Quaternion.Euler(rot);

        float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
        float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
        Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
        if (Cardboard.SDK.Triggered && !started)
        {            
            startFlag = true;
            started = true;
        }
        if (startFlag || (startEngageBully && !reachedBully))
        {
            player.transform.position = Vector3.Lerp(player.transform.position, dest, speed * Time.deltaTime);
        }
        if (reachedBully && !talkedToBully)
        {
            dialogBully.enabled = true;
            // bullyAnimator.SetInteger("Current State", 3);
            // Rotate the bully to face user
            bullyObject.transform.Rotate(Vector3.down * Time.deltaTime * 100.0f);
            if (bullyObject.transform.eulerAngles.y <= 200)
            {
                talkedToBully = true;
                bullyAnimator.SetInteger("Current State", 0);
                changeQuestion("Do you want to interfere?", "Yes", "No");
                // Make char glow to click on trigger
            }
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
                dialogFriend.enabled = true;
            }
        }
        if (friendRotateBack)
        {
            friendObject.transform.Rotate(Vector3.up * Time.deltaTime * 100.0f);
            friendAnimator.SetInteger("Current State", 1);
            if (friendObject.transform.eulerAngles.y >= 350)
            {
                friendRotateBack = false;
                friendAnimator.SetInteger("Current State", 0);
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
                    engageBully();
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

    void engageBully()
    {
        startEngageBully = true;

        player.transform.position = Vector3.Lerp(player.transform.position, dest, speed * Time.deltaTime);
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

        dialogFriend.enabled = true;
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
            dialogFriend.enabled = false;
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
