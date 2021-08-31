using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.ServiceModel.Configuration;
using HotelFA_WCF_client.WorkClassRef;

namespace HotelFA_WCF_client
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkClassesClient classesClient = new WorkClassesClient("BasicHttpBinding_IWorkClasses");
            int isAuth = 0;
            string token = "";
            int userChoice = 0;
            while (true)
            {
                if (isAuth == 0)
                {
                    Console.WriteLine("Что будем делать? \n1 - Авторизация \n2 - Регистрация \n3 - Посмотреть отзывы \n4 - Выход из программы");
                }
                if (isAuth == 1)
                {
                    Console.WriteLine("Что будем делать? \n3 - Посмотреть отзывы \n4 - Выход из программы \n5 - Просмотреть список животных \n6 - Добавить заказ \n7 - Добавить животное \n8 - Добавить отзыв \n9 - Смотреть свой профиль \n10 - Выйти из профиля \n11 - Просмотреть журнал \n12 - Смена пароля \n13 - Посмотреть заказы \n14 - Отправить сообщение в чат \n15 - Смотреть чат \n16 - Смотреть список животных в отеле \n17 - Смотреть информацию о работнике");
                }
                userChoice = Convert.ToInt32(Console.ReadLine());
                if (userChoice == 1 && isAuth == 0)//авторизация
                {

                    Console.WriteLine("Введите логин:");
                    string login = Convert.ToString(Console.ReadLine());
                    Console.WriteLine("Введите пароль:");
                    string password = Convert.ToString(Console.ReadLine());
                    if (classesClient.user_auth(login, password))
                    {
                        isAuth = 1;
                        token = classesClient.getCurrentUser(login, password);
                        Console.WriteLine("Вы успешно авторизовались!");
                    }
                    else
                    {
                        Console.WriteLine("Попытка авторизации провалилась!");
                    }
                }
                if (userChoice == 2 && isAuth == 0)//регистрация пользователя
                {
                    Person person = new Person();
                    Console.WriteLine("Введите логин:");
                    person.login = Convert.ToString(Console.ReadLine());
                    Console.WriteLine("Введите пароль:");
                    person.password = Convert.ToString(Console.ReadLine());
                    Console.WriteLine("Введите ФИО:");
                    person.name = Convert.ToString(Console.ReadLine());
                    Console.WriteLine("Введите телефон:");
                    person.phone = Convert.ToString(Console.ReadLine());
                    Console.WriteLine("Введите email:");
                    person.email = Convert.ToString(Console.ReadLine());
                    Console.WriteLine("Введите дату рождения в формате yyyy-MM-dd:");
                    person.birthday = Convert.ToString(Console.ReadLine());
                    Console.WriteLine("Введите адрес:");
                    person.address = Convert.ToString(Console.ReadLine());
                    person.dateofissuetoken = DateTime.Now;
                    person.state = 0;
                    person.token = "NULL";
                    classesClient.addPerson(person);
                    Console.WriteLine("Вы успешно прошли регистрацию!");
                }
                if (userChoice == 3)//посмотреть все отзывы
                {
                    Review[] reviews = classesClient.getReview();
                    Console.WriteLine("ID||Животное||Отзыв||Автор отзыва||Дата написания");
                    foreach (Review review in reviews)
                    {
                        Console.WriteLine("{0}||{1}||{2}||{3}||{4}", Convert.ToString(review.id), Convert.ToString(review.animaltype.nameoftype), Convert.ToString(review.body), Convert.ToString(review.client.name), Convert.ToString(review.createtime));
                    }
                }
                if (userChoice == 4)//Выход из программы
                {
                    Environment.Exit(0);
                }
                if (userChoice == 5 && isAuth == 1)//получить список животных
                {
                    Animal[] animals = classesClient.getAnimals(token);
                    Console.WriteLine("ID||Кличка животного||Тип животного||Пол||Комментарий||Дата рождения||Хозяин");
                    foreach (Animal animal in animals)
                    {
                        string animalsex;
                        if (animal.sex == 1)
                        {
                            animalsex = "Женский";
                        }
                        else
                        {
                            animalsex = "Мужской";
                        }
                        Console.WriteLine("{0}||{1}||{2}||{3}||{4}||{5}||{6}", Convert.ToString(animal.id), Convert.ToString(animal.name), Convert.ToString(animal.animaltype.nameoftype), animalsex, Convert.ToString(animal.comment), animal.birthday.ToString("yyyy-MM-dd"), Convert.ToString(animal.client.name));
                    }
                }
                if (userChoice == 6 && isAuth == 1)//добавить заказ
                {
                    Order order = new Order();
                    //order.client = currentUser;
                    order.state = "В обработке";
                    Console.WriteLine("Введите дату заезда в отель в формате yyyy-MM-dd:");
                    order.datestart = Convert.ToDateTime(Console.ReadLine());
                    Console.WriteLine("Введите дату отъезда из отеля в формате yyyy-MM-dd:");
                    order.dateend = Convert.ToDateTime(Console.ReadLine());
                    Animal[] animals = classesClient.getAnimals(token);
                    Person[] client = classesClient.getAccount(token);//????????
                    Console.WriteLine("ID||Кличка животного||Тип животного||Пол||Комментарий||Дата рождения||Хозяин");
                    foreach (Animal animal in animals)
                    {
                        Console.WriteLine("{0}||{1}||{2}||{3}||{4}||{5}||{6}", Convert.ToString(animal.id), Convert.ToString(animal.name), Convert.ToString(animal.animaltype.nameoftype), Convert.ToString(animal.sex), Convert.ToString(animal.comment), Convert.ToString(animal.birthday), Convert.ToString(animal.client.name));
                    }
                    Console.WriteLine("Выберите ID животного:");
                    int animalID = Convert.ToInt32(Console.ReadLine());
                    order.animal = animals.First(x => x.id == animalID);
                    Console.WriteLine("Вы согласны на доставку животного до отеля?\n1 - да\n2 - нет");
                    order.deliverytothehotel = Convert.ToInt32(Console.ReadLine());
                    if (order.deliverytothehotel == 1)
                    {
                        Console.WriteLine("Введите адрес, откуда мы сможем забрать вашего питомца: ");
                        order.fromdeliveryaddress = Convert.ToString(Console.ReadLine());
                        Console.WriteLine("Во сколько мы можем забрать вашего питомца: ");
                        order.fromdeliverytime = Convert.ToString(Console.ReadLine());
                    }
                    else
                    {
                        order.fromdeliveryaddress = null;
                        order.fromdeliverytime = null;
                    }
                    Console.WriteLine("Вы согласны на доставку животного из отеля к вам?\n1 - да\n2 - нет");
                    order.deliveryfromhotel = Convert.ToInt32(Console.ReadLine());
                    if (order.deliveryfromhotel == 1)
                    {
                        Console.WriteLine("Введите адрес, куда мы можем привезти вашего питомца: ");
                        order.todeliveryaddress = Convert.ToString(Console.ReadLine());
                        Console.WriteLine("Во сколько мы можем привезти вашего питомца: ");
                        order.todeliverytime = Convert.ToString(Console.ReadLine());
                    }
                    else
                    {
                        order.todeliveryaddress = null;
                        order.todeliverytime = null;
                    }
                    Console.WriteLine("Введите комментарий к заказу:");
                    order.comment = Convert.ToString(Console.ReadLine());
                    classesClient.addOrder(order, token);
                    Console.WriteLine("Заказ был успешно добавлен!");
                }
                if (userChoice == 7 && isAuth == 1)//добавить животное
                {  
                    Animal animal = new Animal();
                    Console.WriteLine("Введите кличку животного:");
                    animal.name = Convert.ToString(Console.ReadLine());
                    AnimalType[] animaltypes = classesClient.getAnimalTypes(token);
                    Console.WriteLine("ID||Тип животного");
                    foreach (AnimalType animaltype in animaltypes)
                    {
                        Console.WriteLine("{0}||{1}", Convert.ToString(animaltype.id), Convert.ToString(animaltype.nameoftype));
                    }
                    Console.WriteLine("Выберите тип животного (введите ID типа): ");
                    int animaltypeid = Convert.ToInt32(Console.ReadLine());
                    animal.animaltype = animaltypes.First(x => x.id == animaltypeid);
                    Console.WriteLine("Введите пол животного: 0-мужской, 1-женский");
                    animal.sex = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Введите комментарий:");
                    animal.comment = Convert.ToString(Console.ReadLine());
                    Console.WriteLine("Введите дату рождения животного в формате yyyy-MM-dd:");
                    animal.birthday = Convert.ToDateTime(Console.ReadLine());
                    classesClient.addAnimal(animal, token);
                    Console.WriteLine("Данные о животном успешно добавили!");
                }
                if (userChoice == 8 && isAuth == 1)//добавление отзыва
                {
                    Review review = new Review();
                    AnimalType[] animaltypes = classesClient.getAnimalTypes(token);
                    Console.WriteLine("ID||Тип животного");
                    foreach (AnimalType animaltype in animaltypes)
                    {
                        Console.WriteLine("{0}||{1}", Convert.ToString(animaltype.id), Convert.ToString(animaltype.nameoftype));
                    }
                    Console.WriteLine("Выберите тип животного (введите ID типа): ");
                    int animaltypeID = Convert.ToInt32(Console.ReadLine());
                    review.animaltype = animaltypes.First(x => x.id == animaltypeID);
                    Console.WriteLine("Введите текст отзыва: ");
                    review.body = Convert.ToString(Console.ReadLine());
                    review.createtime = DateTime.Now;
                    classesClient.addReview(review, token);
                    Console.WriteLine("Спасибо за отзыв!");
                }
                if (userChoice == 9 && isAuth == 1)//смотреть свой профиль
                {
                    Person[] accounts = classesClient.getAccount(token);
                    Console.WriteLine("Логин||Пароль||ФИО||Номер телефона||Email||Дата рождения||Адрес");
                    foreach (Person acc in accounts)
                    {
                        Console.WriteLine("{0}||{1}||{2}||{3}||{4}||{5}||{6}", Convert.ToString(acc.login), Convert.ToString(acc.password), Convert.ToString(acc.name), Convert.ToString(acc.phone), Convert.ToString(acc.email), Convert.ToString(acc.birthday), Convert.ToString(acc.address));
                    }
                }
                if (userChoice == 10 && isAuth == 1)//выйти из профиля
                {
                    classesClient.logout(token);
                    isAuth = 0;
                }
                if (userChoice == 11 && isAuth == 1)//посмотреть журнал
                {
                    Animal[] animals = classesClient.getAnimals(token);
                    Console.WriteLine("Выберите ID животного");
                    Console.WriteLine("ID||Кличка животного||Тип животного||Пол||Комментарий||Дата рождения||Хозяин");
                    foreach (Animal animal in animals)
                    {
                        string animalsex; 
                        if (animal.sex == 1)
                        {
                            animalsex = "Женский";
                        }
                        else
                        {
                            animalsex = "Мужской";
                        }
                        Console.WriteLine("{0}||{1}||{2}||{3}||{4}||{5}||{6}", Convert.ToString(animal.id), Convert.ToString(animal.name), Convert.ToString(animal.animaltype.nameoftype), animalsex, Convert.ToString(animal.comment), Convert.ToString(animal.birthday), Convert.ToString(animal.client.name));
                    }
                    int animalID = Convert.ToInt32(Console.ReadLine());
                    Journal[] journals = classesClient.getJournal(animalID, token);
                    Console.WriteLine("ID||Дата||Время начала||Время конца||ID заказа||Кличка животного||ФИО работника||ID работника||Поручение||Комментарий||Вложение");
                    foreach (Journal journal in journals)
                    {
                        Console.WriteLine("{0}||{1}||{2}||{3}||{4}||{5}||{6}||{7}||{8}||{9}||{10}", Convert.ToString(journal.id), Convert.ToString(journal.date), Convert.ToString(journal.timestart), Convert.ToString(journal.timeend), Convert.ToString(journal.order.id), Convert.ToString(journal.animal.name), Convert.ToString(journal.worker.name), Convert.ToString(journal.worker.id), Convert.ToString(journal.task), Convert.ToString(journal.comment), Convert.ToString(journal.filepath));
                    }
                }
                if (userChoice == 12 && isAuth == 1)//смена пароля
                {
                    Console.WriteLine("Введите новый пароль");
                    string pwd = Console.ReadLine();
                    classesClient.changePwd(token, pwd);
                    Console.WriteLine("Пароль успешно изменён");
                }
                if (userChoice == 13 && isAuth == 1)//посмотреть заказы
                {
                    Order[] myOrders;
                    Console.WriteLine("Введите начальную дату в формате yyyy-MM-dd");
                    String startDate = Convert.ToString(Console.ReadLine());
                    Console.WriteLine("Введите конечную дату в формате yyyy-MM-dd");
                    String endDate = Convert.ToString(Console.ReadLine());
                    if (startDate == "" && endDate == "")
                    {
                        myOrders = classesClient.getOrders(token);
                    }
                    else
                    {
                        myOrders = classesClient.getOrdersByDate(Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), token);
                    }
                    Console.WriteLine("ID||Статус заказа||Дата въезда||Дата выезда||Клиент||Животное||Доставка до отеля||Адрес (откуда забрать животное)||Время (во сколько забрать животное)||Доставка из отеля||Адрес (куда привезти животное)||Время (во сколько привезти животное)||Комментарий к заказу||Стоимость");
                    foreach (Order order in myOrders)
                    {
                        Console.WriteLine("{0}||{1}||{2}||{3}||{4}||{5}||{6}||{7}||{8}||{9}||{10}||{11}||{12}||{13}", Convert.ToString(order.id), order.state, Convert.ToString(order.datestart), Convert.ToString(order.dateend), order.client.name, order.animal.name, order.deliverytothehotel, order.fromdeliveryaddress, order.fromdeliverytime, order.deliveryfromhotel, order.todeliveryaddress, order.todeliverytime, order.comment, Convert.ToString(order.price));
                    }
                }
                if (userChoice == 14 && isAuth == 1)//отправить сообщение в чат
                {
                    ChatMessage chatmessage = new ChatMessage();
                    Console.WriteLine("Введите путь до файла: ");
                    chatmessage.filepath = Convert.ToString(Console.ReadLine());
                    chatmessage.status = 0;
                    Console.WriteLine("Введите текст сообщения: ");
                    chatmessage.text =Convert.ToString(Console.ReadLine());
                    chatmessage.time = DateTime.Now;
                    classesClient.addMessage(chatmessage, token);
                    Console.WriteLine("Сообщение отправлено!");
                }
                if (userChoice == 15 && isAuth == 1)//смотреть чат
                {
                    ChatMessage[] chatmessages;
                    Console.WriteLine("Показать непрочитанные сообщения? 0 - нет, 1 - да: ");
                    int status = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Введите начальную дату в формате yyyy-MM-dd");
                    String startDate = Console.ReadLine();
                    Console.WriteLine("Введите конечную дату в формате yyyy-MM-dd");
                    String endDate = Console.ReadLine();
                    if (startDate == "" && endDate == "")
                    {
                        chatmessages = classesClient.getChatMessages(status, token);
                    }
                    else
                    {
                        chatmessages = classesClient.getChatMessagesByDate(status, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), token);
                    }
                    Console.WriteLine("Время||Текст||ФИО отправителя||Статус||Вложение");
                    foreach (ChatMessage message in chatmessages)
                    {
                        string statusmessage = "";
                        if (message.status == 0){
                            statusmessage = "Непрочитанное";
                        }
                        else
                        {
                            statusmessage = "Прочитанное";
                        }
                        Console.WriteLine("{0}||{1}||{2}||{3}||{4}", Convert.ToString(message.time), message.text, message.client.name, statusmessage, Convert.ToString(message.filepath));
                    }
                }
                if (userChoice == 16 && isAuth == 1)//смотреть список животных в отеле
                {
                    Animal[] animals = classesClient.getAnimalsInHotel(token);
                    Console.WriteLine("ID||Кличка животного||Тип животного||Пол||Комментарий||Дата рождения");
                    foreach (Animal animal in animals)
                    {
                        string sexan;
                        if (animal.sex == 0)
                        {
                            sexan = "Женский";
                        }
                        else
                        {
                            sexan = "Мужской";
                        }
                        Console.WriteLine("{0}||{1}||{2}||{3}||{4}||{5}", Convert.ToString(animal.id), Convert.ToString(animal.name), Convert.ToString(animal.animaltype.nameoftype), sexan, Convert.ToString(animal.comment), animal.birthday.ToString("yyyy-MM-dd"));
                    }
                }
                if (userChoice == 17 && isAuth == 1)//смотреть информацию о работнике
                {
                    Console.WriteLine("Введите ID работника: ");
                    int workerID = Convert.ToInt32(Console.ReadLine());
                    Person[] accounts = classesClient.getAccountWorker(workerID, token);
                    Console.WriteLine("Логин||Пароль||ФИО||Номер телефона||Email||Дата рождения||Адрес");
                    foreach (Person acc in accounts)
                    {
                        Console.WriteLine("{0}||{1}||{2}||{3}||{4}||{5}||{6}", Convert.ToString(acc.login), Convert.ToString(acc.password), Convert.ToString(acc.name), Convert.ToString(acc.phone), Convert.ToString(acc.email), Convert.ToString(acc.birthday), Convert.ToString(acc.address));
                    }
                }
            }

        }
    }
}
