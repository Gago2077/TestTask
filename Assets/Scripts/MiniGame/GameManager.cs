using System.Collections;
using UnityEngine;
using Naninovel;

[RequireComponent(typeof(CardGridGenerator))]
public class GameManager : MonoBehaviour
{
    private CardBehavior firstCard, secondCard;
    private CardBehavior[] allCards;
    private bool canClick = true;
    private bool endGame;

    public bool IsGameFinished => endGame;

    public void SetCards(CardBehavior[] cards)
    {
        allCards = cards;
        if (allCards.Length % 2 != 0)
            Debug.LogWarning("Card count is not even - matching may fail");
    }

    public void CardClicked(CardBehavior clickedCard)
    {
        if (!canClick || clickedCard.IsRevealed) return;

        clickedCard.Reveal();

        if (firstCard == null)
        {
            firstCard = clickedCard;
        }
        else
        {
            secondCard = clickedCard;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        canClick = false;

        
        bool isMatch = firstCard.cardId == secondCard.cardId;

        if (!isMatch)
        {
            yield return new WaitForSeconds(0.5f);
            firstCard.Hide();
            secondCard.Hide();
        }

        ResetSelection();
        
        if (isMatch)
        {
            yield return new WaitForSeconds(0.1f); 
        }

        CheckGameCompletion();
    }

    private void ResetSelection()
    {
        firstCard = null;
        secondCard = null;
        canClick = true;
    }

    

    private void CheckGameCompletion()
    {
        foreach (var card in allCards)
        {
            if (!card.IsRevealed) return;
        }
        
        endGame = true;
        Debug.Log("Game Completed!");
        OnGameFinished();
    }
    public void ResetGame()
    {
        endGame = false;
        firstCard = null;
        secondCard = null;
        canClick = true;
        
        foreach (var card in allCards)
        {
            card.Hide();
        }
        
        GetComponent<CardGridGenerator>().GenerateGrid();
    }
    private void OnGameFinished()
    {
        endGame = true;
        Debug.Log("Game completed - triggering Naninovel continuation");
        
        if (Engine.Initialized)
        {
            var vars = Engine.GetService<ICustomVariableManager>();
            vars.SetVariableValue("minigameComplete", "true");
        }

        MiniGameEvents.OnMiniGameEnd?.Invoke();
    }
}