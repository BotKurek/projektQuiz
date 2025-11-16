    public class Question : IQuestion
    {
        public List<IAnswear> Answears { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Text { get; set; }
        public List<Answear> Answers { get; set; }
    
    public Question() 
        {
            Text = "";
            Answers = new List<Answear>();
        }

        public Question(string text)
        {
            Text = text;
            Answers = new List<Answear>();
        }

        public void AddAnswer(string text, bool isCorrect = false)
        {
            Answers.Add(new Answear(text, isCorrect));
        }

        public void Display()
        {
            Console.WriteLine(Text);
            for (int i = 0; i < Answers.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {Answers[i].Text}");
            }
        }

        public bool CheckAnswer(int choiceIndex)
        {
            if (choiceIndex > 0 && choiceIndex <= Answers.Count)
            {
                return Answers[choiceIndex - 1].IsCorrect;
            }
            
            return false;
        }
    }