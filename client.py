from zeep import Client
import datetime
from datetime import datetime
import re

client = Client('http://localhost:8080/hotelfa?wsdl')
isauth = 0
userСhoice = 0
token = ""

def print_messages(messages):
    print("="*45)
    for message in messages:
        time = str(message["time"])[0:10] + " " + str(message["time"])[11:19]
        if message["status"] == 0:
            statusmsg = "Непрочитанное"
        else:
            statusmsg = "Прочитанное"
        if message["filepath"] == "":
            print("Время: %s\nТекст: %s\nСтатус: %s\nID человека: %s\n" %(time, message["text"], statusmsg, message["client"]["name"]))
        else:
            print("Время: %s\nТекст: %s\nСтатус: %s\nID человека: %s\nФото: %s\n" % (time, message["text"], statusmsg, message["client"]["name"], message["filepath"]))

def print_animals(animals):
    print("="*45)
    for animal in animals:
        if animal["sex"] == 0:
            animalsex = "Мужской"
        else:
            animalsex = "Женский"
        print("ID животного: %s\nКличка: %s\nТип животного: %s\nПол: %s\nКомментарий: %s\nДата рождения: %s\n" % (animal["id"], animal["name"], animal["animaltype"]["nameoftype"], animalsex, animal["comment"], animal["birthday"]))

def print_animaltypes(animaltypes):
    print("="*45)
    for animaltype in animaltypes:
        print("ID типа: %s\nНаименование: %s\n" % (animaltype["id"],animaltype["nameoftype"]))

def print_animals_in_hotel(animals):
    print("="*45)
    for animal in animals:
        if animal["sex"] == 0:
            animalsex = "Мужской"
        else:
            animalsex = "Женский"
        print("ID животного: %s\nКличка: %s\nТип животного: %s\nПол: %s\nКомментарий: %s\nДата рождения: %s\n" % (animal["id"], animal["name"], animal["animaltype"]["nameoftype"], animalsex, animal["comment"], animal["birthday"]))

def print_journals(journals):
    print("="*45)
    for journal in journals:
        date = "1"
        date = str(journal["timestart"])[0:10]
        tstart = str(journal["timestart"])[11:19]
        tend = str(journal["timeend"])[11:19]
        print("ID журнала: %s\nДата: %s\nВремя начала: %s\nВремя конца: %s\nID заказа: %s\nID животного: %s\nID работника: %s\nПоручение: %s\nКомментарий: %s\nФото: %s\n" % (
            journal["id"], date, tstart, tend, journal["order"]["id"], journal["animal"]["name"], journal["worker"]["id"], journal["task"], journal["comment"], journal["filepath"]))

