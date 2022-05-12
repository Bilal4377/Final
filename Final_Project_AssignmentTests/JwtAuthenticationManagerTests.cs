using Microsoft.VisualStudio.TestTools.UnitTesting;
using Final_Project_Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Final_Project_Assignment.Controllers;
using Final_Project_Assignment.Models;

namespace Final_Project_Assignment.Tests
{
    [TestClass()]
    public class JwtAuthenticationManagerTests
    {
        // If login credentials are incorrect, test passes.
        [TestMethod()]
        public void IncorrectLoginTest()
        {
            JwtAuthenticationManager manager = new JwtAuthenticationManager("IncorrectKey12345!");

            User user = new User
            {
                username = "nottest",
                password = "notpassword"
            };

            var ret = manager.Authenticate(user.username, user.password);

            Assert.IsNull(ret);
        }

        // If login credentials are correct, test passes.
        [TestMethod()]
        public void CorrectLoginTest()
        {
            JwtAuthenticationManager manager = new JwtAuthenticationManager("IncorrectKey12345!");

            User user = new User
            {
                username = "test",
                password = "password"
            };

            var ret = manager.Authenticate(user.username, user.password);

            Assert.IsNotNull(ret);
        }

        // IF-Statements for code below written in "JwtAuthenticationManager.cs" file.

        // If the username is greater than or equal to 10 in length, a string value returns, stating that password is too long and test passes.
        // This message is displayed in the swagger ui in the response body.
        [TestMethod()]
        public void IncorrectUsernameLength()
        {
            JwtAuthenticationManager manager = new JwtAuthenticationManager("IncorrectKey12345!");

            User user = new User
            {
                username = "1234567891",
                password = "password1"
            };

            var ret = manager.Authenticate(user.username, user.password);

            Assert.IsNotNull(ret);
        }

        // If the password contains ONLY numbers, a string value returns, stating that password also needs characters and test passes.
        // This message is displayed in the swagger ui in the response body.
        [TestMethod()]
        public void IncorrectDataTypeForPassword()
        {
            JwtAuthenticationManager manager = new JwtAuthenticationManager("IncorrectKey12345!");

            User user = new User
            {
                username = "user",
                password = "12345"
            };

            var ret = manager.Authenticate(user.username, user.password);

            Assert.IsNotNull(ret);
        }

        // If username and password are null, exception thrown which is handled and test passes.
        // This message is displayed in the swagger ui in the response body.
        [TestMethod()]
        public void NullUsernamePassword()
        {
            JwtAuthenticationManager manager = new JwtAuthenticationManager("IncorrectKey12345!");

            User user = new User
            {
                username = "",
                password = ""
            };

            var ret = manager.Authenticate(user.username, user.password);

            Assert.IsNotNull(ret);
        }
    }
}
