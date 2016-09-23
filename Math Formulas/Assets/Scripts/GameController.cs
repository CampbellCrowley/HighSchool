using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
#pragma warning disable 0168

public
class GameController : MonoBehaviour {
 public
  InputField input;
 public
  Slider sliderObject;
 public
  Text sliderText, sliderTextMin, sliderTextMax;
 private
  GameObject[] slider;
 public
  Text InfoText;
 public
  Text problemNumber;
 public
  Text question, output, title;
  [Range(0.0f, 10.0f)] public float tolerance = 0.05f;
  [Range(-5000.0f, 5000.0f)] public float moveDist = 1000f;
  [Range(0.0f, 100.0f)] public float moveSpeed = 12.2f;
  [Range(0.0f, 10.0f)] public float moveMultiplier = 2f;
  [Range(0.0f, 1.0f)] public float textSpeed = 0.03f;
 public
  string decimalInfoText = "At least one digit after the decimal is required";
 public
  string[] RandomText = {
      "Thinking is fun.", "You can do it!", "I believe in you!",
      "Don't take too long on this one.", "Hmm...",
      "I see you!\nJust kidding, I don't have eyes... :(",
      "Failing this test means nothing.", "Passing this test means everything.",
      "Is this even a test?", "What is the meaning of life?",
      "This statement is false.",
      "The Enrichment Center regrets to inform you that this next question " +
          "is impossible. Make no attempt to solve it.",
      "I'm making a note here: HUGE SUCCESS.",
      "I wonder if world domination would be fun...",
      "Take your time, I have all day.",
      "I bet I could do this test faster than you.",
      "Well here we are again\nIt's always such a pleasure", "42",
      "This test is making you smarter, I guarantee it.", "Ayy lmao",
      "Never gonna give you up...", "Is this real life?", "PC master race.",
      "Macintosh? more like BADintosh! Ha ha haaaa.... please laugh...",
      "A long time ago in a galaxy far far away...", "To infinity, and beyond!",
      "STOP... Hammer time!", "Gee wizz! You're a math wizz!",
      "Okay, I'm pretty sure you're cheating.", "Hey! No calculators!",
      "I think living is a myth.", "Anyone up for a game of Sportsball?",
      "\"Everyhing on the internet is true.\"\n-Abraham Lincoln",
      "I dream of being as cool as Google.",
      "Yahoo? More like, YaWhat's that? I'm hilarious, I know.",
      "Spaces of tabs.", "Vim over Emacs.", "Team 1001 111 1 is the best.",
      "Mei is NOT bae", "Unity over Unreal",
      "VR would be pretty cool IF I HAD EYES!", "The top is still spinning.",
      "You're a hairy wizard.", "First is the worst.", "Second is the best.",
      "This has taken you " +
          "9999999999999999999999999999999999999999999999999999999999999...",
      "I am God! Well, at least of this game.",
      "Doctor Who is pretty cool. He's my favorite vulcan on the Millennium " +
          "Falcon.",
      "Imagination is the essence of discovery.",
      "                  Hello over there.", "Why is this text even here?"};
 private
  string[] RandomEmpty = {
      "That's not an answer...",
      "Are you even trying?",
      "The answer box is feeling very empty inside...",
      "Nothing is not an answer.",
      "Nice try. But no.",
      "We wont continue until you at least attempt the question.",
      "I have all day.",
      "I can wait.",
      "Answer please.",
      "Responde por favor.",
      "I'm bilingual, emptiness is not a language I know.",
      "Oh come on, I can't let you off the hook that easily!"};
 private
  string[] RandomCorrect = {
      "Yay! Problem {0} done!",     "Booyah! {0} defeated!",
      "Order {1} coming up!",       "Problem {0} deep fried!",
      "#{0}? Pssh, aint nothin'.",  "Problem {0}? Never heard of it.",
      "Problem {1} is scared now.", "Problem {0}: 0, you: 1",
      "Ohh snap!",                  "You showed #{0} who's boss!",
      "Correctamundo!",             "On to the next!",
      "#{0} --> #{1}",              "#{0} is evolving!",
      "Problem {1} wants a fight!", "Challenge? HA!"};
 private
  bool movedown = false, moveleft = false, moveright = false,
       writingTitle = true, resetTitle = false, isSliderVisible = true;
 private
  string nextTitle = "Ready?\nThe game has already begun!";
 private
  float deltaLastTextUpdate = 0;
 private
  Vector3 questionStartPos, sliderTextStart;

