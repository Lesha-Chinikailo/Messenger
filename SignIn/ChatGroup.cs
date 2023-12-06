using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignIn;

public class ChatGroup
{
    public ChatGroup()
    {
        IdUsers = new List<int>();
        Messages = new List<string>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public List<int> IdUsers {  get; set; }

    public List<string> Messages {  get; set; }
}
