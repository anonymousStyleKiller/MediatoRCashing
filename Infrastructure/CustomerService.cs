using Core.Abstractions;
using Core.Entities;

namespace Infrastructure;

public class CustomerService : ICustomerService
{
    public static IEnumerable<Customer> Customers => new List<Customer>
    {
        new Customer{ Id = 1, Contact = "123456789", Email="test@gmail.com", FirstName="Anton", LastName = "Kharchenko"},
        new Customer{ Id = 2, Contact = "564514501", Email="test1@gmail.com", FirstName="Vlad", LastName = "Kharchenko"},
        new Customer{ Id = 3, Contact = "141510217", Email="test2@gmail.com", FirstName="Smith", LastName = "Kharchenko"},
        new Customer{ Id = 4, Contact = "254112152", Email="test3@gmail.com", FirstName="Mukesh", LastName = "Glinka"},
        new Customer{ Id = 5, Contact = "125452338", Email="test4@gmail.com", FirstName="Ivor", LastName = "Kharchenko"},
        new Customer{ Id = 6, Contact = "985171215", Email="test5@gmail.com", FirstName="Jack", LastName = "Kharchenko"},
        new Customer{ Id = 7, Contact = "653107410", Email="test6@gmail.com", FirstName="Marc", LastName = "Kharchenko"},
        new Customer{ Id = 8, Contact = "165357410", Email="test7@gmail.com", FirstName="Helena", LastName = "Kharchenko"},
        new Customer{ Id = 9, Contact = "012543413", Email="test8@gmail.com", FirstName="Jimmy", LastName = "Kharchenko"},
        new Customer{ Id = 10, Contact = "124633892", Email="test9@gmail.com", FirstName="Ura", LastName = "Kharchenko"},
    };

    public IEnumerable<Customer> GetCustomerList()
    {
        //Assume Database Response takes 3000 ms
        Thread.Sleep(3000);
        return Customers;
    }

    public Customer GetCustomer(int id)
    {
        //Assume Database Response takes 1000 ms
        Thread.Sleep(1000);
        return Customers.Where(c => c.Id == id).FirstOrDefault();
    }
}