 public
  void Awake() {
    sliderTextStart = sliderText.transform.position;
    slider = GameObject.FindGameObjectsWithTag("Slider");
    foreach (GameObject s in slider)
      s.SetActive(false);
    isSliderVisible = false;
  }
 public
  void Start() {
    questionStartPos = question.transform.position;
    question.text = SceneController.data.Questions[SceneController.data.number];
    output.text = "";
    InfoText.text = "";
    input.text = SceneController.data.currentInput;
    input.ActivateInputField();
  }
 public
  void Update() {
    float distFromStart =
        Vector3.Distance(question.transform.position, questionStartPos);
    problemNumber.text = "#" + (SceneController.data.number+1);

    if (moveleft) {
      question.transform.position = new Vector3(
          question.transform.position.x -
              (moveSpeed + distFromStart * moveMultiplier) * Time.deltaTime *
                  10f,
          question.transform.position.y, question.transform.position.z);
      if (question.transform.position.x <= questionStartPos.x - moveDist) {
        moveleft = false;
        moveright = false;
        movedown = true;
      }
    } else if (moveright) {
      question.transform.position = new Vector3(
          question.transform.position.x +
              (moveSpeed + distFromStart * moveMultiplier) * Time.deltaTime *
                  10f,
          question.transform.position.y, question.transform.position.z);
      if (question.transform.position.x >= questionStartPos.x + moveDist) {
        moveright = false;
        movedown = true;
      }
    } else if (movedown) {
      if (question.transform.position.x != questionStartPos.x) {
        question.transform.position =
            new Vector3(questionStartPos.x, questionStartPos.y + moveDist,
                        questionStartPos.z);
      } else {
        if (question.transform.position.y - moveSpeed < questionStartPos.y) {
          question.transform.position = questionStartPos;
          movedown = false;
          input.ActivateInputField();
          output.text = "";
          nextTitle = RandomText[Random.Range(0, RandomText.Length)];
          writingTitle = true;
          if (!isSliderVisible &&
              SceneController.data.number < QuestionData.Types.Length &&
              QuestionData.Types[SceneController.data.number] ==
                  QuestionData.SLIDER) {
            foreach (GameObject s in slider)
              s.SetActive(true);
            isSliderVisible = true;
          } else if (isSliderVisible &&
                     QuestionData.Types[SceneController.data.number] !=
                         QuestionData.SLIDER) {
            foreach (GameObject s in slider)
              s.SetActive(false);
            isSliderVisible = false;
          }
        } else {
          question.transform.position = new Vector3(
              questionStartPos.x, question.transform.position.y - moveSpeed,
              questionStartPos.z);
        }
      }
      if (SceneController.data.number < SceneController.data.Questions.Length) {
        question.text =
            SceneController.data.Questions[SceneController.data.number];
      } else {
        question.text = "";
        title.text = "All done!\nHere comes your score!";
        resetTitle = false;
        writingTitle = false;
      }
      input.text = "";
    } else if (SceneController.data.number >=
               SceneController.data.Questions.Length) {
      toScore();
    }

    deltaLastTextUpdate += Time.deltaTime;
    if (resetTitle && deltaLastTextUpdate >= textSpeed) {
      if (nextTitle.IndexOf(title.text) == 0 && title.text.Length > 0) {
        title.text = nextTitle.Substring(0, title.text.Length + 1);
      } else {
        try {
          title.text =
              nextTitle.Substring((int)(Mathf.Floor(nextTitle.Length / 2) -
                                        Mathf.Floor(title.text.Length / 2) - 1),
                                  (int)(title.text.Length + 1));
        } catch (System.ArgumentOutOfRangeException e) {
          title.text = nextTitle;
        }
      }
      deltaLastTextUpdate = 0;
      if (title.text.Equals(nextTitle)) {
        resetTitle = false;
        writingTitle = false;
      }
    } else if (writingTitle && deltaLastTextUpdate >= textSpeed) {
      if (nextTitle.IndexOf(title.text) == 0 || title.text.Length == 0) {
        resetTitle = true;
        writingTitle = false;
      } else {
        try {
          title.text = title.text.Substring(1, title.text.Length - 2);
        } catch (System.ArgumentOutOfRangeException e) {
          title.text = "";
        }
        deltaLastTextUpdate = 0;
      }
    }

    if (isSliderVisible) {
      input.gameObject.SetActive(false);
      sliderText.transform.position =
          sliderTextStart +
          Vector3.right * (sliderObject.normalizedValue - 0.5f) * 550f / 1.8f;
      sliderTextMax.text = "|\n|\n\n" + sliderObject.maxValue;
      sliderTextMin.text = "|\n|\n\n" + sliderObject.minValue;
      // Slider is 550 wide
      if (moveright || moveright || movedown)
        sliderText.text = sliderObject.value + "";
      else
        sliderText.text = Random.Range(-10, 11) + "";
    } else {
      input.gameObject.SetActive(true);
    }

    if (SceneController.data.number < QuestionData.Types.Length &&
        QuestionData.Types[SceneController.data.number] ==
            QuestionData.NUMBER_FIELD) {
      if (float.Parse(
              SceneController.data.Answers[SceneController.data.number]) *
              1f % 1 !=
          0) {
        InfoText.text = decimalInfoText;
      } else {
        InfoText.text = "";
      }
    }
  }
 public
  void toInstructions() { SceneController.toInstructions(); }
 public
  void toScore() { SceneController.toScore(); }
 public
  void OnTextUpdate() {
    if (SceneController.data.number < QuestionData.Types.Length &&
        QuestionData.Types[SceneController.data.number] ==
            QuestionData.NUMBER_FIELD) {
      input.text = Regex.Replace(input.text, "[^0-9.+-]", "");
    }
    SceneController.data.currentInput = input.text;
  }
 public
  void checkAnswer() {
    if (SceneController.data.number < QuestionData.Types.Length) {
      input.DeactivateInputField();
      if (input.text.Length == 0 &&
          QuestionData.Types[SceneController.data.number] !=
              QuestionData.SLIDER) {
        // title.text = "That's not an answer...";
        title.text = RandomEmpty[Random.Range(0, RandomEmpty.Length)];
        resetTitle = false;
        writingTitle = false;
        output.text = "Please enter an answer";
      } else if (validateAnswer()) {
        // title.text =
        //     "Yay! Problem " + (SceneController.data.number + 1) + " done!";
        title.text =
            RandomCorrect[Random.Range(0, RandomCorrect.Length)]
                .Replace("{0}", (SceneController.data.number + 1) + "")
                .Replace("{1}", (SceneController.data.number + 2) + "");
        resetTitle = false;
        writingTitle = false;
        output.text = "Correct!";
        moveright = true;
        SceneController.data.correct++;
        SceneController.data.number++;
        GameObject.Find("Particle System")
            .GetComponent<ParticleSystem>()
            .Play();
      } else {
        title.text = "Aww... better luck on problem " +
                     (SceneController.data.number + 2);
        resetTitle = false;
        writingTitle = false;
        output.text = "Incorrect";
        moveleft = true;
        SceneController.data.incorrect++;
        SceneController.data.number++;
      }
    }
  }
 private
  bool validateAnswer() {
    switch (QuestionData.Types[SceneController.data.number]) {
      default:
      case QuestionData.NUMBER_FIELD:
        // input and answer strings are identical
        if (input.text.Equals(
                SceneController.data.Answers[SceneController.data.number])) {
          Debug.Log("Correct under condition 1A");
          return true;
        }

        float answer_ = float.Parse(
            SceneController.data.Answers[SceneController.data.number]);
        float input_ = float.Parse(input.text);

        // If rounded values of both the answer and input are identical
        if ((float)(Mathf.Round(answer_ * 100) / 100) ==
            (float)(Mathf.Round(input_ * 100) / 100)) {
          Debug.Log("Correct under condition 2A\nRounded Values\nA:" + answer_ +
                    "\nI:" + input_);
          return true;
        }
        // If input is within a tolerance of the correct answer (not rounded)
        if (answer_ * 10f % 1 != 0 &&
            (answer_ + tolerance > input_ && answer_ - tolerance < input_)) {
          Debug.Log("Correct under condition 3A\nWithin tolerance\nA:" +
                    answer_ + "\nT:" + tolerance + "\nI:" + input_);
          return true;
        }
        Debug.Log("Incorrect\nA:" + answer_ + "\nT:" + tolerance + "\nI:" +
                  input_);
        return false;
      case QuestionData.TEXT_FIELD:
        // input and answer strings are identical
        if (input.text.ToLower().Equals(
                SceneController.data.Answers[SceneController.data.number]
                    .ToLower())) {
          Debug.Log("Correct under condition 1B");
          return true;
        }
        Debug.Log(
            "Incorrect\nA:" +
                SceneController.data.Answers[SceneController.data.number] +
            "\nT:" + tolerance + "\nI:" + sliderObject.value);
        return false;
      case QuestionData.SLIDER:
        if (SceneController.data.Answers[SceneController.data.number].Equals(
                sliderObject.value + "")) {
          Debug.Log("Correct under condition 1C");
          return true;
        }
        Debug.Log(
            "Incorrect\nA:" +
            float.Parse(
                SceneController.data.Answers[SceneController.data.number]) +
            "\nT:" + tolerance + "\nI:" + sliderObject.value);
        return false;
    }
  }
}

