using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int rows = 2;
    [SerializeField] private int columns = 2;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform gridParent;

    private List<CardBehavior> spawnedCards = new List<CardBehavior>();

    private void Start() => GenerateGrid();

    public void GenerateGrid()
    {
        ClearGrid();
        InitializeGridLayout();
        CreateCardPairs();
        NotifyGameManager();
    }

    private void ClearGrid()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);
        
        spawnedCards.Clear();
    }

    private void InitializeGridLayout()
    {
        var grid = gridParent.GetComponent<GridLayoutGroup>();
        if (grid) grid.constraintCount = columns;
    }

    private void CreateCardPairs()
    {
        var cardIds = GenerateCardIds(rows * columns);
        SpawnCards(cardIds);
    }

    private List<int> GenerateCardIds(int totalCards)
    {
        var ids = new List<int>();
        for (int i = 0; i < totalCards / 2; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }
        return Shuffle(ids);
    }

    private List<int> Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
        return list;
    }

    private void SpawnCards(List<int> cardIds)
    {
        foreach (int id in cardIds)
        {
            var card = Instantiate(cardPrefab, gridParent);
            var behavior = card.GetComponent<CardBehavior>();
            behavior.Initialize(id);
            spawnedCards.Add(behavior);
        }
    }

    private void NotifyGameManager()
    {
        var gameManager = GetComponent<GameManager>();
        if (gameManager) gameManager.SetCards(spawnedCards.ToArray());
    }
}