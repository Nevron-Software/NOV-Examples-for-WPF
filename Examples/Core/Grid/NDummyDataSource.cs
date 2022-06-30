using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nevron.Nov.Data;

namespace Nevron.Nov.Examples.Grid
{
    public enum ENGender
    {
        Male,
        Female
    }

    public enum ENCountry
    {
        USA,
        UK,
        Germany,
        France,
        Russia,
        Italy,
        Canada,
        China,
        Japan,
        India
    }

    public enum ENJobTitle
    {
        President,
        VicePresident,
        SalesManager,
        SalesRepresentative,
        LeadDevelop,
        SeniorDeveloper,
        JuniorDeveloper,
    }

    /// <summary>
    /// A static class for creating dummy data sources used in the grid examples.
    /// </summary>
    public static class NDummyDataSource
    {
        static NDataTable dataTable;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static NDataSource CreateCompanySalesDataSource()
        {
            dataTable = new NMemoryDataTable("CompanySales", new NFieldInfo[]{
                new NFieldInfo("Id", typeof(Int32)),
                new NFieldInfo("Company", typeof(String)),
                new NFieldInfo("Sales", typeof(Double)),
                new NFieldInfo("Profit", typeof(Double)),
            });

            for (int i = 0; i < 50000; i++)
            {
                dataTable.AddRow(
                    i,
                    RandomCompanyName(),
                    RandomDouble(500, 5000),
                    RandomDouble(100, 1000)
                );
            }

            return new NDataSource(dataTable);
        }
        /// <summary>
        /// Creates a products data source.
        /// </summary>
        public static NDataSource CreateProductsDataSource()
        {
            NMemoryDataTable dataTable = new NMemoryDataTable("Products", new NFieldInfo[]{
                new NFieldInfo("Id", typeof(Int32)),
                new NFieldInfo("Name", typeof(String)),
                new NFieldInfo("Price", typeof(Double))
            });

            for (int i = 0; i < ProductInfos.Length; i++)
            {
                NProductInfo productInfo = ProductInfos[i];
                dataTable.AddRow(i, productInfo.Name, productInfo.Price);
            }

            return new NDataSource(dataTable);
        }
        /// <summary>
        /// Creates a persons data source.
        /// </summary>
        public static NDataSource CreatePersonsDataSource()
        {
            NMemoryDataTable dataTable = new NMemoryDataTable("Persons", new NFieldInfo[]{
                new NFieldInfo("Id", typeof(Int32)),
                new NFieldInfo("Name", typeof(String), true),
                new NFieldInfo("Gender", typeof(ENGender), true),
                new NFieldInfo("Birthday", typeof(DateTime), true),
                new NFieldInfo("Country", typeof(ENCountry), true),
                new NFieldInfo("Phone", typeof(String), true),
                new NFieldInfo("Email", typeof(String), true),
            });

            for (int i = 0; i < PersonInfos.Length; i++)
            {
                NPersonInfo info = PersonInfos[i];
                dataTable.AddRow(
                    i,              // id
                    info.Name,      // name
                    info.Gender,    // gender
                    info.Birthday,  // birthday
                    info.Country,   // country
                    info.Phone,     // pnone
                    info.Email      // email
                );
            }

            return new NDataSource(dataTable);
        }
        /// <summary>
        /// Creates a persons order data source.
        /// </summary>
        public static NDataSource CreatePersonsOrdersDataSource()
        {
            NMemoryDataTable dataTable = new NMemoryDataTable("PersonsOrders", new NFieldInfo[]{
                new NFieldInfo("PersonId", typeof(Int32)),
                new NFieldInfo("Product Name", typeof(String)),
                new NFieldInfo("Product Name1", typeof(String)),
                new NFieldInfo("Ship To Country", typeof(ENCountry)),
                new NFieldInfo("Ship To Country1", typeof(ENCountry)),
                new NFieldInfo("Ship To City", typeof(String)),
                new NFieldInfo("Ship To City1", typeof(String)),
                new NFieldInfo("Ship To Address", typeof(String)),
                new NFieldInfo("Ship To Address1", typeof(String)),
                new NFieldInfo("Price", typeof(Double)),
                new NFieldInfo("Price1", typeof(Double)),
                new NFieldInfo("Quantity", typeof(Int32))
            });

            for (int i = 0; i < 10000; i++)
            {
                NAddressInfo addressInfo = RandomAddressInfo();
                NProductInfo productInfo = RandomProductInfo();

                dataTable.AddRow(
                    RandomPersonIndex(),    // person id
                    productInfo.Name,       // product name
                    productInfo.Name,       // product name
                    addressInfo.Country,    // ship to country 
                    addressInfo.Country,    // ship to country 
                    addressInfo.City,       // ship to city
                    addressInfo.City,       // ship to city
                    addressInfo.Address,    // ship to address
                    addressInfo.Address,    // ship to address
                    productInfo.Price,      // price
                    productInfo.Price,      // price
                    Random.Next(5) + 1      // quantity

                );
            }

            return new NDataSource(dataTable);
        }

