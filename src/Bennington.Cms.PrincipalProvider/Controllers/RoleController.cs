using System;
using System.Linq;
using System.Web.Mvc;
using Bennington.Cms.Controllers;
using Bennington.Cms.PrincipalProvider.Mappers;
using Bennington.Cms.PrincipalProvider.Models;
using Bennington.Cms.PrincipalProvider.Repositories;
using Bennington.Cms.PrincipalProvider.Services;
using Bennington.ContentTree.Providers.SectionNodeProvider.Repositories;
using Bennington.Core.Helpers;

namespace Bennington.Cms.PrincipalProvider.Controllers
{
    [Authorize]
    public class RoleController : ListManageController<Role, RoleInputModel>
    {
        private readonly IRoleRepository roleRepository;
        private readonly IRoleToRoleInputModelMapper roleToRoleInputModelMapper;
        private readonly IGuidGetter guidGetter;
        private readonly IProcessRoleInputModelService processRoleInputModelService;
        private readonly IModuleRepository moduleRepository;
        private readonly IContentTreeSectionNodeRepository contentTreeSectionNodeRepository;

        public RoleController(IRoleRepository roleRepository,
            IRoleToRoleInputModelMapper roleToRoleInputModelMapper,
            IGuidGetter guidGetter,
            IProcessRoleInputModelService processRoleInputModelService,
            IModuleRepository moduleRepository,
            IContentTreeSectionNodeRepository contentTreeSectionNodeRepository)
        {
            this.roleRepository = roleRepository;
            this.roleToRoleInputModelMapper = roleToRoleInputModelMapper;
            this.guidGetter = guidGetter;
            this.processRoleInputModelService = processRoleInputModelService;
            this.moduleRepository = moduleRepository;
            this.contentTreeSectionNodeRepository = contentTreeSectionNodeRepository;
        }

        protected override IQueryable<Role> GetListItems(Core.List.ListViewModel listViewModel)
        {
            return roleRepository.GetAll().AsQueryable();
        }

        public override RoleInputModel GetFormById(object id)
        {
            var roleInputModel = roleToRoleInputModelMapper.CreateInstance(roleRepository.GetAll().Where(a => a.Id == id.ToString()).FirstOrDefault());
            roleInputModel.Modules = moduleRepository.GetAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id }).OrderBy(x => x.Text);
            roleInputModel.ContentSections = contentTreeSectionNodeRepository.GetAllContentTreeSectionNodes().Select(x => new SelectListItem { Text = x.Name, Value = x.Id }).OrderBy(x => x.Text);
            return roleInputModel;
        }

        protected override RoleInputModel CreateForm()
        {
            var createForm = base.CreateForm();
            createForm.Modules = moduleRepository.GetAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id }).OrderBy(x => x.Text);
            createForm.ContentSections = contentTreeSectionNodeRepository.GetAllContentTreeSectionNodes().Select(x => new SelectListItem { Text = x.Name, Value = x.Id }).OrderBy(x => x.Text);

            return createForm;
        }

        public override void InsertForm(RoleInputModel form)
        {
            form.Id = guidGetter.GetGuid().ToString();
            processRoleInputModelService.ProcessAndReturnId(form);
            base.InsertForm(form);
        }

        public override void UpdateForm(RoleInputModel form)
        {
            processRoleInputModelService.ProcessAndReturnId(form);
            base.UpdateForm(form);
        }

        public override ActionResult Delete(object id)
        {
            roleRepository.Delete(GetIdForDelete(id));
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
    }
}