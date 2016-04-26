using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class STBMovement : MonoBehaviour {

    public float speed;
    public GameObject player;
    private Vector3 goHere;
    private CanvasGroup panelGroup;
    private GameObject bully;
    private Animator bullyAnimator;
    private Button opt1;
    private Button opt2;
    private Text decision;
    private Quaternion headPos;
    private float delayTime;
    private bool showMenu;
    private bool fadeMenu;
    private bool run;
    private bool bullyRun;
    private bool bullyWalk;
    private bool question1;
    private bool question2;
    private bool question3;
    public GameObject runDestination;
    void Start () {
        goHere = player.transform.position;
        panelGroup = GameObject.Find("QuestionMenu").GetComponent<CanvasGroup>();
        decision = panelGroup.transform.FindChild("Question").GetComponent<Text>();
        opt1 = GameObject.Find("Button1").GetComponent<Button>();
        opt2 = GameObject.Find("Button2").GetComponent<Button>();
        panelGroup.alpha = 0;
        bully = GameObject.Find("Bully");
        bullyAnimator = bully.GetComponent<Animator>();
        bullyWalk = true;
        fadeMenu = false;
    }

    void Update() {
        player.transform.position = Vector3.MoveTowards(player.transform.position, goHere, speed * Time.deltaTime);
        if (bullyWalk)
        {
            bully.transform.position = Vector3.MoveTowards(bully.transform.position, this.transform.position, speed * Time.deltaTime);
            if(Vector3.Distance(this.transform.position, bully.transform.position) < .05f)
            {
                bullyAnimator.SetInteger("Current State", 1);
                bullyWalk = false;
            }
        }
        if (Vector3.Distance(player.transform.position, goHere) < 1)
        {
            goHere = player.transform.position;
        }

        if (showMenu && !fadeMenu)
        {
            enableMenu();
        }

        if (fadeMenu && !showMenu)
        {
            disableMenu();
        }

        if (run)
        {
            player.GetComponentInChildren<CardboardHead>().trackRotation = false;
            player.transform.Rotate(Vector3.down * Time.deltaTime * 200.0f);
            if (player.transform.eulerAngles.y <= 180)
            {
                delayTime = Time.time + 2f;
                speed = 7;
                player.GetComponentInChildren<CardboardHead>().trackRotation = true;
                run = false;
                goHere = runDestination.transform.position;
                bullyRun = true;
            }
        }

        if (bullyRun && delayTime < Time.time)
        {
            bully.transform.position = Vector3.MoveTowards(bully.transform.position, runDestination.transform.position, speed * Time.deltaTime * 1.3f);
            bully.transform.LookAt(player.transform.FindChild("LookAtPivot").transform);
            Debug.Log(Vector3.Distance(bully.transform.position, player.transform.position));
            if (Vector3.Distance(bully.transform.position, player.transform.position) < 2.5)
            {
                bullyRun = false;
                goHere = player.transform.position;
                bullyAnimator.SetInteger("Current State", 1);
                decision.text = "Will you fight back or do nothing?";
                opt1.GetComponentInChildren<Text>().text = "Fight back";
                opt2.GetComponentInChildren<Text>().text = "Do nothing";
                question2 = true; 
            }
        }
        if (question2 && player.transform.eulerAngles.y >= 90)
        {
            player.transform.Rotate(Vector3.down * Time.deltaTime * -200.0f);
            player.transform.FindChild("Head").Rotate(Vector3.down * Time.deltaTime * -200.0f); 
            if (player.transform.eulerAngles.y <= 90)
            {
                showMenu = true;
            }
        }

        if (bullyAnimator.GetCurrentAnimatorStateInfo(0).IsName("kick_to_the_groin"))
        {
            if (bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                bullyAnimator.SetInteger("Current State", 4);
            }
        }
        Debug.Log(headPos);
    }

    void OnTriggerEnter()
    {
        Debug.Log("Hello?");
        if (!fadeMenu)
        {
            showMenu = true;
        }
        question1 = true;
    }
    void enableMenu()
    {
        panelGroup.alpha += 1f * Time.deltaTime;
        if (panelGroup.alpha == 1f)
        {
            opt1.interactable = true;
            opt2.interactable = true;
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

    // Question X Choice 1
    public void onPositive()
    {
        if (question1)
        {
            question1 = false;
            run = true;
            fadeMenu = true;
            bullyAnimator.SetInteger("Current State", 2);
        }
        if (question2)
        {
            question2 = false;
            fadeMenu = true;
            bullyAnimator.SetInteger("Current State", 3);
        }
    }
    // Question X Choice 2
    public void onNegative()
    {
        if (question1)
        {
            print("Continue bully scene");
            question1 = false;
        }
    }
}
