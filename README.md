# Owleye
A Simple Endpoint Availability Checker. 

Owleye is a .Net based watchdog system in order to availability check of endpoints and notify interrupts via email,sms, etc.
The main component of this system is a Windows service which, after running, while registering all dependencies, uses Quartz as a scheduling system.

By default, we have four time periods set:
- thirty seconds
- one minute
- five minutes
- fifteen minutes

At the time of Windows service loading, these intervals are registered and started with the help of quartz. Then the system refers to the database in the specified time intervals and finds the sensors corresponding to the time interval and then executes it.
All the information of endpoints, sensors and timers are stored in a SQL database(by default), its structure is designed to be very simple and flexible.
After that a dispatcher receives commands and directs them to the handler associated with that command. For example, the ping command is sent to the associated handler.
In each of the handlers, the activity history of that endpoint is stored. If there is a change in the service status. The related change is sent to the related handlers with notification. Here, Redis is used to keep the history of activities(For example, notification via email or SMS).


  - Ping availability of an IP Address
  - DNS Resolve Check
  - Page Load Check
 
 Owleye is based on .Net core 5/SQL/Entity Framework/Redis/Quartz

 
 |  Other technologies | README |
| ------ | ------ |
| Quartz for backgroud jobs | https://github.com/quartz-scheduler/quartz |
| MediatR for messaging | https://github.com/jbogard/MediatR |
| Redis for caching and memory storage | https://github.com/redis/redis |
 
License
----
MIT
 
