using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Helicopter : MonoBehaviour, ISubscriber<OnQuestItemPickUpEvent>, ISubscriber<OnQuestItemDroppedEvent>, ISubscriber<OnEnemyKilledEvent>
{
    private bool HaveHelicopterKey = false;
    private int EnemyKilledCounter;
    private const int ENEMY_KILLED_REQUIREMENT = 20;
    private string DefaultMessage = "We need to find the \n'Helicopter Key' first";
    private string ActionMessage = "";
    private int NumOfPlayersInRange = 0; // all players need to be at the helicopter
    private const float BUTTON_HELD_DOWN_TIME = 0.5f;
    private float ButtonHeldTimer = 0f;
    private InteractionHandler CurrentInteractor; // the one who is interacting with this object (Helicopter)

    // Start is called before the first frame update

    private void Start()
    {
        EventAggregator.GetInstance().Register<OnQuestItemDroppedEvent>(this);
        EventAggregator.GetInstance().Register<OnQuestItemPickUpEvent>(this);
        EventAggregator.GetInstance().Register<OnEnemyKilledEvent>(this);
    }

    private void OnDestroy()
    {
        EventAggregator.GetInstance().Unregister<OnQuestItemDroppedEvent>(this);
        EventAggregator.GetInstance().Unregister<OnQuestItemPickUpEvent>(this);
        EventAggregator.GetInstance().Unregister<OnEnemyKilledEvent>(this);
    }

    private void Update()
    {
        if(CurrentInteractor != null)
        {
            if (HaveHelicopterKey)
            {
                ActionMessage = "Hold '" + CurrentInteractor.DownPlatformButton + "' to use \n'Helicopter Key'";
                CurrentInteractor.ShowInteractionPanel(this.name, ActionMessage);
                ListenForUserInput();
            }
            else
            {
                CurrentInteractor.ShowInteractionPanel(this.name, DefaultMessage);
            }
        }
    }

    public void OnHelicopterTriggerEnter(InteractionHandler interactor)
    {
        if (interactor != null)
        {
            NumOfPlayersInRange++;
            CurrentInteractor = interactor;
            this.enabled = true;
        }
    }

    public void OnHelicopterTriggerExit(InteractionHandler interactor)
    {
        if(interactor!= null)
        {
            interactor.RemoveInteractionPanel();
            NumOfPlayersInRange--;
        }
        CurrentInteractor = null;
        this.enabled = false;
    }

    public void ListenForUserInput()
    {
        if(CurrentInteractor != null)
        {
            if (Input.GetButton(CurrentInteractor.PlayerInput.DownButton))
            {
                if (ButtonHeldTimer < BUTTON_HELD_DOWN_TIME)
                {
                    ButtonHeldTimer += Time.deltaTime;
                    CurrentInteractor.ShowLoadBar(ButtonHeldTimer, BUTTON_HELD_DOWN_TIME);
                }
                else
                {
                    CurrentInteractor.RemoveInteractionPanel();
                    //Load next level
                    Debug.Log("Hello World");
                    LoadNextScene();
                    ButtonHeldTimer = 0;
                    this.enabled = false;
                }
                
            }

            if (Input.GetButtonUp(CurrentInteractor.PlayerInput.DownButton))
            {
                CurrentInteractor.RemoveLoadBar();
                ButtonHeldTimer = 0;
            }
        }
    }

    private void LoadNextScene()
    {

        LoadProfileList.SavePlayerProgress();

        StartCoroutine(LoadNextSceneAsync());
    }

    IEnumerator LoadNextSceneAsync()
    {
        yield return null;

        AsyncOperation EndOfDemoScene = SceneManager.LoadSceneAsync("EndOfDemo");
        EndOfDemoScene.allowSceneActivation = false;
        while (!EndOfDemoScene.isDone)
        {
            if (EndOfDemoScene.progress >= 0.9f)
            {
                EndOfDemoScene.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void OnEventHandler(OnQuestItemPickUpEvent eventData)
    {
        Settings.PrintDebugMsg("Quest Item Picked Up");
        HaveHelicopterKey = true;
    }

    public void OnEventHandler(OnQuestItemDroppedEvent eventData)
    {
        Settings.PrintDebugMsg("Quest Item Dropped");
        HaveHelicopterKey = false;
    }

    public void OnEventHandler(OnEnemyKilledEvent eventData)
    {
        EnemyKilledCounter++;
    }
}
