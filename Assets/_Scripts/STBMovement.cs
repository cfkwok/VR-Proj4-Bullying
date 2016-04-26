using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class STBMovement : MonoBehaviour {

    public float speed;
    public GameObject player;
    private Vector3 goHere;
    private CanvasGroup panelGroup;
    private GameObject head;
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
    private bool playerTurn;
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
        head = player.transform.FindChild("Head").gameObject;
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
                showMenu = true;
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
            showMenu = false;
            fadeMenu = true;
            bully.transform.position = Vector3.MoveTowards(bully.transform.position, runDestination.transform.position, speed * Time.deltaTime * 1.3f);
            bully.transform.LookAt(player.transform.FindChild("LookAtPivot").transform);
            if (Vector3.Distance(bully.transform.position, player.transform.position) < 2.5)
            {
                bullyRun = false;
                goHere = player.transform.position;
                bullyAnimator.SetInteger("Current State", 1);
                decision.text = "Will you fight back\nor do nothing?";
                opt1.GetComponentInChildren<Text>().text = "Fight back";
                opt2.GetComponentInChildren<Text>().text = "Do nothing";
                question2 = true;
                fadeMenu = false;
                opt2.colors = opt1.colors;
                playerTurn = true; 
            }
        }
        if (playerTurn)
        {
            if (head.transform.eulerAngles.y < 8 || head.transform.eulerAngles.y > 180)
            {
                head.GetComponent<CardboardHead>().trackRotation = false;
                player.transform.Rotate(Vector3.down * Time.deltaTime * -200.0f);
            }
            if(head.transform.eulerAngles.y >= 12 && head.transform.eulerAngles.y <= 180)
            {
                head.GetComponent<CardboardHead>().trackRotation = false;
                player.transform.Rotate(Vector3.down * Time.deltaTime * 200.0f);

            }
            if (head.transform.eulerAngles.y > 5 && head.transform.eulerAngles.y <= 13)
            {
                head.GetComponent<CardboardHead>().trackRotation = true;
                playerTurn = false;
                showMenu = true;
            }
        }
        if (bullyAnimator.GetCurrentAnimatorStateInfo(0).IsName("kick_to_the_groin") || bullyAnimator.GetCurrentAnimatorStateInfo(0).IsName("shoulder_hit_and_fall"))
        {
            if (bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                bullyAnimator.SetInteger("Current State", 5);
            }
        }
        if (bullyAnimator.GetCurrentAnimatorStateInfo(0).IsName("getting_up"))
        {
            if (bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                bullyAnimator.SetInteger("Current State", 6);
            }
        }
        if (bullyAnimator.GetCurrentAnimatorStateInfo(0).IsName("zombie_punching"))
        {
            if (bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 3)
            {
                bullyAnimator.SetInteger("Current State", 7);
            }
        }
    }

    void OnTriggerEnter()
    {
        if (!fadeMenu)
        {
            showMenu = true;
        }
        question1 = true;
    }
    void enableMenu()
    {
        if (question1)
        {
            var cb = opt2.colors;
            opt2.interactable = false;
            cb.disabledColor = new Color(0, 0, 0, 0);
            opt2.colors = cb;
        }
        panelGroup.alpha += 1f * Time.deltaTime;
        if (panelGroup.alpha == 1f)
        {
            if(!question1)
            {
                opt2.interactable = true;
            }
            opt1.interactable = true;
            
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
            if (question2)
            {
                opt2.colors = opt1.colors;
            }
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
        if (question3)
        {
            question3 = false;
            fadeMenu = true;
            bullyAnimator.SetInteger("Current State", 3);
        }
        if (question2)
        {
            question2 = false;
            question3 = true;
            decision.text = "Kick or punch?";
            opt1.GetComponentInChildren<Text>().text = "kick";
            opt2.GetComponentInChildren<Text>().text = "Punch";
        }
    }
    // Question X Choice 2
    public void onNegative()
    {
        if (question1)
        {
            fadeMenu = true;
            question1 = false;
        }
        if (question2)
        {
            question2 = false;
            fadeMenu = true;
            bullyAnimator.SetInteger("Current State", 6);
        }
        if (question3)
        {
            question3 = false;
            fadeMenu = true;
            bullyAnimator.SetInteger("Current State", 4);
        }
    }
}
