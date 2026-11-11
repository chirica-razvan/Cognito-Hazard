using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    private enum ButtonColor { None = 0, Red = 1, Yellow = 2, Blue = 3, Green = 4 }
    private enum Rule { Ink, Word, NoGo }

    [Serializable]
    private struct Trial
    {
        public string word;
        public ButtonColor ink;
        public ButtonColor meaning;
        public Rule rule;
    }

    [SerializeField] private Trial[] trials;
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private TMP_Text bannerText;
    [SerializeField] private float noGoDuration = 2f;
    [SerializeField] private float pauseBetweenTrials = 0.5f;

    private ButtonColor pressed;
    private bool accepting;
    private float shownAt;
    private float reactionTime;

    private void Start() => StartCoroutine(RunTest());

    // Apelata din Select Entered: Rosu=1, Galben=2, Albastru=3, Verde=4
    public void SubmitAnswer(int button)
    {
        if (!accepting) return;
        accepting = false;
        pressed = (ButtonColor)button;
        reactionTime = Time.time - shownAt;
    }

    private IEnumerator RunTest()
    {
        int correct = 0;

        foreach (var trial in trials)
        {
            Show(trial);
            pressed = ButtonColor.None;
            reactionTime = 0f;
            shownAt = Time.time;
            accepting = true;

            float deadline = trial.rule == Rule.NoGo
                ? shownAt + noGoDuration
                : float.PositiveInfinity;

            while (accepting && Time.time < deadline)
                yield return null;

            accepting = false;

            bool isCorrect = pressed == Expected(trial);
            if (isCorrect) correct++;

            Debug.Log($"{trial.word};{trial.ink};{trial.rule};{pressed};{isCorrect};{reactionTime:F3}");

            instructionText.text = "";
            bannerText.text = "";
            yield return new WaitForSeconds(pauseBetweenTrials);
        }

        instructionText.color = Color.white;
        instructionText.text = $"Final: {correct} din {trials.Length} corecte";
    }

    private void Show(Trial trial)
    {
        instructionText.color = ToColor(trial.ink);
        instructionText.text = trial.rule == Rule.NoGo ? trial.word + " !" : trial.word;
        bannerText.text = trial.rule == Rule.Word ? "APASA CE SCRIE!" : "";
    }

    private static ButtonColor Expected(Trial trial) => trial.rule switch
    {
        Rule.Ink => trial.ink,
        Rule.Word => trial.meaning,
        _ => ButtonColor.None // la No-Go, corect inseamna sa nu apesi nimic
    };

    private static Color ToColor(ButtonColor color) => color switch
    {
        ButtonColor.Red => Color.red,
        ButtonColor.Yellow => Color.yellow,
        ButtonColor.Blue => Color.blue,
        ButtonColor.Green => Color.green,
        _ => Color.white
    };
}