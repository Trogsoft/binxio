using Binxio.Common.Clients;
using Binxio.Common.Manage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Binxio.Common.Projects
{
    [XioModel("Project", "project", "Project")]
    public class ProjectModel : XioModel
    {
        public ProjectModel()
        {
        }

        [ReadOnly(true)]
        [Hide(HideFlags.Both)]
        [PropertyType(PropertyType.UrlPart)]
        public string UrlPart { get; set; }

        [DisplayName("Project Name")]
        [EditorLink]
        [PropertyType(PropertyType.DisplayName)]
        public string Title { get; set; }

        [PropertyType(PropertyType.ReferenceType)]
        [ModelReference("client")]
        public ClientModel Client{ get; set; }

    }
}
