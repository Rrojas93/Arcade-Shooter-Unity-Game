using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Author: Raul Rojas
//CSE 1302
//Project One
//7-13-2017

public class NewScoreControl : MonoBehaviour
{
    #region Fields
    #region Public
    public GameObject CenterPanel;
    public InputField nameInputField;
    public GameObject noHighScorePanel;
    public Button button;
    public Text titleText;
    #endregion

    #region Private
    private List<Text> NameTextFields;
    private List<Text> ScoreTextFields;
    private bool isNewHighScore;
    private StreamWriter sw;
    private StreamReader sr;
    private List<Score> scoreList;
    private int playerScore;
    private int place;
    private bool fromGameScene;
    private Text buttonText;
    #endregion
    #endregion

    #region MonoBehaviours
    private void Start()
    {
        button.onClick.AddListener(buttonClicked);
        buttonText = button.transform.GetChild(0).GetComponent<Text>();
        List<GameObject> placePanels = new List<GameObject>();
        NameTextFields = new List<Text>();
        ScoreTextFields = new List<Text>();
        scoreList = new List<Score>();

        for (int i = 0; i < CenterPanel.transform.childCount; i++)
        {
            placePanels.Add(CenterPanel.transform.GetChild(i).gameObject);  
        }

        foreach(GameObject go in placePanels)
        {
            NameTextFields.Add(go.transform.GetChild(1).GetComponent<Text>());  
            ScoreTextFields.Add(go.transform.GetChild(2).GetComponent<Text>()); 
        }

        getScores();    //read high Score file, store, and orginize scores. 
        printScoreList();   //for debugging.
        displayScores();    //display to highscore table. 
        getPrefs();     //retreive player score from PlayerPrefs   
        titleText.text = "High Scores";
        
        if (fromGameScene)
        {
            buttonText.text = "Menu";
            setFields();    //determine if the player is inputting new high score or just displaying high scores.         
        }
        else
        {
            buttonText.text = "Back";
            noHighScorePanel.SetActive(false);
            nameInputField.gameObject.SetActive(false);
        }
            
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            buttonClicked();
        }
    }
    #endregion

    #region Private Methods
    void setFields()
    {
        if (isNewHighScore)
        {
            titleText.text = "NEW HIGH SCORE!!";
            buttonText.text = "Enter";
            scoreList.Add(new Score("", playerScore));
            scoreList.Sort();
            scoreList.RemoveAt(5);
            displayScores();
            noHighScorePanel.SetActive(false);
            nameInputField.gameObject.SetActive(true);
            Transform panel = CenterPanel.transform.GetChild(place).transform;
            nameInputField.transform.SetParent(panel);
            nameInputField.gameObject.GetComponent<RectTransform>().position = panel.GetChild(1).GetComponent<RectTransform>().position;
            panel.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            buttonText.text = "Menu";
            noHighScorePanel.SetActive(true);
            nameInputField.gameObject.SetActive(false);
            noHighScorePanel.transform.GetChild(0).GetComponent<Text>().text = "Your Score";
            noHighScorePanel.transform.GetChild(1).GetComponent<Text>().text = playerScore.ToString();
        }
    }

    void getPrefs()
    {
        if (PlayerPrefs.GetInt(GameStructs.PrefKeys.FromGameScene) > 0)
        {
            fromGameScene = true;
            PlayerPrefs.SetInt(GameStructs.PrefKeys.FromGameScene, 0);
            if (PlayerPrefs.HasKey(GameStructs.PrefKeys.PlayerScore))
            {
                playerScore = PlayerPrefs.GetInt(GameStructs.PrefKeys.PlayerScore);
                PlayerPrefs.SetInt(GameStructs.PrefKeys.PlayerScore, 0);
                for (int i = 0; i < 5; i++)
                {
                    if (playerScore > scoreList[i].ScoreP)
                    {
                        isNewHighScore = true;
                        place = i;
                        break;
                    }
                    else
                    {
                        isNewHighScore = false;
                    }

                }
            }
            else
            {
                Debug.Log("No Player score found.");
                playerScore = 0;
                isNewHighScore = false;
            }
        }                
    }

    void getScores()
    {
        bool complete = false;
        int backUpCount = 0;
        do
        {
            backUpCount++;
            if (backUpCount >= 2)   //avoid infinate loop if finding file failed and creating default file failed. 
            {
                break;
            }
            try
            {
                sr = new StreamReader("HighScores.txt");
                string temp = sr.ReadLine();
                if (temp == null)
                {
                    //temp = sr.ReadLine();
                    Debug.Log("Temp is null before loop. ");
                }
                int count = 0; //only get the first 5 lines to have a total of 5 scores ONLY
                while (temp != null && count <= 5)
                {                
                    string[] lineData = temp.Split(':');
                    scoreList.Add(new Score(lineData[0], int.Parse(lineData[1])));
                    count++;
                    temp = sr.ReadLine();
                }
                complete = true;
            }
            catch (FileNotFoundException fnfe)
            {
                Debug.Log("A problem finding the Highscores file was encountered: " + fnfe.Message);
                StreamWriter swTemp = new StreamWriter("HighScores.txt");   //create missing file. 
                //swTemp.WriteLine("-----:0");
                //swTemp.WriteLine("-----:0");
                //swTemp.WriteLine("-----:0");
                //swTemp.WriteLine("-----:0");
                //swTemp.WriteLine("-----:0");
                swTemp.Close();         
            }
            catch(ArgumentException ae)
            {
                Debug.Log("A problem occured while reading the HighScores file: " + ae.Message);
            }
            catch(NullReferenceException nre)
            {
                Debug.Log("A problem occured while reading the HighScores file: " + nre.Message);
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
            }
            finally
            {
                if (complete)
                {
                    sr.Close();
                }
            }
        } while (!complete);

        //if (complete)
        //{
            if (scoreList.Count < 5)
            {
                for (int i = scoreList.Count; i < 5; i++)
                {
                    scoreList.Add(new Score("-----", 0));
                }
            }
            scoreList.Sort();

        //}
    }

    void displayScores()
    {
        for (int i = 0; i < 5; i++)
        {
            NameTextFields[i].text = scoreList[i].Name;
            ScoreTextFields[i].text = scoreList[i].ScoreP.ToString();
        }
    }

    void printScoreList()
    {
        foreach(Score s in scoreList)
        {
            Debug.Log(s.ToString());
        }
    }

    void buttonClicked()
    {
        AudioManager.instance.Play(GameStructs.ClipName.ButtonClick);
        if (isNewHighScore && fromGameScene)
        {
            string playerInput = nameInputField.transform.GetChild(2).GetComponent<Text>().text;    //the text in the inputField
            if (playerInput.Length > 0)    //check if its place holder text  
            {
                //record score, close file, and change to normal high score scene.
                scoreList[place].Name = playerInput;
                saveScores();
                PlayerPrefs.SetInt(GameStructs.PrefKeys.FromGameScene, 0);
                SceneManager.LoadScene("HighScores", LoadSceneMode.Single);
            }
            else
            {
                //Tell player to input a name in the text field. 
                Debug.Log("Please enter your name.");
            }
            
        }
        else
        {
            //return to main title menu scene. 
            SceneManager.LoadScene("Title", LoadSceneMode.Single);
        }
    }

    void saveScores()
    {
        try
        {
            sw = new StreamWriter("HighScores.txt");

            foreach(Score s in scoreList)
            {
                sw.WriteLine(s.Name + ":" + s.ScoreP);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }
        finally
        {
            sw.Close();
        }
    }
    #endregion
}
