using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Scene3AEGO : MonoBehaviour {

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
    private Scene3AFade sceneFader;
    
    public AudioSource teacherAudio;
    private bool stopTeacherAudio;

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
        sceneFader = GameObject.Find("Fader").GetComponent<Scene3AFade>();
        teacherAudio.Play();
        captionText = ("You: Looks like he decided to talk with the teacher about it...");
        StartCoroutine(ChangeCaptionText(2f, captionText));
        captionText = "";
        StartCoroutine(ChangeCaptionText(8f, captionText));
        captionText = ("You: I'm sure it will work out for him.");
        StartCoroutine(ChangeCaptionText(11f, captionText));
        captionText = ("");
        StartCoroutine(ChangeCaptionText(16f, captionText));
        StartCoroutine(StartNextScene(19f));

        narrativeText.text = "The victim took your advice and decided to talk to someone about his problem...";
    }
	
	// Update is called once per frame
	void Update () {

        if (goNextScene)
        {
            sceneFader.EndScene(4);
            stopTeacherAudio = true;
        }

        if (stopTeacherAudio)
        {
            teacherAudio.volume -= 0.01f;
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

    IEnumerator ChangeCaptionText(float time, string textStr)
    {
        yield return new WaitForSeconds(time);
        captionTextUI.text = textStr;
    }

    IEnumerator StartNextScene(float time)
    {
        yield return new WaitForSeconds(time);
        goNextScene = true;
    }    
}
