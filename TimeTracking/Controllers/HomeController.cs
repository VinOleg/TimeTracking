using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TimeTracking.Models;
namespace TimeTracking.Controllers
{
    public class HomeController : Controller
    {
        #region BASECONTROLLER
        ///<summary>
        ///DATABASE
        ///</summary>
        private DBContextMSSQL db;
        ///<summary>
        ///Логгер
        ///</summary>
        private readonly ILogger<HomeController> _logger;
        public HomeController(DBContextMSSQL context, ILogger<HomeController> logger)
        {
            db = context;

            _logger = logger;
        }
        #endregion
        #region USERS
        ///<summary>
        ///Функция отображения главной страницы
        ///</summary>
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Open Index");
            return View(await db.Users.ToListAsync());
        }
        ///<summary>
        ///Функция отображения создания пользователя
        ///</summary>
        public IActionResult Create()
        {
            _logger.LogInformation("Open Create");
            return View();
        }
        ///<summary>
        ///Функция добавления пользователя
        ///</summary>
        [HttpPost]
        public async Task<IActionResult> Create(Users users)
        {
            try {
                _logger.LogInformation("Create User: "+ users.Surname);
                db.Users.Add(users);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");

            }
            catch(Exception ex)
            {
                _logger.LogInformation("Error Create User: " + users.Surname);
                string[] arr = ex.ToString().Split(new char[] { '(', ')' }, StringSplitOptions.None);
                string NumberOfError = arr[1];
                
                if (NumberOfError == "0x80131904")
                {
                    ModelState.AddModelError("Email", "Такой Email уже есть");
                }
                else
                {
                    ModelState.AddModelError("Email", "Ошибка необработана. Код ошибки: "+ NumberOfError+ " Свяжитесь с администратором и скажите ему эту ошибку");
                }

                return View(users); 
            }
        }
        ///<summary>
        ///Функция редактирования пользователя
        ///</summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Users user = await db.Users.FirstOrDefaultAsync(p => p.id == id);
                if (user != null)
                    return View(user);
                _logger.LogInformation("EDIT " +user.Surname);
            }
            _logger.LogInformation("USER IS NULL");
            return NotFound();
        }
        ///<summary>
        ///Функция сохранения редактирования пользователя
        ///</summary>
        [HttpPost]
        public async Task<IActionResult> Edit(Users user)
        {
            db.Users.Update(user);
            await db.SaveChangesAsync();
            _logger.LogInformation("EDIT " + user.Surname);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Users user = await db.Users.FirstOrDefaultAsync(p => p.id == id);
                if (user != null)
                    _logger.LogInformation("Delete " + user.Surname);
                return View(user);
            }
            _logger.LogInformation("USER IS NULL ");
            return NotFound();
        }
        ///<summary>
        ///Функция удаления пользователя
        ///</summary>
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Users user = await db.Users.FirstOrDefaultAsync(p => p.id == id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    _logger.LogInformation("DELETE " + user.Surname);
                    return RedirectToAction("Index");
                }
            }
            _logger.LogInformation("EDIT ERROR USER IS NULL ");
            return NotFound();
        }
        #endregion
        #region REPORTS
        ///<summary>
        ///Функция отображения страницы пользователя
        ///</summary>
        public async Task<IActionResult> OpenUser(int id)
        {
            ViewData["UserID"] = id;
            _logger.LogInformation("OPENUSER WHERE ID " + id);
            return View(await db.DailyWork.Where(p => p.UserID == id).ToListAsync());
        }

        ///<summary>
        ///Функция добавления отчета
        ///</summary>
        public IActionResult CreateReport(int userid)
        {
            ViewData["UserID"] = userid;
            _logger.LogInformation("CREATEREPORT WHERE USERID " + userid);
            return View();
        }
        ///<summary>
        ///Функция сохранения отчета
        ///</summary>
        [HttpPost]
        public async Task<IActionResult> CreateReport(DailyWork report)
        {
            try
            {
                db.DailyWork.Add(report);
                await db.SaveChangesAsync();
                _logger.LogInformation("CREATEREPORT WHERE ID " + report.ID);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Note", ex.ToString());
                _logger.LogInformation("ERROR CREATEREPORT WHERE ID " + report.ID);
                return View(report);
            }
        }
        ///<summary>
        ///Функция подтверждения удаления отчета
        ///</summary>
        [HttpGet]
        [ActionName("DeleteReport")]
        public async Task<IActionResult> ConfirmDeleteReport(int? id)
        {
            if (id != null)
            {
                DailyWork report = await db.DailyWork.FirstOrDefaultAsync(p => p.ID == id);
                if (report != null)
                    _logger.LogInformation("ConfirmDeleteReport WHERE ID " + id);
                return View(report);
            }
            _logger.LogInformation("ERROR ConfirmDeleteReport WHERE ID " + id);
            return NotFound();
        }
        ///<summary>
        ///Функция удаления отчета
        ///</summary>
        [HttpPost]
        public async Task<IActionResult> DeleteReport(int? id)
        {
            if (id != null)
            {
                DailyWork report = await db.DailyWork.FirstOrDefaultAsync(p => p.ID == id);
                if (report != null)
                {
                    db.DailyWork.Remove(report);
                    await db.SaveChangesAsync();
                    _logger.LogInformation("DeleteReport WHERE ID " + id);
                    return RedirectToAction("Index");
                }
            }
            _logger.LogInformation("ERROR DeleteReport WHERE ID " + id);
            return NotFound();
        }

        ///<summary>
        ///Функция редактирования отчета
        ///</summary>
        public async Task<IActionResult> EditReport(int? id)
        {
            if (id != null)
            {
                DailyWork report = await db.DailyWork.FirstOrDefaultAsync(p => p.ID == id);
                if (report != null)
                    _logger.LogInformation("EditReport WHERE ID " + id);
                return View(report);
            }
            _logger.LogInformation("ERROR EditReport WHERE ID " + id);
            return NotFound();
        }
        ///<summary>
        ///Функция сохранения редактирования отчета
        ///</summary>
        [HttpPost]
        public async Task<IActionResult> EditReport(DailyWork report)
        {
            db.DailyWork.Update(report);
            await db.SaveChangesAsync();
            _logger.LogInformation("EditReport WHERE ID " + report.ID);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
