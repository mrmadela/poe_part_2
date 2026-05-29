using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace poe_part2
{// start of namespase
    
    public partial class MainWindow : Window
    {// start of mainwindow

        //creating an instance for the class Array
        // arrayList to store chatbot responses
        ArrayList reply = new ArrayList();

        // ArrayList to to store ignored words
        ArrayList ignore = new ArrayList();

        // object of classes
        // object for username validation and memory recall
        user_name check_name = new user_name();

        // object for chatbot processing
        chatbot bot;

        // object for automatic interest reminders
        auto_interest interestReminder;

        //global variable
        string username = "";


        public MainWindow()
        {// start of main method
            
            InitializeComponent();

            new respond(reply, ignore) { };

            // Creating an instance for the greet_user
            // with a constructor
            new voice_greet();

            bot = new chatbot(reply, ignore, chats);
            interestReminder = new auto_interest(chats);
        }// end of main method


        // start app event hendler
        private void start_app(object sender, RoutedEventArgs e)
        {// start of start_app method

            // set the greet_grid to collapse
            greet_grid.Visibility = Visibility.Collapsed;

            // set the welcome_grid to hidden
            welcome_grid.Visibility = Visibility.Hidden;

            //set the name_grid to visible
            name_grid.Visibility = Visibility.Visible;

        }// end of start_app method

        
        // submit name event hendler
        private void submit_name(object sender, RoutedEventArgs e)
        {//  start of submit_name method

            // check if the user has entered the name
            if (string.IsNullOrWhiteSpace(user_name.Text))
            {// start of if statement

                // display error message
                MessageBox.Show(
                    "Please enter your name before continuing.");


                // keep focus on textbox
                user_name.Focus();

                // stop method 
                return;
            }// end of if statement

            //check the user name from memory recall
            username = check_name.submit_name(user_name, chats);


            //Hide username page grid and set chats grid visible
            name_grid.Visibility = Visibility.Hidden;
            chatting_grid.Visibility = Visibility.Visible;

        }// end of sibmit_name method



        // send question event hendler
        private void send(object sender, RoutedEventArgs e)
        {// start of send event hendler 

            string rawQuestion = question.Text.Trim();

            // check if the question is null or empty and return a message
            if (string.IsNullOrWhiteSpace(rawQuestion)) 
            {
                // show error message
                message_display.show(chats, "ChatBot", "Please enter a question");
                return;

            }// end of if statement

            // display user's question in the chat area
            message_display.show(chats, username, rawQuestion);

            // send question to chatbot ia method
            bot.ai_check(rawQuestion, username);

            // automatically show user interest after some interections
            interestReminder.auto_show_interest(username, bot);

            // clear textbox after sending message
            question.Clear();

        }// end of send event hendler


    }// end of mainwindow class

}// end of namespace
