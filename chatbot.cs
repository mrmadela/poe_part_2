using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace poe_part2
{// start of namespace

    public class chatbot
    {// start of chatbot class

        // ArrayList for chatbot replies
        ArrayList reply;

        // ArrayList for ignored words
        ArrayList ignore;

        // Chat display ListBox
        ListBox chats;



        // Constructor
        public chatbot(ArrayList reply, ArrayList ignore, ListBox chats)
        {// start constructor

            this.reply = reply;
            this.ignore = ignore;
            this.chats = chats;

        }// end constructor


        // Chatbot response method
        public void ai_check(string question, string username)

        {// start ai_check method

            // Remove special characters
            question = special_character.RemoveSpecialCharacters(question);

            // Check if question is empty
            if (string.IsNullOrWhiteSpace(question))
            {// start if statement

                message_display.show(
                    chats,
                    "ChatBot",
                    "Please enter a valid question."
                );

                return;
            }// end if statement


            // Split question into words
            string[] words =
                question.ToLower().Split(
                    new char[]
                    {
                        ' ',
                        ',',
                        '.',
                        '?',
                        '!'
                    },

                    StringSplitOptions.RemoveEmptyEntries
                );


            // Check if question is follow-up
            bool followUp = conversation_memory.is_followup(words);


            // Continue previous topic
            if (followUp)
            {// start if statement

                question = conversation_memory.continue_topic(question);

                // Rebuild words
                words =
                    question.ToLower().Split(
                        new char[]
                        {
                            ' ',
                            ',',
                            '.',
                            '?',
                            '!'
                        },

                        StringSplitOptions.RemoveEmptyEntries
                    );

            }// end if statement


            // Random object
            Random random = new Random();


            // Store final responses
            List<string> finalResponses = new List<string>();


            // Check every word
            foreach (string word in words)
            {// start foreach loop


                // Ignore useless words
                if (
                    word.Length < 3 ||
                    ignore.Contains(word)
                )
                {
                    continue;
                }


                // List for topic responses
                List<string> topicResponses = new List<string>();


                // Search every response
                foreach (string item in reply)
                {// start foreach

                    // Convert object to string
                    string text = item.ToString();


                    // Split keyword and response
                    string[] parts = text.Split(':');

                    // Safety check
                    if (parts.Length < 2)
                    {
                        continue;
                    }


                    // Store keyword
                    string keyword = parts[0];

                    // Store response
                    string response = parts[1];

                    // Check keyword match
                    if (keyword == word)
                    {
                        topicResponses.Add(response);
                    }

                }// end foreach


                // If responses exist
                if (topicResponses.Count > 0)
                {// start if statement

                    // Pick random response
                    string selected =
                        topicResponses[
                            random.Next(
                                topicResponses.Count
                            )
                        ];


                    // Add response
                    finalResponses.Add(selected);


                    // Save topic memory
                    conversation_memory.save_topic(word);

                }// end if statement

            }// end foreach loop


            // Remove duplicate responses
            finalResponses =
                finalResponses
                .Distinct()
                .ToList();


            // If responses exist
            if (finalResponses.Count > 0)
            {// start if statement


                // Combine all responses
                string finalMessage =
                    string.Join(
                        " ",
                        finalResponses
                    );


                // Display chatbot response
                message_display.show(
                    chats,
                    "ChatBot",
                    finalMessage
                );

            }// end if statement


            // If no responses found
            else
            {// start else statement

                // Fallback responses
                string[] fallback =
                {
                    "I don't understand that.",
                    "Please ask another question.",
                    "Can you rephrase that?",
                    "I could not find an answer for that topic."
                };

                // Pick random fallback
                string randomFallback =
                    fallback[random.Next(fallback.Length)];

                // Display fallback
                message_display.show(
                    chats,
                    "ChatBot",
                    randomFallback
                );

            }// end else statement

        }// end ai_check method

    }// end chatbot class

}// end namespace