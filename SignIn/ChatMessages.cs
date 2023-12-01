using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignIn
{
    public class ChatMessages
    {
        public ChatMessages()
        {
            
        }
        public ChatMessages(int IdUser1, int IdUser2)
        {
            user1ID = IdUser1;
            user2ID = IdUser2;
            if (messages == null)
                messages = new List<string>();
        }
        public int user1ID;
        public int user2ID;
        public List<string> messages;
    }
}
