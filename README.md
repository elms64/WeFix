This is a university project, web application designed to provide a comprehensive system for a case study on "WeFix Mechanics" 

This application uses Microsoft C#, ASP.NET Core 7.0, Entity Framework, the Microsoft Identity API and Razor Pages. Data is stored using SQLite in the <b>"App.db"</b> file included in the main project.



<br><br><br>
The application seeds several accounts into the database, with varying levels of authorisation. The "dotnet secret password" is set as <b>"Test1234,"</b>

<table>
  <tr>
    <th>Email</th>
    <th>Access Level / Role</th>
    <th>Permissions</th>
  </tr>
  <tr>
    <td>admin@example.com</td>
    <td>"SysAdmin"</td>
    <td>Admins have full control (CRUD) over the system and can approve appointments, remove users and permissions or part records, excluding the technician system for conciseness</td>
  </tr>
  <tr>
    <td>manager@example.com</td>
    <td>"Manager"</td>
    <td>Access to manage, approve and deny appointments, No access to user management</td>
  </tr>
  <tr>
    <td>technician@example.com</td>
    <td>"Technician"</td>
    <td>Technicians have access to an interface for recording details of services and generating invoices. View customer appointments, vehicles and full access to parts data</td>
  </tr>
  <tr>
    <td>reception@example.com</td>
    <td>"Reception"</td>
    <td>Permissions to access and create appointments. View parts and customer information</td>
  </tr>
  <tr>
    <td>customer@example.com</td>
    <td>"User"</td>
    <td>Low access level. Users can create new accounts, request and manage appointments associated with their account and view parts data. As well as edit their personal details</td>
  </tr>
  <tr>
    <td>customer1@example.com</td>
    <td>"User"</td>
    <td>--</td>
  </tr>
  <tr>
    <td>customer2@example.com</td>
    <td>"User"</td>
    <td>--</td>
  </tr>
  <tr>
    <td>customer3@example.com</td>
    <td>"User"</td>
    <td>--</td>
  </tr>
  <tr>
    <td>customer4@example.com</td>
    <td>"User"</td>
    <td>--</td>
  </tr>
</table>
