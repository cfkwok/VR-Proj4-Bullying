using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class STASceneFadeInOut : MonoBehaviour
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

        if (narrativeText.text.Contains("talked"))
        {
            statsPath = 1;
        }
        if (narrativeText.text.Contains("mind"))
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
                narrativeText.text = "Stats say bullies are mean0 Stats say bullies are mean0 Stats say bullies are mean0";
                textInt++;
                timeToRead = 5f;
            }
            else if (textInt == 1)
            {
                narrativeText.text = "Stats say bullies are mean1 Stats say bullies are mean1";
                textInt++;
                timeToRead = 10f;
            }
            else if (textInt == 2)
            {
                narrativeText.text = "Stats say bullies are mean2";
                textInt++;
                timeToRead = 5f;
            }
            else if (textInt == 3)
            {
                narrativeText.text = "Stats say bullies are mean3";
                textInt++;
                timeToRead = 2f;
            }
        }

        else if (statsPath == 2)
        {
            if (textInt == 0)
            {
                narrativeText.text = "2Stats say bullies are mean0 Stats say bullies are mean0 Stats say bullies are mean0";
                textInt++;
                timeToRead = 5f;
            }
            else if (textInt == 1)
            {
                narrativeText.text = "2Stats say bullies are mean1 Stats say bullies are mean1";
                textInt++;
                timeToRead = 10f;
            }
            else if (textInt == 2)
            {
                narrativeText.text = "2Stats say bullies are mean2";
                textInt++;
                timeToRead = 5f;
            }
            else if (textInt == 3)
            {
                narrativeText.text = "2Stats say bullies are mean3";
                textInt++;
                timeToRead = 2f;
            }
        }
        callRoutine = true;
    }
}