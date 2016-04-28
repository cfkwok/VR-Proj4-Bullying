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
    private GameObject teacher;
    private Animator teacherAnimator;
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
    private bool playerTurn2;
    private bool teacherWalk;
    private bool question2;
    private bool question3;
    public GameObject runDestination;
    public GameObject teacherDestination;

    private Text narrativeText;
    private string captionText;
    private Text captionTextUI;
    private bool dialog;
    private bool delayOption;
    private bool fight;
    private Transform lookHere;
    private SceneFadeInOut2 sceneFade;
    private Image hit;
    public AudioSource bullyAudio;
    public AudioSource teacherAudio;

    void Start () {
        narrativeText = GameObject.Find("CardboardMain/Head/Main Camera/Fader/Narrative").GetComponent<Text>();
        captionTextUI = GameObject.Find("CardboardMain/Head/Main Camera/Fader/Caption").GetComponent<Text>();
        sceneFade = GameObject.Find("CardboardMain/Head/Main Camera/Fader").GetComponent<SceneFadeInOut2>();
        hit = GameObject.Find("CardboardMain/Head/Main Camera/Hit Red").GetComponent<Image>();
        dialog = false;
        goHere = player.transform.position;
        panelGroup = GameObject.Find("QuestionMenu").GetComponent<CanvasGroup>();
        decision = panelGroup.transform.FindChild("Question").GetComponent<Text>();
        opt1 = GameObject.Find("Button1").GetComponent<Button>();
        opt2 = GameObject.Find("Button2").GetComponent<Button>();
        panelGroup.alpha = 0;
        bully = GameObject.Find("Bully");
        teacher = GameObject.Find("Liam");
        teacherAnimator = teacher.GetComponent<Animator>();
        bullyAnimator = bully.GetComponent<Animator>();
        bullyWalk = true;
        fadeMenu = false;
        head = player.transform.FindChild("Head").gameObject;
        playerTurn2 = false;
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
                delayTime = Time.time + 1.5f;
                speed = 7;
                player.GetComponentInChildren<CardboardHead>().trackRotation = true;
                run = false;
                goHere = runDestination.transform.position;
                bullyRun = true;
                captionText = "Bully: Hey! Get back here!";
                StartCoroutine(ChangeCaptionText(.5f, captionText));
                captionText = "";
                StartCoroutine(ChangeCaptionText(4f, captionText));
                bullyAudio.Play();
                StartCoroutine(StopBullyAudio(4));
            }
        }

        if (bullyRun && delayTime < Time.time)
        {
            showMenu = false;
            fadeMenu = true;


            bully.transform.position = Vector3.MoveTowards(bully.transform.position, runDestination.transform.position, speed * Time.deltaTime * 1.2f);
            bully.transform.LookAt(player.transform.FindChild("LookAtPivot").transform);
            if (Vector3.Distance(bully.transform.position, player.transform.position) < 2.5)
            {
                bullyRun = false;
                goHere = player.transform.position;
                bullyAnimator.SetInteger("Current State", 1);
                dialog = true;
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
            }
        }
        if (bullyAnimator.GetCurrentAnimatorStateInfo(0).IsName("kick_to_the_groin") || bullyAnimator.GetCurrentAnimatorStateInfo(0).IsName("shoulder_hit_and_fall"))
        {
            if (bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                bullyAnimator.SetInteger("Current State", 5);
                captionText = "I can't believe you did that! Now you're dead!";
                StartCoroutine(ChangeCaptionText(0, captionText));
            }
        }
        if (bullyAnimator.GetCurrentAnimatorStateInfo(0).IsName("getting_up"))
        {
            if (bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                captionText = "Let's see how you like this!";
                StartCoroutine(ChangeCaptionText(0, captionText));
                captionText = "";
                StartCoroutine(ChangeCaptionText(4, captionText));
                StartCoroutine(StopBullyAudio(4));
                bullyAnimator.SetInteger("Current State", 6);
            }
        }
        if (bullyAnimator.GetCurrentAnimatorStateInfo(0).IsName("zombie_punching"))
        {
            if (bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > .4 && bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                hit.color = Color.Lerp(hit.color, Color.red, 2 * Time.deltaTime);
            }
            if(bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.4){
                hit.color = Color.Lerp(hit.color, Color.clear, 2 * Time.deltaTime);
            }
            if (bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.4 && bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 2)
            {
                hit.color = Color.Lerp(hit.color, Color.red, 2 * Time.deltaTime);
            }
            if (bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 2 && bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 2.4){
                hit.color = Color.Lerp(hit.color, Color.clear, 2 * Time.deltaTime);
            }
            if (bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 2.4 && bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 3)
            {
                hit.color = Color.Lerp(hit.color, Color.red, 2 * Time.deltaTime);
            }
            if (bullyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 3)
            {
                bullyAnimator.SetInteger("Current State", 7);
            }
        }
        if (bullyAnimator.GetCurrentAnimatorStateInfo(0).IsName("zombie_headbutt"))
        {
            if (Vector3.Distance(teacher.transform.position, teacherDestination.transform.position) > .5f)
            {
                teacherWalk = true;
            }
        }
        if (teacherWalk)
        {
            teacher.transform.position = Vector3.MoveTowards(teacher.transform.position, teacherDestination.transform.position, speed * Time.deltaTime * 1f);
            teacher.transform.LookAt(lookHere);
            if (Vector3.Distance(teacher.transform.position, teacherDestination.transform.position) < .5f)
            {
                teacherWalk = false;
                playerTurn2 = true;
                teacherAnimator.SetInteger("Current State", 1);
            }
        }
        if (playerTurn2)
        {
            hit.color = Color.Lerp(hit.color, Color.clear, 2 * Time.deltaTime);
            player.transform.Rotate(Vector3.down * Time.deltaTime * -100.0f);
            head.GetComponent<CardboardHead>().trackRotation = false;
            if (head.transform.eulerAngles.y > 70 && head.transform.eulerAngles.y < 100)
            {
                head.GetComponent<CardboardHead>().trackRotation = true;
                playerTurn2 = false;
                dialog = true;
            }
        }
        if(dialog && !question1 && !question2 && !question3)
        {
            if (fight)
            {
                teacherAudio.Play();
                captionText = "Teacher: Hey! Stop this fighting right now!";
                StartCoroutine(ChangeCaptionText(0, captionText));
                captionText = "Both of you are in trouble.";
                StartCoroutine(ChangeCaptionText(4.5f, captionText));
                captionText = "I'm going to have to report this to the principal.";
                StartCoroutine(ChangeCaptionText(8.5f, captionText));
                captionText = "You both can expect a suspension to head your way.";
                StartCoroutine(ChangeCaptionText(13f, captionText));
                captionText = "";
                StartCoroutine(ChangeCaptionText(17f, captionText));
                StartCoroutine(DelayOptions(18));
                teacherAudio.Play();
                StartCoroutine(StopTeacherAudio(17));
                dialog = false;
            }
            if (!fight)
            {
                captionText = "Teacher: Hey! Stop this fighting right now!";
                StartCoroutine(ChangeCaptionText(0, captionText));
                captionText = "Bully I saw the whole thing.";
                StartCoroutine(ChangeCaptionText(4.5f, captionText));
                captionText = "Fighting is not allowed in school.";
                StartCoroutine(ChangeCaptionText(8.5f, captionText));
                captionText = "I'm going to have to report this to the principal.";
                StartCoroutine(ChangeCaptionText(12f, captionText));
                captionText = "Bully you're going to be suspended for your actions.";
                StartCoroutine(ChangeCaptionText(15f, captionText));
                captionText = "";
                StartCoroutine(ChangeCaptionText(19f, captionText));
                StartCoroutine(DelayOptions(20));
                teacherAudio.Play();
                StartCoroutine(StopTeacherAudio(19));
                dialog = false;
            }
        }
        if(delayOption && !question1 && !question2 && !question3)
        {
           // Debug.Log("Fai");
            sceneFade.EndScene(1);
        }
        if (question1 && dialog)
        {
            captionText = "Bully: There you are! I've been looking for you!";
            StartCoroutine(ChangeCaptionText(0, captionText));
            captionText = "I was going to let it slide that you interfered but you lost your chance";
            StartCoroutine(ChangeCaptionText(4.5f, captionText));
            captionText = "when I heard you told the teacher what had happened!";
            StartCoroutine(ChangeCaptionText(8.5f, captionText));
            captionText = "";
            StartCoroutine(ChangeCaptionText(13f, captionText));
            StartCoroutine(DelayOptions(13.5f));
            bullyAudio.Play();
            StartCoroutine(StopBullyAudio(13));
            dialog = false;
        }
        if (question1 && !fadeMenu && delayOption)
        {
            showMenu = true;
            delayOption = false;
        }
        if (question2 && dialog)
        {
            captionText = "Bully: Did you really think you could get away?";
            StartCoroutine(ChangeCaptionText(0, captionText));
            captionText = "I'm the fastest kid in school! Never forget that!";
            StartCoroutine(ChangeCaptionText(4.5f, captionText));
            captionText = "It is time for your beating! I hope you're prepared!";
            StartCoroutine(ChangeCaptionText(8.5f, captionText));
            captionText = "";
            StartCoroutine(ChangeCaptionText(13f, captionText));
            StartCoroutine(DelayOptions(13.5f));
            bullyAudio.Play();
            StartCoroutine(StopBullyAudio(13));
            dialog = false;
        }
        if (question2 && !fadeMenu && delayOption)
        {
            decision.text = "Will you fight back\nor do nothing?";
            opt1.GetComponentInChildren<Text>().text = "Fight back";
            opt2.GetComponentInChildren<Text>().text = "Do nothing";
            showMenu = true;
            delayOption = false;
        }
    }

    void OnTriggerEnter()
    {
        question1 = true;
        dialog = true;

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
            captionText = "Bully: ARRRGGGHHH!";
            bullyAudio.Play();
            StartCoroutine(ChangeCaptionText(0, captionText));
            bullyAnimator.SetInteger("Current State", 3);
        }
        if (question2)
        {
            question2 = false;
            question3 = true;
            fight = true;
            narrativeText.text = "You chose to fight back";
            decision.text = "Kick or punch?";
            opt1.GetComponentInChildren<Text>().text = "kick";
            opt2.GetComponentInChildren<Text>().text = "punch";
            lookHere = player.transform.FindChild("LookAtPivot").transform;
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
            fight = false;
            narrativeText.text = "You chose to not fight back";
            captionText = "Let's see how you like this!";
            StartCoroutine(ChangeCaptionText(1, captionText));
            captionText = "";
            StartCoroutine(ChangeCaptionText(4, captionText));
            bullyAudio.Play();
            StartCoroutine(StopBullyAudio(4));
            bullyAnimator.SetInteger("Current State", 6);
            lookHere = bully.transform.FindChild("LookAtPivot").transform;
        }
        if (question3)
        {
            question3 = false;
            fadeMenu = true;
            captionText = "Bully: ARRRGGGHHH!";
            bullyAudio.Play();
            StartCoroutine(ChangeCaptionText(0, captionText));
            bullyAnimator.SetInteger("Current State", 4);
        }
    }

    IEnumerator ChangeCaptionText(float time, string textStr)
    {
        yield return new WaitForSeconds(time);
        captionTextUI.text = textStr;
    }
    IEnumerator DelayOptions(float time)
    {
        yield return new WaitForSeconds(time);
        delayOption = true;
    }
    IEnumerator AudioPlay(float time)
    {
        yield return new WaitForSeconds(time);
        delayOption = true;
    }
    IEnumerator StopBullyAudio(float time)
    {
        yield return new WaitForSeconds(time);
        bullyAudio.Stop();
    }
    IEnumerator StopTeacherAudio(float time)
    {
        yield return new WaitForSeconds(time);
        teacherAudio.Stop();
    }
}