        #region Static Fields

        static Random Random = new Random();

        #endregion

        #region Numbers

        /// <summary>
        /// Creates a random double in the specified range.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double RandomDouble(int min, int max)
        {
            int range = max - min;
            int value = Random.Next(range);
            return min + value;
        }
        /// <summary>
        /// Creates a random Int32 in the specified range.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RandomInt32(int min, int max)
        {
            int range = max - min;
            int value = Random.Next(range);
            return min + value;
        }

        #endregion

        #region Company Names

        /// <summary>
        /// Picks a random Company Name from the CompanyNames array.
        /// </summary>
        /// <returns></returns>
        public static string RandomCompanyName()
        {
            int count = CompanyNames.Length;
            int index = Random.Next(count);
            return CompanyNames[NMath.Clamp(0, count - 1, index)];
        }
        /// <summary>
        /// List of dummy company names.
        /// </summary>
        public static string[] CompanyNames = new string[]{
            "Feugiat Metus Foundation",
            "Ligula Consectetuer Foundation",
            "Non LLC",
            "Tincidunt Institute",
            "Sollicitudin A Malesuada Industries",
            "Ornare In Faucibus Industries",
            "In Faucibus Industries",
            "Iaculis Aliquet Diam Company",
            "Porttitor Interdum LLC",
            "Egestas Duis Ltd",
            "Vel Turpis Aliquam LLC",
            "Ullamcorper Velit Associates",
            "Arcu Associates",
            "Sapien Aenean Massa PC",
            "Convallis Dolor Quisque PC",
            "A Ultricies Adipiscing Corp.",
            "Enim Corporation",
            "Luctus Sit Amet Consulting",
            "Eu PC",
            "Dui Nec Urna PC",
            "Pede Nonummy Consulting",
            "Vestibulum Mauris Company",
            "Consequat Ltd",
            "Mi Lacinia Mattis LLC",
            "Ultrices Vivamus Rhoncus PC",
            "Nec Ante Blandit LLP",
            "Sollicitudin Commodo Ipsum Corp.",
            "Diam Company",
            "Orci Corporation",
            "Tristique Pharetra Corporation",
            "Blandit Nam Limited",
            "Magna A Consulting",
            "Commodo Auctor Velit Ltd",
            "Donec Corporation",
            "Est Nunc Ullamcorper Foundation",
            "Accumsan Industries",
            "Ullamcorper Viverra LLC",
            "Ornare Ltd",
            "Pharetra Ltd",
            "Praesent Eu Institute",
            "Orci Tincidunt Incorporated",
            "Urna Vivamus Molestie Institute",
            "Ante Bibendum Company",
            "Aliquam Enim Nec Company",
            "Sem PC",
            "Euismod Est Limited",
            "Orci Lobortis Augue PC",
            "Curabitur Massa Vestibulum Institute",
            "Velit LLP",
            "Risus Foundation",
            "Sed Industries",
            "Dignissim Tempor Company",
            "Facilisis Corporation",
            "Aenean Massa Company",
            "Arcu Imperdiet Corp.",
            "Aliquam Corporation",
            "Nulla Facilisi Sed Associates",
            "Aliquam Arcu Aliquam Associates",
            "Ligula Incorporated",
            "Egestas Urna Justo Limited",
            "At Ltd",
            "Vivamus Sit Amet Corporation",
            "Lacus Aliquam Rutrum Company",
            "Enim Sit LLC",
            "Turpis Ltd",
            "Etiam Bibendum Fermentum Company",
            "Ultrices Iaculis Incorporated",
            "Penatibus Et Magnis Incorporated",
            "Aliquam Rutrum Industries",
            "Fringilla LLP",
            "Nonummy Fusce Corporation",
            "Nibh Vulputate Mauris Incorporated",
            "Praesent PC",
            "Lorem Ut Aliquam Consulting",
            "Est Incorporated",
            "Nisl Maecenas Corp.",
            "Lorem Sit Amet Ltd",
            "Lorem Company",
            "Risus Donec Egestas Corporation",
            "Sit Corporation",
            "Vulputate Associates",
            "Imperdiet Dictum Ltd",
            "Et Magna Praesent Incorporated",
            "Sem Eget LLP",
            "Rutrum Eu Incorporated",
            "Lorem Consulting",
            "Nisl Sem Consequat Industries",
            "Cubilia Curae; Industries",
            "Enim Corp.",
            "Arcu Corp.",
            "Suspendisse Tristique Neque Corp.",
            "Ut LLP",
            "Condimentum Donec Industries",
            "Aliquet Vel Vulputate Limited",
            "Imperdiet Institute",
            "Dictum Limited",
            "Duis Limited",
            "Augue Ac LLC",
            "Condimentum Donec Limited",
            "Consequat Company"};

