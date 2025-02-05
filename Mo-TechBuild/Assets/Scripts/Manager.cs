using UnityEditor;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager _instance;

    public static Manager Instance {get {return _instance;}}

    private GameState gamestate = GameState.Playing;

    public Canvas pauseCanvas;

    public Canvas GameUI;

    public float startTime;

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
        EnablePause();
    }

    public float GetTimeStamp(){
        return Time.time-startTime;
    }

    // Update is called once per frame
    void Update()
    {
        switch(gamestate){
            case GameState.Playing:
                if(Input.GetKeyDown(KeyCode.Escape)){
                    EnablePause();
                }
                
                break;
            case GameState.Pause:
                if(Input.GetKeyDown(KeyCode.Escape)){
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
        gamestate = GameState.Pause;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        //GameUI.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(true);
    }

    public void ResumePlay(){
        gamestate = GameState.Playing;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        //GameUI.gameObject.SetActive(true);
        pauseCanvas.gameObject.SetActive(false);
    }
}
