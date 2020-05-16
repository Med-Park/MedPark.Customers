using MedPark.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPark.CustomersService.Domain
{
    public class CustomerMedicalScheme : BaseIdentifiable
    {
        public CustomerMedicalScheme(Guid id) : base(id)
        {
            Active = true;
        }

        public Guid CustomerId { get; private set; }
        public Guid SchemeId { get; private set; }
        public string MembershipNo { get; private set; }
        public bool Active { get; private set; }

        public void UpdatedModifiedDate()
            => UpdatedModified();

        public void SetCustomer(Guid schemeId)
        {
            CustomerId = schemeId;
        }

        public void SetScheme(Guid schemeId)
        {
            SchemeId = schemeId;
        }

        public void SetCustomerSchemeMembershipNo(string membershipNo)
        {
            MembershipNo = membershipNo;
        }
    }
}
