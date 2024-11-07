using DataLayer.Services;
using Models;

namespace DataLayer;

public class Seed
{
    public static void Populate(Context context) //Seeding the database of fake data
    {
        #region CommisionRate
        Commission commissionRate1 = new Commission(0.12);

        #endregion


        #region Employee

        Employee employee1 = new Employee(
            "1337",
            "19850314-1234",
            "Sten",
            "Hård",
            "Vasagatan 12",
            "50100",
            "Borås",
            "Sten.Hård@toppforsakringar.se",
            "VD",
            "070-123 45 67"
        );
        Employee employee2 = new Employee(
            "9911",
            "19791230-5678",
            "Ann-Sofie",
            "Larsson",
            "Storgatan 45",
            "52298",
            "Borås",
            "Ann-Sofie.Larsson@toppforsakringar.se",
            "Ekonomiassistent",
            "070-234 56 78"
        );
        Employee employee3 = new Employee(
            "2322",
            "19900915-7890",
            "Iren",
            "Panik",
            "Kungsgatan 5",
            "51111",
            "Borås",
            "Iren.Panik@toppforsakringar.se",
            "Försäljningschef",
            "070-345 67 89"
        );

        Employee employee4 = new Employee(
            "6423",
            "19870123-4567",
            "Karin",
            "Sundberg",
            "Sveavägen 23",
            "50109",
            "Borås",
            "maria.lindgren@toppforsakringar.se",
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
            "50555",
            "Borås",
            "oskar.berg@toppforsakringar.se",
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
            "51234",
            "Borås",
            "sofia.eriksson@toppforsakringar.se",
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
            "50101",
            "Borås",
            "oskar.berg@toppforsakringar.se",
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
            "58992",
            "Borås",
            "sofia.eriksson@toppforsakringar.se",
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
            "50567",
            "Borås",
            "viktor.nystrom@toppforsakringar.se",
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
            "54321",
            "Borås",
            "oskar.berg@toppforsakringar.se",
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
            "50251",
            "Borås",
            "sofia.eriksson@toppforsakringar.se",
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
            "50331",
            "Borås",
            "viktor.nystrom@toppforsakringar.se",
            "Utesäljare",
            "070-789 01 23",
            commissionRate1
        );

        #endregion

        #region User
        PasswordHasher passwordHasher = new PasswordHasher();
        string hashPassoword = passwordHasher.Hash("password");
        string hashPassowordAdmin = passwordHasher.Hash("admin");

        User user1 = new User(hashPassoword, AuthorizationLevel.CEO, employee1, "CEO1337");
        User user2 = new User(
            hashPassoword,
            AuthorizationLevel.EconomyAssistant,
            employee2,
            "EA9911"
        );
        User user3 = new User(hashPassoword, AuthorizationLevel.SalesManager, employee3, "SC2322");

        User user4 = new User(
            hashPassoword,
            AuthorizationLevel.SalesAssistant,
            employee4,
            "SA6423"
        );
        User user5 = new User(hashPassoword, AuthorizationLevel.SalesPerson, employee4, "SP6423");

        User user6 = new User(hashPassoword, AuthorizationLevel.SalesPerson, employee5, "SP2547");
        User user7 = new User(hashPassoword, AuthorizationLevel.SalesPerson, employee6, "SP2447");
        User user8 = new User(hashPassoword, AuthorizationLevel.SalesPerson, employee7, "SP5836");
        User user9 = new User(hashPassoword, AuthorizationLevel.SalesPerson, employee8, "SP2264");
        User user10 = new User(hashPassoword, AuthorizationLevel.SalesPerson, employee9, "SP1153");
        User user11 = new User(hashPassoword, AuthorizationLevel.SalesPerson, employee10, "SP7473");
        User user12 = new User(hashPassoword, AuthorizationLevel.SalesPerson, employee11, "SP4331");
        User user13 = new User(hashPassoword, AuthorizationLevel.SalesPerson, employee12, "SP7337");

        User user14 = new User(hashPassoword, AuthorizationLevel.Admin, employee1, "AD1337");
        User user15 = new User(hashPassoword, AuthorizationLevel.Admin, employee2, "AD9911");
        User user16 = new User(hashPassowordAdmin, AuthorizationLevel.Admin, employee2, "Admin");

        #endregion

        #region PrivateCustomer
        PrivateCustomer privateCustomer1 = new PrivateCustomer(
            "0706689932",
            "Ingalill.Bengtsson@live.com",
            "Vasagatan 1",
            "11120",
            "Stockholm",
            "19650230-5678",
            "Inga-Lill",
            "Bengtsson"
        );

        PrivateCustomer privateCustomer2 = new PrivateCustomer(
            "0706689932",
            "Gustaf.Fridsson@hotmail.com",
            "Lilla Torg 3",
            "21134",
            "Malmö",
            "19890530-5678",
            "Gustaf",
            "Fridsson"
        );

        PrivateCustomer privateCustomer3 = new PrivateCustomer(
            "0706689932",
            "Kalle.Karlsson@emial.com",
            "Östra Hamngatan 18",
            "41109",
            "Göteborg",
            "19891020-5678",
            "Kalle",
            "Karlsson"
        );
        #endregion

        #region CompanyCustomer
        CompanyCustomer companyCustomer1 = new CompanyCustomer(
            "0705102355",
            "Ekonomi@NordicSolutions.com",
            "Kungsgatan 3",
            "41119",
            "Göteborg",
            "556986-8667",
            "Sofia Bergström",
            "0710192844",
            "Nordic Solutions AB"
        );

        CompanyCustomer companyCustomer2 = new CompanyCustomer(
            "07061111222",
            "Kundservicen@GronaHemTradgardsservice.com",
            "Storgatan 12B",
            "58223",
            "Linköping",
            "556642-4567",
            "Erik Johansson",
            "0740568646",
            "Gröna Hem Trädgårdsservice"
        );

        CompanyCustomer companyCustomer3 = new CompanyCustomer(
            "07061783521",
            "Info@SvenskaByggDesign.com",
            "Drottninggatan 45",
            "11121",
            "Stockholm",
            "554323-4567",
            "Anna Lundgren",
            "0700101010",
            "Svenska Bygg & Design"
        );
        #endregion

        #region ProspectNote
        ProspectNote prospectNote1 = new ProspectNote(
            "Kunden gick inte med på avtal",
            DateTime.Now.AddDays(-20),
            user1,
            privateCustomer1
        );
        ProspectNote prospectNote2 = new ProspectNote(
            "Kunden vill att vi ringer upp imorgon",
            DateTime.Now.AddDays(-10),
            user2,
            companyCustomer1
        );
        #endregion

        #region InsuredPerson
        InsuredPerson insuredPerson1 = new InsuredPerson("Kalle", "Karlsson", "19891020-5678");
        InsuredPerson insuredPerson2 = new InsuredPerson("Gustaf", "Fridsson", "19890530-5678");

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
            insuranceType3
        );
        #endregion

