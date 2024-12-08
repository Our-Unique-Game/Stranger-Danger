using UnityEngine;
using TMPro; // For TextMeshPro UI elements
using UnityEngine.UI; // For Button UI elements
using System.Collections; // For Coroutines

public class QuestionTrigger : MonoBehaviour
{
    [Header("Question Settings")]
    [SerializeField] private string questionText = "What is 2 + 2?"; // The question to ask
    [SerializeField] private string correctAnswer = "4"; // The correct answer

    [Header("UI Elements")]
    [SerializeField] private GameObject questionPanel; // Reference to the UI panel
    [SerializeField] private TMP_Text questionTextUI; // Reference to the TextMeshPro Text element
    [SerializeField] private TMP_InputField answerInputField; // Reference to the TMP InputField
    [SerializeField] private TMP_Text feedbackText; // Feedback text for correct or wrong answers
    [SerializeField] private Button submitButton; // Submit button for answers
    [SerializeField] private Button closeButton; // Close button for the panel

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer questionMarkRenderer; // The SpriteRenderer for the question mark
    [SerializeField] private Sprite redQuestionMarkSprite; // Default red question mark
    [SerializeField] private Sprite greenQuestionMarkSprite; // Green question mark for correct answer

    private PlayerMovement playerMovement; // Reference to PlayerMovement
    private bool answeredCorrectly = false; // Tracks if the question has already been answered

    private void Awake()
    {
        // Find the player's movement script
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only trigger if the player collides and the question hasn't been answered correctly
        if (collision.CompareTag("Player") && !answeredCorrectly)
        {
            // Disable player movement
            if (playerMovement != null)
                playerMovement.CanMove = false;

            // Show the question panel
            questionPanel.SetActive(true);

            // Set the question text
            questionTextUI.text = questionText;

            // Add listeners for the buttons
            submitButton.onClick.AddListener(SubmitAnswer);
            closeButton.onClick.AddListener(ClosePanel);
        }
    }

    private void SubmitAnswer()
    {
        // Get the player's answer
        string playerAnswer = NormalizeWhitespace(answerInputField.text);

        // Check if the answer is correct (case-insensitive comparison)
        if (string.Equals(playerAnswer, NormalizeWhitespace(correctAnswer), System.StringComparison.OrdinalIgnoreCase))
        {
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;

            // Mark as answered correctly
            answeredCorrectly = true;

            // Change the SpriteRenderer to the green question mark
            if (questionMarkRenderer != null && greenQuestionMarkSprite != null)
            {
                questionMarkRenderer.sprite = greenQuestionMarkSprite;
            }

            // Start coroutine to hide panel after 3 seconds
            StartCoroutine(HidePanelAfterDelay(3f));
        }
        else
        {
            feedbackText.text = "Wrong! Try again.";
            feedbackText.color = Color.red;
        }

        // Clear the input field for the next question
        answerInputField.text = "";
    }

    private IEnumerator HidePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Close the panel after delay
        ClosePanel();
    }

    private void ClosePanel()
    {
        // Hide the question panel
        questionPanel.SetActive(false);

        // Clear the feedback text
        feedbackText.text = "";

        // Re-enable player movement
        if (playerMovement != null)
            playerMovement.CanMove = true;

        // Remove listeners to avoid duplicate calls
        submitButton.onClick.RemoveListener(SubmitAnswer);
        closeButton.onClick.RemoveListener(ClosePanel);
    }

    // Helper method to normalize whitespace
    private string NormalizeWhitespace(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ").Trim();
    }
}
