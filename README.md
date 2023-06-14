This is a university project, web application designed to provide a comprehensive system for a case study on "WeFix Mechanics" 

This application uses Microsoft C#, ASP.NET Core 7.0, Entity Framework, Microsoft Identity API and Razor Pages.


Users can create new accounts, manage appointments and make reqeuests.

Admins have full control over the system and can approve appointments, remove users and permissions or part records.

Technicians have access to an interface for recording details of services and generating invoices.


The application includes several seeded accounts to get started with the dotnet secret password set as "Test1234,"

- admin@example.com                 Full access, excluding technician system
- manager@example.com               Access to manage, approve and deny appointments. No access to user management.
- technician@example.com            Access to technician system and permissions to view customer appointments, vehicles and full access to parts data.
- reception@example.com             Permissions to access and create appointments, view parts and customer information.

- customer@example.com              Low access level. Submit requests for appointments, read parts data and edit personal details.
- customer1@example.com
- customer2@example.com
- customer3@example.com
- customer4@example.com
