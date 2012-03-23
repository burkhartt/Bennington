using System.Linq;
using System.Web.Mvc;
using Bennington.Cms.Controllers;
using Bennington.Cms.PrincipalProvider.Mappers;
using Bennington.Cms.PrincipalProvider.Models;
using Bennington.Cms.PrincipalProvider.Repositories;
using Bennington.Cms.PrincipalProvider.Services;
using Bennington.Core.Helpers;

namespace Bennington.Cms.PrincipalProvider.Controllers
{
    [Authorize]
    public class UserController : ListManageController<User, UserInputModel>
    {
    	private readonly IProcessUserInputModelService processUserInputModelService;
    	private readonly IUserRepository userRepository;
    	private readonly IUserToUserInputModelMapper userToUserInputModelMapper;
    	private readonly IGuidGetter guidGetter;
        private readonly IRoleRepository roleRepository;

        public UserController(IProcessUserInputModelService processUserInputModelService,
								IUserRepository userRepository,
								IUserToUserInputModelMapper userToUserInputModelMapper,
								IGuidGetter guidGetter,
            IRoleRepository roleRepository)
    	{
    		this.guidGetter = guidGetter;
    	    this.roleRepository = roleRepository;
    	    this.userToUserInputModelMapper = userToUserInputModelMapper;
    		this.userRepository = userRepository;
    		this.processUserInputModelService = processUserInputModelService;
    	}

        protected override IQueryable<User> GetListItems(Core.List.ListViewModel listViewModel)
        {
            return userRepository.GetAll().AsQueryable();
        }

        protected override UserInputModel CreateForm()
        {
            var form = base.CreateForm();
            form.Roles = roleRepository.GetAll().Where(x => x.Inactive == false).Select(x => new SelectListItem
                                                                                                     {
                                                                                                         Text = x.Name,
                                                                                                         Value = x.Id
                                                                                                     }).OrderBy(x => x.Text);

            return form;
        }

        public override UserInputModel GetFormById(object id)
        {
            var form = userToUserInputModelMapper.CreateInstance(userRepository.GetAll().Where(a => a.Id == id.ToString()).FirstOrDefault());
            form.Roles = roleRepository.GetAll().Where(x => x.Inactive == false).Select(x => new SelectListItem
                                                                                                     {
                                                                                                         Text = x.Name,
                                                                                                         Value = x.Id
                                                                                                     }).OrderBy(x => x.Text);

            return form;
        }

        public override void InsertForm(UserInputModel form)
        {
            form.Id = guidGetter.GetGuid().ToString();
            processUserInputModelService.ProcessAndReturnId(form);
            base.InsertForm(form);
        }

        public override void UpdateForm(UserInputModel form)
        {
            processUserInputModelService.ProcessAndReturnId(form);
            base.UpdateForm(form);
        }

        public override ActionResult Delete(object id)
        {
            userRepository.Delete(GetIdForDelete(id));
            return base.Delete(id);
        }

        private static string GetIdForDelete(object id)
        {
            var idToUse = id as string;
            if (idToUse == null)
            {
                var idArray = id as string[];
                if (idArray == null) return null;
                idToUse = idArray.FirstOrDefault();
            }
            return idToUse;
        }

        //[Authorize]
        //public ActionResult Create()
        //{
        //    return View("Modify", new ModifyViewModel()
        //                                {
        //                                    UserInputModel = new UserInputModel()
        //                                    {
        //                                        Id = guidGetter.GetGuid().ToString()
        //                                    }
        //                                });
        //}

        //[Authorize]
        //[HttpPost]
        //public ActionResult Create(UserInputModel userInputModel)
        //{
        //    return Modify(userInputModel);
        //}

        //[Authorize]
        //public ActionResult Modify(string id)
        //{
        //    var user = userRepository.GetAll().Where(a => a.Id == id).FirstOrDefault();

        //    return View("Modify", modifyViewModelBuilder.BuildViewModel(userToUserInputModelMapper.CreateInstance(user)));
        //}

        //[Authorize]
        //[HttpPost]
        //public ActionResult Modify(UserInputModel userInputModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        processUserInputModelService.ProcessAndReturnId(userInputModel);
        //        var routeValues = new RouteValueDictionary();
        //        routeValues.Add("Controller", "User");
        //        routeValues.Add("Action", "Modify");
        //        routeValues.Add("id", userInputModel.Id);

        //        return new RedirectToRouteResult(routeValues);
        //    }

        //    return View("Modify", modifyViewModelBuilder.BuildViewModel(userInputModel));
        //}

        //[Authorize]
        //public ActionResult Delete(string id)
        //{
        //    var routeValues = new RouteValueDictionary();
        //    routeValues.Add("Controller", "User");
        //    routeValues.Add("Action", "Index");
			
        //    userRepository.Delete(id);

        //    return new RedirectToRouteResult(routeValues);
        //}
    }
}
