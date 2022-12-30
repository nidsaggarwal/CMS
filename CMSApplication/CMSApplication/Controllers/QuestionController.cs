using CMSApplication.Data.Entity;
using CMSApplication.Enums;
using CMSApplication.Models;
using CMSApplication.Services.Abstraction;
using CMSApplication.Services.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMSApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IQuizService _quizService;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController
        (
            IQuestionService questionService,
            IQuizService quizService,
            ILogger<QuestionController> logger
        )
        {
            _questionService = questionService;
            _quizService = quizService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<object> Post(Question question)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    question = await _questionService.addQuestion(question);

                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", question));
                }

                return await Task.FromResult(new ResponseModel(ResponseCode.Error, null, null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }

        [HttpPut]
        public async Task<object> put(Question question)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    question = await _questionService.updateQuestion(question);

                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", question));
                }

                return await Task.FromResult(new ResponseModel(ResponseCode.Error, null, null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }

        [HttpGet("/api/Question/{Id}")]
        public async Task<object> getQuestionsOfQuiz(long Id)
        {
            try
            {

                var item = await _questionService.getQuestionsOfQuiz(new Quiz(){Id = Id});

                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", item));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }

        [HttpGet("/api/Question/all/{qid}")]
        public async Task<object> getQuestionsOfQuizAdmin(long qid)
        {
            try
            {

                var item = await _questionService.getQuestionsOfQuiz(new Quiz() { Id = qid });

                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", item));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }

        [HttpGet("/{quesId}")]
        public async Task<object> get(long quesId)
        {
            try
            {

                var item = await _questionService.getQuestion(quesId);

                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", item));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }



        }


        [HttpDelete("{Id}")]
        public async Task<object> Delete(long Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _questionService.deleteQuestion(Id);

                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", null));
                }

                return await Task.FromResult(new ResponseModel(ResponseCode.Error, null, null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }

        [HttpPost("/api/Question/eval-quiz")]
        public async Task<object> evalQuiz(List<Question> questions)
        {
            try
            {
                double marksGot = 0;
                int correctAnswers = 0;
                int attempted = 0;

                foreach (var item in questions)
                {
                    var question = await _questionService.getQuestion(item.QuizID);
                    var quiz = await _quizService.addQuiz(new Quiz(){ Id = question.QuizID } );

                    if (question.answer.Equals(question.givenAnswer))
                    {
                        correctAnswers++;

                        double marksSingle = double.Parse(quiz.maxMarks) / questions.Count; 
                        marksGot += marksSingle;

                    }

                    if (question.givenAnswer != null)
                    {
                        attempted++;
                    }
                }

                Dictionary<string, object> map = new Dictionary<string, object>()
                {
                    { "marksGot", marksGot },
                    { "correctAnswers", correctAnswers },
                    { "attempted", attempted },
                };

                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", map));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }


    }
}
