using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.AutoMapper;
using Abp.Runtime.Validation;

namespace Adbp.Zero.Authorization.Roles.Dto
{
    [AutoMap(typeof(Role))]
    public class UpdateRoleDto : EntityDto<int>, ICustomValidate
    {
        [StringLength(AbpRoleBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpRoleBase.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        [StringLength(Role.MaxDescriptionLength)]
        public string Description { get; set; }

        public bool IsStatic { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (!IsStatic)
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    context.Results.Add(new ValidationResult("Role name is required!"));
                }
            }
        }
    }
}
