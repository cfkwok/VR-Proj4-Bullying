using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SOneMovement : MonoBehaviour {
	public GameObject player;
    public float speed = 0.1f;

    //private CardboardHead head = null;
    private GameObject head;
    private Animator friendAnimator;
    private Animator victimAnimator;
    private CanvasGroup panelGroup;
    private Vector3 dest = new Vector3(0, 0, 0);
    private bool started = false;
    private bool startFlag = false;
    private bool destReached = false;
    private bool showMenu = false;
    private bool fadeMenu = false;
    private Text textQuestion;
    private Button opt1;
    private Button opt2;
    float smooth = 2.0F;
    float tiltAngle = 30.0F;

    // Use this for initialization
    void Start () {
		//head = Camera.main.GetComponent<StereoController>().Head;
        head = GameObject.Find("CardboardMain/Head");
        panelGroup = GameObject.Find("QuestionMenu").GetComponent<CanvasGroup>();
        dest = new Vector3(player.transform.position.x, player.transform.position.y, -3.0f);
        textQuestion = GameObject.Find("Question").GetComponent<Text>();
        opt1 = GameObject.Find("Button1").GetComponent<Button>();
        opt2 = GameObject.Find("Button2").GetComponent<Button>();
        textQuestion.text = "Is Craig a scrub?";
        opt1.GetComponentInChildren<Text>().text = "Yes";
        opt2.GetComponentInChildren<Text>().text = "Of course";
        disableMenu();

        friendAnimator = GameObject.Find("Friend").GetComponent<Animator>();
        victimAnimator = GameObject.Find("Bullied").GetComponent<Animator>();
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
        
           
    }

    void enableMenu()
    {
        panelGroup.alpha += 0.5f * Time.deltaTime;
        opt1.interactable = true;
        opt2.interactable = true;
        if (panelGroup.alpha == 1f)
        {
            showMenu = false;
        }
    }

    void disableMenu()
    {
        panelGroup.alpha -= 0.5f * Time.deltaTime;
        opt1.interactable = false;
        opt2.interactable = false;
        if (panelGroup.alpha == 0f)
        {
            fadeMenu = false;
        }
    }

    /*
    // For delayed script execution
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        victimAnimator.SetInteger("Current State", 2);
    }
    */

    void OnTriggerEnter(Collider other)
    {
        // Player reached trigger dest, can do animation here
        startFlag = false;
        destReached = true;
        friendAnimator.SetInteger("Current State", 1);
        victimAnimator.SetInteger("Current State", 1);
        // StartCoroutine(ExecuteAfterTime(5));

    }

    public void OnGazeClick()
    {
        if (destReached && !fadeMenu)
        {
            print("Perform actions");
            showMenu = true;
        }
    }

    public void onFight()
    {
        print("Show fight scene");
        if (!showMenu)
        {
            friendAnimator.SetInteger("Current State", 2);
            fadeMenu = true;
            // StartCoroutine(ExecuteAfterTime(5));
        }
    }

    public void onNotFight()
    {
        print("Continue bully scene");
        if (!showMenu)
        {
            fadeMenu = true;
        }
    }
}
