using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HHTravel.Site.Models.OrderWizard
{
    public interface IStepModel
    {
        int StepNo { get; }
    }
}