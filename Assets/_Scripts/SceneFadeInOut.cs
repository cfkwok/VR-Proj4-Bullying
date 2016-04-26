using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFadeInOut : MonoBehaviour
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

        if (narrativeText.text.Contains("stopped"))
        {
            statsPath = 1;
        }
        if (narrativeText.text.Contains("intervene"))
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
        yield return new WaitForSeconds(time);
        if (statsPath == 1)
        {
            if (textInt == 0)
            {
                narrativeText.text = "\"In a 2007 study, 86% of LGBT students said that they had experienced harassment at school during the previous year\"";
                textInt++;
                timeToRead = 10f;
            }
            else if (textInt == 1)
            {
                narrativeText.text = "\"DHHS reports that bullying stops within 10 seconds 57 percent of the time when someone intervenes\"";
                textInt++;
                timeToRead = 9f;
            }
            else if (textInt == 2)
            {
                narrativeText.text = "\"If I say anything, he’ll turn on me next!\" is a common mentality bystanders have.";
                timeToRead = 8f;
            }
        }

        else if (statsPath == 2)
        {
            if (textInt == 0)
            {
                narrativeText.text = "\"In a 2007 study, 86% of LGBT students said that they had experienced harassment at school during the previous year\"";
                textInt++;
                timeToRead = 10f;
            }
            else if (textInt == 1)
            {
                narrativeText.text = "Most bystanders believe that someone else will take charge, so they take no action.";
                textInt++;
                timeToRead = 8f;
            }
            else if (textInt == 2)
            {
                narrativeText.text = "85% of the time, there is no intervention in a bullying situation.";
                textInt++;
                timeToRead = 8f;
            }
        }
        callRoutine = true;
    }
}