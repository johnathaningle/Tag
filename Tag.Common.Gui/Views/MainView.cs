using System;
using Eto.Forms;
using Tag.Common.Gui.Models;
using Tag.Common.Gui.Services;

namespace Tag.Common.Gui.Views
{
    public class MainView : Panel
    {
        public MainView()
        {
            usernameBox = new TextBox();
            passwordBox = new PasswordBox();

            var vm = new LoginModel();
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
            usernameBox.Bind<LoginModel>("Username", vm, "Username", DualBindingMode.TwoWay);
            usernameStack.Items.Add(usernameLabel);
            usernameStack.Items.Add(usernameBox);
            row.Items.Add(usernameStack);

            //password
            var passwordStack = new StackLayout();
            passwordStack.HorizontalContentAlignment = HorizontalAlignment.Center;
            var passwordLabel = new Label();
            passwordLabel.Text = "Password";
            passwordBox = new PasswordBox();
            passwordBox.Bind<LoginModel>("Password", vm, "Password", DualBindingMode.TwoWay);
            passwordStack.Items.Add(passwordLabel);
            passwordStack.Items.Add(passwordBox);
            row.Items.Add(passwordStack);

            //add the login form row
            stack.Items.Add(row);

            //login button
            var loginButton = new Button();
            loginButton.Text = "Login";
            loginButton.Command = vm.LoginCommand;
            stack.Items.Add(loginButton);
            loginButton.Click += new EventHandler<EventArgs>((sender, e) => {
                var username = usernameBox.Text;
                var password = passwordBox.Text;
                Console.WriteLine($"{username} {password}");
                var vm = ServiceLocator.Current.Get<MainViewModel>();
            });

            this.Content = stack;
        }
        private TextBox usernameBox { get; set; }
        public PasswordBox passwordBox { get; set; }
    }
}