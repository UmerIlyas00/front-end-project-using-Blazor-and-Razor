# Blazor/Razor components with SSR(server side rendered) accessed through browser, built in .Net Core(2.1) using Entity Framework code first interfacing In-memory/MS-Sql db 

Project showcases Blazor/Razor Components frontend UI(using C#) rendered on server(SSR) consuming data from the in-memory/sql database using .Net Core service with entity framework code first to database pattern

Overall project communicates between client & server using SignalR(part of the framework and does not require explicit implementation) to render HTML(built on server) & C# oriented UI

<br/>

[Live demo link - https://dotnetefrazorcompssr.herokuapp.com/](https://dotnetefrazorcompssr.herokuapp.com/)

<br/>

![alt text](https://github.com/NileshSP/dotnetefrazorcompssr/blob/master/screenshot.gif "Working example..")

<br/>
# Steps to get the project running

Pre-requisites:

>1. [.Net Core 2.1 SDK](https://www.microsoft.com/net/download/dotnet-core/2.1)
>2. [Visual Studio Code](https://code.visualstudio.com/) or Recommended - [Visual Studio Community editon version 15.9.1](https://visualstudio.microsoft.com/vs/community/) or later editor

<br/>

Clone the current repository locally as
 `git clone https://github.com/NileshSP/dotnetefrazorcompssr.git`

<br/>

Steps: using Visual Studio community edition editor
>1. Open the solution file (DotnetEFRazorCompSSR.sln) available in the root folder of the downloaded repository
>2. Await until the project is ready as per the status shown in taskbar which loads required packages in the background
>3. Hit -> F5 or select 'Debug -> Start Debugging' option to run the project

<br/>

Steps: using Visual Studio code editor
>1. Open the root folder of the downloaded repository 
>2. Await until the project is ready as per the status shown in taskbar which loads required packages in the background
>3. Open Terminal - 'Terminal -> New Terminal' and execute commands as `dotnet build` & `dotnet run` sequentially
OR
>4. Hit -> F5 or select 'Debug -> Start Debugging' option to run the project

<br/>

Once the project is build and run, a browser page would be presented with navigation options on right wherein 'Websites data' option contains functionality related to data access from in-memory/sql database

<br/>

# Root folder contents: 
>1. DotnetEFRazorCompSSR.App folder: contains frontend UI built using Blazor/Razor components and .Net Core services
>2. DotnetEFRazorCompSSR.Server folder: server side rendering defaults
>3. DotnetEFRazorCompSSR.Tests folder: unit tests for services
>4. DotnetEFRazorCompSSR.sln solution file
>5. Readme.md file for project information

