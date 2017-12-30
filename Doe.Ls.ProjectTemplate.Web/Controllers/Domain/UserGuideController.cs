using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Settings;

namespace Doe.Ls.ProjectTemplate.Web.Controllers.Domain
    {
    public class UserGuideController : AppControllerBase
        {
        public string UserGuidePath => ProjectTemplateSettings.Site.GetUserGuidePath();
        public ActionResult Index()
            {

            return RedirectToAction("Display", new { page = "index" });
            }

        [Authorize(Roles = Enums.UserRoleValues.Administrator+", "+ Enums.UserRoleValues.PowerUser+", "+ Enums.UserRoleValues.SystemAdministrator)]

        public ActionResult List()
        {
            var dir = UserGuidePath;

            var di = new DirectoryInfo(dir);
            var model = di.GetFiles("*.html");

            return View(model);
            }


        public ActionResult Display(string page)
            {
            page = ClearText(page);
            var model = GetPageContentModel(page);
            if(string.IsNullOrWhiteSpace(model.Content))
                {
                throw new HttpException(404, $"This page {page} is not found");

                }
            return View(model);
            }
        [Authorize(Roles = Enums.UserRoleValues.Administrator + ", " + Enums.UserRoleValues.PowerUser + ", " + Enums.UserRoleValues.SystemAdministrator)]
        public ActionResult Edit(string page)
            {

            page = ClearText(page);
            var model = EditPageContentModel(page);
            if(string.IsNullOrWhiteSpace(model.Content))
                {
                throw new HttpException(404, string.Format("This page {0} is not found", page));

                }
            return View(model);
            }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = Enums.UserRoleValues.Administrator+", "+ Enums.UserRoleValues.PowerUser+", "+ Enums.UserRoleValues.SystemAdministrator)]

        public ActionResult Edit(PageModel pageModel)
            {
            pageModel = ClearText(pageModel);
            if (!Directory.Exists(UserGuidePath))
            {
                Directory.CreateDirectory(UserGuidePath);
            }
            var path = Path.Combine(UserGuidePath, pageModel.Name + ".html");
            System.IO.File.WriteAllText(path, pageModel.Content);
            return RedirectToAction("Display", new { page = pageModel.Name });
            }
        [Authorize(Roles = Enums.UserRoleValues.Administrator+", "+ Enums.UserRoleValues.PowerUser+", "+ Enums.UserRoleValues.SystemAdministrator)]

        public ActionResult Delete(string page)
            {
            var model = EditPageContentModel(page);
            if(string.IsNullOrWhiteSpace(model.Content))
                {
                throw new HttpException(404, string.Format("This page {0} is not found", page));

                }
            return View(model);
            }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = Enums.UserRoleValues.Administrator+", "+ Enums.UserRoleValues.PowerUser+", "+ Enums.UserRoleValues.SystemAdministrator)]

        public ActionResult Delete(PageModel pageModel)
            {
            pageModel = ClearText(pageModel);
            var path = Path.Combine(UserGuidePath, pageModel.Name + ".html");
            System.IO.File.Delete(path);
            return RedirectToAction("List");
            }


        private PageModel GetPageContentModel(string page)
            {
            page = ClearText(page);

            var path = string.Empty;
            if(string.IsNullOrWhiteSpace(page))
                {
                page = "index";
                }
            path = Path.Combine(UserGuidePath, page + ".html");
            string content = null;
            if(System.IO.File.Exists(path))
                {
                content = System.IO.File.ReadAllText(path).Trim();
                content = ResolveLinks(content);
                }
            if(string.IsNullOrWhiteSpace(content)) content = "New page";

            var result =

            new PageModel
                {
                Title = page.Wordify(),
                Name = page,
                Content = content
                };
            ClearText(result);

            return result;
            }

        private string ResolveLinks(string content)
            {
            const string pattern = @"\[\[(.*?)\]]";
            string query = content;
            var matches = Regex.Matches(query, pattern);

            foreach(Match m in matches)
                {
                var expression = m.Groups[0].Value;
                var val = m.Groups[1];
                var pageExist = System.IO.File.Exists(Path.Combine(UserGuidePath, val.ToString() + ".html"));

                var newExpression = string.Format("<a href='Display?page={0}' class='{1}'>{0}</a>", val, !pageExist ? "new-page" : "");
                content = content.Replace(expression, newExpression);
                }


            return content;
            }

        private PageModel EditPageContentModel(string page)
            {
            ClearText(page);
            string path;
            if(string.IsNullOrWhiteSpace(page))
                {
                path = Path.Combine(UserGuidePath, "index.html");
                }
            else
                {
                path = Path.Combine(UserGuidePath, page + ".html");
                }
            var content = string.Empty;
            if(System.IO.File.Exists(path))
                {
                content = System.IO.File.ReadAllText(path);
                }
            if(string.IsNullOrWhiteSpace(content)) content = "New page";
            return new PageModel
                {
                Title = page.Wordify(),
                Name = page,
                Content = content
                };
            }

        private T ClearText<T>(T page) where T : class
            {
            if(typeof(T) == typeof(String))
                {
                string val = (page as string).Trim();
                return val as T;
                }

            if(typeof(T) == typeof(PageModel))
                {
                var pageModel = page as PageModel;
                if(pageModel != null)
                    {
                    pageModel.Name = pageModel.Name?.Trim() ?? string.Empty;
                    pageModel.Title = pageModel.Title?.Trim() ?? string.Empty;
                    pageModel.Content = pageModel.Content?.Trim() ?? string.Empty;

                    return pageModel as T;
                    }
                }

            throw new InvalidOperationException($"This type {typeof(T).Name} is not supported");

            }
        }
    }
