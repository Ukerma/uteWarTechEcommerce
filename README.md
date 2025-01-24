<h1>🚀 UTE WarTech E-Commerce Platform</h1>
<p>
   This project is a <strong>military aircraft sales platform</strong> developed as part of the Internet Programming course at Piri Reis University. The platform is designed to provide users with a seamless e-commerce experience, focusing on modularity, scalability, and user-friendliness.
</p>

<h2>📌 Features</h2>
<ul>
   <li> <strong>User Functionality:</strong> Product browsing, shopping cart management, and secure checkout.</li>
   <li> <strong>Admin Panel:</strong> Manage banners, products, orders, users, and platform settings with dedicated tools.</li>
   <li> <strong>Authentication:</strong> Cookie-based authentication and secure password recovery via email.</li>
   <li> <strong>Responsive UI:</strong> A clean and dynamic user interface built with ASP.NET Core MVC.</li>
   <li> <strong>Database Integration:</strong> MSSQL database for reliable data management.</li>
</ul>

<h2>🛠️ Technologies Used</h2>
<ul>
   <li> <strong>Framework:</strong> ASP.NET Core MVC</li>
   <li> <strong>Database:</strong> MSSQL 20.0</li>
   <li> <strong>Programming Language:</strong> C#</li>
   <li> <strong>Development Environment:</strong> Visual Studio 2022</li>
   <li> <strong>NuGet Packages:</strong>
      <ul>
         <li>Microsoft.EntityFrameworkCore.Design (9.0.0)</li>
         <li>Microsoft.EntityFrameworkCore.SqlServer (9.0.0)</li>
         <li>Microsoft.EntityFrameworkCore.Tools (9.0.0)</li>
         <li>Newtonsoft.Json (13.0.3)</li>
         <li>System.Configuration.ConfigurationManager (9.0.1)</li>
      </ul>
   </li>
</ul>

<h2>📋 Project Structure</h2>
<ul>
   <li> <strong>Controllers:</strong> Responsible for handling user requests and executing business logic (e.g., <code>BannersController.cs</code>, <code>CommentsController.cs</code>).</li>
   <li> <strong>Models:</strong> Define the database structure and data validation rules (e.g., <code>Banner.cs</code>, <code>Comment.cs</code>).</li>
   <li> <strong>Views:</strong> Provide the user interface using Razor syntax (e.g., <code>Products/Index.cshtml</code>).</li>
   <li> <strong>Program.cs:</strong> Configures middleware, routing, and authentication mechanisms.</li>
</ul>

<h2>📂 File Structure</h2>
<pre>
UTEWarTech/
├── Controllers/
│   ├── BannersController.cs
│   ├── CommentsController.cs
├── Models/
│   ├── Db/
│   │   ├── Banner.cs
│   │   ├── Comment.cs
├── Views/
│   ├── Products/
│   │   ├── Index.cshtml
├── Program.cs
</pre>

<h2>🚀 How to Run</h2>
<ol>
   <li> Clone this repository: <code>git clone https://github.com/Ukerma/uteWarTechEcommerce.git</code></li>
   <li> Open the solution in Visual Studio 2022.</li>
   <li> Configure the database connection in <code>appsettings.json</code>.</li>
   <li> Apply database migrations using Entity Framework tools:</li>
   <ul>
      <li><code>Update-Database</code></li>
   </ul>
   <li> Run the application in IIS Express or your preferred hosting environment.</li>
</ol>

<h2>👨‍💻 Authors</h2>
<p>
   This project was developed by: 
   <ul>
      <li><strong>Umut Kerim Acar:</strong> Frontend and Backend Developer, UI/UX Designer, Graphic Designer, and Database Specialist</li>
      <li><strong>Tuna Durukan:</strong> Backend Developer, Quality Assurance, and Test Specialist</li>
      <li><strong>Emir Değirmenci:</strong> System and Network Administrator, Hosting and Domain Specialist, and Test Specialist</li>
   </ul>
</p>
