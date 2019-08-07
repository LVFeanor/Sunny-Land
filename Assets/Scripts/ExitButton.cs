using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour {

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            Debug.Log("exit");
            Exit();
        });
    }
    private void Exit()
    {
        Application.Quit();
    }
}
