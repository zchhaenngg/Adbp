using System;
using System.Collections.Generic;
using System.Text;

namespace Adbp.Web.Models
{
    public class StaticResource
    {
        /// <summary>
        /// 有些资源是不支持压缩的，当然以后也许会被支持
        /// </summary>
        public bool CanBundle { get; set; }

        public StaticResourceType ResourceType { get; set; }

        public string Tag { get; set; }

        public string VirtualPath { get; set; }

        public StaticResource(StaticResourceType resourceType, string virtualPath, bool canBundle = true)
        {
            CanBundle = canBundle;
            ResourceType = resourceType;
            VirtualPath = virtualPath;
        }
    }

    public enum StaticResourceType
    {
        CSS = 1,
        JS = 2
    }
}
