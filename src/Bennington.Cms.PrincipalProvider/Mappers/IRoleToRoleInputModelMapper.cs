using AutoMapperAssist;
using Bennington.Cms.PrincipalProvider.Models;

namespace Bennington.Cms.PrincipalProvider.Mappers
{
    public interface IRoleToRoleInputModelMapper
    {
        RoleInputModel CreateInstance(Role firstOrDefault);
    }

    public class RoleToRoleInputModelMapper : Mapper<Role, RoleInputModel>, IRoleToRoleInputModelMapper
    {
        public override void DefineMap(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Role, RoleInputModel>();
            //.ForMember(a => a.Password, b => b.Ignore())
            //.ForMember(a => a.ConfirmPassword, b => b.Ignore());
        }
    }
}