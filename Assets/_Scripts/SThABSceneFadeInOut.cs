using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SThABSceneFadeInOut : MonoBehaviour
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
        mainCamRet = GameObject.Find("Character/Head/Main Camera/CardboardReticle");
        narrativeText = GameObject.Find("Character/Head/Main Camera/Fader/Narrative").GetComponent<Text>();
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
        
        statsPath = 1;
     

        // If the screen is almost black...
        if (FadeImg.color.a >= 0.95f)
        {
            if (callRoutine)
            {
                StartCoroutine(ExecuteAfterTime(timeToRead));
                narrativeText.color = Color.white;
            }
        }

    }

    IEnumerator ExecuteAfterTime(float time)
    {

        callRoutine = false;
        yield return new WaitForSeconds(time);
        if (statsPath == 1)
        {
            levelName = "scene1_crowd";
            if (textInt == 0)
            {
                narrativeText.text = "\"Approximately 160,000 teens skip school every day because of bullying.\"";
                textInt++;
                timeToRead = 8f;
            }
            else if (textInt == 1)
            {
                narrativeText.text = "\"1 in 10 students drop out of school because of repeated bullying\"";
                textInt++;
                timeToRead = 5f;
            }
            else if (textInt == 2)
            {
                narrativeText.text = "\"Bully victims are between 2 to 9 times more likely to consider suicide than non-victims, according to studies by Yale University\"";
                textInt++;
                timeToRead = 13f;
            }
            else if (textInt == 3)
            {
                narrativeText.text = "and...";
                textInt++;
                timeToRead = 3f;
            }
            else if (textInt == 4)
            {
                narrativeText.text = "";
                textInt++;
                timeToRead = 1.5f;
            }
            else if (textInt == 5)
            {
                narrativeText.text = "Suicide is the third leading cause of death among young people, resulting in about 4,400 deaths per year";
                textInt++;
                timeToRead = 10f;
            }
            else if (textInt == 6)
            {
                narrativeText.text = "End of route";
                textInt++;
                timeToRead = 5f;
            }
            else if (textInt == 7)
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