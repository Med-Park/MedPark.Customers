using MedPark.CustomersService.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPark.CustomersService.Config
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<CustomersDbContext>())
                {
                    EnsureSeedData(context);
                }
            }
        }

        private static void EnsureSeedData(CustomersDbContext context)
        {
            Console.WriteLine("Seeding database...");

            Console.WriteLine("Schemes being populated");
            List<MedicalScheme> schemes = GetSchemes(new List<MedicalScheme>());
            foreach (var scheme in schemes)
            {
                if (context.MedicalScheme.Where(x => x.SchemeName == scheme.SchemeName).FirstOrDefault() == null)
                {
                    context.MedicalScheme.Add(scheme);
                    context.SaveChanges();
                }
            }

            Console.WriteLine("Done Seeding Customer database...");
        }

        private static List<MedicalScheme> GetSchemes(List<MedicalScheme> schemes)
        {
            schemes.Add(new MedicalScheme(Guid.NewGuid()) { SchemeName = "Discovery Health" });
            schemes.Add(new MedicalScheme(Guid.NewGuid()) { SchemeName = "Bestmed" });
            schemes.Add(new MedicalScheme(Guid.NewGuid()) { SchemeName = "Bonitas" });
            schemes.Add(new MedicalScheme(Guid.NewGuid()) { SchemeName = "Fedhealth" });
            schemes.Add(new MedicalScheme(Guid.NewGuid()) { SchemeName = "Medihelp" });
            schemes.Add(new MedicalScheme(Guid.NewGuid()) { SchemeName = "Momentum Health" });
            schemes.Add(new MedicalScheme(Guid.NewGuid()) { SchemeName = "Profmed" });
            schemes.Add(new MedicalScheme(Guid.NewGuid()) { SchemeName = "Bankmed" });

            return schemes;
        }
    }
}
