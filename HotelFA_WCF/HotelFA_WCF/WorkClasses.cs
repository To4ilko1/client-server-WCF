using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Data.SQLite;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace HotelFA_WCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WorkClasses : IWorkClasses
    {
        private List<Animal> animals = new List<Animal>();
        private List<Journal> journals = new List<Journal>();
        private List<AnimalType> animaltypes = new List<AnimalType>();
        private List<Chat> chats = new List<Chat>();
        private List<ChatMessage> chatmessages = new List<ChatMessage>();
        private List<Order> orders = new List<Order>();
        private List<Person> persons = new List<Person>();
        private List<Review> reviews = new List<Review>();
        public List<Journal> getJournal(int animalid, string token)
        {
            if (checkToken(token) == true)
            {
                List<Journal> useranimalsJournal = getJournal().FindAll(x => x.animal.id == animalid);
                return useranimalsJournal;
            }
            else
            {
                return null;
            }
        }
        private List<Journal> getJournal()
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
            {
                string sql = "select * from Journals where DelTime is NULL";
                SQLiteCommand command = new SQLiteCommand(sql, Connect);
                Connect.Open(); // открыть соединение
                command = new SQLiteCommand(sql, Connect);
                SQLiteDataReader reader = command.ExecuteReader();
                journals.Clear();
                while (reader.Read())
                {
                    Journal journal = new Journal();
                    journal.id = Convert.ToInt32(reader["id"]);
                    journal.date = Convert.ToDateTime(reader["Date"]);
                    journal.timestart = Convert.ToDateTime(reader["TimeStart"]);
                    journal.timeend = Convert.ToDateTime(reader["TimeEnd"]);
                    journal.task = Convert.ToString(reader["Task"]);
                    journal.comment = Convert.ToString(reader["Comment"]);
                    journal.filepath = Convert.ToString(reader["FilePath"]);
                    orders = getOrders();
                    journal.order = orders.Find(x => x.id == Convert.ToInt32(reader["OrderID"]));
                    persons = getPersons();
                    journal.worker = persons.Find(x => x.state == 1 && x.id == Convert.ToInt32(reader["WorkerID"]));
                    animals = getAnimals();
                    journal.animal = animals.Find(x => x.id == Convert.ToInt32(reader["AnimalID"]));
                    journals.Add(journal);
                }
                return journals;
            }
        }
        public List<ChatMessage> getChatMessages(int status, string token)//если статус равен 0, то показывает все сообщения, если 1, то непрочитанные
        {
            if (checkToken(token) == true)
            {
                Chat chatclients = getChats().First(x => x.client.token == token);
                if (status == 1)
                {
                    List<ChatMessage> chatsclients = getChatMessages().FindAll(x => x.chat.id == chatclients.id && x.status == 0);
                    using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
                    {

                        string sql = String.Format(@"Update ChatMessages Set Status = 1 Where Status = 0");//правильно работает?
                        SQLiteCommand command = new SQLiteCommand(sql, Connect);
                        Connect.Open(); // открыть соединение
                        command = new SQLiteCommand(sql, Connect);
                        command.ExecuteNonQuery();
                    }
                    return chatsclients;
                }
                else
                {
                    List<ChatMessage> chatsclients = getChatMessages().FindAll(x => x.chat.id == chatclients.id);
                    return chatsclients;
                    
                }
            }
            else
            {
                return null;
            }
        }
        public List<ChatMessage> getChatMessagesByDate(int status, DateTime datestart, DateTime dateend, string token)
        {
            if (checkToken(token) == true)
            {
                Chat chatclients = getChats().First(x => x.client.token == token);
                if (status == 1)
                {
                    List<ChatMessage> chatsclients = getChatMessages().FindAll(x => x.chat.id == chatclients.id && x.status == 0);
                    using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
                    {

                        string sql = String.Format(@"Update ChatMessages Set Status = 1 Where Status = 0");//правильно работает?
                        SQLiteCommand command = new SQLiteCommand(sql, Connect);
                        Connect.Open(); // открыть соединение
                        command = new SQLiteCommand(sql, Connect);
                        command.ExecuteNonQuery();
                    }
                    return chatsclients;
                }
                else
                {
                    List<ChatMessage> chatsclients = getChatMessages().FindAll(x => x.chat.id == chatclients.id && x.time >= datestart && x.time <= dateend);
                    return chatsclients; 
                }
            }
            else
            {
                return null;
            }
        }
        private List<ChatMessage> getChatMessages()
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
            {
                string sql = "select * from ChatMessages where DelTime is NULL";
                SQLiteCommand command = new SQLiteCommand(sql, Connect);
                Connect.Open(); // открыть соединение
                command = new SQLiteCommand(sql, Connect);
                SQLiteDataReader reader = command.ExecuteReader();
                chatmessages.Clear();
                while (reader.Read())
                {
                    ChatMessage chatmessage = new ChatMessage();
                    chatmessage.id = Convert.ToInt32(reader["id"]);
                    chatmessage.time = Convert.ToDateTime(reader["Time"]);
                    chats = getChats();
                    chatmessage.chat = chats.Find(x => x.id == Convert.ToInt32(reader["ChatID"]));
                    chatmessage.text = Convert.ToString(reader["Text"]);
                    chatmessage.filepath = Convert.ToString(reader["FilePath"]);
                    chatmessage.status = Convert.ToInt32(reader["Status"]);
                    persons = getPersons();
                    chatmessage.client = persons.Find(x => x.state == 0 && x.id == Convert.ToInt32(reader["ClientID"]));
                    chatmessages.Add(chatmessage);
                }
                return chatmessages;
            }
        }
        private List<Chat> getChats()
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
            {
                string sql = "select * from Chats where DelTime is NULL";
                SQLiteCommand command = new SQLiteCommand(sql, Connect);
                Connect.Open(); // открыть соединение
                command = new SQLiteCommand(sql, Connect);
                SQLiteDataReader reader = command.ExecuteReader();
                chats.Clear();
                while (reader.Read())
                {
                    Chat chat = new Chat();
                    chat.id = Convert.ToInt32(reader["id"]);
                    persons = getPersons();
                    chat.client = persons.Find(x => x.state == 0 && x.id == Convert.ToInt32(reader["ClientID"]));
                    chats.Add(chat);
                }
                return chats;
            }
        }
        public List<Animal> getAnimalsInHotel(string token)
        {
            if (checkToken(token) == true)
            {
                List<Animal> animalsinhotel = new List<Animal>();
                List<Order> userOrders = getOrders().FindAll(x => x.client.token == token && x.datestart <= DateTime.Now && x.dateend >= DateTime.Now);
                foreach (Order usord in userOrders)
                {
                    Animal an = new Animal();
                    an = getAnimals().First(x => x.id == usord.animal.id);
                    animalsinhotel.Add(an);
                }
                return animalsinhotel;//надо как-то сделать так, чтобы животные из заказа выводились но не повторялись
            }
            else
            {
                return null;
            }
        }
        public List<Person> getAccountWorker(int workerid, string token)
        {
            if (checkToken(token) == true)
            {
                List<Person> workers = getPersons().FindAll(x => x.state == 1 && x.id == workerid);
                return workers;
            }
            else
            {
                return null;
            }
        }
        public List<Person> getAccount(string token)
        {
            if (checkToken(token) == true)
            {
                List<Person> userAccount = getPersons().FindAll(x => x.token == token);
                return userAccount;
            }
            else
            {
                return null;
            }
        }
        public string getCurrentUser(string login, string password)
        {
            Person currentUser = getPersons().First(x => x.login == login && x.password == password);
            if (checkToken(currentUser.token) == true)
            {
                return currentUser.token;
            }
            else return null;
        }
        public List<Order> getOrders(string token)
        {
            if (checkToken(token) == true)
            {
                List<Order> userOrders = getOrders().FindAll(x => x.client.token == token);
                return userOrders;
            }
            else
            {
                return null;
            }
        }
        private List<Order> getOrders()
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
            {
                string sql = "select * from Orders where DelTime is NULL";
                SQLiteCommand command = new SQLiteCommand(sql, Connect);
                Connect.Open(); // открыть соединение
                command = new SQLiteCommand(sql, Connect);
                SQLiteDataReader reader = command.ExecuteReader();
                orders.Clear();
                while (reader.Read())
                {
                    Order order = new Order();
                    order.id = Convert.ToInt32(reader["id"]);
                    order.state = Convert.ToString(reader["State"]);
                    order.datestart = Convert.ToDateTime(reader["DateStart"]);
                    order.dateend = Convert.ToDateTime(reader["DateEnd"]);
                    persons = getPersons();
                    order.client = persons.Find(x => x.state == 0 && x.id == Convert.ToInt32(reader["ClientID"]));
                    animals = getAnimals();
                    order.animal = animals.Find(x => x.id == Convert.ToInt32(reader["AnimalID"]));
                    order.deliverytothehotel = Convert.ToInt32(reader["DeliveryToTheHotel"]);
                    order.fromdeliveryaddress = Convert.ToString(reader["FromDeliveryAddress"]);
                    order.fromdeliverytime = Convert.ToString(reader["FromDeliveryTime"]);
                    order.deliveryfromhotel = Convert.ToInt32(reader["DeliveryFromHotel"]);
                    order.todeliveryaddress = Convert.ToString(reader["ToDeliveryAddress"]);
                    order.todeliverytime = Convert.ToString(reader["ToDeliveryTime"]);
                    order.comment = Convert.ToString(reader["Comment"]);
                    order.price = Convert.ToDouble(reader["Price"]);
                    orders.Add(order);
                }
                return orders;
            }
        }
        public List<Order> getOrdersByDate(DateTime startDate, DateTime endDate, string token)//Получить список заказов в разрезе времени
        {
            Person currentUser = getPersons().First(x => x.token == token);
            if (checkToken(token) == true)
            {
                List<Order> allOrders = getOrders();
                List<Order> resultOrders = allOrders.FindAll(x => x.client.token == token && x.datestart >= startDate && x.dateend <= endDate);
                return resultOrders;
            }
            else return null;
        }
        public List<Animal> getAnimals(string token)
        {
            if (checkToken(token) == true)
            {
                List<Animal> userAnimals = getAnimals().FindAll(x => x.client.token == token);
                return userAnimals;
            }
            else
            {
                return null;
            }
        }
        private List<Animal> getAnimals()
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
            {
                string sql = "select * from Animals where DelTime is NULL";
                SQLiteCommand command = new SQLiteCommand(sql, Connect);
                Connect.Open(); // открыть соединение
                command = new SQLiteCommand(sql, Connect);
                SQLiteDataReader reader = command.ExecuteReader();
                animals.Clear();
                while (reader.Read())
                {
                    Animal animal = new Animal();
                    animal.id = Convert.ToInt32(reader["id"]);
                    animal.name = Convert.ToString(reader["Name"]);
                    animaltypes = getAnimalTypes();
                    animal.animaltype = animaltypes.Find(x => x.id == Convert.ToInt32(reader["AnimalTypeID"]));
                    animal.sex = Convert.ToInt32(reader["Sex"]);
                    animal.comment = Convert.ToString(reader["Comment"]);
                    animal.birthday = Convert.ToDateTime(reader["Birthday"]);
                    persons = getPersons();
                    animal.client = persons.Find(x => x.state == 0 && x.id == Convert.ToInt32(reader["ClientID"]));
                    animals.Add(animal);
                }
                return animals;
            }
        }
        public List<Review> getReview()
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
            {
                string sql = "select * from Reviews where DelTime is NULL";
                SQLiteCommand command = new SQLiteCommand(sql, Connect);
                Connect.Open(); // открыть соединение
                command = new SQLiteCommand(sql, Connect);
                SQLiteDataReader reader = command.ExecuteReader();
                reviews.Clear();
                while (reader.Read())
                {
                    Review review = new Review();
                    review.id = Convert.ToInt32(reader["id"]);
                    animaltypes = getAnimalTypes();
                    review.animaltype = animaltypes.Find(x => x.id == Convert.ToInt32(reader["AnimalTypeID"]));
                    review.body = Convert.ToString(reader["Body"]);
                    review.createtime = Convert.ToDateTime(reader["CreateTime"]);
                    persons = getPersons();
                    review.client = persons.Find(x => x.state == 0 && x.id == Convert.ToInt32(reader["ClientID"]));
                    reviews.Add(review);
                }
                return reviews;
            }
        }
        private List<AnimalType> getAnimalTypes()
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
            {
                string sql = "select * from AnimalTypes where DelTime is NULL";
                SQLiteCommand command = new SQLiteCommand(sql, Connect);
                Connect.Open(); // открыть соединение
                command = new SQLiteCommand(sql, Connect);
                SQLiteDataReader reader = command.ExecuteReader();
                animaltypes.Clear();
                while (reader.Read())
                {
                    AnimalType animaltype = new AnimalType();
                    animaltype.id = Convert.ToInt32(reader["id"]);
                    animaltype.nameoftype = Convert.ToString(reader["NameOfType"]);
                    animaltypes.Add(animaltype);
                }
                return animaltypes;
            }
        }
        public List<AnimalType> getAnimalTypes(string token)
        {
            if (checkToken(token) == true)
            {
                List<AnimalType> userAnimalTypes = getAnimalTypes();
                return userAnimalTypes;
            }
            else
            {
                return null;
            }
        }
        public List<Person> getPersons()
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
            {
                string sql = "select * from Persons where DelTime is NULL";
                SQLiteCommand command = new SQLiteCommand(sql, Connect);
                Connect.Open(); // открыть соединение
                command = new SQLiteCommand(sql, Connect);
                SQLiteDataReader reader = command.ExecuteReader();
                persons.Clear();
                while (reader.Read())
                {
                    Person person = new Person();
                    person.id = Convert.ToInt32(reader["id"]);
                    person.state = Convert.ToInt32(reader["State"]);//0 - клиент, 1 - worker
                    person.email = Convert.ToString(reader["Email"]);
                    person.phone = Convert.ToString(reader["Phone"]);
                    person.birthday = Convert.ToString(reader["Birthday"]);
                    person.address = Convert.ToString(reader["Address"]);
                    //DateTime dateofissuetok = Convert.ToDateTime(reader["DateOfIssueToken"]);
                    if (reader["DateOfIssueToken"] != DBNull.Value)
                    {
                        person.dateofissuetoken = Convert.ToDateTime(reader["DateOfIssueToken"]);
                    }
                    person.login = Convert.ToString(reader["Login"]);
                    person.password = Convert.ToString(reader["Password"]);
                    if ((Convert.ToString(reader["Token"])) != "")
                    {
                        person.token = Convert.ToString(reader["Token"]);
                    }
                    person.name = Convert.ToString(reader["Name"]);
                    persons.Add(person);
                }
                return persons;
            }
        }
        private bool checkToken(string sendetToken)
        {
            //string id = "";
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //генерация уникального токена на C#;
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
            {
                string sql = String.Format(@"Select * From Persons where Persons.token = ""{0}""", sendetToken);
                Connect.Open(); // открыть соединение
                SQLiteCommand command = new SQLiteCommand(sql, Connect);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    token = Convert.ToString(reader["Token"]);
                }
            }//два раза получает человека с данным токеном в логауте и чектокене
            if (sendetToken == token)
            {
                return true;
            }
            return false;
        }
        public void addPerson(Person person)
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
            {
                string sql = String.Format(@"Insert into Persons (Token, DateOfIssueToken, State, Login, Password, Name, Phone, Email, Birthday, Address, DelTime) values (""{0}"", ""{1}"", {2}, ""{3}"", ""{4}"", ""{5}"", ""{6}"", ""{7}"", ""{8}"", ""{9}"", NULL)", person.token, person.dateofissuetoken.ToString("yyyy-MM-dd HH:MM"), person.state, person.login, person.password, person.name, person.phone, person.email, person.birthday, person.address);
                SQLiteCommand command = new SQLiteCommand(sql, Connect);
                Connect.Open(); // открыть соединение
                command = new SQLiteCommand(sql, Connect);
                command.ExecuteNonQuery();
            }
            if (person.state == 0)
            {
                addChat(person.login, person.password);
            }
        }
        private void addChat(string login, string password)
        {
            Person pers = getPersons().First(x => x.login == login && x.password == password);
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
            {
                string sql = String.Format(@"Insert into Chats (ClientID, DelTime) values ({0}, NULL)",pers.id);
                SQLiteCommand command = new SQLiteCommand(sql, Connect);
                Connect.Open(); // открыть соединение
                command = new SQLiteCommand(sql, Connect);
                command.ExecuteNonQuery();
            }
        }
        public void addMessage(ChatMessage message, string token)
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
            {
                Person currentUser = getPersons().First(x => x.token == token);
                if (checkToken(token) == true)
                {
                    chats = getChats();
                    int chatID = chats.Find(x => x.client.id == currentUser.id).id;
                    string sql = String.Format(@"Insert into ChatMessages (Time, ChatID, Text, FilePath, Status, ClientID, DelTime) values (""{0}"", {1}, ""{2}"", ""{3}"", {4}, {5}, NULL)", message.time.ToString("yyyy-MM-dd HH:MM"), chatID, message.text, message.filepath, message.status, currentUser.id);
                    SQLiteCommand command = new SQLiteCommand(sql, Connect);
                    Connect.Open(); // открыть соединение
                    command = new SQLiteCommand(sql, Connect);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void addAnimal(Animal animal, string token)
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
            {
                Person currentUser = getPersons().First(x => x.token == token);
                if (checkToken(token) == true)
                {
                    int animaltypeID = animaltypes.Find(x => x.id == animal.animaltype.id).id;
                    string sql = String.Format(@"Insert into Animals (Name, AnimalTypeID, Sex, Comment, Birthday, ClientID, DelTime) values (""{0}"", {1}, {2}, ""{3}"", ""{4}"", {5}, NULL)",animal.name.ToString(), animaltypeID, animal.sex, animal.comment, animal.birthday.ToString("yyyy-MM-dd"), currentUser.id);//&&&
                    SQLiteCommand command = new SQLiteCommand(sql, Connect);
                    Connect.Open(); // открыть соединение
                    command = new SQLiteCommand(sql, Connect);
                    command.ExecuteNonQuery();
                }
            }
        }
        public bool user_auth(string login, string password)//функция авторизации, возвращает 1 - если авторизован, 0 - ошибка
        {
            checkDB();
            bool isAuth = false;
            List<Person> allPersons = getPersons();
            Person authuser = allPersons.Find(x => x.login == login && x.password == password);
            if (authuser != null)
            {
                string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //генерация уникального токена на C#
                authuser.token = token;
                authuser.dateofissuetoken = DateTime.Now;
                using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
                {

                    string sql = String.Format(@"Update Persons Set Token = ""{0}"", DateOfIssueToken = ""{1}"" Where id = {2}", authuser.token, authuser.dateofissuetoken.ToString("yyyy-MM-dd HH:MM"), Convert.ToInt32(authuser.id));
                    SQLiteCommand command = new SQLiteCommand(sql, Connect);
                    Connect.Open(); // открыть соединение
                    command = new SQLiteCommand(sql, Connect);
                    command.ExecuteNonQuery();
                }
                isAuth = true;
            }
            else
            {
                isAuth = false;
            }
            return isAuth;
        }
        public int logout(string token)//функция выхода, возвращает 1 если попытка выхода прошла успешно, 0 - ошибка
        {
            int isLoggedOut = 0;
            if (checkToken(token) == true)
            {
                using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
                {
                    string sql = String.Format(@"Update Persons Set Token = ""{0}"", DateOfIssueToken = NULL Where token = ""{2}""", "", "", token);
                    SQLiteCommand command = new SQLiteCommand(sql, Connect);
                    Connect.Open(); // открыть соединение
                    command = new SQLiteCommand(sql, Connect);
                    command.ExecuteNonQuery();
                }
                isLoggedOut = 1;
            }
            else
            {
                isLoggedOut = 0;
            }
            return isLoggedOut;
        }
        public void changePwd(string token, string password)//функция смены пароля
        {
            List<Person> allPersons = getPersons();
            Person authuser = allPersons.Find(x => x.token == token);
            if (checkToken(token) == true)
            {
                using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
                {
                    string sql = String.Format(@"Update Persons Set Password = ""{0}"" Where id = {1}", password, authuser.id);
                    SQLiteCommand command = new SQLiteCommand(sql, Connect);
                    Connect.Open(); // открыть соединение
                    command = new SQLiteCommand(sql, Connect);
                    command.ExecuteNonQuery();
                }
                authuser.password = password;
            }

        }
        public void addOrder(Order order, string token)
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
            {
                Person currentUser = getPersons().First(x => x.token == token);
                if (checkToken(token) == true)
                {
                    //order.client = currentUser;
                    int personID = getPersons().Find(x => x.id == currentUser.id).id;
                    int animalID = getAnimals().Find(x => x.id == order.animal.id).id;
                    string sql = String.Format(@"Insert into Orders (State, DateStart, DateEnd, ClientID, AnimalID, DeliveryToTheHotel, FromDeliveryAddress, FromDeliveryTime, DeliveryFromHotel, ToDeliveryAddress, ToDeliveryTime, Comment, Price, DelTime) values (""{0}"", ""{1}"", ""{2}"", {3}, {4}, {5}, ""{6}"", ""{7}"", {8}, ""{9}"", ""{10}"", ""{11}"", {12}, NULL)", order.state, order.datestart.ToString("yyyy-MM-dd HH:MM"), order.dateend.ToString("yyyy-MM-dd HH:MM"), currentUser.id, animalID, order.deliverytothehotel, order.fromdeliveryaddress, order.fromdeliverytime, order.deliveryfromhotel, order.todeliveryaddress, order.todeliverytime, order.comment, order.price);
                    SQLiteCommand command = new SQLiteCommand(sql, Connect);
                    Connect.Open(); // открыть соединение
                    command = new SQLiteCommand(sql, Connect);
                    command.ExecuteNonQuery();
                }

            }
        }
        public void addReview(Review review, string token)
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
            {
                Person currentUser = getPersons().First(x => x.token == token);
                if (checkToken(token) == true)
                {
                    persons = getPersons();
                    animaltypes = getAnimalTypes();
                    int personID = persons.Find(x => x.state == 0 && x.id == currentUser.id).id;
                    int animaltypeID = animaltypes.Find(x => x.id == review.animaltype.id).id;
                    string sql = String.Format(@"Insert into Reviews (AnimalTypeID, Body, CreateTime, ClientID, DelTime) values ({0}, ""{1}"", ""{2}"", {3}, NULL)", animaltypeID, review.body, review.createtime.ToString("yyyy-MM-dd HH:MM"), currentUser.id);
                    SQLiteCommand command = new SQLiteCommand(sql, Connect);
                    Connect.Open(); // открыть соединение
                    command = new SQLiteCommand(sql, Connect);
                    command.ExecuteNonQuery();
                }

            }
        }
        private void checkDB()
        {
            if (!File.Exists(@"HotelFA.db")) // если базы данных нету, то...
            {
                SQLiteConnection.CreateFile(@"HotelFA.db"); // создать базу данных, по указанному пути содаётся пустой файл базы данных
                using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=HotelFA.db; Version=3"))
                {
                    string commandText = @"BEGIN TRANSACTION;
                    CREATE TABLE IF NOT EXISTS ""Persons"" (
	                    ""id""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        ""Token""	TEXT,
                        ""DateOfIssueToken""	DATETIME,
	                    ""State""	INTEGER,
                        ""Login""	TEXT,
	                    ""Password""	TEXT,
                        ""Name""	TEXT,
                        ""Phone""	TEXT,
	                    ""Email""	TEXT,  
	                    ""Birthday""	TEXT,
	                    ""Address""	TEXT,
                        ""DelTime"" DATETIME
                    );
                    CREATE TABLE IF NOT EXISTS ""Orders"" (
	                    ""id""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        ""State""	TEXT,
                        ""DateStart""   DATETIME,
                        ""DateEnd"" DATETIME,
                        ""ClientID""    INTEGER,
                        ""AnimalID""    INTEGER,
                        ""DeliveryToTheHotel""  INTEGER,
	                    ""FromDeliveryAddress"" STRING,
	                    ""FromDeliveryTime""	STRING,
	                    ""DeliveryFromHotel""	INTEGER,
	                    ""ToDeliveryAddress""	STRING,
	                    ""ToDeliveryTime""	STRING,
	                    ""Comment""	STRING,
                        ""Price""	DOUBLE,
                        ""DelTime"" DATETIME,
                        FOREIGN KEY(""ClientID"")   REFERENCES ""Persons""(""id""),
                        FOREIGN KEY(""AnimalID"")	REFERENCES ""Animals""(""id"")
                    );
                    CREATE TABLE IF NOT EXISTS ""Reviews"" (
	                    ""id""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        ""AnimalTypeID"" INTEGER,
                        ""Body""	TEXT,
                        ""CreateTime""	DATETIME,
	                    ""ClientID""	INTEGER,
	                    ""DelTime"" DATETIME,
                        FOREIGN KEY(""ClientID"") REFERENCES ""Persons""(""id""),
	                    FOREIGN KEY(""AnimalTypeID"") REFERENCES ""AnimalTypes""(""id"")
                    );
                    CREATE TABLE IF NOT EXISTS ""Animals"" (
	                    ""id""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	                    ""Name""	TEXT,
                        ""AnimalTypeID""	INTEGER,
                        ""Sex""	INTEGER,
                        ""Comment""	TEXT,
	                    ""Birthday""	TEXT,
	                    ""ClientID""	INTEGER,
	                    ""DelTime"" DATETIME,
                        FOREIGN KEY(""ClientID"") REFERENCES ""Persons""(""id""),
	                    FOREIGN KEY(""AnimalTypeID"") REFERENCES ""AnimalTypes""(""id"")
                    );
                    CREATE TABLE IF NOT EXISTS ""AnimalTypes"" (
	                    ""id""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	                    ""NameOfType""	TEXT,
                        ""DelTime""	DATETIME
                    );
                    CREATE TABLE IF NOT EXISTS ""Chats"" (
	                    ""id""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	                    ""ClientID""	INTEGER,
	                    ""DelTime""	DATETIME,
                        FOREIGN KEY(""ClientID"") REFERENCES ""Persons""(""id"")
                    );
                    CREATE TABLE IF NOT EXISTS ""ChatMessages"" (
	                    ""id""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        ""Time""	DATETIME,
                        ""ChatID""	INTEGER,
                        ""Text""    TEXT,
                        ""FilePath""    TEXT,
                        ""Status""    INTEGER,
	                    ""ClientID""	INTEGER,
	                    ""DelTime""	DATETIME,
                        FOREIGN KEY(""ClientID"") REFERENCES ""Persons""(""id""),
                        FOREIGN KEY(""ChatID"") REFERENCES ""Chats""(""id"")
                    );
                    CREATE TABLE IF NOT EXISTS ""Journals"" (
	                    ""id""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	                    ""Date""	DATETIME,
	                    ""TimeStart""	DATETIME,
                        ""TimeEnd""	DATETIME,
                        ""OrderID"" INTEGER,
                        ""AnimalID"" INTEGER,
                        ""WorkerID"" INTEGER,
                        ""Task"" TEXT,
                        ""Comment"" TEXT,
                        ""FilePath"" TEXT,
                        ""DelTime"" DATETIME,
                        FOREIGN KEY(""OrderID"") REFERENCES ""Orders""(""id""),
                        FOREIGN KEY(""AnimalID"") REFERENCES ""Animals""(""id""),
                        FOREIGN KEY(""WorkerID"") REFERENCES ""Workers""(""id"")
                    );
                    INSERT INTO ""Persons"" VALUES (1,'',NULL,1,'1','1','Tochilova E.A.', '88005553535','tochilova.1999@mail.ru','11.10.1999','Pervomaiskii', NULL);
                    INSERT INTO ""Persons"" VALUES (2,'V7gg32nUk0ejuJ+qTkvrdw==','2021-05-30 20:05',0,'2','2','Yurkov', '88005553535','gogganesko.1999@mail.ru','11.10.1999','Pervomaiskii', NULL);
                    INSERT INTO ""Orders"" VALUES (1,'V processe','2020-05-28 20:05', '2020-05-28 20:05', 2,1,0,'','',1,'rr','rr','Кошка привита',555,NULL);
                    INSERT INTO ""Journals"" VALUES (1,'2020-05-28 20:05','2020-05-28 20:05','2020-05-28 20:05',1,1,1,'Покормить кошку кормом', 'Она кусается', '' ,NULL);
                    INSERT INTO ""Animals"" VALUES (1,'Бесси', 1,0,'У нее панкреатит', '2016-01-20',2,NULL);
                    INSERT INTO ""AnimalTypes"" VALUES (1,'Кошка',NULL);
                    INSERT INTO ""AnimalTypes"" VALUES (2,'Собака',NULL);
                    INSERT INTO ""Reviews"" VALUES (1,1,'Моей кошке все понравилось!','2021-01-01 00:00',2,NULL);
                    INSERT INTO ""Chats"" VALUES (1,2,NULL);
                    INSERT INTO ""ChatMessages"" VALUES (1,'2021-03-01 20:05',1,'Привет! Как там моя кошка?','', 1, 2,NULL);
                    INSERT INTO ""ChatMessages"" VALUES (2,'2020-05-28 20:10',1,'Привет! Все хорошо!','', 0, 1,NULL);
                    COMMIT;
                    ";
                    SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                    Connect.Open(); // открыть соединение
                    Command.ExecuteNonQuery(); // выполнить запрос
                    Connect.Close(); // закрыть соединение
                }
            }

        }
    }
}