def print_orders(orders):#добавить клиента имя
    for order in orders:
        datestart = str(order["datestart"])[0:10]
        dateend = str(order["dateend"])[0:10]
        if (order["deliverytothehotel"] == 1) & (order["deliveryfromhotel"] == 1):
            print("ID заказа: %s\nЦена: %s\nID животного: %s\nКличка животного: %s\nДата заезда: %s\nДата выезда: %s\nДоставка до отеля: %s\nДоставка из отеля: %s\nАдрес доставки до отеля: %s\nАдрес доставки из отеля: %s\nВремя доставки до отеля: %s\nВремя доставки из отеля: %s\nКомментарий: %s\nСтатус: %s\n" % (
                order["id"], order["price"], order["animal"]["id"], order["animal"]["name"], datestart, dateend, order["deliverytothehotel"], order["deliveryfromhotel"], order["fromdeliveryaddress"], order["todeliveryaddress"], order["fromdeliverytime"], order["todeliverytime"], order["comment"], order["state"]))
        if (order["deliverytothehotel"] == 0) & (order["deliveryfromhotel"] == 0):
            print("ID заказа: %s\nЦена: %s\nID животного: %s\nКличка животного: %s\nДата заезда: %s\nДата выезда: %s\nКомментарий: %s\nСтатус: %s\n" % (
                order["id"], order["price"], order["animal"]["id"], order["animal"]["name"], datestart, dateend, order["comment"], order["state"]))
        if (order["deliverytothehotel"] == 1) & (order["deliveryfromhotel"] == 0):
            print("ID заказа: %s\nЦена: %s\nID животного: %s\nКличка животного: %s\nДата заезда: %s\nДата выезда: %s\nДоставка до отеля: %s\nАдрес доставки до отеля: %s\nВремя доставки до отеля: %s\nКомментарий: %s\nСтатус: %s\n" % (
                order["id"], order["price"], order["animal"]["id"], order["animal"]["name"], datestart, dateend, order["deliverytothehotel"], order["fromdeliveryaddress"], order["fromdeliverytime"], order["comment"], order["state"]))
        if (order["deliverytothehotel"] == 0) & (order["deliveryfromhotel"] == 1):
            print("ID заказа: %s\nЦена: %s\nID животного: %s\nКличка животного: %s\nДата заезда: %s\nДата выезда: %s\nДоставка из отеля: %s\nАдрес доставки из отеля: %s\nВремя доставки из отеля: %s\nКомментарий: %s\nСтатус: %s\n" % (
                order["id"], order["price"], order["animal"]["id"], order["animal"]["name"], datestart, dateend,  order["deliveryfromhotel"], order["todeliveryaddress"], order["todeliverytime"], order["comment"], order["state"]))

def print_reviews(reviews):
    print("="*45)
    for review in reviews:
        time = str(review["createtime"])[0:10] + " " + str(review["createtime"])[11:19]
        # time = datetime.strptime(review["AddTime"],"%Y-%m-%d %I:%M")
        print("ID отзыва: %s\nОтправитель: %s\nТекст: %s\nТип животного: %s\nВремя добавления: %s\n" % (
            review["id"], review["client"]["name"], review["body"], review["animaltype"]["nameoftype"], time))

def print_account(account):
    print("="*45)
    for acc in account:
        print("ID: %s\nФИО: %s\nТелефон: %s\nE-mail: %s\nДата рождения: %s\nАдрес: %s\n" %(acc["id"], acc["name"], acc["phone"], acc["email"], acc["birthday"], acc["address"]))
