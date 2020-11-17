using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Text _highScoreLabel;
    
    private InputActions _inputActions;
    
    private void OnEnable() => _inputActions.Enable();
    private void OnDisable() => _inputActions.Disable();

    void Awake()
    {
        _inputActions = new InputActions();
    }
    
    void Start()
    {
        _highScoreLabel.text = PlayerPrefs.GetInt("HighScore",0).ToString();
        
        _inputActions.UI.AnyKey.performed += LoadGame;
    }

    private void LoadGame(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene("GameScene");
    }

}
