using UnityEngine.SceneManagement;
using UnityEngine;

//? Operate inside an scene
public class StageOperator : MonoBehaviour
{
	#region Set this class to singleton
	public static StageOperator i {get{if(_i==null){_i = GameObject.FindObjectOfType<StageOperator>();}return _i;}} static StageOperator _i;
	#endregion

	[Header("Gameplay")]
	public bool paused;
	public GameObject pauseMenu;
	float pausedTimeScale;
	
	public void Pausing()
	{
		//Toggle the pause menu
		pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
		//Toggle pause
		paused = !paused;
		//Change game time pause on if it currently pause of not
		if(paused) 
		{
			//Save the time scale before pausing
			pausedTimeScale = Time.timeScale;
			Time.timeScale = 0; 
		}
		//When no longer pause
		else
		{
			//Revert time scale back like before it pause
			Time.timeScale = pausedTimeScale;
		}
	}

	void Update()
	{
		//Pause when press escaped
		if(pauseMenu != null && Input.GetKeyDown(KeyCode.Escape)) Pausing();
	}

    public void LoadSceneIndex(int i) {SceneManager.LoadScene(i, LoadSceneMode.Single);}
    public void QuitGame()  {Application.Quit();}
}