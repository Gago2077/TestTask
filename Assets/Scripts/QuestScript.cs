using UnityEngine;
using TMPro;
using Naninovel;
using System.Collections;

public class QuestScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentQuest;
    [SerializeField] private GameObject checkMarkImage;
    [SerializeField] private float questUpdateDelay = 0.3f;
    [SerializeField] private float checkmarkDuration = 1.5f; // New duration parameter

    private ICustomVariableManager variableManager;
    private bool isQuestComplete;
    private Coroutine questUpdateRoutine;

    private void OnEnable()
    {
        variableManager = Engine.GetService<ICustomVariableManager>();
        variableManager.OnVariableUpdated += HandleVariableUpdate;
        InitializeQuest();
    }

    private void OnDisable()
    {
        if (variableManager != null)
            variableManager.OnVariableUpdated -= HandleVariableUpdate;
    }

    private void InitializeQuest()
    {
        checkMarkImage.SetActive(false);
        UpdateQuestState();
    }

    private void HandleVariableUpdate(CustomVariableUpdatedArgs args)
    {
        if (args.Name == "talkedToKirio" || 
            args.Name == "hasTheBag" || 
            args.Name == "talkedToMyself")
        {
            UpdateQuestState();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void UpdateQuestState()
    {
        if (isQuestComplete) return;

        var talkedToKirio = GetBoolVariable("talkedToKirio");
        var talkedToMyself = GetBoolVariable("talkedToMyself");
        var hasTheBag = GetBoolVariable("hasTheBag");
        var gameEnded= GetBoolVariable("GameEnded");

        if (!talkedToKirio)
        {
            SetQuest("Поищите сумку в прачечной");
        }
        else if (talkedToKirio && !hasTheBag)
        {
            SetQuest("Возьмите сумку из кладовки");
        }
        else if (hasTheBag || talkedToMyself)
        {
            SetQuest("Поговорите с Аской");
        }

        
    }

    private bool GetBoolVariable(string name)
    {
        var value = variableManager.GetVariableValue(name)?.ToString();
        return bool.TryParse(value, out bool result) && result;
    }

    public void SetQuest(string questText, bool immediate = false)
    {
        if (questUpdateRoutine != null)
            StopCoroutine(questUpdateRoutine);

        questUpdateRoutine = StartCoroutine(UpdateQuestText(questText, immediate));
    }

    private IEnumerator UpdateQuestText(string questText, bool immediate)
    {
        if (!immediate)
            yield return new WaitForSeconds(questUpdateDelay);

        currentQuest.text = questText;
        checkMarkImage.SetActive(false);
    }

    public void CompleteQuest()
    {
        if (!isQuestComplete)
        {
            isQuestComplete = true;
            checkMarkImage.SetActive(true);
            
            // Start coroutine to handle completion delay
            if (questUpdateRoutine != null)
                StopCoroutine(questUpdateRoutine);
            
            questUpdateRoutine = StartCoroutine(CompletionDelay());
        }
    }

    private IEnumerator CompletionDelay()
    {
        // Show checkmark for specified duration
        yield return new WaitForSeconds(checkmarkDuration);
        
        // Reset completion state and update quest
        isQuestComplete = false;
        checkMarkImage.SetActive(false);
        UpdateQuestState();
    }
}