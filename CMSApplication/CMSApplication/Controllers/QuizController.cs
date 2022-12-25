using CMSApplication.Data.Entity;
using CMSApplication.Enums;
using CMSApplication.Models;
using CMSApplication.Models.DTO;
using CMSApplication.Services.Abstraction;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMSApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly ILogger<QuizController> _logger;

        public QuizController
        (
            IQuizService quizService,
            ILogger<QuizController> logger
        )
        {
            _quizService = quizService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<object> Post(Quiz quiz)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    quiz = await _quizService.addQuiz(quiz);
                         
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", quiz));
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
        public async Task<object> put(Quiz quiz)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    quiz = await _quizService.updateQuiz(quiz);

                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", quiz));
                }

                return await Task.FromResult(new ResponseModel(ResponseCode.Error, null, null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }

        [HttpGet]
        public async Task<object> Get( )
        {
            try
            {
                
                    var list = await _quizService.getQuizzes();

                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", list));
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }

        [HttpGet("{Id}")]
        public async Task<object> GetQuiz(long Id)
        {
            try
            {
                 
                    var item = await _quizService.getQuiz(Id);

                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", item));
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }

        [HttpDelete("{id}")]
        public async Task<object> Delete(long Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                     await _quizService.deleteQuiz(Id);

                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "",null));
                }

                return await Task.FromResult(new ResponseModel(ResponseCode.Error, null, null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }


        [HttpGet("/category/{id}")]
        public async Task<object> getQuizzesOfCategory(long Id)
        {
            try
            {
                var list = await _quizService.getQuizzesOfCategory(new Category(){Id = Id});
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", list));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }

        [HttpGet("/active")]
        public async Task<object> getActiveQuizzes()
        {
            try
            {
                var list = await _quizService.getActiveQuizzes();
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", list));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }


        [HttpGet("/category/active/{Id}")]
        public async Task<object> getActiveQuizzes(long Id)
        {
            try
            {
                var list = await _quizService.getActiveQuizzesOfCategory(new Category(){Id=Id});
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", list));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }


    }
}
