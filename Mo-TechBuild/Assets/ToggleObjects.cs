using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject objectToDisable; // 需要关闭的对象
    public GameObject objectToEnable;  // 需要开启的对象

    public bool resumePlay;
    void Update()
    {
        // 检测是否按下了手柄的X键 (Xbox手柄通常是 "joystick button 2")
        if (Input.GetKeyDown(KeyCode.JoystickButton8) || Input.GetKeyDown(KeyCode.E))
        {
            Toggle();
        }
    }

    void Toggle()
    {
        // 切换对象的激活状态
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
            if(resumePlay){
                Manager.Instance.ResumeControls();
            }
        }

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }else{
            Manager.Instance.TutorialEnd();
        }
    }
}