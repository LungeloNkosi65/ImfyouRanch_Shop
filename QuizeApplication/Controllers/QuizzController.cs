using QuizeApplication.Models;
using QuizeApplication.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizeApplication.Controllers
{
    public class QuizzController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();

        // GET: Quizz
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetUser(UserVm user)
        {
            //var userConnected = dbContext.Users.(u => u.FullName == user.FullName)
            //                             .Select(u => new UserVM
            //                             {
            //                                 UserID = u.UserID,
            //                                 FullName = u.FullName,
            //                                 ProfilImage = u.ProfilImage,

            //                             }).FirstOrDefault();

            //if (userConnected != null)
            //{
            //    Session["UserConnected"] = userConnected;
            //    return RedirectToAction("SelectQuizz");
            //}
            //else
            //{
            //    ViewBag.Msg = "Sorry : user is not found !!";
            //    return View();
            //}
            return RedirectToAction("SelectQuizz");
        }


        [HttpGet]
        public ActionResult SelectQuizz()
        {
            QuizVM quiz = new QuizVM();
            quiz.ListOfQuizz = dbContext.Quizs.Select(q => new SelectListItem
            {
                Text = q.QuizName,
                Value=q.QuizId.ToString()
            }).ToList();


            return View(quiz);
        }
        [HttpPost]
        public ActionResult SelectQuizz(QuizVM quiz)
        {
            QuizVM quizSelected = dbContext.Quizs.Where(q => q.QuizId == quiz.QuizID).Select(q => new QuizVM
            {
                QuizID = q.QuizId,
                QuizName = q.QuizName,

            }).FirstOrDefault();

            if (quizSelected != null)
            {
                Session["SelectedQuiz"] = quizSelected;

                return RedirectToAction("QuizTest");
            }

            return View();
        }

        [HttpGet]
        public ActionResult QuizTest()
        {
            QuizVM quizSelected = Session["SelectedQuiz"] as QuizVM;
            IQueryable<QuestionVM> questions = null;

            if (quizSelected != null)
            {
                //questions = dbContext.Questions.Where(q => q.Quiz.QuizID == quizSelected.QuizID)
                //   .Select(q => new QuestionVM
                //   {
                //       QuestionID = q.QuestionID,
                //       QuestionText = q.QuestionText,
                //       Choices = q.Choices.Select(c => new ChoiceVM
                //       {
                //           ChoiceID = c.ChoiceID,
                //           ChoiceText = c.ChoiceText
                //       }).ToList()

                //   }).AsQueryable();
                questions = dbContext.Questions.Where(x => x.QuizId == quizSelected.QuizID).AsQueryable();
            }

            return View(questions);
        }

        [HttpPost]
        public ActionResult QuizTest(List<QuizAnswersVM> resultQuiz)

        {
            List<QuizAnswersVM> finalResultQuiz = new List<Models.ViewModels.QuizAnswersVM>();

            foreach (QuizAnswersVM answser in resultQuiz)
            {
                QuizAnswersVM result = dbContext.Answers.Where(a => a.QuestionId == answser.QuestionID).Select(a => new QuizAnswersVM
                {
                    QuestionID = a.QuestionId,
                    AnswerQ = a.AnswerText,
                    isCorrect = (answser.AnswerQ.ToLower().Equals(a.AnswerText.ToLower()))

                }).FirstOrDefault();

                finalResultQuiz.Add(result);
            }

            return Json(new { result = finalResultQuiz }, JsonRequestBehavior.AllowGet);
        }
    }
}