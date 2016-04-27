// This scene is from "Scene 2: Kid is depressed", and user "Says nothing"

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SThABMovement : MonoBehaviour {

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
    private SThABSceneFadeInOut sceneFader;

    public AudioSource classChat;
    public AudioSource teacherAudio;
    private bool stopClassChat;
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
        sceneFader = GameObject.Find("Fader").GetComponent<SThABSceneFadeInOut>();

        StartCoroutine(StopClassChat(3f));
        StartCoroutine(StartTeacherAudio(2.8f));
        captionText = ("Teacher: OK class, time to call roll...");
        StartCoroutine(ChangeCaptionText(3f, captionText));
        captionText = ("Classmate: Pssst look who's not here again...");
        StartCoroutine(ChangeCaptionText(7f, captionText));
        captionText = ("");
        StartCoroutine(ChangeCaptionText(12f, captionText));

        StartCoroutine(StartSceneFlow(13));
        
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 rot = Camera.main.transform.rotation.eulerAngles;
        rot.x = 0;
        rot.z = 270;
        rot.y += 95;
        dialogVictim.transform.rotation = Quaternion.Euler(rot);

        if (goNextScene)
        {
            sceneFader.EndScene(6);
        }
        if (stopClassChat)
        {
            classChat.volume -= 0.01f;
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

    public void OnGazeClick()
    {
        if (!fadeMenu)
        {
            disableDialog();
            captionText = ("You: Looks like he's not here again today... wonder if he's OK...");
            StartCoroutine(ChangeCaptionText(0.7f, captionText));
            captionText = ("");
            StartCoroutine(ChangeCaptionText(5.7f, captionText));
            StartCoroutine(StartNextScene(6f));
            StartCoroutine(StopTeacherAudio(6f));
        }
    }

    IEnumerator StartSceneFlow(float time)
    {
        yield return new WaitForSeconds(time);
        enableDialog();
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

    IEnumerator StopClassChat(float time)
    {
        yield return new WaitForSeconds(time);
        stopClassChat = true;
    }

    IEnumerator StartTeacherAudio(float time)
    {
        yield return new WaitForSeconds(time);
        teacherAudio.Play();
    }

    IEnumerator StopTeacherAudio(float time)
    {
        yield return new WaitForSeconds(time);
        stopTeacherAudio = true;
    }
}
