using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Manage
{
    public class ModelInfo
    {
        public string Title { get; set; }
        public string UrlPart { get; set; }
        public List<ModelPropertyInfo> Properties { get; set; } = new List<ModelPropertyInfo>();
    }
    public class ModelPropertyInfo
    {
        public bool IsEditorLink { get; set; }
        public string Title { get; set; }
        public string UrlPart { get; set; }
        public string ReferenceModel { get; set; }
        public bool IsReadOnly { get; set; }
        public int Priority { get; set; }
        public bool ShowInList { get; set; }
        public bool ShowInEditor { get; set; }
    }
}
