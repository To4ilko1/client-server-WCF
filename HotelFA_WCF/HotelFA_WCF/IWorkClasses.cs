using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace HotelFA_WCF
{
    [ServiceContract]
    public interface IWorkClasses
    {
        [OperationContract]
        string getCurrentUser(string login, string password);//
        [OperationContract]
        List<Order> getOrders(string token);
        [OperationContract]
        List<Order> getOrdersByDate(DateTime startDate, DateTime endDate, string token);
        [OperationContract]
        void changePwd(string token, string password);//
        [OperationContract]
        List<Person> getPersons();////////////////
        [OperationContract]
        void addOrder(Order order, string token);//другие значения на входе
        [OperationContract]
        void addReview(Review review, string token);//другие значения на входе
        [OperationContract]
        void addPerson(Person person);
        [OperationContract]
        void addAnimal(Animal animal, string token);//другие значения на входе
        [OperationContract]
        void addMessage(ChatMessage message, string token);//другие значения на входе
        [OperationContract]
        bool user_auth(String login, string password);
        [OperationContract]
        int logout(string token);//оставить так как есть?
        [OperationContract]
        List<Animal> getAnimals(string token);
        [OperationContract]
        List<AnimalType> getAnimalTypes(string token);
        [OperationContract]
        List<Animal> getAnimalsInHotel(string token);
        [OperationContract]
        List<Review> getReview();
        [OperationContract]
        List<Journal> getJournal(int animalid, string token);
        [OperationContract]
        List<ChatMessage> getChatMessages(int status, string token);
        [OperationContract]
        List<ChatMessage> getChatMessagesByDate(int status, DateTime datestart, DateTime dateend, string token);
       
        [OperationContract]
        List<Person> getAccountWorker(int workerid, string token);
        [OperationContract]
        List<Person> getAccount(string token);

    }
}
