using MedPark.Common;
using MedPark.Common.Handlers;
using MedPark.CustomersService.Domain;
using MedPark.CustomersService.Model;
using MedPark.CustomersService.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPark.CustomersService.Handlers.Customers
{
    public class GetCustomerMedicalSchemesHandler : IQueryHandler<GetCustomerMedicalSchemes, List<CustomerMedicalSchemeItem>>
    {
        private IMedParkRepository<CustomerMedicalScheme> _customerSchemesRepo { get; }
        private IMedParkRepository<MedicalScheme> _schemesRepo { get; }

        public GetCustomerMedicalSchemesHandler(IMedParkRepository<CustomerMedicalScheme> customerSchemesRepo, IMedParkRepository<MedicalScheme> schemesRepo)
        {
            _customerSchemesRepo = customerSchemesRepo;
            _schemesRepo = schemesRepo;
        }

        public async Task<List<CustomerMedicalSchemeItem>> HandleAsync(GetCustomerMedicalSchemes query)
        {
            var schemes = await _schemesRepo.GetAllAsync();
            var customerSchemes = await _customerSchemesRepo.FindAsync(x => x.CustomerId == query.CustomerId);

            List<CustomerMedicalSchemeItem> result = new List<CustomerMedicalSchemeItem>();

            customerSchemes.ToList().ForEach(cs =>
            {
                result.Add(new CustomerMedicalSchemeItem
                {
                    CustomerId = query.CustomerId,
                    Active = cs.Active,
                    CustomerMedicalSchemeId = cs.Id,
                    MedicalSchemeName = schemes.Where(x => x.Id == cs.SchemeId).FirstOrDefault().SchemeName,
                    MembershipNo = cs.MembershipNo
                });
            });

            return result;
        }
    }
}
