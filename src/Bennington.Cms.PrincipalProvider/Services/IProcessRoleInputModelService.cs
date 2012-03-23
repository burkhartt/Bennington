using System;
using System.Linq;
using Bennington.Cms.PrincipalProvider.Mappers;
using Bennington.Cms.PrincipalProvider.Models;
using Bennington.Cms.PrincipalProvider.Repositories;

namespace Bennington.Cms.PrincipalProvider.Services
{
    public interface IProcessRoleInputModelService
    {
        string ProcessAndReturnId(RoleInputModel roleInputModel);
    }

    public class ProcessRoleInputModelService : IProcessRoleInputModelService
    {
        private readonly IRoleRepository roleRepository;
        private readonly IRoleInputModelToRoleMapper roleInputModelToRoleMapper;

        public ProcessRoleInputModelService(IRoleRepository roleRepository,
            IRoleInputModelToRoleMapper roleInputModelToRoleMapper)
        {
            this.roleRepository = roleRepository;
            this.roleInputModelToRoleMapper = roleInputModelToRoleMapper;
        }

        public string ProcessAndReturnId(RoleInputModel roleInputModel)
        {
            var role = roleInputModelToRoleMapper.CreateInstance(roleInputModel);

            return roleRepository.SaveAndReturnId(role);
        }
    }
}