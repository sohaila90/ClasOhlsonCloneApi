using System.Collections;

namespace ClasOhlsonCloneApi;

public class PostData
{

    public PostData(int id, string firname, string lastname, string email, string number, string password)
    {
        Id = id;
        Firname = firname;
        Lastname = lastname;
        Email = email;
        Number = number;
        Password = password;
    }

   

    public int Id {get; set;}
    public string Firname { get; set; }
    public string Lastname { get; set; } 
    public string Email { get; set; }
    public string Number { get; set; }
    public string Password { get; set; }
    
}