while True:
    if (isauth == 0):
        {
            print("Главное меню: \n1 - Авторизоваться \n2 - Зарегистрироваться \n3 - Посмотреть отзывы\n4 - Выйти из программы\n")
        }
    task = int(input())
    msg = {}
    if task == 1:
        login = input("Введите логин:")
        pwd = input("Введите пароль:")
        if (client.service.user_auth(login, pwd) == True):
            isauth = 1
            token = client.service.getCurrentUser(login, pwd)#возвращает токен
            print("Вы успешно авторизовались!")
        else:
            print("Попытка авторизации провалилась!")
    if task == 2:
        person = {}
        person["token"] = ""
        person["dateofissuetoken"] = None
        person["login"] = str(input("Введите логин:\n"))
        person["password"] = str(input("Введите пароль:\n"))
        person["name"] = str(input("Введите ФИО:\n"))
        person["phone"] = str(input("Введите телефон:\n"))
        person["email"] = str(input("Введите e-mail:\n"))
        person["birthday"] = str(input("Введите дату рождения:\n"))
        person["address"] = str(input("Введите адрес:\n"))
        person["state"] = 0
        client.service.addPerson(person)
        print("Регистрация прошла успешна!")
    if task == 3:
        reviews = client.service.getReview()
        if reviews != None:
            print_reviews(reviews)
        else:
            print("Список пуст\n")
    if task == 4:
        exit(0)
    if task == 5:
        animals = client.service.getAnimals(token)
        if animals != None:
            print_animals(animals)
        else:
            print("Список пуст\n")
    if task == 6:
        order = {}
        animals = client.service.getAnimals(token)
        print_animals(animals)
        animalID = input("Введите ID животного:\n")
        for animal in animals:
            if (int(animalID) == animal["id"]):
                order["animal"] = animal
        orderdatestart = str(input("Введите дату заезда в отель в формате\nгггг-мм-дд: "))
        while re.findall(r'[0-9]{4}-[0-9]{2}-[0-9]{2}', orderdatestart) == []:
            orderdatestart = input(("Неправильный формат! Введите дату заезда в отель в формате\nгггг-мм-дд: "))
        order["datestart"] = orderdatestart
        orderdateend = str(
            input("Введите дату отъезда из отеля в формате\nгггг-мм-дд: "))
        while re.findall(r'[0-9]{4}-[0-9]{2}-[0-9]{2}', orderdateend) == []:
            orderdateend = input(("Неправильный формат! Введите дату заезда в отель в формате\nгггг-мм-дд: "))
        order["dateend"] = orderdateend
        order["deliverytothehotel"] = int(input("Вы согласны на доставку животного до отеля: 0-нет, 1-да "))
        if order["deliverytothehotel"] == 1:
            order["fromdeliveryaddress"] = str(input("Введите адрес, откуда мы сможем забрать вашего питомца: "))
            order["fromdeliverytime"] = str(input("Во сколько мы можем забрать вашего питомца: "))
        else:
            order["fromdeliveryaddress"] = None
            order["fromdeliverytime"] = None
        order["deliveryfromhotel"] = int(input("Вы согласны на доставку животного из отеля к вам: 0-нет, 1-да "))
        if order["deliveryfromhotel"] == 1:
            order["todeliveryaddress"] = str(input("Введите адрес, куда мы можем привезти вашего питомца: "))
            order["todeliverytime"] = str(input("Во сколько мы можем привезти вашего питомца: "))
        else:
            order["todeliveryaddress"] = None
            order["todeliverytime"] = None
        order["state"] = "В процессе"
        order["price"] = 3000
        order["comment"] = str(input("Введите комментарий к заказу: "))
        client.service.addOrder(order, token)
        print("Заказ был успешно добавлен")
    if task == 7:
        animal = {}
        animal["name"] = str(input("Введите кличку животного:\n"))
        animaltypes = client.service.getAnimalTypes(token)
        print_animaltypes(animaltypes)
        animaltypeID = input("Выберите ID типа животного\n")
        for animaltype in animaltypes:
            if (int(animaltypeID) == animaltype["id"]):
                animal["animaltype"] = animaltype
        animal["sex"] = int(input("Введите пол животного: 0-мужской, 1-женский\n"))
        animal["comment"] = str(input("Введите комментарий:\n"))
        animal["birthday"] = str(input("Введите дату рождения:\n"))
        client.service.addAnimal(animal, token)
        print("Информация о животном успешно добавлена!")
    if task == 8:
        review = {}
        animaltypes = client.service.getAnimalTypes(token)
        print_animaltypes(animaltypes)
        animaltypeID = input("Выберите ID типа животного\n")
        for animaltype in animaltypes:
            if (int(animaltypeID) == animaltype["id"]):
                review["animaltype"] = animaltype
        review["body"] = str(input("Введите текст отзыва:\n"))
        review["createtime"] = datetime.now()
        client.service.addReview(review, token)
        print("Отзыв был успешно добавлен")
    if task == 9:
        acc = client.service.getAccount(token)
        print_account(acc)
    if task == 10:
        client.service.logout(token)
        isauth = 0
    if task == 11:
        animalid = input("Введите ID животного:\n")
        journals = client.service.getJournal(animalid, token)
        if journals != None:
            print_journals(journals)
        else:
            print("Список пуст\n")
    if task == 12:
        pwd = input("Введите новый пароль\n")
        client.service.changePwd(token, pwd)
        print("Пароль успешно изменён")
    if task == 13:
        sorting = int(input("Осортировать заказы по дате? 0 - нет, 1 - да: "))
        if sorting == 1:
            orderdatestart = str(input("Введите начальную дату для поиска в формате\nгггг-мм-дд: "))
            if orderdatestart !="":
                while re.findall(r'[0-9]{4}-[0-9]{2}-[0-9]{2}', orderdatestart) == []:
                    orderdatestart = input(("Неправильный формат! Введите начальную дату для поиска в формате\nгггг-мм-дд: "))
            DateStart = orderdatestart
            orderdateend = str(input("Введите конечную дату для поиска в формате\nгггг-мм-дд: "))
            if orderdateend !="":
                while re.findall(r'[0-9]{4}-[0-9]{2}-[0-9]{2}', orderdateend) == []:
                    orderdateend = input(("Неправильный формат! Введите конечную дату для поиска в формате\nгггг-мм-дд: "))
            DateEnd = orderdateend
            myOrders = client.service.getOrdersByDate(DateStart, DateEnd, token)
            if myOrders != None:
                print_orders(myOrders)
            else:
                print("Список пуст\n")
        else:
            myOrders = client.service.getOrders(token)
            if myOrders != None:
                print_orders(myOrders)
            else:
                print("Список пуст\n")
    if task == 14:
        message = {}
        message["filepath"] = str(input("Введите путь до файла:"))
        if message["filepath"] == "":
            message["filepath"] = None
        message["text"] = str(input("Введите текст сообщения:"))
        message["time"] = datetime.now()
        message["status"] = 0
        client.service.addMessage(message, token)
        print("Сообщение отправлено!")
    if task == 15:
        status = int(input("Показать непрочитанные сообщения? 0 - нет, 1 - да: "))
        sorting = int(input("Осортировать сообщния по дате? 0 - нет, 1 - да: "))
        if sorting == 1:
            msgdatestart = str(input("Введите начальную дату для поиска в формате\nгггг-мм-дд-чч-мм : "))
            while re.findall(r'[0-9]{4}-[0-9]{2}-[0-9]{2}-[0-9]{2}-[0-9]{2}', msgdatestart) == []:
                msgdatestart = input(("Неправильный формат! Введите начальную дату для поиска в формате\nгггг-мм-дд: "))
            DateStart = msgdatestart
            msgdateend = str(input("Введите конечную дату для поиска в формате\nгггг-мм-дд-чч-мм: "))
            while re.findall(r'[0-9]{4}-[0-9]{2}-[0-9]{2}-[0-9]{2}-[0-9]{2}', msgdateend) == []:
                msgdateend = input(("Неправильный формат! Введите конечную дату для поиска в формате\nгггг-мм-дд-чч-мм: "))
            DateEnd = msgdateend
            messages = client.service.getChatMessagesByDate(status, DateStart, DateEnd, token)
            if messages != None:
                print_messages(messages)
            else:
                print("Список пуст\n")
        else:
            messages = client.service.getChatMessages(status, token)
            if messages != None:
                print_messages(messages)
            else:
                print("Список пуст\n")
    if task == 16:
        animals_in_hotel = client.service.getAnimalsInHotel(token)
        if animals_in_hotel!= None:
            print_animals(animals_in_hotel)
        else:
            print("Список пуст\n")
    if task == 17:
        worker_id = str(input("Введите ID работника: "))
        workeracc = client.service.getAccountWorker(worker_id, token)
        if workeracc != None:
            print_account(workeracc)
        else:
            print("Список пуст\n")
    if (isauth == 1):
        print("2 - Зарегистрироваться")
        print("3 - Посмотреть отзывы")
        print("4 - Выйти из программы")
        print("5 - Просмотреть список животных")
        print("6 - Добавить заказ")
        print("7 - Добавить животное")
        print("8 - Добавить отзыв")
        print("9 - Смотреть свой профиль")
        print("10 - Выйти из профиля")
        print("11 - Просмотреть журнал")
        print("12 - Смена пароля")
        print("13 - Посмотреть заказы")
        print("14 - Отправить сообщение в чат")
        print("15 - Смотреть чат")
        print("16 - Смотреть список животных в отеле")
        print("17 - Смотреть информацию о работнике")