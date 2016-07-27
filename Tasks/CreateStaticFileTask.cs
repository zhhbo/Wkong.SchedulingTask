using System.Collections.Generic;
using Orchard.Localization;
using Wkong.SchedulingTask.Models;
using Orchard;
using Orchard.Events;
using Wkong.SchedulingTask.Services;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using System.Linq;
namespace Wkong.SchedulingTask.Tasks
{
    public interface ICreateStaticFile : IEventHandler
    {
        void CreateJob(IDictionary<string, object> parameters);
    }
    public class CreateStaticFileTask : ISchedulingTask, ICreateStaticFile
    {
        private readonly IContentManager _contentManger;
        private readonly IStaticService _staticService;
        private readonly IWorkContextAccessor _wca;
        public CreateStaticFileTask(
             IStaticService staticService,
            IWorkContextAccessor wca
            )
        {
            _wca = wca;
            T = NullLocalizer.Instance;
            _contentManger = _wca.GetContext().Resolve<IContentManager>();
            _staticService = staticService;
        }
        public string Name { get { return "静态化页面"; } }
        public LocalizedString Category { get { return T("静态化"); } }
        public LocalizedString Description { get { return T("生成静态页面"); } }

        public string Form { get { return "CreateStaticFileTaskForm"; } }
        public string MessageName { get { return "ICreateStaticFile.CreateJob"; } }

        public Localizer T { get; set; }
        public void CreateJob(IDictionary<string, object> parameters)
        {
            var contentTypes = parameters["contentTypes"].ToString() ; 
            string[] cTypes = contentTypes.Split(',');


           
            var BeginId = int.Parse(parameters["BeginId"].ToString() );
            var EndId = int.Parse(parameters["EndId"].ToString());
            IEnumerable<AutoroutePart> test1;
            if (string.IsNullOrEmpty(contentTypes))
            {
                test1 = _contentManger.List<AutoroutePart>().Where(x => x.Id >= BeginId && x.Id < EndId);
            }
            else
            {
                test1 = _contentManger.List<AutoroutePart>(cTypes).Where(x => x.Id >= BeginId && x.Id < EndId);
            }
            var urls = test1.Select(x => new { Id = x.Id, Path = "/" + x.Path });
            foreach (var entry in urls)
                {
                    try
                    {

                        _staticService.CreateFile(entry.Path);
                    }
                    catch { continue; }
                }
            }


    }
}
