using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public enum JobTitle
    {
        [Display(Name = "Software Engineer")]
        SoftwareEngineer,
        [Display(Name = "Web Developer")]
        WebDeveloper,
        [Display(Name = "Data Engineer")]
        DataEngineer,
        [Display(Name = "DevOps Engineer")]
        DevOpsEngineer,
        [Display(Name = "Software Architect")]
        SoftwareArchitect,
        [Display(Name = "Technical Lead")]
        TechnicalLead,
        [Display(Name = "Project Manager")]
        ProjectManager,
        [Display(Name = "Software Tester")]
        SoftwareTester,
        [Display(Name = "Full Stack Developer")]
        FullStackDeveloper,
        [Display(Name = "Front End Engineer")]
        FrontEndEngineer,
        [Display(Name = "Back End Engineer")]
        BackEndEngineer
    }
}
