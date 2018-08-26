using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace CompanyName.ProjectName.Guests.Dto
{
    [AutoMapTo(typeof(Guest))]
    public class CreateGuestDto
    {
        /// <summary>
        /// 嘉宾姓名
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Name { get; set; }

        /// <summary>
        /// 中文拼音
        /// </summary>
        [StringLength(10)]
        public string PinYin { get; set; }

        /// <summary>
        /// 是否为女性
        /// </summary>
        public bool? IsFemale { get; set; }

        /// <summary>
        /// 院长、主任、专家
        /// </summary>
        [StringLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// 单位/公司
        /// </summary>
        [StringLength(100)]
        public string Company { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(Guest.MaxTelephoneLength)]
        public string Telephone { get; set; }
    }
}
