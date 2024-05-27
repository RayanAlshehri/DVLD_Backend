using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsTheoryTestQuestion
    {
        private enum enMode { Add, Update}

        private enMode _Mode;
        public int ID { get; set; }
        public string QuestionText{ get; set; }
        public string Answer1 {  get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public int NumberOfCorrectAnswer { get; set; }
        public string ImagePath { get; set; }

        public clsTheoryTestQuestion()
        {
            _Mode = enMode.Add;

            ID = -1;
            QuestionText = string.Empty;
            Answer1 = string.Empty;
            Answer2 = string.Empty;
            Answer3 = string.Empty;
            NumberOfCorrectAnswer = -1;
            ImagePath = null;
        }

        public clsTheoryTestQuestion(int QuestionID, string QuestionText, string Answer1, string Answer2, string Answer3, int NumberOfCorrectAnswe, string ImagePath)
        {
            _Mode = enMode.Update;

            this.ID = QuestionID;
            this.QuestionText = QuestionText;
            this.Answer1 = Answer1;
            this.Answer2 = Answer2;
            this.Answer3 = Answer3;
            this.NumberOfCorrectAnswer = NumberOfCorrectAnswe;
            this.ImagePath = ImagePath;
        }

        public static int[] GetAllQuestionsIDs()
        {
            return clsTheoryTestQuestionData.GetAllQuestionsIDsRandomOrder();
        }

        public static clsTheoryTestQuestion GetQuestion(int QuestionID)
        {
            string QuestionText = string.Empty;
            string Answer1 = string.Empty;
            string Answer2 = string.Empty;
            string Answer3 = string.Empty;
            int NumberOfCorrectAnswer = -1;
            string ImagePath = null;

            if (clsTheoryTestQuestionData.GetQuestion(QuestionID, ref QuestionText, ref Answer1, ref Answer2, ref Answer3, ref NumberOfCorrectAnswer, ref ImagePath)) 
            {
                return new clsTheoryTestQuestion(QuestionID, QuestionText, Answer1, Answer2, Answer3, NumberOfCorrectAnswer, ImagePath);
            }

            return null;
        }

        public static int GetNumberOfQuestions()
        {
            return clsTheoryTestQuestionData.GetNumberQuestion();
        }
    }
}
