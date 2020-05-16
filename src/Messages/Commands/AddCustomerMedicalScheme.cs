using MedPark.Common.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPark.CustomersService.Messages.Commands
{
    public class AddCustomerMedicalScheme : ICommand
    {
        public Guid CustomerId { get; private set; }
        public Guid MedicalSchemeId { get; private set; }
        public string MembershipNo { get; private set; }

        [JsonConstructor]
        public AddCustomerMedicalScheme(Guid customerid, Guid medicalschemeid, string membershipNo)
        {
            CustomerId = customerid;
            MedicalSchemeId = medicalschemeid;
            MembershipNo = membershipNo;
        }
    }
}
