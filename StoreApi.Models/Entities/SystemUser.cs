namespace StoreApi.Models.Entities;

public class SystemUser
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    //Propiedades de navegacion 
    public List<Order> Orders { get; set; }
}