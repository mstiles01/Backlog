# Backlog

Backlog uses Entity Frame Work for use steps to download are listed below:

Install the NuGet package from an OTN-downloaded local package source or from nuget.org.


   From Local Package Source
   -------------------------
   A. Click on the Settings button in the lower left of the dialog box.

   B. Click the "+" button to add a package source. In the Source field, enter in the directory location where the 
   NuGet package(s) were downloaded to. Click the Update button, then the Ok button.

   C. On the left side, under the Online root node, select the package source you just created. The ODP.NET for 
   Entity Framework NuGet package will appear.


   From Nuget.org
   --------------
   A. In the Search box in the upper right, search for the package with id, 
   "Oracle.ManagedDataAccess.EntityFramework". Verify that the package uses this unique ID to ensure it is the 
   offical Oracle Data Provider for .NET, Managed Driver for Entity Framework downloads.

   B. Select the package you wish to install.


4. Click on the Install button to select the desired NuGet package(s) to include with the project. Accept the 
license agreement and Visual Studio will continue the setup. ODP.NET, Managed Driver will be installed 
automatically as a dependency for ODP.NET, Managed Driver for Entity Framework.

5. Open the app/web.config file to configure the ODP.NET connection string and local naming parameters 
(i.e. tnsnames.ora). Below is an example of configuring the local naming parameters:

  <oracle.manageddataaccess.client>
    <version number="*">
      <dataSources>
        <!-- Customize these connection alias settings to connect to Oracle DB -->
        <dataSource alias="SampleDataSource" descriptor="(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL))) " />
      </dataSources>
    </version>
  </oracle.manageddataaccess.client>

6. Modify the app/web.config file's connection string to create a DbContext your Entity Framework application 
will use. Below is an example of a configured DbContext.

  <connectionStrings>
    <add name="OracleDbContext" providerName="Oracle.ManagedDataAccess.Client"
      connectionString="User Id=hr;Password=hr;Data Source=MyDataSource"/>
  </connectionStrings>

After following these instructions, ODP.NET, Managed Driver for Entity Framework is now configured and ready 
to use.
