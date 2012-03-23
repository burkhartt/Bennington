using AutoMapperAssist;
using Bennington.Cms.PrincipalProvider.Models;

namespace Bennington.Cms.PrincipalProvider.Mappers
{
    public interface IRoleInputModelToRoleMapper
    {
        Role CreateInstance(RoleInputModel roleInputModel);
    }

    public class RoleInputModelToRoleMapper : Mapper<RoleInputModel, Role>, IRoleInputModelToRoleMapper
    {
        public override void DefineMap(AutoMapper.IConfiguration configuration)
		{
			configuration.CreateMap<RoleInputModel, Role>();
		}
    }
}