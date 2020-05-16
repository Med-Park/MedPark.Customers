using MedPark.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPark.CustomersService.Domain
{
    public class MedicalScheme : BaseIdentifiable
    {
        public MedicalScheme(Guid id) : base(id)
        {

        }

        public string SchemeName { get; set; }

        public void UpdatedModifiedDate()
            => UpdatedModified();
    }
}
