using Binxio.Abstractions;
using Binxio.Common.Manage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Binxio.Common.Projects
{
    public class ProjectCreateModel : CreateModel
    {

        [DisplayName("Client")]
        [Required]
        [DefaultValue(XioDefaultValue.ClientId)]
        [HideUnless(HideUnless.MoreThanOneOption)]
        [XioEditor(PropertyEditor.Client)]
        [Options("Client")]
        public long ClientId { get; set; }

        [DisplayName("Project Title")]
        [Required]
        public string Title { get; set; }
    }
}
