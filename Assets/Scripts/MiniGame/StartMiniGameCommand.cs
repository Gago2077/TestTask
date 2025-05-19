using Naninovel;
using UnityEngine;

[CommandAlias("startMiniGame")]
public class StartMiniGameCommand : Command
{
    [ParameterAlias("reset")]
    public BooleanParameter ResetState = true;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        if (!Engine.Initialized) return;

        var variableManager = Engine.GetService<ICustomVariableManager>();
        var controller = Object.FindObjectOfType<MiniGameController>();
        var gameManager = Object.FindObjectOfType<GameManager>();

        variableManager.SetVariableValue("minigameComplete", "false");
        if (ResetState && gameManager != null) 
            gameManager.ResetGame();

        MiniGameEvents.OnMiniGameStart?.Invoke();
        
        await UniTask.WaitUntil(() => 
            variableManager.GetVariableValue("minigameComplete") == "true"
        );

        MiniGameEvents.OnMiniGameEnd?.Invoke();
        Debug.Log("Mini-game completed, resuming Naninovel");
    }
}