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
    private Text captionTextUI;
    private string captionText;
    private STASceneFadeInOut sceneFader;

    public AudioSource laughter;
    public AudioSource victimAudio;
    public AudioSource classAudio;
    private bool stopClassAudio;

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
        captionTextUI = GameObject.Find("Character/Head/Main Camera/Fader/Caption").GetComponent<Text>();
        sceneFader = GameObject.Find("Fader").GetComponent<STASceneFadeInOut>();

        captionText = ("Classmate1: Hey, I heard that kid got his butt kicked today!");
        StartCoroutine(ChangeCaptionText(3f, captionText));
        StartCoroutine(StartLaughter(10.0f));
        captionText = ("Classmate2: Hahaha! Look at him back there crying!");
        StartCoroutine(ChangeCaptionText(9f, captionText));
        captionText = ("");
        StartCoroutine(ChangeCaptionText(17f, captionText));
        StartCoroutine(StartSceneFlow(17.5f));
        
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
                StartCoroutine(StartVictimAudio(0.5f));
                StartCoroutine(StopVictimAudio(4.3f));
                captionText = ("Victim: Just leave me alone...");
                StartCoroutine(ChangeCaptionText(0.3f, captionText));
                captionText = "";
                StartCoroutine(ChangeCaptionText(4f, captionText));
                moveToVictim = false;
                Invoke("enableDialog", 4f);
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
            stopClassAudio = true;
        }

        if (stopClassAudio)
        {
            classAudio.volume -= 0.01f;
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
            captionText = ("You: Look, I know you have it rough being picked on because of who you are.");
            StartCoroutine(ChangeCaptionText(0.3f, captionText));
            captionText = "Just know that I am on your side, you do not deserve this and I am very willing to";
            StartCoroutine(ChangeCaptionText(6f, captionText));
            captionText = "lend a hand if you will let me. Let's deal with this together.";
            StartCoroutine(ChangeCaptionText(11f, captionText));
            captionText = "";
            StartCoroutine(ChangeCaptionText(17f, captionText));
            // print("*Victim picks his head up*");
            StartCoroutine(StartVictimAudio(19.5f));
            StartCoroutine(StopVictimAudio(23.7f));
            captionText = ("Victim: Thanks for your help... just give me some alone time.");
            StartCoroutine(ChangeCaptionText(19f, captionText));
            captionText = ("You: Remember, you have people you can trust, who you can talk to...");
            StartCoroutine(ChangeCaptionText(24f, captionText));
            captionText = ("");
            StartCoroutine(ChangeCaptionText(29f, captionText));
            narrativeText.text = "You talked with your classmate and offered support to him in hopes to solve this issue.";
            StartCoroutine(StartNextScene(31f));
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

            captionText = ("Classmate2: His face is all swollen from that beating!");
            StartCoroutine(ChangeCaptionText(0.3f, captionText));
            captionText = "Looks like a face, with more faces...";
            StartCoroutine(ChangeCaptionText(4f, captionText));
            StartCoroutine(StartLaughter(5f));
            captionText = ("Classmate1: You're stupid. It looks more like a squash!");
            StartCoroutine(ChangeCaptionText(6.6f, captionText));
            captionText = ("Classmate2: Haha, you're right!");
            StartCoroutine(StartLaughter(8.2f));
            StartCoroutine(ChangeCaptionText(10f, captionText));
            captionText = ("");
            StartCoroutine(ChangeCaptionText(13f, captionText));
        }
        if (question2 && !showMenu)
        {
            fadeMenu = true;
            disableDialog();
            captionText = ("Classmate2: His face is all swollen from that beating!");
            StartCoroutine(ChangeCaptionText(0.3f, captionText));
            captionText = "Looks like a face, with more faces...";
            StartCoroutine(ChangeCaptionText(4f, captionText));
            StartCoroutine(StartLaughter(5f));
            captionText = ("Classmate1: You're stupid. It looks more like a squash!");
            StartCoroutine(ChangeCaptionText(6.6f, captionText));
            captionText = ("Classmate2: Haha, you're right!");
            StartCoroutine(StartLaughter(8.2f));
            StartCoroutine(ChangeCaptionText(10f, captionText));
            captionText = ("");
            StartCoroutine(ChangeCaptionText(13f, captionText));
        }
        narrativeText.text = "You decided to mind your own business and think he should be able to solve his own problems...";
        StartCoroutine(StartNextScene(13f));
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

    IEnumerator ChangeCaptionText(float time, string textStr)
    {
        yield return new WaitForSeconds(time);
        captionTextUI.text = textStr;
    }

    IEnumerator StartSceneFlow(float time)
    {
        yield return new WaitForSeconds(time);
        enableDialog();
        changeQuestion("Do you want to talk to him?", "Talk", "Don't Talk");
    }

    IEnumerator StartNextScene(float time)
    {
        yield return new WaitForSeconds(time);
        goNextScene = true;
    }

    IEnumerator StartLaughter(float time)
    {
        yield return new WaitForSeconds(time);
        if (laughter.isPlaying)
        {
            laughter.Stop();
        }
        laughter.Play();
    }

    IEnumerator StartVictimAudio(float time)
    {
        yield return new WaitForSeconds(time);
        victimAudio.Play();
    }

    IEnumerator StopVictimAudio(float time)
    {
        yield return new WaitForSeconds(time);
        victimAudio.Stop();
    }
}