        #endregion

        #region Person Infos

        /// <summary>
        /// Picks a random Index in the PersonInfos array.
        /// </summary>
        /// <returns></returns>
        public static int RandomPersonIndex()
        {
            int count = PersonInfos.Length;
            int index = Random.Next(count);
            return NMath.Clamp(0, count - 1, index);
        }
        /// <summary>
        /// Picks a random NPersonInfo in the PersonInfos array.
        /// </summary>
        /// <returns></returns>
        public static NPersonInfo RandomPersonInfo()
        {
            return PersonInfos[RandomPersonIndex()];
        }
        /// <summary>
        /// List of dummy person names.
        /// </summary>
        public static NPersonInfo[] PersonInfos = new NPersonInfo[]{
            new NPersonInfo("Jinny Collazo", ENGender.Female),
            new NPersonInfo("John Duke", ENGender.Male),
            new NPersonInfo("Kellie Ferrell", ENGender.Female),
            new NPersonInfo("Sibyl Woosley", ENGender.Female),  
            new NPersonInfo("Kourtney Mattoon", ENGender.Female), 
            new NPersonInfo("Bruce Fail", ENGender.Male), 
            new NPersonInfo("Dario Karl", ENGender.Male),
            new NPersonInfo("Aliza Sermons", ENGender.Female),
            new NPersonInfo("Sung Trout", ENGender.Male),
            new NPersonInfo("Barb Stiner", ENGender.Female),
            new NPersonInfo("Ashlie Bynum", ENGender.Female),
            new NPersonInfo("Carola Saeed", ENGender.Female),
            new NPersonInfo("Brunilda Hermanson", ENGender.Female),
            new NPersonInfo("Lois Capel", ENGender.Male),
            new NPersonInfo("Jerome Moody", ENGender.Male),
            new NPersonInfo("Booker Quach", ENGender.Male),
            new NPersonInfo("Malcolm Luckett", ENGender.Male), 
            new NPersonInfo("Mana Snapp", ENGender.Female),
            new NPersonInfo("Georgianna Leung", ENGender.Female),
            new NPersonInfo("Romana Gentle", ENGender.Female),
            new NPersonInfo("Garfield Tranmer", ENGender.Male), 
            new NPersonInfo("Rossana Heintzelman", ENGender.Female),
            new NPersonInfo("Carmina Vanorden", ENGender.Female),
            new NPersonInfo("Roger Patten", ENGender.Male),
            new NPersonInfo("Cleopatra Morrill", ENGender.Female), 
            new NPersonInfo("Tammara Cumberbatch", ENGender.Female), 
            new NPersonInfo("Vita Trinh", ENGender.Female),
            new NPersonInfo("Evangeline Aguinaldo", ENGender.Female),
            new NPersonInfo("Angelo Arzola", ENGender.Male),
            new NPersonInfo("Reynaldo Manahan", ENGender.Male), 
            new NPersonInfo("Luis Peoples", ENGender.Male),
            new NPersonInfo("Sheena Fritsch", ENGender.Female),
            new NPersonInfo("Isaiah Key", ENGender.Female),
            new NPersonInfo("Sunny Vath", ENGender.Male),
            new NPersonInfo("Isidro Monsen", ENGender.Male), 
            new NPersonInfo("Mariko Ek", ENGender.Female),
            new NPersonInfo("Jessenia Northup", ENGender.Female),
            new NPersonInfo("Cordie Lesage", ENGender.Female),
            new NPersonInfo("Henrietta Fuentes", ENGender.Female),  
            new NPersonInfo("Han Snover", ENGender.Male),
            new NPersonInfo("Alesia Pearman", ENGender.Female),  
            new NPersonInfo("Mai Czapla", ENGender.Female),
            new NPersonInfo("Maryam Torgersen", ENGender.Female),
            new NPersonInfo("Ken Vaca", ENGender.Male),
            new NPersonInfo("Martina Matson", ENGender.Female),
            new NPersonInfo("Blanche Desrochers", ENGender.Female),
            new NPersonInfo("Rosie Griffing", ENGender.Female),
            new NPersonInfo("Nona Mccroskey", ENGender.Female),
            new NPersonInfo("Arnold Mayen", ENGender.Male),
            new NPersonInfo("Yulanda Wenner", ENGender.Female)
        };

