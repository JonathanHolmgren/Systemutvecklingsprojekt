using Models;

namespace DataLayer;

public class Seed
{
    
        public static void Populate(Context context)
        {
            
            #region CommisionRate 
            Commission commissionRate1 = new Commission(0.12);
            
            #endregion
            
            #region PostalCodeCity 
            PostalCodeCity postalCodeCity1 = new PostalCodeCity("50331","Borås");
            PostalCodeCity postalCodeCity2 = new PostalCodeCity("50251","Borås");
            PostalCodeCity postalCodeCity3 = new PostalCodeCity("50321","Borås");

            #endregion
            
           #region Employee

           Employee employee1 = new Employee("1337", "19850314-1234", "Sten", "Hård", "Vasagatan 12",postalCodeCity3, "Sten.Hård@exempel.se", "VD", "070-123 45 67");
           Employee employee2 = new Employee("9911", "19791230-5678", "Ann-Sofie", "Larsson", "Storgatan 45",postalCodeCity1, "Ann-Sofie.Larsson@exempel.se", "Ekonomiassistent", "070-234 56 78");
           Employee employee3 = new Employee("2322", "19900915-7890", "Iren", "Panik", "Kungsgatan 5", postalCodeCity2,"Iren.Panik@exempel.se", "Försäljningschef", "070-345 67 89");
           
           Employee employee4 = new Employee("6423", "19870123-4567", "Karin", "Sundberg", "Sveavägen 23",postalCodeCity1, "maria.lindgren@exempel.se", "Försäljningsassistent, Innesäljare", "070-456 78 90",commissionRate1);
           Employee employee5 = new Employee("2547", "19950530-6789", "Irene", "Johansson", "Norrlandsgatan 8",postalCodeCity3, "oskar.berg@exempel.se", "Innesäljare", "070-567 89 01",commissionRate1);
           Employee employee6 = new Employee("2447", "19881120-3456", "Vigo", "Persson", "Drottninggatan 14",postalCodeCity2, "sofia.eriksson@exempel.se", "Innesäljare", "070-678 90 12",commissionRate1);
           
           Employee employee7 = new Employee("5836", "19950530-6789", "Birgitta", "Frisk", "Norrlandsgatan 8",postalCodeCity1, "oskar.berg@exempel.se", "Utesäljare", "070-567 89 01",commissionRate1);
           Employee employee8 = new Employee("2264", "19881120-3456", "Boris", "Alm", "Drottninggatan 14",postalCodeCity2, "sofia.eriksson@exempel.se", "Utesäljare", "070-678 90 12",commissionRate1);
           Employee employee9 = new Employee("1153", "19930710-1234", "Linda", "Jonsson", "Hamngatan 9",postalCodeCity1, "viktor.nystrom@exempel.se", "Utesäljare", "070-789 01 23",commissionRate1);
           Employee employee10 = new Employee("7473", "19950530-6789", "Malin", "Nilsdotter", "Norrlandsgatan 8",postalCodeCity2, "oskar.berg@exempel.se", "Utesäljare", "070-567 89 01",commissionRate1);
           Employee employee11 = new Employee("4331", "19881120-3456", "Mikael", "Lund", "Drottninggatan 14",postalCodeCity1, "sofia.eriksson@exempel.se", "Utesäljare", "070-678 90 12",commissionRate1);
           Employee employee12 = new Employee("7337", "19930710-1234", "Patrik", "Hedman", "Hamngatan 9",postalCodeCity3, "viktor.nystrom@exempel.se", "Utesäljare", "070-789 01 23",commissionRate1);
           
           
           
           #endregion

           context.CommissionRates.Add(commissionRate1);

           context.PostalCodeCities.Add(postalCodeCity1);
           context.PostalCodeCities.Add(postalCodeCity2);
           context.PostalCodeCities.Add(postalCodeCity3);

           context.Employees.AddRange(employee1, employee2, employee3, employee4,
           employee5, employee6, employee7, employee8,
           employee9, employee10, employee11, employee12
           );
           
           
            context.SaveChanges();
        }
    
}