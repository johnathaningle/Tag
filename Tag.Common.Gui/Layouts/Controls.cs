using Eto.Forms;
using Tag.Common.Gui.Models;
using System;
using Tag.Common.Gui.Services;

namespace Tag.Common.Gui.Layouts
{
    public class Views
    {
        public static StackLayout GetLoginLayout(LoginModel vm)
        {
            var row = new StackLayout();
            row.Orientation = Orientation.Horizontal;
            row.Spacing = 10;

            var stack = new StackLayout();
            stack.Padding = new Eto.Drawing.Padding(30);
            stack.Spacing = 10;
            stack.HorizontalContentAlignment = HorizontalAlignment.Center;
            stack.VerticalContentAlignment = VerticalAlignment.Center;

            //username
            var usernameStack = new StackLayout();
            usernameStack.HorizontalContentAlignment = HorizontalAlignment.Center;
            var usernameLabel = new Label();
            usernameLabel.Text = "Username";
            var usernameBox = new TextBox();
            usernameBox.Bind<LoginModel>("Username", vm, "Username", DualBindingMode.TwoWay);
            usernameStack.Items.Add(usernameLabel);
            usernameStack.Items.Add(usernameBox);
            row.Items.Add(usernameStack);

            //password
            var passwordStack = new StackLayout();
            passwordStack.HorizontalContentAlignment = HorizontalAlignment.Center;
            var passwordLabel = new Label();
            passwordLabel.Text = "Password";
            var textBox = new TextBox();
            textBox.Bind<LoginModel>("Password", vm, "Password", DualBindingMode.TwoWay);
            passwordStack.Items.Add(passwordLabel);
            passwordStack.Items.Add(textBox);
            row.Items.Add(passwordStack);

            //add the login form row
            stack.Items.Add(row);

            //login button
            var loginButton = new Button();
            loginButton.Text = "Login";
            loginButton.Command = vm.LoginCommand;
            stack.Items.Add(loginButton);
            loginButton.Click += new EventHandler<EventArgs>((sender, e) => {
                var vm = ServiceLocator.Current.Get<MainViewModel>();
            });

            return stack;
        }

    }
}