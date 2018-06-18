using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Adbp.Sample.Guests.Dto
{
    [AutoMapFrom(typeof(Guest))]
    public class GuestDto : EntityDto<long>
    {
        /// <summary>
        /// 嘉宾姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 中文拼音
        /// </summary>
        public string PinYin { get; set; }

        /// <summary>
        /// 是否为女性
        /// </summary>
        public bool? IsFemale { get; set; }

        /// <summary>
        /// 院长、主任、专家
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 单位/公司
        /// </summary>
        public string Company { get; set; }
        
        public string Description { get; set; }
        
        public string Telephone { get; set; }
        
        public string Guid { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
