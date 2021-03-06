# EventGuardian
## Spatio News Stands
CPT331 SP3 2016 RMIT University collborative project.

## Project Descriptions

### CPT331.Core
Contains shared types, such as extensions and the object model.

### CPT331.Data
Contains repository types.

### CPT331.Data.Migration
Console app used to create and populate the database.

### CPT331.Data.Parsers
Contains parser types used to convert various file-based data sources into a common object model.

### CPT331.iOS
iOS app implementation.

### CPT331.Web
ASP.NET MVC website used to administer the database.

### CPT331.Web.Tests
Visual Studio unit tests for the administration website.

### CPT331.WebAPI
ASP.NET Web API website used to serve data to a client.

### CPT331.WebAPI.Parsers
Contains parser types used to convert various web data sources into a common object model.

### CPT331.WebAPI.Tests
Visual Studio  unit tests for the RESTful API.

## GIT Setup
The very first step is to clone this repo. From gitbash:

1. Get git installed on your machine. It is recommended that you get familiar working with git bash, it is popular and doesn�t shield you from what git is actually doing (important)
2. Set up a directory on your machine for development work � e.g, 'C:\Development' is suitably creative
3. Open git bash, and CD into your development directory
4. Clone this repo: https://github.com/s3364859/CPT331.git
5. Take a fork of this repo from here: https://github.com/s3364859/CPT331
6. In git bash: git remote rename origin upstream
7. Note the URL of your fork�s repo � it should be something like this: https://github.com/YOURUSERNAME/CPT331.git
8. In git bash: git remote add origin: https://github.com/YOURUSERNAME/CPT331.git
9. Finally, run git remote -v, and you should see something like this:
 - origin      https://github.com/YOURUSERNAME/CPT331.git (fetch)
 - origin      https://github.com/YOURUSERNAME/CPT331.git (push)
 - upstream    https://github.com/s3364859/CPT331.git (fetch)
 - upstream    https://github.com/s3364859/CPT331.git (push)
10. All done, bam!

## Local Website Projects Setup
1. Make sure IIS is installed
2. Make sure you�ve cloned the git repo and have the latest code
3. Open the file C:\Windows\System32\drivers\etc\hosts
 - You may need to take ownership of this file as Windows typically doesn�t want you fiddling about in there
4. Add the following entries:
 - 127.0.0.1              cpt331.api.internal
 - 127.0.0.1              cpt331.web.internal
5. Save the file
 - You may need to change write permissions for this file, but eventually you will be able to save it
6. Verify these changes have worked:
 - Open a command prompt and ping each host � you should see a response from 127.0.0.1
7. Open up IIS Manager
8. Right click on Sites -> Add Website
 - Site name: CPT331 Web
 - Application pool: .NET v4.5
 - Physical path: \CPT331\CPT331.Web
 - Host name: cpt331.web.internal
 - Click OK
9. Right click on Sites -> Add Website
 - Site name: CPT331 Web API
 - Application pool: .NET v4.5
 - Physical path: \CPT331\CPT331.WebAPI
 - Host name: cpt331.api.internal
 - Click OK
10. The two sites are now setup
11. Verify your sites are working:
 - http://cpt331.api.internal/swagger/ - you should see a page with a green header
 - http://cpt331.web.internal/ - you should see a page about ASP.NET
 - You may need to adjust each site's connection string values to point to your SQL Server instance

## Database Setup
1. Make sure SQL Server is installed
 - 2016 is preferred, 2014 will cut the mustard however
2. Make sure you�ve cloned the git repo and have the latest code
3. Build the project
4. Ensure that you have located and extracted all the data sources to the following location:
 - XML: 'C:\Downloads\Crime Data\XML Data Sources'
 - KML: 'C:\Downloads\Crime Data\KML Data Sources'
 - The directory here should match the MigrationDataSourceDirectory value in the configuration settings of the migrator app
4. Open a command prompt, and change directory to the migration project directory
 - The path will be something like \CPT331\CPT331.Data.Migration\bin\Debug
5. Run the following command:
 - CPT331.Data.Migration -d True -x ALL -k ALL
 - The -d switch drops and recreates the database
 - The -x switch imports the data inside all the XML documents
 - The -k switch imports the data inside all the KML documents
 - More information is available on this below
6. Go and make a sanger - the import will take a while (15-30 minutes)
 - You may need to adjust the app's connection string values to point to your SQL Server instance
 
You should now be good to go!

## Migrator Console App - Importing Data
The migrator console app can be used to drop or create the database, run migrations, and also to import KML and XML data sources.

The migrator console app supports the following switches:
 - -d: drop and create the Database
 - -k: import KML Data
 - -s: use simple transaction log when creating the database. Recommended for development databases
 - -x: import XML Data

Examples:
 - Create a fresh database: CPT331.Data.Migration -d True
 - Load all KML files into an existing database: CPT331.Data.Migration -d False -k ALL
 - Load NSW XML data into an existing database: CPT331.Data.Migration -d False -x NSW
 - Load NSW and VIC XML data into an existing database: CPT331.Data.Migration -d False -x NSW,VIC

Note that using the 'ALL' value with the -k and -x switches is rewritten to a list of supported parsers of the appropriate type as defined by ParserFactory.SupportedKmlParserNames and ParserFactory.SupportedXmlParserNames.

## Project Contributions
Follow the steps below to contribute towards the EventGuardian app:

1. Make code changes to your local branch
2. Commit your code with a meaningful message using Git Bash.
3. Push changes to the origin remote. This will be fork described above.
   Do not push directly to the upstream remote.
4. Open GitHub and navigate to your fork.
5. Create a Pull Request from your current branch to the base fork s3364859/CPT331.
 - The code changes will be discussed and reviewed within the pull request.
 - On completion your changes may either be merged or rejected by another contributer.
