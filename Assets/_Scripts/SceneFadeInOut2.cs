using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFadeInOut2 : MonoBehaviour
{
    public Image FadeImg;
    public float fadeSpeed = 1.5f;
    public bool sceneStarting = true;

    private GameObject mainCamRet;
    private int textInt = 0;
    private float timeToRead = 5f;
    private Text narrativeText;
    private bool callRoutine = true;
    private int statsPath = 0;
    public string levelName;
    AsyncOperation async;
    private bool loadScene;
    private bool loadFlag = true;

    void Awake()
    {
        mainCamRet = GameObject.Find("CardboardMain/Head/Main Camera/CardboardReticle");
        narrativeText = GameObject.Find("CardboardMain/Head/Main Camera/Fader/Narrative").GetComponent<Text>();
        //FadeImg.rectTransform.localScale = new Vector2(Screen.width, Screen.height);

        narrativeText.color = Color.clear;
    }

    void Update()
    {
        // If the scene is starting...
        if (sceneStarting)
            // ... call the StartScene function.
            StartScene();
        if (loadScene && loadFlag)
        {
            loadFlag = false;
            StartLoading();
        }
    }


    void FadeToClear()
    {
        // Lerp the colour of the image between itself and transparent.
        FadeImg.color = Color.Lerp(FadeImg.color, Color.clear, fadeSpeed * Time.deltaTime);
    }


    void FadeToBlack()
    {
        // Lerp the colour of the image between itself and black.
        FadeImg.color = Color.Lerp(FadeImg.color, Color.black, fadeSpeed * Time.deltaTime);
    }


    void StartScene()
    {
        // Fade the texture to clear.
        FadeToClear();

        // If the texture is almost clear...
        if (FadeImg.color.a <= 0.05f)
        {
            // ... set the colour to clear and disable the RawImage.
            FadeImg.color = Color.clear;
            FadeImg.enabled = false;

            // The scene is no longer starting.
            sceneStarting = false;
        }
    }


    public void EndScene(int SceneNumber)
    {

        // Make sure the RawImage is enabled.
        FadeImg.enabled = true;
        mainCamRet.SetActive(false);

        // Start fading towards black.
        FadeToBlack();

        if (narrativeText.text.Contains("You chose to fight back"))
        {
            statsPath = 1;
        }
        if (narrativeText.text.Contains("You chose to not fight back"))
        {
            statsPath = 2;
        }

        // If the screen is almost black...
        if (FadeImg.color.a >= 0.95f)
        {
            if (callRoutine)
            {
                StartCoroutine(ExecuteAfterTime(timeToRead));
                narrativeText.color = Color.white;
            }
        }
            

        if (textInt == 10)
        {
            SceneManager.LoadScene(SceneNumber);
        }
            
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        callRoutine = false;
        levelName = "scene1_crowd";
        yield return new WaitForSeconds(time);
        if (statsPath == 1)
        {
            if (textInt == 0)
            {
                narrativeText.text = "\"30% of students told an adult after witnessing another student being bullied.\"";
                textInt++;
                timeToRead = 8f;
            }
            else if (textInt == 1)
            {
                narrativeText.text = "\"According to the 2009 survey by the Teen Online & Wireless Safety 58 % of bullies are trying to get back at the victim for various reasons.\"";
                textInt++;
                timeToRead = 11f;
            }
            else if (textInt == 2)
            {
                narrativeText.text = "Schools with zero tolerance policies towards violence, students who retaliate to bullying will also get in trouble";
                timeToRead = 8f;
                textInt++;
            }
            else if (textInt == 3)
            {
                narrativeText.text = "End Route";
                timeToRead = 5f;
                textInt++;
            }
            else if (textInt == 4)
            {
                ActivateScene();
            }
        }

        else if (statsPath == 2)
        {
            if (textInt == 0)
            {
                narrativeText.text = "\"30% of students told an adult after witnessing another student being bullied.\"";
                textInt++;
                timeToRead = 8f;
            }
            else if (textInt == 1)
            {
                narrativeText.text = "\"According to the 2009 survey by the Teen Online & Wireless Safety 58 % of bullies are trying to get back at the victim for various reasons.\"";
                textInt++;
                timeToRead = 11f;
            }
            else if (textInt == 2)
            {
                narrativeText.text = "\"22 percent reported being bullied outside on school grounds.\"";
                textInt++;
                timeToRead = 8f;
            }
            else if (textInt == 3)
            {
                narrativeText.text = "End Route";
                timeToRead = 5f;
                textInt++;
            }
            else if(textInt == 4)
            {
                ActivateScene();
            }
        }
        loadScene = true;
        callRoutine = true;
    }


    public void StartLoading()
    {
        StartCoroutine("load");
    }



    public void ActivateScene()
    {
        async.allowSceneActivation = true;
    }

    IEnumerator load()
    {
        Debug.LogWarning("ASYNC LOAD STARTED - " +
           "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
        async = SceneManager.LoadSceneAsync(levelName);
        async.allowSceneActivation = false;
        yield return async;
    }
}