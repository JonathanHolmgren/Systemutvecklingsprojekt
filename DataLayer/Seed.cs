using Models;
using System;

namespace DataLayer;

public class Seed
{
    public static void Populate(Context context)
    {
        #region CommisionRate
        Commission commissionRate1 = new Commission(0.12);

        #endregion

        #region PostalCodeCity
        PostalCodeCity postalCodeCity1 = new PostalCodeCity("50331", "Borås");
        PostalCodeCity postalCodeCity2 = new PostalCodeCity("50251", "Borås");
        PostalCodeCity postalCodeCity3 = new PostalCodeCity("50321", "Borås");

        #endregion

        #region Employee

        Employee employee1 = new Employee(
            "1337",
            "19850314-1234",
            "Sten",
            "Hård",
            "Vasagatan 12",
            postalCodeCity3,
            "Sten.Hård@exempel.se",
            "VD",
            "070-123 45 67"
        );
        Employee employee2 = new Employee(
            "9911",
            "19791230-5678",
            "Ann-Sofie",
            "Larsson",
            "Storgatan 45",
            postalCodeCity1,
            "Ann-Sofie.Larsson@exempel.se",
            "Ekonomiassistent",
            "070-234 56 78"
        );
        Employee employee3 = new Employee(
            "2322",
            "19900915-7890",
            "Iren",
            "Panik",
            "Kungsgatan 5",
            postalCodeCity2,
            "Iren.Panik@exempel.se",
            "Försäljningschef",
            "070-345 67 89"
        );

        Employee employee4 = new Employee(
            "6423",
            "19870123-4567",
            "Karin",
            "Sundberg",
            "Sveavägen 23",
            postalCodeCity1,
            "maria.lindgren@exempel.se",
            "Försäljningsassistent, Innesäljare",
            "070-456 78 90",
            commissionRate1
        );
        Employee employee5 = new Employee(
            "2547",
            "19950530-6789",
            "Irene",
            "Johansson",
            "Norrlandsgatan 8",
            postalCodeCity3,
            "oskar.berg@exempel.se",
            "Innesäljare",
            "070-567 89 01",
            commissionRate1
        );
        Employee employee6 = new Employee(
            "2447",
            "19881120-3456",
            "Vigo",
            "Persson",
            "Drottninggatan 14",
            postalCodeCity2,
            "sofia.eriksson@exempel.se",
            "Innesäljare",
            "070-678 90 12",
            commissionRate1
        );

        Employee employee7 = new Employee(
            "5836",
            "19950530-6789",
            "Birgitta",
            "Frisk",
            "Norrlandsgatan 8",
            postalCodeCity1,
            "oskar.berg@exempel.se",
            "Utesäljare",
            "070-567 89 01",
            commissionRate1
        );
        Employee employee8 = new Employee(
            "2264",
            "19881120-3456",
            "Boris",
            "Alm",
            "Drottninggatan 14",
            postalCodeCity2,
            "sofia.eriksson@exempel.se",
            "Utesäljare",
            "070-678 90 12",
            commissionRate1
        );
        Employee employee9 = new Employee(
            "1153",
            "19930710-1234",
            "Linda",
            "Jonsson",
            "Hamngatan 9",
            postalCodeCity1,
            "viktor.nystrom@exempel.se",
            "Utesäljare",
            "070-789 01 23",
            commissionRate1
        );
        Employee employee10 = new Employee(
            "7473",
            "19950530-6789",
            "Malin",
            "Nilsdotter",
            "Norrlandsgatan 8",
            postalCodeCity2,
            "oskar.berg@exempel.se",
            "Utesäljare",
            "070-567 89 01",
            commissionRate1
        );
        Employee employee11 = new Employee(
            "4331",
            "19881120-3456",
            "Mikael",
            "Lund",
            "Drottninggatan 14",
            postalCodeCity1,
            "sofia.eriksson@exempel.se",
            "Utesäljare",
            "070-678 90 12",
            commissionRate1
        );
        Employee employee12 = new Employee(
            "7337",
            "19930710-1234",
            "Patrik",
            "Hedman",
            "Hamngatan 9",
            postalCodeCity3,
            "viktor.nystrom@exempel.se",
            "Utesäljare",
            "070-789 01 23",
            commissionRate1
        );

        #endregion

        #region User
        User user1 = new User("försäkring1", AuthorizationLevel.SalesPerson, employee9);
        User user2 = new User("försäkring2", AuthorizationLevel.SalesPerson, employee10);
        User user3 = new User("försäkring3", AuthorizationLevel.EconomyAssistant, employee2);
        User user4 = new User("försäkring3", AuthorizationLevel.Admin, employee2);
        #endregion

        #region PrivateCustomer
        PrivateCustomer privateCustomer1 = new PrivateCustomer(
            "0706689932",
            "ingalillblommor52@emial.com",
            "Gatuvägen 21",
            postalCodeCity2,
            "19521019-1234",
            "Inga-Lill",
            "Bengtsson"
        );
        #endregion

        #region CompanyCustomer
        CompanyCustomer companyCustomer1 = new CompanyCustomer(
            "0705102355",
            "klas123@email.com",
            "Vägkanten 54",
            postalCodeCity1,
            "771122-1234",
            "Klas Persson",
            "0710192844",
            "Tesla"
        );
        #endregion

        #region InsuredPerson
        InsuredPerson insuredPerson1 = new InsuredPerson("Inga-Lill", "Bengtsson","1952/06/10");
        InsuredPerson insuredPerson2 = new InsuredPerson("Klas", "Persson", "1974/03/19");
        #endregion

        #region InsuranceType
        InsuranceType insuranceType1 = new InsuranceType(
            "Sjuk- och olycksförsäkring för barn (t.o.m 17 års åldern)"
        );
        InsuranceType insuranceType2 = new InsuranceType(
            "Sjuk- och olycksfallsförsäkring för vuxen"
        );
        InsuranceType insuranceType3 = new InsuranceType("Livförsäkring för vuxen");

        InsuranceType insuranceType4 = new InsuranceType("Fastighet och inventarieförsäkring");
        InsuranceType insuranceType5 = new InsuranceType("Fordonsförsäkring");
        InsuranceType insuranceType6 = new InsuranceType("Ansvarsförsäkring");
        #endregion


        #region InsuranceTypeAttribute
        InsuranceTypeAttribute insuranceTypeAttribute1 = new InsuranceTypeAttribute(
            "Datum",
            insuranceType1
        );
        InsuranceTypeAttribute insuranceTypeAttribute2 = new InsuranceTypeAttribute(
            "Grundbelopp",
            insuranceType1
        );
        InsuranceTypeAttribute insuranceTypeAttribute3 = new InsuranceTypeAttribute(
            "Månadspremie",
            insuranceType1
        );
        InsuranceTypeAttribute insuranceTypeAttribute4 = new InsuranceTypeAttribute(
            "Tillval",
            insuranceType1
        );

        InsuranceTypeAttribute insuranceTypeAttribute5 = new InsuranceTypeAttribute(
            "Datum",
            insuranceType2
        );
        InsuranceTypeAttribute insuranceTypeAttribute6 = new InsuranceTypeAttribute(
            "Grundbelopp",
            insuranceType2
        );
        InsuranceTypeAttribute insuranceTypeAttribute7 = new InsuranceTypeAttribute(
            "Månadspremie",
            insuranceType2
        );
        InsuranceTypeAttribute insuranceTypeAttribute8 = new InsuranceTypeAttribute(
            "Tillval 1",
            insuranceType2
        );
        InsuranceTypeAttribute insuranceTypeAttribute9 = new InsuranceTypeAttribute(
            "Tillval 2",
            insuranceType2
        );
        #endregion

        #region Insurance
        Insurance insurance1 = new Insurance(
            DateTime.Today,
            BillingInterval.Månad,
            InsuranceStatus.Preliminary,
            "Snäll kund, ge rabbat",
            user1,
            insuredPerson1,
            privateCustomer1,
            insuranceType1
        );
        Insurance insurance2 = new Insurance(
            DateTime.Today,
            BillingInterval.År,
            InsuranceStatus.Active,
            "Stor kund, vill hålla kvar",
            user2,
            insuredPerson2,
            companyCustomer1,
            insuranceType2
        );
        #endregion

        #region InsuranceSpec
        DateTime dateTime1 = new DateTime(2023 - 01 - 01);
        InsuranceSpec insuranceSpec1 = new InsuranceSpec(
            "2023-01-01",
            insurance1,
            insuranceTypeAttribute1
        );
        InsuranceSpec insuranceSpec2 = new InsuranceSpec(
            "700000",
            insurance1,
            insuranceTypeAttribute2
        );
        InsuranceSpec insuranceSpec3 = new InsuranceSpec(
            "380",
            insurance1,
            insuranceTypeAttribute3
        );
        InsuranceSpec insuranceSpec4 = new InsuranceSpec(
            "100000",
            insurance1,
            insuranceTypeAttribute4
        );

        InsuranceSpec insuranceSpec5 = new InsuranceSpec(
            "2024-01-01",
            insurance2,
            insuranceTypeAttribute5
        );
        InsuranceSpec insuranceSpec6 = new InsuranceSpec(
            "300000",
            insurance2,
            insuranceTypeAttribute6
        );
        InsuranceSpec insuranceSpec7 = new InsuranceSpec(
            "182,5",
            insurance2,
            insuranceTypeAttribute7
        );
        InsuranceSpec insuranceSpec8 = new InsuranceSpec(
            "100000",
            insurance2,
            insuranceTypeAttribute8
        );
        InsuranceSpec insuranceSpec9 = new InsuranceSpec(
            "500",
            insurance2,
            insuranceTypeAttribute9
        );
        #endregion

        context.CommissionRates.Add(commissionRate1);

        context.PostalCodeCities.Add(postalCodeCity1);
        context.PostalCodeCities.Add(postalCodeCity2);
        context.PostalCodeCities.Add(postalCodeCity3);

        context.Employees.AddRange(
            employee1,
            employee2,
            employee3,
            employee4,
            employee5,
            employee6,
            employee7,
            employee8,
            employee9,
            employee10,
            employee11,
            employee12
        );

        context.Users.AddRange(user1, user2, user3, user4);

        context.CompanyCustomers.Add(companyCustomer1);

        context.PrivateCustomers.Add(privateCustomer1);

        context.Insurances.AddRange(insurance1, insurance2);

        context.InsuranceSpecs.AddRange(
            insuranceSpec1,
            insuranceSpec2,
            insuranceSpec3,
            insuranceSpec4,
            insuranceSpec5,
            insuranceSpec6,
            insuranceSpec7,
            insuranceSpec8,
            insuranceSpec9
        );

        context.InsuranceTypeAttributes.AddRange(
            insuranceTypeAttribute1,
            insuranceTypeAttribute2,
            insuranceTypeAttribute3,
            insuranceTypeAttribute4,
            insuranceTypeAttribute5,
            insuranceTypeAttribute6,
            insuranceTypeAttribute7,
            insuranceTypeAttribute8,
            insuranceTypeAttribute9
        );

        context.InsuranceTypes.AddRange(
            insuranceType1,
            insuranceType2,
            insuranceType3,
            insuranceType4,
            insuranceType5,
            insuranceType6
        );

        context.InsuredPersons.AddRange(insuredPerson1, insuredPerson2);

        context.SaveChanges();
    }
}
