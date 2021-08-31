using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace HotelFA_WCF
{
    [DataContract]
    public class Review
    {
        [DataMember]
        public int id;
        [DataMember]
        public AnimalType animaltype;
        [DataMember]
        public string body;
        [DataMember]
        public DateTime createtime;
        [DataMember]
        public Person client;
        [DataMember]
        public DateTime deltime;
    }

    [DataContract]
    public class Order
    {
        [DataMember]
        public int id;
        [DataMember]
        public string state;
        [DataMember]
        public DateTime datestart;
        [DataMember]
        public DateTime dateend;    
        [DataMember]
        public Person client;
        [DataMember]
        public Animal animal;
        [DataMember]
        public int deliverytothehotel;
        [DataMember]
        public string fromdeliveryaddress;
        [DataMember]
        public string fromdeliverytime;
        [DataMember]
        public int deliveryfromhotel;
        [DataMember]
        public string todeliveryaddress;
        [DataMember]
        public string todeliverytime;
        [DataMember]
        public string comment;
        [DataMember]
        public double price;
        [DataMember]
        public DateTime deltime;
    }
    [DataContract]
    public class Person
    {
        [DataMember]
        public int id;
        [DataMember]
        public string token;
        [DataMember]
        public DateTime dateofissuetoken;
        [DataMember]
        public int state;
        [DataMember]
        public string login;
        [DataMember]
        public string password;
        [DataMember]
        public string name;
        [DataMember]
        public string phone;
        [DataMember]
        public string email;
        [DataMember]
        public string birthday;
        [DataMember]
        public string address;
        [DataMember]
        public DateTime deltime;
    }
    [DataContract]
    public class Animal
    {
        [DataMember]
        public int id;
        [DataMember]
        public string name;
        [DataMember]
        public AnimalType animaltype;
        [DataMember]
        public int sex;
        [DataMember]
        public string comment;
        [DataMember]
        public DateTime birthday;
        [DataMember]
        public Person client;
        [DataMember]
        public DateTime deltime;
    }
    [DataContract]
    public class AnimalType
    {
        [DataMember]
        public int id;
        [DataMember]
        public string nameoftype;
        [DataMember]
        public DateTime deltime;
    }
    [DataContract]
    public class Chat
    {
        [DataMember]
        public int id;
        [DataMember]
        public Person client;
        [DataMember]
        public DateTime deltime;
    }
    [DataContract]
    public class ChatMessage
    {
        [DataMember]
        public int id;
        [DataMember]
        public DateTime time;
        [DataMember]
        public Chat chat;
        [DataMember]
        public string text;
        [DataMember]
        public string filepath;
        [DataMember]
        public int status;
        [DataMember]
        public Person client;
        [DataMember]
        public DateTime deltime;
    }
    [DataContract]
    public class Journal
    {
        [DataMember]
        public int id;
        [DataMember]
        public DateTime date;
        [DataMember]
        public DateTime timestart;
        [DataMember]
        public DateTime timeend;
        [DataMember]
        public Order order;
        [DataMember]
        public Animal animal;
        [DataMember]
        public Person worker;
        [DataMember]
        public string task;
        [DataMember]
        public string comment;
        [DataMember]
        public string filepath;
        [DataMember]
        public DateTime deltime;
    }
}
