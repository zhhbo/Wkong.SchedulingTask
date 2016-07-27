using System.Linq;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using System.Collections.Generic;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Wkong.SchedulingTask.Models;
using Wkong.SchedulingTask.Services;
using Orchard.Mvc;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using Orchard;
using Orchard.Forms.Services;
using Wkong.SchedulingTask.ViewModels;
using Orchard.Utility;
using System.Web;
using Orchard.Localization.Services;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace Wkong.SchedulingTask.Controllers {
    [Admin]
    [OrchardFeature("Wkong.SchedulingTask.UI")]
    public class AdminController : Controller {
        private readonly ISchedulingTaskManager _schedulingTaskManager;
        private readonly IOrchardServices _services;
        private readonly ISchedulingTaskProcessor _SchedulingTaskProcessor;
        private readonly ISchedulingTaskService _schedulingTaskService;
        private readonly IFormManager _formManager;
        private readonly IDateLocalizationServices _dateLocalizationServices;
        public AdminController(
            ISchedulingTaskManager schedulingTaskManager, 
            IShapeFactory shapeFactory,
            IOrchardServices services, 
            ISchedulingTaskService schedulingTaskService,
            IFormManager formManager,
            IDateLocalizationServices dataLocalizationServices,
            ISchedulingTaskProcessor schedulingTaskProcessor) {
            _schedulingTaskManager = schedulingTaskManager;
            _services = services;
            _SchedulingTaskProcessor = schedulingTaskProcessor;
            _schedulingTaskService = schedulingTaskService;
            New = shapeFactory;
            T = NullLocalizer.Instance;
            _dateLocalizationServices = dataLocalizationServices;
            _formManager = formManager;
        }

        public dynamic New { get; set; }
        public Localizer T { get; set; }

        public ActionResult List(PagerParameters pagerParameters) {
            var pager = new Pager(_services.WorkContext.CurrentSite, pagerParameters);

            var jobsCount = _schedulingTaskManager.GetTasksCount();
            var jobs = _schedulingTaskManager.GetAllTask(pager.GetStartIndex(), pager.PageSize).ToList();
            var model = _services.New.ViewModel()
                .Pager(_services.New.Pager(pager).TotalItemCount(jobsCount))
                .Jobs(jobs)
                ;

            return View(model);
        }

        public ActionResult AddTask()
        {          
            var allTasks= _schedulingTaskManager.GetSchedulingTasks();
            var model = new SchedulingTaskViewModel()
            {
                SchedulingTasks = allTasks.Select(x => new SchedulingTaskEntry()
                {
                    Category = x.Category.Text,
                    Description = x.Description.Text
                    ,
                    MessageName = x.MessageName,
                    Selected = false,
                    TaskName = x.Name
                }).ToList(),

            };
            return View(model);
            
        }
         [HttpPost, ActionName("AddTask")]
        public ActionResult AddTask(SchedulingTaskViewModel model )
         {
             var selectedTask = model.SchedulingTasks.FirstOrDefault(x=>x.Selected);
             if (selectedTask != null)
             {
                 var utcDateTime = _dateLocalizationServices.ConvertFromLocalizedString(model.Date, model.Time);
                 _schedulingTaskService.Enqueue(model.TaskName, selectedTask.MessageName, model.Priority, utcDateTime.Value, model.Frequency, model.SpaceNum);
               
             }
             return RedirectToAction("List");
         }
         public ActionResult PauseTask(int Id)
         {
             var task = _schedulingTaskManager.GetTask(Id);
             task.CanExecute = false;
             _schedulingTaskService.EditTask(task);
             return RedirectToAction("List");

         }
         public ActionResult ResumeTask(int Id)
         {
             var task = _schedulingTaskManager.GetTask(Id);
             task.CanExecute = true;             
             _schedulingTaskService.EditTask(task);
             return RedirectToAction("List");

         }
         public ActionResult EditTask(int Id)
         {
             var task = _schedulingTaskManager.GetTask(Id);
             var allTasks = _schedulingTaskManager.GetSchedulingTasks();
             var model = new SchedulingTaskViewModel()
             {
                 SchedulingTasks = allTasks.Select(x => new SchedulingTaskEntry()
                 {
                     Category = x.Category.Text,
                     Description = x.Description.Text
                     ,
                     MessageName = x.MessageName,
                     Selected = false,
                     TaskName = x.Name
                 }).ToList(),

             };
             model.Id = task.Id;
             model.Priority = task.Priority;
             model.SpaceNum = task.SpaceNum;
             model.TaskName = task.TaskName;
             model.Frequency = task.Frequency;
             model.Date = _dateLocalizationServices.ConvertToLocalizedDateString(task.ScheduledUtc);
             model.Time = _dateLocalizationServices.ConvertToLocalizedTimeString(task.ScheduledUtc);
             return View(model);

         }
         [HttpPost, ActionName("EditTask")]
         public ActionResult EditTask(SchedulingTaskViewModel model)
         {
             var task = _schedulingTaskManager.GetTask(model.Id);
             var utcDateTime = _dateLocalizationServices.ConvertFromLocalizedString(model.Date, model.Time);

             task.ScheduledUtc = utcDateTime.Value;
             task.Priority = model.Priority;
             task.SpaceNum = model.SpaceNum;
             task.TaskName = model.TaskName;
             task.Frequency = model.Frequency;
             _schedulingTaskService.EditTask(task);
             return RedirectToAction("List");
         }
         public ActionResult EditTaskParameters(int id)
         {

             var model = _schedulingTaskManager.GetTask(id);

             var task = _schedulingTaskManager.GetSchedulingTaskByMessageName(model.Message);

             if (task == null)
             {
                 return HttpNotFound();
             }
             var form = task.Form == null ? null : _formManager.Build(task.Form);
             if (!string.IsNullOrEmpty(model.Parameters))
             {
                 var parameters =model.Parameters.ToDic();//FormParametersHelper.FromJsonString();
                 var para1=(IDictionary<string, object>)parameters["parameters"];
                 var par =new Dictionary<string,string>(); //;
                 foreach(string key in para1.Keys)
                 {
                     par.Add(key,Convert.ToString(para1[key]));
                 }

                 _formManager.Bind(form, new DictionaryValueProvider<string>(par, System.Globalization.CultureInfo.InvariantCulture));
             }
             var viewModel = New.ViewModel(Id: id, Form: form, State: model.Parameters);

             return View(viewModel);
         }
         [HttpPost, ActionName("EditTaskParameters")]
         [FormValueRequired("_submit.Save")]
         public ActionResult EditTaskParameters(int Id,  FormCollection formValues)
         {
             var name=_schedulingTaskManager.GetTask(Id).Message;
             var task = _schedulingTaskManager.GetSchedulingTaskByMessageName(name);

             if (task == null)
             {
                 return HttpNotFound();
             }

             _formManager.Validate(new ValidatingContext { FormName = task.Form, ModelState = ModelState, ValueProvider = ValueProvider });

             if (!ModelState.IsValid)
             {

                 var form = task.Form == null ? null : _formManager.Build(task.Form);

                 _formManager.Bind(form, ValueProvider);
                 var viewModel = New.ViewModel(Id: Id, Form: form);

                 return View(viewModel);
             }
              string  value = FormParametersHelper.ToJsonString(formValues);
              _schedulingTaskService.EditTask(Id, JsonConvert.DeserializeObject(value));
             return RedirectToAction("List" );
         }

    }
}