        #region Insurance
        Insurance insurance1 = new Insurance(
            DateTime.Today,
            BillingInterval.Månad,
            InsuranceStatus.Preliminary,
            true,
            DateTime.Now,
            "Snäll kund, ge rabbat",
            user6,
            insuredPerson1,
            privateCustomer1,
            insuranceType1
        );
        Insurance insurance2 = new Insurance(
            DateTime.Today,
            BillingInterval.År,
            InsuranceStatus.Active,
            false,
            DateTime.Now,
            "Stor kund, vill hålla kvar",
            user6,
            insuredPerson2,
            companyCustomer1,
            insuranceType2
        );

        Insurance insurance3 = new Insurance(
            DateTime.Today,
            BillingInterval.Månad,
            InsuranceStatus.Preliminary,
            true,
            DateTime.Now,
            "Snäll kund, ge rabbat",
            user6,
            insuredPerson1,
            privateCustomer2,
            insuranceType2
        );
        Insurance insurance4 = new Insurance(
            DateTime.Today,
            BillingInterval.År,
            InsuranceStatus.Active,
            true,
            DateTime.Now,
            "Stor kund, vill hålla kvar",
            user6,
            insuredPerson2,
            privateCustomer3,
            insuranceType3
        );

        Insurance insurance5 = new Insurance(
            DateTime.Today,
            BillingInterval.Månad,
            InsuranceStatus.Preliminary,
            true,
            DateTime.Now,
            "Snäll kund, ge rabbat",
            user6,
            insuredPerson1,
            companyCustomer2,
            insuranceType4
        );
        Insurance insurance6 = new Insurance(
            DateTime.Today,
            BillingInterval.År,
            InsuranceStatus.Active,
            false,
            DateTime.Now,
            "Stor kund, vill hålla kvar",
            user6,
            insuredPerson2,
            companyCustomer3,
            insuranceType5
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
            insurance4,
            insuranceTypeAttribute9
        );
        #endregion

        context.CommissionRates.Add(commissionRate1);

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

        context.Users.AddRange(
            user1,
            user2,
            user3,
            user4,
            user5,
            user6,
            user7,
            user8,
            user9,
            user10,
            user11,
            user12,
            user13,
            user14,
            user15,
            user16
        );

        context.ProspectNotes.AddRange(prospectNote1, prospectNote2); //Nya notesen

        context.CompanyCustomers.AddRange(companyCustomer1, companyCustomer2, companyCustomer3);

        context.PrivateCustomers.AddRange(privateCustomer1, privateCustomer2);

        context.Insurances.AddRange(
            insurance1,
            insurance2,
            insurance3,
            insurance4,
            insurance5,
            insurance6
        );

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
