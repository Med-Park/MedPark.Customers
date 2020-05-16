using MedPark.Common.Types;
using MedPark.CustomersService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPark.CustomersService.Queries
{
    public class GetCustomerMedicalSchemes : IQuery<List<CustomerMedicalSchemeItem>>
    {
        public Guid CustomerId { get; set; }
    }
}
