using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CardBehavior : MonoBehaviour
{
    public int cardId { get; private set; }
    public bool IsRevealed { get; private set; }

    [SerializeField] private TextMeshProUGUI numberText;
    private Button button;
    private GameManager gameManager;

    public void Initialize(int id)
    {
        cardId = id;
        Hide();
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnCardClicked);
    }

    private void OnCardClicked()
    {
        if (!IsRevealed) 
            gameManager.CardClicked(this);
    }

    public void Reveal()
    {
        numberText.text = cardId.ToString();
        IsRevealed = true;
    }

    public void Hide()
    {
        numberText.text = string.Empty;
        IsRevealed = false;
    }
}