        /// <summary>
        /// Represents a fictional person information.
        /// </summary>
        public class NPersonInfo
        {
            public NPersonInfo(string name, ENGender gender)
            {
                Name = name;
                Gender = gender;

                // random birthday of 20-50 year olds
                int days = Random.Next(30 * 365) + 20 * 365;
                Birthday = DateTime.Now - new TimeSpan(days, 0, 0, 0);

                // random country
                Country = (ENCountry)Random.Next(NEnum.GetValues(typeof(ENCountry)).Length);

                // random phone
                int areaCode = Random.Next(999);
                int firstPhonePart = Random.Next(999);
                int secondPhonePart = Random.Next(9999);
                Phone = String.Format("({0})-{1}-{2}", areaCode.ToString("000"), firstPhonePart.ToString("000"), secondPhonePart.ToString("0000"));

                // email
                Email = Name.ToLower().Replace(" ", ".") + "@domain.com";
            }

            public string Name;
            public ENGender Gender;
            public DateTime Birthday;
            public ENCountry Country;
            public string Phone;
            public string Email;
        }

        #endregion

        #region Product Names

        /// <summary>
        /// Picks a random Product Name from the ProductNames array.
        /// </summary>
        /// <returns></returns>
        public static NProductInfo RandomProductInfo()
        {
            return ProductInfos[RandomProductIndex()];
        }
        /// <summary>
        /// Picks a random Index
        /// </summary>
        /// <returns></returns>
        public static int RandomProductIndex()
        {
            int count = ProductInfos.Length;
            int index = Random.Next(count);
            return NMath.Clamp(0, count - 1, index);
        }
        /// <summary>
        /// List of dummy product names.
        /// </summary>
        public static NProductInfo[] ProductInfos = new NProductInfo[]{
            new NProductInfo("Dingtincof"),
            new NProductInfo("Conremdox"),
            new NProductInfo("Sonlight"),
            new NProductInfo("Vivatom"),
            new NProductInfo("Trans Tax"),
            new NProductInfo("Super Matdox"),
            new NProductInfo("Geodomflex"),
            new NProductInfo("Re Zootop"),
            new NProductInfo("Hot-Lam"),
            new NProductInfo("Single-Top"),
            new NProductInfo("Tranhome"),
            new NProductInfo("Gravenimhold"),
            new NProductInfo("Zun Zimtam"),
            new NProductInfo("Ancore"),
            new NProductInfo("Zun Keydom"),
            new NProductInfo("Ontodex"),
            new NProductInfo("Freshdox"),
            new NProductInfo("Yearair"),
            new NProductInfo("Lam-Dox"),
            new NProductInfo("Toughcom"),
            new NProductInfo("Zoo Ing"),
            new NProductInfo("Aptax"),
            new NProductInfo("Statfan"),
            new NProductInfo("Joykix"),
            new NProductInfo("Indigolam"),
            new NProductInfo("Hattom")};
        /// <summary>
        /// Represents a fictional product
        /// </summary>
        public class NProductInfo
        {
            public NProductInfo(string name)
            {
                Name = name;
                Price = ((double)Random.Next(8000) + 2000) / 100;
            }
            public string Name;
            public double Price;
        }

