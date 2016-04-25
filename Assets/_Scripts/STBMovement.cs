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
    private bool showMenu;
    private bool fadeMenu;
    private bool run;
    private bool bullyRun;
    private bool question1;
    private bool question2;
    public GameObject runDestination;
    void Start () {
        goHere = this.transform.position;
        panelGroup = GameObject.Find("QuestionMenu").GetComponent<CanvasGroup>();
        opt1 = GameObject.Find("Button1").GetComponent<Button>();
        opt2 = GameObject.Find("Button2").GetComponent<Button>();
        panelGroup.alpha = 0;
        bully = GameObject.Find("Bully");
        bullyAnimator = bully.GetComponent<Animator>();
    }

    void Update() {
        player.transform.position = Vector3.Lerp(player.transform.position, goHere, speed * Time.deltaTime * 10);
        if (Vector3.Distance(player.transform.position, goHere) < 1)
        {
            goHere = player.transform.position;
        }
        if (showMenu)
        {
            enableMenu();
        }
        if (fadeMenu)
        {
            disableMenu();
        }
        if (run)
        {
            player.GetComponentInChildren<CardboardHead>().trackRotation = false;
            player.transform.Rotate(Vector3.down * Time.deltaTime * 100.0f);
            if (player.transform.eulerAngles.y <= 180)
            {
                player.GetComponentInChildren<CardboardHead>().trackRotation = true;
                run = false;
                goHere = runDestination.transform.position;
                bullyRun = true;
            }
        }
        if (bullyRun)
        {
            bully.transform.position = Vector3.Lerp(bully.transform.position, goHere, speed * Time.deltaTime * 10);
            if(Vector3.Distance(bully.transform.position, goHere) < 2)
            {
                bullyRun = false;
            }
        }
    }

    void OnTriggerEnter()
    {
        goHere = player.transform.position;
        showMenu = true;
        question1 = true;
        bullyAnimator.SetInteger("Current State", 1);
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
        if (panelGroup.alpha == 1f)
        {
            opt1.interactable = true;
            opt2.interactable = true;
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
