using System;
using System.Linq;
using System.Web.Mvc;
using Orchard.ContentManagement.MetaData;
using Orchard.DisplayManagement;
using Orchard.Forms.Services;
using Orchard.Localization;
namespace Wkong.SchedulingTask.Forms
{
    public class CreateStaticFileTaskForm : IFormProvider {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        protected dynamic Shape { get; set; }
        public Localizer T { get; set; }
        public CreateStaticFileTaskForm(
            IShapeFactory shapeFactory,
            IContentDefinitionManager contentDefinitionManager) {
            _contentDefinitionManager = contentDefinitionManager;
            Shape = shapeFactory;
            T = NullLocalizer.Instance;
        }

        public void Describe(DescribeContext context) {
            Func<IShapeFactory, dynamic> form =
                shape => {

                    var f = Shape.Form(
                        Id: "AnyOfContentTypes",
                        _Parts: Shape.SelectList(
                            Id: "contentTypes", Name: "contentTypes",
                            Title: T("内容类型"),
                            Description: T("请选择一个或多个."),
                            Classes: new[] { "required" },
                            Size: 10,
                            Multiple: true
                            ),
                      _BeginId: Shape.TextBox(
                        Id: "beginId", Name: "BeginId",
                        Title: T("开始Id"),
                        Description: T("开始Id."),
                        Classes: new[] { "text medium required" }),
                        _EndId: Shape.TextBox(
                        Id: "endId", Name: "EndId",
                        Title: T("结束Id"),
                        Description: T("结束Id."),
                        Classes: new[] { "text medium required" })   
                        );

                    f._Parts.Add(new SelectListItem { Value = "", Text = T("Any").Text });

                    foreach (var contentType in _contentDefinitionManager.ListTypeDefinitions().OrderBy(x => x.DisplayName)) {
                        f._Parts.Add(new SelectListItem { Value = contentType.Name, Text = contentType.DisplayName });
                    }

                    return f;
                };

            context.Form("CreateStaticFileTaskForm", form);

        }

    }
}