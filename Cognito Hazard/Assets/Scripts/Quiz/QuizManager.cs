using System;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    private enum Answer { Red = 1, Yellow = 2, Blue = 3, Green = 4 }

    [Serializable]
    private struct Question
    {
        public string text;
        public Answer correct;
    }

    [SerializeField] private Question[] questions;

    private int current;
    private int correctAnswers;
    private int wrongAnswers;

    private void Start() => ShowQuestion();

    public void SubmitAnswer(int button)
    {
        if (current >= questions.Length) return;

        if ((Answer)button == questions[current].correct)
        {
            correctAnswers++;
            Debug.Log("Corect!");
        }
        else
        {
            wrongAnswers++;
            Debug.Log("Gresit!");
        }

        current++;
        ShowQuestion();
    }

    private void ShowQuestion()
    {
        if (current < questions.Length) Debug.Log(questions[current].text);
        else Debug.Log($"Final: {correctAnswers} corecte, {wrongAnswers} gresite");
    }
}