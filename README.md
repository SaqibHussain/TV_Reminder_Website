TV Reminder System
========

The TV reminder system is a website which allows users to subscribe to TV shows they may watch and recieve reminds for when they will air via email and text. It was built as my final year project for my undergraduate degree.

The website is built using C#, pulls TV information from the TVRage API, utlises forms authentication, a Data Access layer for all communication between it and the backend SQL database. 

A subsystem "Updater" is written in C# and runs to keep the local TV databases updated using the TVRage API and to send any necessary alerts and reminders to users. The updater shows usage examples of SQL connections, Collections, HttpWebRequests, Streams, Exceptions, timers, SmptClients, external libraries and general OO programming concepts.

The usage text file defines an easy to follow way to make the website live if you wish to test it.