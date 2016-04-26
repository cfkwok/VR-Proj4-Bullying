using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class STAMovement : MonoBehaviour {

    private MeshRenderer dialogVictim;
    private BoxCollider talkTrig;

    private CanvasGroup panelGroup;
    private Text textQuestion;
    private Button opt1;
    private Button opt2;
    private bool showMenu;
    private bool fadeMenu;
    private bool question1 = true;
    private bool question2;
    private bool changeTextTwo;
    private bool elevateCamera;
    private bool goNextScene;

    private GameObject player;
    private GameObject victim;
    private bool moveToVictim;
    private Text narrativeText;
    private STASceneFadeInOut sceneFader;

    // Use this for initialization
    void Start () {
        dialogVictim = GameObject.Find("DialogVictim").GetComponent<MeshRenderer>();
        talkTrig = GameObject.Find("TalkTrigger").GetComponent<BoxCollider>();

        panelGroup = GameObject.Find("QuestionMenu").GetComponent<CanvasGroup>();
        textQuestion = GameObject.Find("Question").GetComponent<Text>();
        opt1 = GameObject.Find("Button1").GetComponent<Button>();
        opt2 = GameObject.Find("Button2").GetComponent<Button>();

        player = GameObject.Find("Character");
        victim = GameObject.Find("Bullied");

        narrativeText = GameObject.Find("Character/Head/Main Camera/Fader/Narrative").GetComponent<Text>();
        sceneFader = GameObject.Find("Fader").GetComponent<STASceneFadeInOut>();

        print("Classmate1: Hey, I heard that kid got his butt kicked today! Hahaha! Look at him back there crying!");
        print("Classmates: LOL!");
        StartCoroutine(StartSceneFlow(5));
        
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 rot = Camera.main.transform.rotation.eulerAngles;
        rot.x = 0;
        rot.z = 270;
        rot.y += 95;
        dialogVictim.transform.rotation = Quaternion.Euler(rot);

        if (showMenu && !fadeMenu)
        {
            enableMenu();
        }
        if (fadeMenu && !showMenu)
        {
            disableMenu();
        }

        if (elevateCamera)
        {
            player.transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
            if (player.transform.localScale.y >= 1f)
            {
                elevateCamera = false;
                moveToVictim = true;
            }
        }     
        
        if (moveToVictim)
        {
            Vector3 dest = player.transform.position;
            dest.z = victim.transform.position.z - 0.9f;
            
            if (player.transform.position.z >= victim.transform.position.z - 1.0f)
            {
                dest.x = victim.transform.position.x - 0.4f;
            }
            player.transform.position = Vector3.MoveTowards(player.transform.position, dest, 1.2f * Time.deltaTime);
            
            if (player.transform.position.x >= victim.transform.position.x - 0.7f)
            {
                print("Victim: Just leave me alone... *sob sob*");
                moveToVictim = false;
                enableDialog();
                /*
                // Audio should work something like this...
                if (!AudioSource.isPlaying && !audVicOne) {
                    audVicOne = true;
                    AudioSource.clip = LeaveMeAlone;
                    AudioSource.audio.Play();
                }
                else if (audVicOne && !AudioSource.isPlaying) {
                    moveToVictim = false;
                    enableDialog();
                }
                */
            }

        }  

        if (goNextScene)
        {
            sceneFader.EndScene(4);
        }

    }

    void enableDialog()
    {
        dialogVictim.enabled = true;
        talkTrig.enabled = true;
    }

    void disableDialog()
    {
        dialogVictim.enabled = false;
        talkTrig.enabled = false;
    }

    void enableMenu()
    {
        panelGroup.alpha += 1f * Time.deltaTime;
        opt1.enabled = true;
        opt2.enabled = true;
        panelGroup.GetComponent<Canvas>().enabled = true;
        if (panelGroup.alpha == 1f)
        {
            showMenu = false;
        }
    }

    void disableMenu()
    {
        panelGroup.alpha -= 1f * Time.deltaTime;
        opt1.enabled = false;
        opt2.enabled = false;
        panelGroup.GetComponent<Canvas>().enabled = false;
        if (panelGroup.alpha == 0f)
        {
            fadeMenu = false;
        }
    }

    public void changeQuestion(string question, string btn1, string btn2)
    {
        textQuestion.text = question;
        opt1.GetComponentInChildren<Text>().text = btn1;
        opt2.GetComponentInChildren<Text>().text = btn2;
    }

    // Question X Choice 1
    public void onPositive()
    {
        if (question1 && !showMenu)
        {
            fadeMenu = true;
            question1 = false;
            disableDialog();
            changeTextTwo = true;
            elevateCamera = true;            
        }
        if (question2 && !showMenu)
        {
            fadeMenu = true;
            disableDialog();
            print("User: Look, I know you have it rough being picked on because of how you are. " +
                "Just know that I am on your side, you do not deserve this and I am very willing to " +
                "lend a hand if you will let me. Let's deal with this together.");
            print("*Victim picks his head up*");
            print("Victim: Thank you for your support... QQ");
            print("User: Remember, you have people you can trust, who you can talk to...");           
            narrativeText.text = "You talked with your classmate and offered support to him in hopes to solve this issue.";
            goNextScene = true;
        }
    }
    // Question X Choice 2
    public void onNegative()
    {
        if (question1 && !showMenu)
        {
            fadeMenu = true;
            question1 = false;
            disableDialog();  

            print("Classmate2: I saw his face! It's all swollen from that beating. Almost looks like a... face, with more faces...");
            print("Classmate1: You're stupid. It looks more like a squash!");
            print("Classmate2: LOL YOU RIGHT!");
            
        }
        if (question2 && !showMenu)
        {
            fadeMenu = true;
            disableDialog();
            print("Classmate2: I saw his face! It's all swollen from that beating. Almost looks like a... face, with more faces...");
            print("Classmate1: You're stupid. It looks more like a squash!");
            print("Classmate2: LOL YOU RIGHT!");
        }
        narrativeText.text = "You decided to mind your own business. He should be able to solve his own problems.";
        goNextScene = true;
    }

    public void OnGazeClick()
    {
        if (changeTextTwo)
        {
            changeQuestion("Do you want to give him your support?", "Talk to Him", "Leave Him");
            question2 = true;
        }
        if (!fadeMenu)
        {
            disableDialog();
            showMenu = true;
        }
    }

    IEnumerator StartSceneFlow(float time)
    {
        yield return new WaitForSeconds(time);
        enableDialog();
        changeQuestion("Do you want to talk to him?", "Talk", "Don't Talk");
    }
}