        #endregion

        #region Address Info

        /// <summary>
        /// Picks a random NAddressInfo AddressInfo from the AddressInfos array
        /// </summary>
        /// <returns></returns>
        public static NAddressInfo RandomAddressInfo()
        {
            return AddressInfos[RandomAddressIndex()];
        }
        /// <summary>
        /// Picks a random index in the AddressInfos array
        /// </summary>
        /// <returns></returns>
        public static int RandomAddressIndex()
        {
            int count = AddressInfos.Length;
            int index = Random.Next(count);
            return NMath.Clamp(0, count - 1, index);
        }
        /// <summary>
        /// Array of fictional addresses
        /// </summary>
        public static NAddressInfo[] AddressInfos = new NAddressInfo[]{
            // USA
            new NAddressInfo(ENCountry.USA, "New York", "7414 Park Place"),
            new NAddressInfo(ENCountry.USA, "New York", "1394 Bayberry Drive"),
            new NAddressInfo(ENCountry.USA, "New York", "9436 Parker Street"),

            new NAddressInfo(ENCountry.USA, "Los Angeles", "2101 William Street"),
            new NAddressInfo(ENCountry.USA, "Los Angeles", "5073 Eagle Street"),
            new NAddressInfo(ENCountry.USA, "Los Angeles", "439 Atlantic Avenue"),

            new NAddressInfo(ENCountry.USA, "Chicago", "245 Beech Street"),
            new NAddressInfo(ENCountry.USA, "Chicago", "420 Hamilton Road"),
            new NAddressInfo(ENCountry.USA, "Chicago", "540 Maple Lane "),

            // UK
            new NAddressInfo(ENCountry.UK, "London", "854 Lawrence Street"),
            new NAddressInfo(ENCountry.UK, "London", "13 Park Street"),
            new NAddressInfo(ENCountry.UK, "London", "461 Front Street North"),

            new NAddressInfo(ENCountry.UK, "Birmingham", "281 4th Street North"),
            new NAddressInfo(ENCountry.UK, "Birmingham", "49 Division Street"),
            new NAddressInfo(ENCountry.UK, "Birmingham", "848 8th Street South"),

            new NAddressInfo(ENCountry.UK, "Leeds", "334 Catherine Street"),
            new NAddressInfo(ENCountry.UK, "Leeds", "885 Grant Street"),
            new NAddressInfo(ENCountry.UK, "Leeds", "468 Main Street"),

            // Germany
            new NAddressInfo(ENCountry.Germany, "Berlin", "Fischerinsel 81"),
            new NAddressInfo(ENCountry.Germany, "Berlin", "Budapester Strasse 14"),
            new NAddressInfo(ENCountry.Germany, "Berlin", "Knesebeckstrasse 1"),

            new NAddressInfo(ENCountry.Germany, "Hamburg", "Gotzkowskystrasse 74"),
            new NAddressInfo(ENCountry.Germany, "Hamburg", "Prenzlauer Allee 42"),
            new NAddressInfo(ENCountry.Germany, "Hamburg", "Am Borsigturm 60"),

            new NAddressInfo(ENCountry.Germany, "Munich", "An der Schillingbrucke 98"),
            new NAddressInfo(ENCountry.Germany, "Munich", "Rankestraße 2"),
            new NAddressInfo(ENCountry.Germany, "Munich", "An der Schillingbrucke 97"),

            // France
            new NAddressInfo(ENCountry.France, "Paris", "87, Rue de Strasbourg"),
            new NAddressInfo(ENCountry.France, "Paris", "73, place Stanislas"),
            new NAddressInfo(ENCountry.France, "Paris", "67, avenue Ferdinand de Lesseps"),

            new NAddressInfo(ENCountry.France, "Marseille", "30, rue Gontier-Patin"),
            new NAddressInfo(ENCountry.France, "Marseille", "65, rue Gontier-Patin"),
            new NAddressInfo(ENCountry.France, "Marseille", "20, rue Grande Fusterie"),

            new NAddressInfo(ENCountry.France, "Lyon", "57, Rue St Ferréol"),
            new NAddressInfo(ENCountry.France, "Lyon", "25, boulevard de Prague"),
            new NAddressInfo(ENCountry.France, "Lyon", "25, Avenue des Tuileries"),

            // Russia
            new NAddressInfo(ENCountry.Russia, "Moscow", "ul. Podgorska 67"),
            new NAddressInfo(ENCountry.Russia, "Moscow", "ul. Boleslawa 63"),
            new NAddressInfo(ENCountry.Russia, "Moscow", "ul. Dukielska 105"),

            new NAddressInfo(ENCountry.Russia, "Saint Petersburg", "ul. Uri Lubelsky 2"),
            new NAddressInfo(ENCountry.Russia, "Saint Petersburg", "ul. Tehniku 23"),
            new NAddressInfo(ENCountry.Russia, "Saint Petersburg", "ul. Ana Kolska 89"),

            new NAddressInfo(ENCountry.Russia, "Novosibirsk", "ul. Sobranie 45"),
            new NAddressInfo(ENCountry.Russia, "Novosibirsk", "ul. Pobeda 68"),
            new NAddressInfo(ENCountry.Russia, "Novosibirsk", "ul. Kolarski Les 46"),

            // Italy
            new NAddressInfo(ENCountry.Italy, "Rome", "Via Santa Teresa degli Scalzi, 55"),
            new NAddressInfo(ENCountry.Italy, "Rome", "Via Agostino Depretis, 110"),
            new NAddressInfo(ENCountry.Italy, "Rome", "Via Firenze, 59"),

            new NAddressInfo(ENCountry.Italy, "Milan", "Via Torricelli, 137"),
            new NAddressInfo(ENCountry.Italy, "Milan", "Via Nazario Sauro, 125"),
            new NAddressInfo(ENCountry.Italy, "Milan", "Via Zannoni, 19"),

            new NAddressInfo(ENCountry.Italy, "Naples", "Via del Caggio, 55"),
            new NAddressInfo(ENCountry.Italy, "Naples", "Corso Porta Borsari, 71"),
            new NAddressInfo(ENCountry.Italy, "Naples", "Via Santa Maria di Costantinopoli, 55"),

            // Canada
            new NAddressInfo(ENCountry.Canada, "Toronto", "1696 Heritage Drive"),
            new NAddressInfo(ENCountry.Canada, "Toronto", "592 Sturgeon Drive"),
            new NAddressInfo(ENCountry.Canada, "Toronto", "2188 York St"),

            new NAddressInfo(ENCountry.Canada, "Montreal", "4875 Bloor Street"),
            new NAddressInfo(ENCountry.Canada, "Montreal", "2458 Hammarskjold Dr"),
            new NAddressInfo(ENCountry.Canada, "Montreal", "3384 James Street"),

            new NAddressInfo(ENCountry.Canada, "Calgary", "4454 rue Saint-Édouard"),
            new NAddressInfo(ENCountry.Canada, "Calgary", "628 Merivale Road"),
            new NAddressInfo(ENCountry.Canada, "Calgary", "4748 Exmouth Street"),

            // China
            new NAddressInfo(ENCountry.China, "Shanghai", "No. 1234, 1241, Yun He Zhen Jie Fang Lu"),
            new NAddressInfo(ENCountry.China, "Shanghai", "No. 3452, 1983, 1013, Nan Yuan Lu"),
            new NAddressInfo(ENCountry.China, "Shanghai", "No. 8985, 1028, Feng Cun Wang Bo Zi"),

            new NAddressInfo(ENCountry.China, "Beijing", "No. 1034, 1179, Lin Jiang San Cun"),
            new NAddressInfo(ENCountry.China, "Beijing", "No. 1781, 1212, Fu Xing Hou Jie"),
            new NAddressInfo(ENCountry.China, "Beijing", "No. 1462, 1143, Qun Li Xiang"),

            new NAddressInfo(ENCountry.China, "Tianjin", "No. 1896, 1278, Bei Gang Gong Ye Lou"),
            new NAddressInfo(ENCountry.China, "Tianjin", "No. 1174, 1035, Xu Cheng Zhen Dui Bao Xiang"),
            new NAddressInfo(ENCountry.China, "Tianjin", "No. 1979, 1045, Nan Shen Gou"),

            // Japan
            new NAddressInfo(ENCountry.Japan, "Tokyo", "293-1086, Kozukayamate, Tarumi-ku Kobe-shi"),
            new NAddressInfo(ENCountry.Japan, "Tokyo", "493-1081, Takanodaiminami, Sugito-machi Kitakatsushika-gun"),
            new NAddressInfo(ENCountry.Japan, "Tokyo", "137-1285, Mizunaka, Takayama-mura Kamitakai-gun"),

            new NAddressInfo(ENCountry.Japan, "Yokohama", "185-1227, Mizuho, Hanamigawa-ku Chiba-shi"),
            new NAddressInfo(ENCountry.Japan, "Yokohama", "233-1273, Shimogamo Kodonocho, Sakyo-ku Kyoto-shi"),
            new NAddressInfo(ENCountry.Japan, "Yokohama", "396-1162, Nakata, Noheji-machi Kamikita-gun"),

            new NAddressInfo(ENCountry.Japan, "Osaka", "230-1058, Honen, Shirakawa-shi"),
            new NAddressInfo(ENCountry.Japan, "Osaka", "234-1267, Sechibarucho Akakoba, Sasebo-shi"),
            new NAddressInfo(ENCountry.Japan, "Osaka", "189-1273, Yorozuyamachi, Nagasaki-shi"),
            
            // India
            new NAddressInfo(ENCountry.India, "Bombay", "Kismat Nagar, Kurla West, Mumba, 173021"),
            new NAddressInfo(ENCountry.India, "Bombay", "Friends Colony, Hallow Pul, Kurla West, 573201"),
            new NAddressInfo(ENCountry.India, "Bombay", "Hallow Pul, Kurla West, 273001"),

            new NAddressInfo(ENCountry.India, "Calcutta", "Maulana Ishaque Street, Ar Rashidiyyah, General Ganj, 733051"),
            new NAddressInfo(ENCountry.India, "Calcutta", "Ar Rashidiyyah, General Ganj, Kanpur, 567825"),
            new NAddressInfo(ENCountry.India, "Calcutta", "General Ganj, Kanpur, Uttar Pradesh, 837547"),

            new NAddressInfo(ENCountry.India, "Delhi", "George Town Rd, George Town, Allahabad, 273001"),
            new NAddressInfo(ENCountry.India, "Delhi", "Hubli - Gadag Rd, Railway Colony, 123456"),
            new NAddressInfo(ENCountry.India, "Delhi", "Akshaibar Singh Marg, Golghar, Gorakhpur, 223152"),
        };
        /// <summary>
        /// Represents a fictional address.
        /// </summary>
        public class NAddressInfo
        {
            public NAddressInfo(ENCountry country, string city, string address)
            {
                Country = country;
                City = city;
                Address = address;
            }
            public ENCountry Country;
            public string City;
            public string Address;
        }

        #endregion
    }
}