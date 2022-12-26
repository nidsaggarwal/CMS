using CMSApplication.Data;
using CMSApplication.Data.Entity;
using CMSApplication.Services.Abstraction;

namespace CMSApplication.Services.Implementation
{
    public class QuestionService : IQuestionService
    {
        private readonly DBContext _context;

        public QuestionService(DBContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Question> addQuestion(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

            return question;
        }

        public async Task<Question> updateQuestion(Question question)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Question>> getQuestions()
        {
            throw new NotImplementedException();
        }

        public async Task<Question> getQuestion(long questionId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Question>> getQuestionsOfQuiz(Quiz quiz)
        {
            throw new NotImplementedException();
        }

        public async Task deleteQuestion(long quesId)
        {
            throw new NotImplementedException();
        }

        public async Task<Question> get(long questionsId)
        {
            throw new NotImplementedException();
        }
    }
}
