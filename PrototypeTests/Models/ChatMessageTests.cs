using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.Models.Tests
{
    [TestClass()]
    public class ChatMessageTests
    {

        ApplicationUser toUser;
        ApplicationUser fromUser;
        string messagecontent; 

        public ChatMessageTests()
        {
            toUser = new ApplicationUser();
            toUser.Id = "TestIdTo";
            fromUser = new ApplicationUser();
            fromUser.Id = "TestIdFrom";
            messagecontent = "Test Content"; 
        }


        [TestMethod()]
        public void ChatMessageConstructorWithArgsTest()
        {
            ChatMessage chatmessage = new ChatMessage(fromUser, toUser, messagecontent);

            Assert.AreEqual(chatmessage.FromUser, fromUser);
            Assert.AreEqual(chatmessage.FromUserId, fromUser.Id);
            Assert.AreEqual(chatmessage.ToUser, toUser);
            Assert.AreEqual(chatmessage.ToUserId, toUser.Id);
            Assert.AreEqual(chatmessage.Message, messagecontent);
        }
    }
}