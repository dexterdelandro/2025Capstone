using StarterAssets;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Manager : MonoBehaviour
{
    private static Manager _instance;

    public static Manager Instance {get {return _instance;}}

    private GameState gamestate = GameState.Playing;

    public Canvas pauseCanvas;

    public Canvas GameUI;

    public float startTime;

    public ThirdPersonController playerController;

    public bool didFinishTutorial = false;

    public enum GameState{
        Playing,
        Loading,
        Pause,
        Menu,
    }

    
    public GameState GetGameState(){
        return gamestate;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        if(_instance !=null && _instance != this){
            Destroy(this.gameObject);
        }else{
            _instance = this;
        }
    }
    void Start()
    {
        startTime = Time.time;
        Cursor.lockState = CursorLockMode.None;
        //EnablePause();
    }

    public float GetTimeStamp(){
        return Time.time-startTime;
    }

    // Update is called once per frame
    void Update()
    {
        switch(gamestate){
            case GameState.Playing:
                if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.JoystickButton9))
                {
                    EnablePause();
                }
                
                break;
            case GameState.Pause:
                if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.JoystickButton9)){
                    ResumePlay();
                }
                break;
        }

    }

    public void QuitGame(){
        Application.Quit();
    }

    void LoadScene(){
        
    }
    

    public void EnablePause(){
        PauseControls();
        gamestate = GameState.Pause;

        //Cursor.lockState = CursorLockMode.None;
        GameUI.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(true);
    }

    public void ResumePlay(){
        pauseCanvas.gameObject.SetActive(false);
        gamestate = GameState.Playing;
       // Cursor.lockState = CursorLockMode.Locked;
        if(didFinishTutorial)GameUI.gameObject.SetActive(true);
        pauseCanvas.gameObject.SetActive(false);
        ResumeControls();
    }

    public void restart()
    {
        SceneManager.LoadScene("Level GDC_Audio");
        didFinishTutorial = false;
    }

    public void TutorialEnd(){
        didFinishTutorial = true;
        ResumeControls();
    }

    public void PauseControls(){
        if(playerController==null){
            playerController = FindAnyObjectByType<ThirdPersonController>();
        }

        if(playerController){
            playerController.LockCameraPosition = true;
            Time.timeScale = 0;
        }


    }

    public void ResumeControls(){
        if(playerController==null){
            playerController = FindAnyObjectByType<ThirdPersonController>();
        }

        if(playerController){
            playerController.LockCameraPosition = false;
            Time.timeScale = 1;
        }
    }
}
